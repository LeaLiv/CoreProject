using Microsoft.AspNetCore.Mvc;
using firstProject.Models;
using firstProject.Interfaces;

namespace firstProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoesController : ControllerBase
{

    private IShoesService ShoesService;

    public ShoesController(IShoesService shoesService)
    {
        this.ShoesService = shoesService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Shoes>> GetAll() => ShoesService.GetAll();



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
    public IActionResult Put(int code, Shoes newShoes)
    {
        if (code == newShoes.Code)
            return BadRequest();
        var existingShoe=ShoesService.Get(code);
        if (existingShoe is null)
        {
            return NotFound();
        }
        ShoesService.Update(newShoes);
        return NoContent();
    }

    [HttpDelete("{code}")]
    public IActionResult Delete(int code)
    {
        var shoe=ShoesService.Get(code);
        if (shoe is null)
        {
            return NotFound();
        }
        ShoesService.Delete(code);
        return Content(ShoesService.Count.ToString());
    }
}
