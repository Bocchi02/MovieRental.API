using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieRental.API.Models
{
    public class RentalHeaderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentalHeaderDetailId { get; set; }
        public int RentalHeaderId { get; set;}
        public int MovieId { get; set;}
        public string Status { get; set; }

        [JsonIgnore]
        public RentalHeader? RentalHeader { get; set; }
        public Movie? Movie { get; set; }
    }
}
