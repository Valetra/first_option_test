using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using Services;
using Exceptions;

namespace Controllers;

[ApiController]
public class SupplyController(ISupplyService supplyService, IMapper mapper) : ControllerBase
{
    [HttpGet("Supplies")]
    public async Task<ActionResult<List<DAL.Models.Supply>>> GetSupplies()
    {
        List<DAL.Models.Supply> supplies = await supplyService.GetAll();

        return Ok(supplies);
    }

    [HttpPost("Supply")]
    public async Task<ActionResult> CreateSupply(RequestObjects.Supply supply)
    {
        try
        {
            await supplyService.Create(mapper.Map<DAL.Models.Supply>(supply));

            return Ok();
        }
        catch (SupplyDuplicateKeyValueException)
        {
            return BadRequest($"Supply with name '{supply.Name}' already exists.");
        }
    }

    [HttpDelete("Supply")]
    public async Task<ActionResult> DeleteSupply(Guid id)
    {
        bool isSupplyDeleted = await supplyService.Delete(id);

        if (isSupplyDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}