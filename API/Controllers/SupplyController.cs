using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using Services;

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
        bool isSupplyCreationSucceeded = await supplyService.Create(mapper.Map<DAL.Models.Supply>(supply));

        if (isSupplyCreationSucceeded)
        {
            return Ok();
        }

        return BadRequest();
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