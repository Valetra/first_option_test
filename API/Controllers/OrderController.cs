using Microsoft.AspNetCore.Mvc;

using Services;

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
        bool isOrderCreated = await orderService.Create(supplies);

        if (isOrderCreated)
        {
            return Ok();
        }

        return BadRequest("Id of a non-existing supply was passed");
    }

    [HttpDelete("Order")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        bool isOrderDeleted = await orderService.Delete(id);

        if (isOrderDeleted)
        {
            return NoContent();
        }

        return NotFound($"Order with id '{id}' is not exists");
    }
}