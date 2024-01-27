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

    public async Task Create(Supply supply)
    {
        try
        {
            await supplyRepository.Create(supply);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            throw new SupplyDuplicateKeyValueException();
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        return await supplyRepository.Delete(id);
    }
}