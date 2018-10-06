using System;
using System.Text;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Honeymustard
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        protected IUserRepository Repository;
        protected Tokens Tokens;
        protected ICredentials Credentials;
        protected ILogger<AuthController> Logger;

        public AuthController(
            IUserRepository repository,
            Tokens tokens,
            ICredentials credentials,
            ILogger<AuthController> logger
        )
        {
            Repository = repository;
            Tokens = tokens;
            Credentials = credentials;
            Logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GetToken([FromBody] UserModel user)
        {
            if (!ModelState.IsValid) {
                Logger.LogWarning("User model failed to validate");
                return BadRequest("Authentication failed");
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

            Logger.LogWarning($"Authentication failed for user: {user.Username}");
            return BadRequest("Authentication failed");
        }
    }
}