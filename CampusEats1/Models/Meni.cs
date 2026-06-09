using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models;

public class Meni
{
    public int Id { get; set; }

    [DataType(DataType.Date)]
    public DateTime Datum { get; set; } = DateTime.Today;

    public int ObrokId { get; set; }
    public Obrok? Obrok { get; set; }
}
