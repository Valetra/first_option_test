using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public class Supply : BaseModel<Guid>
{
    public string Name { get; set; }
    public int Cost { get; set; }
}
