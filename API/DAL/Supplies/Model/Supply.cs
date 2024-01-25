using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Index(nameof(Name), IsUnique = true)]
public class Supply : BaseModel<Guid>
{
    public string Name { get; set; } = "";
    public int Cost { get; set; } = 0;
}
