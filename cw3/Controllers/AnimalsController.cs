using System.Data.SqlClient;
using System.Net;
using cw3.Models;
using cw3.Models.Dto;
using cw3.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers;

[Route("api/animals")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _service;

    public AnimalsController(IAnimalService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ICollection<Animal>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get([FromQuery] string? orderBy)
    {
        var result = await _service.GetAnimals(orderBy);

        if (!result.Any())
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Animal), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await _service.GetById(id);
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Animal), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> Create([FromBody] AnimalDto dto)
    {
        if (await _service.Exists(dto.Id))
            return Conflict();
        
        var result = await _service.CreateAnimal(dto);
        
        return Created($"/api/animals/{result.Id}", result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Animal), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDto dto)
    {
        if (!await _service.Exists(id))
            return NotFound();

        var result = await _service.Update(id, dto);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!await _service.Exists(id))
            return NotFound();

        await _service.Delete(id);

        return Ok();
    }
}