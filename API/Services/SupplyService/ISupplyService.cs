using DAL.Models;

namespace Services;

public interface ISupplyService
{
    Task<List<Supply>> GetAll();
    Task Delete(Guid id);
    Task<Supply> Create(Supply supply);
}