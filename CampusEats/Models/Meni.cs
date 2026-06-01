namespace CampusEats.Models
{
    public class Meni
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Datum { get; set; }

    }
}
