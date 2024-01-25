using DAL.Models;

namespace Services;

public interface ISupplyService
{
    Task<List<Supply>> GetAll();
    Task<bool> Create(Supply supply);
    Task<bool> Delete(Guid id);
}