using System;
using System.Text;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;

namespace Honeymustard
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        protected IRepository<UserDocument> Repository;
        protected Tokens Tokens;
        protected ICredentials Credentials;

        public AuthController(IRepository<UserDocument> repository, Tokens tokens, ICredentials credentials)
        {
            Repository = repository;
            Tokens = tokens;
            Credentials = credentials;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GetToken([FromBody] UserModel user)
        {
            if (!ModelState.IsValid) {
                return BadRequest("User model failed to validate");
            }

            var document = AutoMapper.Mapper.Map<UserDocument>(user);
            document.Password = Hashing.GenerateHash(document.Password, Credentials.UserSalt);

            var matches = Repository.FindAny(UserRepository.FilterMatch(document));

            if (matches.Count() == 1)
            {
                return Ok(new
                {
                    token = Tokens.Token
                });
            }

            return BadRequest("Authentication failed");
        }
    }
}