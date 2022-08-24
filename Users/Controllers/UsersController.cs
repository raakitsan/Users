using Microsoft.AspNetCore.Mvc;
using Users.Models;
using System;

// For more information on enabling Web API for empty projects,
// visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public static List<User> Users = new List<User>();

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return Users;
            //return new string[] { "value1", "value2" }
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return null;
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            user.Id = 1;
            user.DateAdded = DateTime.UtcNow;
            Users.Add(user);

            var result = new { Id = user.Id, Password = "pass" };

            // Look at the "Location" header in the response output in Postman
            return CreatedAtAction(nameof(Get), new { id = user.Id }, result);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}