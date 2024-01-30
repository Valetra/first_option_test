using DAL.Models;

namespace UnitTests;

public class UnitTest
{
    bool EqualsSuppliesByProperties(Supply lhs, Supply rhs)
    {
        return lhs.Id == rhs.Id
            && lhs.Name == rhs.Name
            && lhs.Cost == rhs.Cost;
    }

    bool EqualsSuppliesByProperties(List<Supply> lhs, List<Supply> rhs)
    {
        for (int i = 0; i < lhs.Count; i++)
        {
            if (!EqualsSuppliesByProperties(lhs[i], rhs[i]))
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
            && EqualsSuppliesByProperties(lhs.Supplies, rhs.Supplies)
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
        private readonly List<Order> _entities = [];

        public Task<List<Order>> GetAll() => => await _entities;
        public IQueryable<Order> GetAllQuery() => _entities.AsQueryable();
        public Task<Order?> Get(Guid id) => _entities.FirstOrDefaultAsync(m => Equals(m.Id, id));
        public Task<Order> Create(Order order)
        {
            _entities.Add(order);

            return Task.FromResult(order);
        }
        public Task<bool> Delete(Guid id)
        {
            Order? orderToDelete = await _entities.FirstOrDefault(m => Equals(m.Id, id));

            if (orderToDelete is not null)
            {
                _entities.Remove(orderToDelete);

                return true;
            }

            return false;
        }
    }

    public class InMemorySupplyRepository : IBaseRepository<Supply, Guid>
    {
        private readonly List<Supply> _entities = [];

        public Task<List<Supply>> GetAll() => => await _entities;
        public IQueryable<Supply> GetAllQuery() => _entities.AsQueryable();
        public Task<Supply?> Get(Guid id) => _entities.FirstOrDefaultAsync(m => Equals(m.Id, id));
        public Task<Supply> Create(Supply supply)
        {
            _entities.Add(supply);

            return Task.FromResult(supply);
        }
        public Task<bool> Delete(Guid supply)
        {
            Supply? supplyToDelete = await _entities.FirstOrDefault(m => Equals(m.Id, supply.id));

            if (supplyToDelete is not null)
            {
                _entities.Remove(supplyToDelete);

                return true;
            }

            return false;
        }
    }
}