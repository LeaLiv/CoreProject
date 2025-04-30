using Microsoft.AspNetCore.Mvc;
using firstProject.Models;
using firstProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using firstProject.Services;

namespace firstProject.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ShoesController : ControllerBase
    {

        IService<Shoes> ShoesService;

        
        public ShoesController(IService<Shoes> shoesService)
        {
            ShoesService = shoesService;
        }

        [HttpGet]
        [Authorize(Policy = "user")]
        public ActionResult<List<Shoes>> GetAll() {
            string token = Request.Headers["Authorization"].ToString();
            User loggedUser = UserTokenService.GetUserFromToken(token);
            if (loggedUser.Role== "admin")
                return ShoesService.GetAll();
            return ShoesService.GetAll().Where(s => s.UserId == loggedUser.Id).ToList();
        }



        [HttpGet("{code}")]
        [Authorize(Policy = "user")]
        public ActionResult<Shoes> Get(int code)
        {
            string token = Request.Headers["Authorization"].ToString();
            User loggedUser = UserTokenService.GetUserFromToken(token);
            var shoes = ShoesService.Get(code);
            if (shoes == null || shoes.UserId!=loggedUser.Id && loggedUser.Role!= "admin")
                return NotFound();
            return shoes;
        }

        [HttpPost]
        [Authorize(Policy = "user")]
        public IActionResult Post(Shoes newShoes)
        {
            // var newCode = ShoesService.Insert(newShoes);
            // if (newCode == -1)
            // {
            //     return BadRequest();
            // }
            ShoesService.Insert(newShoes);
            return CreatedAtAction(nameof(Post), new { Code = newShoes.Code }, newShoes);
        }


        [HttpPut("{code}")]
        public IActionResult Update(int code, Shoes newShoes)
        {
            // Console.WriteLine("in start Put method");
            string token = Request.Headers["Authorization"].ToString();
            User loggedUser = UserTokenService.GetUserFromToken(token);
            if (code != newShoes.Code)
            {
                //Console.WriteLine(@"code != newShoes.Code code: {code} newShoes: {newShoes}");
                return BadRequest();

            }
            if(loggedUser.Role!= "admin" && loggedUser.Id!= newShoes.UserId)
            {
                return Forbid();
            }
            //Console.WriteLine("in Put method");
            var existingShoe = ShoesService.Get(code);
            if (existingShoe is null)
            {
                return NotFound();
            }
            ShoesService.Update(newShoes);
            // Console.WriteLine("in end Put method");
            return NoContent();
        }

        [HttpDelete("{code}")]
        [Authorize(Policy = "user")]
        public IActionResult Delete(int code)
        {
            string token = Request.Headers["Authorization"].ToString();
            User loggedUser = UserTokenService.GetUserFromToken(token);
            var shoe = ShoesService.Get(code);
            if(loggedUser.Role!= "admin" && loggedUser.Id!= shoe.UserId)
            {
                return Forbid();
            }
            if (shoe is null)
            {
                return NotFound();
            }
            ShoesService.Delete(code);
            return Content(ShoesService.GetAll().Count.ToString());
        }
    }
}