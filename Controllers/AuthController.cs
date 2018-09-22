using System;
using System.Text;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Honeymustard
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        protected IRepository<UserDocument> Repository;
        protected Tokens Tokens;
        protected Secrets Secrets;

        public AuthController(IRepository<UserDocument> repository, Tokens tokens, Secrets secrets)
        {
            Repository = repository;
            Tokens = tokens;
            Secrets = secrets;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GetToken([FromBody] UserModel user)
        {
            if (!ModelState.IsValid) {
                return BadRequest("User model failed to validate");
            }

            var document = AutoMapper.Mapper.Map<UserDocument>(user);
            document.Password = Hashing.GenerateHash(document.Password, Secrets.Salt);

            var matches = Repository.FindAny(UserRepository.FilterMatch(document));

            if (matches.Count() == 1)
            {
                var token = new JwtSecurityToken(
                    issuer: Tokens.Issuer,
                    expires: Tokens.Expires,
                    signingCredentials: Tokens.SigningCredentials
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Authentication failed");
        }
    }
}