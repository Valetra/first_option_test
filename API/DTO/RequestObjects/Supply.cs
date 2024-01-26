using System.ComponentModel.DataAnnotations;

namespace RequestObjects;

public class Supply
{
    [Required]
    [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
    public required string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The field Cost must be greater than or equal 1")]
    public required string Cost { get; set; }
}