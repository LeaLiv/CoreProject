
using firstProject.Interfaces;
using firstProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace firstProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        IService<User> UserService;

        public UsersController(IService<User> userService)
        {
            UserService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll() =>
        UserService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        public IActionResult Post(User newUser)
        {
            UserService.Insert(newUser);
            return CreatedAtAction(nameof(Post), new { Id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User newUser)
        {
            if (id != newUser.Id)
            {
                return BadRequest();
            }
            var existingUser = UserService.Get(id);
            if (existingUser is null)
                return NotFound();
            UserService.Update(newUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingUser = UserService.Get(id);
            if (existingUser is null)
                return NotFound();
            UserService.Delete(id);
            return NoContent();
        }
    }
}