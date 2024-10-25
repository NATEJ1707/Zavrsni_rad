using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Klima
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Model { get; set; }

    [Required]
    public decimal Cijena { get; set; }

    [Required]
    public int Garancija { get; set; }

    [Required]
    public bool EnergetskiUcinkovita { get; set; }

    [ForeignKey("Marka")]
    public int MarkaId { get; set; }

    public Marka? Marka { get; set; }
}
