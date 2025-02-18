using Microsoft.AspNetCore.Mvc;
using firstProject.Models;
using firstProject.Services;
namespace firstProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoesController : ControllerBase
{

    [HttpGet]
    public ActionResult<IEnumerable<Shoes>> Get()
    {
        return ShoesService.Get();
    }


    [HttpGet("{code}")]
    public ActionResult<Shoes> Get(int code)
    {
        var shoes = ShoesService.Get(code);
        if (shoes == null)
            return NotFound();
        return shoes;
    }

    [HttpPost]
    public ActionResult Post(Shoes newShoes)
    {
        var newCode = ShoesService.Insert(newShoes);
        if (newCode == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Code = newCode });
    }


    [HttpPut("{code}")]
    public ActionResult Put(int code, Shoes newShoes)
    {
        if (ShoesService.Update(code, newShoes))
        {
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{code}")]
    public ActionResult Delete(int code)
    {
        if (ShoesService.Delete(code))
        {
            return Ok();
        }
        return NotFound();
    }
}
