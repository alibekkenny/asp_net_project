using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly UsersContext _context;

        public PersonsController(UsersContext usersContext)
        {
            _context = usersContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Task.Run(() => _context.Persons));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Task.Run(() => _context.Persons.Find(id)));
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> Token(string email, string password)
        {
            var identity = GetIdentity(email, password);
            if(identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password" });
            }
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = _context.Persons.FirstOrDefault(x => x.Email == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Person person)
        {

            if (_context.Persons.Any(x => x.Email == person.Email))
            {
                return BadRequest(new { errorText = "User with this email already exists" });
            }

            _context.Persons.Add(person);

            await _context.SaveChangesAsync();

            return Ok(person);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Person person)
        {
            _context.Update(person);

            await _context.SaveChangesAsync();

            return Ok(person);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Person person = _context.Persons.Find(id);

            _context.Persons.Remove(person);

            await _context.SaveChangesAsync();

            return Ok(person);
        }
    }
}
