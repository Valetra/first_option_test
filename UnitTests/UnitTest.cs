using DAL.Models;
using DAL.Repositories;
using Services;

namespace UnitTests;

public class UnitTest
{
    bool EqualsTwoSuppliesByProperties(Supply lhs, Supply rhs)
    {
        return lhs.Id == rhs.Id
            && lhs.Name == rhs.Name
            && lhs.Cost == rhs.Cost;
    }

    bool EqualsSuppliesByProperties(List<Supply> lhs, List<Supply> rhs)
    {
        for (int i = 0; i < lhs.Count; i++)
        {
            if (!EqualsTwoSuppliesByProperties(lhs[i], rhs[i]))
            {
                return false;
            }
        }
        return true;
    }

    static bool EqualsItemsByIds(List<Guid> lhs, List<Guid> rhs)
    {
        for (int i = 0; i < lhs.Count; i++)
        {
            if (!Equals(lhs[i], rhs[i]))
            {
                return false;
            }
        }
        return true;
    }

    bool EqualsOrdersByProperties(Order lhs, Order rhs)
    {
        return lhs.Id == rhs.Id
            && lhs.Number == rhs.Number
            && lhs.Status == rhs.Status
            && lhs.CreateDateTime == rhs.CreateDateTime
            && EqualsItemsByIds(lhs.Supplies, rhs.Supplies)
            && lhs.Cost == rhs.Cost;
    }

    bool EqualsOrdersByProperties(List<Order> lhs, List<Order> rhs)
    {
        for (int i = 0; i < lhs.Count; i++)
        {
            if (!EqualsOrdersByProperties(lhs[i], rhs[i]))
            {
                return false;
            }
        }
        return true;
    }

    public class InMemoryOrderRepository : IBaseRepository<Order, Guid>
    {
        public readonly List<Order> entities = [];

        public Task<List<Order>> GetAll() => Task.FromResult(entities);
        public IQueryable<Order> GetAllQuery() => entities.AsQueryable();
        public Task<Order?> Get(Guid id) => Task.FromResult(entities.FirstOrDefault(m => Equals(m.Id, id)));
        public Task<Order> Create(Order order)
        {
            entities.Add(order);

            return Task.FromResult(order);
        }
        public Task<bool> Delete(Guid id)
        {
            Order? orderToDelete = entities.FirstOrDefault(m => Equals(m.Id, id));

            if (orderToDelete is not null)
            {
                entities.Remove(orderToDelete);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }

    public class InMemorySupplyRepository : IBaseRepository<Supply, Guid>
    {
        public readonly List<Supply> entities = [];

        public Task<List<Supply>> GetAll() => Task.FromResult(entities);
        public IQueryable<Supply> GetAllQuery() => entities.AsQueryable();
        public Task<Supply?> Get(Guid id) => Task.FromResult(entities.FirstOrDefault(m => Equals(m.Id, id)));
        public Task<Supply> Create(Supply supply)
        {
            entities.Add(supply);

            return Task.FromResult(supply);
        }
        public Task<bool> Delete(Guid id)
        {
            Supply? supplyToDelete = entities.FirstOrDefault(m => Equals(m.Id, id));

            if (supplyToDelete is not null)
            {
                entities.Remove(supplyToDelete);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }

    [Fact]
    public async void OrderServiceGetAllTest()
    {
        InMemoryOrderRepository inMemoryOrderRepository = new();
        InMemorySupplyRepository inMemorySupplyRepository = new();

        OrderService orderService = new(inMemoryOrderRepository, inMemorySupplyRepository);

        Order order = new()
        {
            Number = 1,
            Status = "Created",
            Supplies = [new Guid(), new Guid(), new Guid()],
            Cost = 10
        };

        inMemoryOrderRepository.entities.Add(order);

        List<Order> methodResult = await orderService.GetAll();

        List<Order> expectedResult = [order];

        Assert.True(EqualsOrdersByProperties(expectedResult, methodResult));
    }
}