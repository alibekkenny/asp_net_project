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
    [EnableCors("MyPolicy")]
    public class QuestionsController : Controller
    {
        private readonly UsersContext _context;

        public QuestionsController(UsersContext usersContext)
        {
            _context = usersContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Task.Run(() => _context.Questions.ToArray()));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Task.Run(() => _context.Questions.Find(id)));
        }
        [HttpGet]
        [Route("{id}/answers")]
        public async Task<IActionResult> GetAnswersById(int id)
        {
            return Ok(await Task.Run(() => _context.Answers.Where(x=> x.QuestionId == id).ToArray()));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Question question)
        {
            _context.Questions.Add(question);

            await _context.SaveChangesAsync();

            return Ok(question);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Question question)
        {
            _context.Update(question);

            await _context.SaveChangesAsync();

            return Ok(question);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Question question = _context.Questions.Find(id);

            _context.Questions.Remove(question);

            await _context.SaveChangesAsync();

            return Ok(question);
        }
    }
}
