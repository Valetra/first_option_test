using DAL.Models;
using DAL.Repositories;
using Services;

namespace UnitTests;

public class UnitTest
{
    static bool EqualsTwoSuppliesByProperties(Supply lhs, Supply rhs)
    {
        return lhs.Id == rhs.Id
            && lhs.Name == rhs.Name
            && lhs.Cost == rhs.Cost;
    }

    static bool EqualsSuppliesByProperties(List<Supply> lhs, List<Supply> rhs)
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

    static bool EqualsGuidLists(List<Guid> lhs, List<Guid> rhs)
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

    static bool EqualsOrdersByProperties(Order lhs, Order rhs)
    {
        return lhs.Id == rhs.Id
            && lhs.Number == rhs.Number
            && lhs.Status == rhs.Status
            && lhs.CreateDateTime == rhs.CreateDateTime
            && EqualsGuidLists(lhs.Supplies, rhs.Supplies)
            && lhs.Cost == rhs.Cost;
    }

    static bool EqualsOrderListsByProperties(List<Order> lhs, List<Order> rhs)
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

        OrderService orderService = new(inMemoryOrderRepository, new InMemorySupplyRepository());

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

        Assert.True(EqualsOrderListsByProperties(expectedResult, methodResult));
    }

    [Fact]
    public async void OrderServiceCreateTest()
    {
        InMemoryOrderRepository inMemoryOrderRepository = new();
        InMemorySupplyRepository inMemorySupplyRepository = new();

        OrderService orderService = new(inMemoryOrderRepository, inMemorySupplyRepository);

        List<Supply> supplies =
        [
            new Supply() { Id = new Guid("ef4e544a-29c9-468f-89b3-ab007af49e8f"), Name = "Apple", Cost = 5},
            new Supply() { Id = new Guid("296f2d8d-0e79-4955-87d4-fe3a7563725d"), Name = "Pineapple", Cost = 25},
            new Supply() { Id = new Guid("efddefb4-c36d-4165-91d3-160b07d63eb9"), Name = "Banana", Cost = 10},
            new Supply() { Id = new Guid("b3ab5b98-ba14-4ea4-98c1-0c743cb46b54"), Name = "Orange", Cost = 15},
            new Supply() { Id = new Guid("ea062867-a902-4fc0-aff3-fd919946999c"), Name = "Lemon", Cost = 20}
        ];

        inMemorySupplyRepository.entities.AddRange(supplies);

        List<Guid> orderSupplies =
        [
            new Guid("ef4e544a-29c9-468f-89b3-ab007af49e8f"),
            new Guid("ef4e544a-29c9-468f-89b3-ab007af49e8f"),
            new Guid("ef4e544a-29c9-468f-89b3-ab007af49e8f"),
            new Guid("efddefb4-c36d-4165-91d3-160b07d63eb9"),
            new Guid("efddefb4-c36d-4165-91d3-160b07d63eb9"),
            new Guid("ea062867-a902-4fc0-aff3-fd919946999c"),
            new Guid("ea062867-a902-4fc0-aff3-fd919946999c")
        ];

        await orderService.Create(orderSupplies);

        Order methodResult = inMemoryOrderRepository.entities.First();

        Order expectedResult = new()
        {
            Number = 1,
            Status = "Created",
            Supplies = orderSupplies,
            Cost = 75
        };

        Assert.True(EqualsOrdersByProperties(expectedResult, methodResult));
    }
}