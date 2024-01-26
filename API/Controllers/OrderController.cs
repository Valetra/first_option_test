using Microsoft.AspNetCore.Mvc;

using Services;
using Exceptions;

namespace Controllers;

[ApiController]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet("Orders")]
    public async Task<ActionResult<List<DAL.Models.Order>>> GetOrders()
    {
        List<DAL.Models.Order> orders = await orderService.GetAll();

        return Ok(orders);
    }

    [HttpPost("Order")]
    public async Task<ActionResult> CreateOrder(List<Guid> supplies)
    {
        try
        {
            await orderService.Create(supplies);

            return Ok();
        }
        catch (UnknownSupplyIdInOrderException)
        {
            return BadRequest("Unknown supply id was passed in order");
        }
    }

    [HttpDelete("Order")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        bool isOrderDeleted = await orderService.Delete(id);

        if (isOrderDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}