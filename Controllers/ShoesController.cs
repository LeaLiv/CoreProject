using Microsoft.AspNetCore.Mvc;
using firstProject.Models;
using firstProject.Interfaces;

namespace firstProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoesController : ControllerBase
{

    private IShoesService shoesService;

    public ShoesController(IShoesService shoesService)
    {
        this.shoesService=shoesService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Shoes>> Get()
    {
        return shoesService.Get();
    }


    [HttpGet("{code}")]
    public ActionResult<Shoes> Get(int code)
    {
        var shoes = shoesService.Get(code);
        if (shoes == null)
            return NotFound();
        return shoes;
    }

    [HttpPost]
    public ActionResult Post(Shoes newShoes)
    {
        var newCode = shoesService.Insert(newShoes);
        if (newCode == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Code = newCode });
    }


    [HttpPut("{code}")]
    public ActionResult Put(int code, Shoes newShoes)
    {
        if (shoesService.Update(code, newShoes))
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{code}")]
    public ActionResult Delete(int code)
    {
        if (shoesService.Delete(code))
        {
            return Ok();
        }
        return NotFound();
    }
}
