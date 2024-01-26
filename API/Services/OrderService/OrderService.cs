using DAL.Models;
using DAL.Repositories;
using Exceptions;

namespace Services;

public class OrderService(IBaseRepository<Order, Guid> orderRepository, IBaseRepository<Supply, Guid> supplyRepository) : IOrderService
{
    public async Task<List<Order>> GetAll()
    {
        return await orderRepository.GetAll();
    }

    public async Task<bool> Create(List<Guid> orderSupplies)
    {
        IQueryable<Order> orders = orderRepository.GetAllQuery();
        IQueryable<Supply> supplies = supplyRepository.GetAllQuery();

        int lastOrderNumber = 0;
        int totalCost = 0;

        if (orders.Any())
        {
            lastOrderNumber = orders.Select(o => o.Number).Max();
        }

        if (supplies.Any())
        {
            foreach (Guid supplyId in orderSupplies)
            {
                Supply? supply = supplies.FirstOrDefault(s => s.Id == supplyId);

                if (supply is null)
                {
                    return false;
                }

                totalCost += supply.Cost;
            }
        }

        Order newOrder = new Order()
        {
            Number = lastOrderNumber + 1,
            Status = "Created",
            Supplies = orderSupplies,
            OrderCost = totalCost,
        };

        await orderRepository.Create(newOrder);

        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            await orderRepository.Delete(id);

            return true;
        }
        catch (NonExistedItemException)
        {
            return false;
        }
    }
}