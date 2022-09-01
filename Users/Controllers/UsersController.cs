using Microsoft.AspNetCore.Mvc;
using Users.Models;
using System;
using HelloWorldService;

// For more information on enabling Web API for empty projects,
// visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Users.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authenticator]
  public class UsersController : ControllerBase
    {
        public static List<User> users = new List<User>();
        private static int CurrentId = 100;

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return users;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public IActionResult GetSpecific(int id)
        {
            var user = users.FirstOrDefault(t => t.Id == id);

            if (user == null)
            {
                return NotFound(null);
            }

            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            user.Id = CurrentId++;
            user.DateAdded = DateTime.UtcNow;
            users.Add(user);
            
            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("Null or Empty Email field");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Null or Empty Password field");
            }

            //var result = new { Id = user.Id, Email = user.Email, Password = user.Password, DateAdded = user.DateAdded };

            return CreatedAtAction(nameof(GetSpecific), new { id = user.Id }, user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User value)
        {
            if (string.IsNullOrEmpty(value.Email))
            {
                return BadRequest("Null or Empty Email field");
            }
            if (string.IsNullOrEmpty(value.Password))
            {
                return BadRequest("Null or Empty Password field");
            }

            var user = users.FirstOrDefault(t => t.Id == id);
            if (user != null)
            {
                user.Email = value.Email;
                user.Password = value.Password;
                return Ok(user);
            }
            return NotFound();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rowsDeleted = users.RemoveAll(x => x.Id == id);
            if (rowsDeleted == 0)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}