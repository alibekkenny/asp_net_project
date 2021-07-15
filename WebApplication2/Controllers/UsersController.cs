using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {

        private readonly UsersContext _context;

        public UsersController(UsersContext usersContext)
        {
            _context = usersContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await Task.Run(() => _context.Users.ToArray()));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok(await Task.Run(() => _context.Users.Find(id)));
        }
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody]User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUsers([FromBody]User user)
        {
            _context.Update(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            User user = _context.Users.Find(id);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
