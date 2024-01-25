using DAL.Models;

namespace Services;

public interface ISupplyService
{
    Task<List<Supply>> GetAll();
    Task<bool> Delete(Guid id);
    Task<bool> Create(Supply supply);
}