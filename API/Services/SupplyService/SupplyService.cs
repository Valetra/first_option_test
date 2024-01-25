using DAL.Models;
using DAL.Repositories;

namespace Services;

public class SupplyService(IBaseRepository<Supply, Guid> supplyRepository) : ISupplyService
{
    public async Task<List<Supply>> GetAll()
    {
        return await supplyRepository.GetAll();
    }

    public Task Delete(Guid id)
    {
        return supplyRepository.Delete(id);
    }

    public async Task<Supply> Create(Supply supply)
    {
        return await supplyRepository.Create(supply);
    }
}