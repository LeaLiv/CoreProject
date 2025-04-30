using System.Security.Claims;
using firstProject.Interfaces;
using firstProject.Models;
using firstProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

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
        [Authorize(Policy="admin")]
        public ActionResult<List<User>> GetAll() => UserService.GetAll();

        [HttpGet("{id}")]
        [Authorize(Policy="user")]
        public ActionResult<User> Get(int id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return NotFound();
            string token = Request.Headers["Authorization"].ToString();
        User loggedUser = UserTokenService.GetUserFromToken(token);
        if (loggedUser.Role=="admin" || loggedUser.Id==user.Id)
            return user;
        return Forbid();
        }

        [HttpPost]
        [Authorize(Policy="admin")]
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
        [Authorize(Policy="admin")]
        public IActionResult Delete(int id)
        {
            var existingUser = UserService.Get(id);
            if (existingUser is null)
                return NotFound();
            UserService.Delete(id);
        IService<Shoes> shoesServiceConst = new ShoesServiceConst(new HostingEnvironment());
            shoesServiceConst.GetAll().Where(s => s.UserId == existingUser.Id).ToList().ForEach(s => shoesServiceConst.Delete(s.Code));
            return NoContent();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            var RequestUser=UserService.GetByUserName(user.UserNAme);
            var claims = new List<Claim>();
            var dt = DateTime.Now;
            var RequestUser=UserService.Get(user.Id);
            if (RequestUser==null || RequestUser.Password!=user.Password || RequestUser.UserNAme!=user.UserNAme)
                return Unauthorized();
            if (RequestUser.Role != "admin" || !RequestUser.Password.StartsWith("admin"))
            {
                claims = new List<Claim>
                {
                    new Claim("type","user"),
                    new Claim("id",RequestUser.Id.ToString()),
                };
            }

            else
            {
                claims = new List<Claim>
                {
                    new Claim("type","admin"),
                    new Claim("id",RequestUser.Id.ToString()),
                };
            }
            var token = UserTokenService.GetToken(claims);
            return new OkObjectResult(UserTokenService.WriteToken(token));
        }
    }
}
