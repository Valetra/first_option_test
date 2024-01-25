using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public class Order : BaseModel<Guid>
{
    public int Number { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public List<Guid> Supplies { get; set; } = new List<Guid>();
    public int OrderCost { get; set; }
}
