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
        IUserService UserService;

        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        // [HttpGet]
        // [Authorize(Policy="admin")]
        // public ActionResult<List<User>> GetAll() => UserService.GetAll();
        [HttpGet]
        [Authorize(Policy = "user")]
        public ActionResult<List<User>> GetAll() {
            System.Console.WriteLine("GetAll called");
            string token = Request.Headers["Authorization"].ToString();
            User loggedUser = UserTokenService.GetUserFromToken(token);
            
            if (loggedUser.Role== "admin") 
                return UserService.GetAll();
            // System.Console.WriteLine(loggedUser.Name);
            return UserService.GetAll().Where(u=>u.Id==loggedUser.Id).ToList();
        }

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
            newUser.Password = PasswordService.HashPassword(newUser.Password);
            UserService.Insert(newUser);
            return CreatedAtAction(nameof(Post), new { Id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        [Authorize(Policy="user")]
        public IActionResult Update(int id, User newUser)
        {
            if (id != newUser.Id)
            {
                return BadRequest();
            }

            var existingUser = UserService.Get(id);
            if (existingUser is null)
                return NotFound();
            if(existingUser.Role=="user" && newUser.Role=="admin")
            return Forbid();
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
            // System.Console.WriteLine("in Login method called");
            User RequestUser=UserService.GetByuserName(user.userName);           
            var dt = DateTime.Now;
            if (RequestUser==null || !PasswordService.VerifyPassword(user.Password,RequestUser.Password ) || RequestUser.userName!=user.userName)
                return Unauthorized();
            var claims = new List<Claim>
            {
                 new Claim("id",RequestUser.Id.ToString())
            };
            if (RequestUser.Role == "admin")
            {
                claims.Add(new Claim("type","admin"));
            }

            else
            {
               claims.Add(new Claim("type","user"));
            }
            var token = UserTokenService.GetToken(claims);
            return new OkObjectResult(UserTokenService.WriteToken(token));
        }
    }
}
