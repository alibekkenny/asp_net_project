using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("[controller]")]
    public class UniversitiesController : Controller
    {
        private readonly UsersContext _context;

        public UniversitiesController(UsersContext usersContext)
        {
            _context = usersContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetUniversities()
        {
            return Ok(await Task.Run(() => _context.Universities.ToArray()));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUniversityById(int id)
        {
            return Ok(await Task.Run(() => _context.Universities.Find(id)));
        }
        [HttpPost]
        public async Task<IActionResult> PostUniversities([FromBody] University university)
        {
            _context.Universities.Add(university);

            await _context.SaveChangesAsync();

            return Ok(university);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUniversity([FromBody] University university)
        {
            _context.Update(university);

            await _context.SaveChangesAsync();

            return Ok(university);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUniversity(int id)
        {
            University university = _context.Universities.Find(id);

            _context.Universities.Remove(university);

            await _context.SaveChangesAsync();

            return Ok(university);
        }
    }
}
