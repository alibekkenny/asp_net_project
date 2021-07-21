using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("[controller]")]
    public class AnswersController : Controller
    {
        private readonly UsersContext _context;

        public AnswersController(UsersContext usersContext)
        {
            _context = usersContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Task.Run(() => _context.Answers.ToArray()));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Task.Run(() => _context.Answers.Find(id)));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Answer answer)
        {
            _context.Answers.Add(answer);

            await _context.SaveChangesAsync();

            return Ok(answer);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Answer answer)
        {
            _context.Update(answer);

            await _context.SaveChangesAsync();

            return Ok(answer);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Answer answer = _context.Answers.Find(id);

            _context.Answers.Remove(answer);

            await _context.SaveChangesAsync();

            return Ok(answer);
        }
    }
}
