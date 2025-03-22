using Microsoft.AspNetCore.Mvc;
using firstProject.Models;
using firstProject.Interfaces;

namespace firstProject.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ShoesController : ControllerBase
    {

        IShoesService ShoesService;

        public ShoesController(IShoesService shoesService)
        {
            this.ShoesService = shoesService;
        }

        [HttpGet]
        public ActionResult<List<Shoes>> GetAll() => 
        ShoesService.GetAll();



        [HttpGet("{code}")]
        public ActionResult<Shoes> Get(int code)
        {
            var shoes = ShoesService.Get(code);
            if (shoes == null)
                return NotFound();
            return shoes;
        }

        [HttpPost]
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
            Console.WriteLine("in start Put method");
            if (code != newShoes.Code)
            {
                Console.WriteLine(@"code != newShoes.Code code: {code} newShoes: {newShoes}");
                return BadRequest();

            }
            Console.WriteLine("in Put method");
            var existingShoe = ShoesService.Get(code);
            if (existingShoe is null)
            {
                return NotFound();
            }
            ShoesService.Update(newShoes);
            Console.WriteLine("in end Put method");
            return NoContent();
        }

        [HttpDelete("{code}")]
        public IActionResult Delete(int code)
        {
            var shoe = ShoesService.Get(code);
            if (shoe is null)
            {
                return NotFound();
            }
            ShoesService.Delete(code);
            return Content(ShoesService.Count.ToString());
        }
    }
}