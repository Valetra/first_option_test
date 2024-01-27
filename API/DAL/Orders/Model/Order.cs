using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Index(nameof(Number))]
public class Order : BaseModel<Guid>
{
    public int Number { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public List<Guid> Supplies { get; set; } = new List<Guid>();
    public int Cost { get; set; }
}
