using System.ComponentModel.DataAnnotations;

public class Marka
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}
