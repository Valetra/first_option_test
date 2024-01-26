using DAL.Models;

namespace Services;

public interface IOrderService
{
    Task<List<Order>> GetAll();
    Task Create(List<Guid> orderSupplies);
    Task<bool> Delete(Guid id);
}