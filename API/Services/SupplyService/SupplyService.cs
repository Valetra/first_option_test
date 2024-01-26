using DAL.Models;
using DAL.Repositories;
using Exceptions;

namespace Services;

public class SupplyService(IBaseRepository<Supply, Guid> supplyRepository) : ISupplyService
{
    public async Task<List<Supply>> GetAll()
    {
        return await supplyRepository.GetAll();
    }

    public async Task<bool> Create(Supply supply)
    {
        try
        {
            await supplyRepository.Create(supply);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        return await supplyRepository.Delete(id);
    }
}