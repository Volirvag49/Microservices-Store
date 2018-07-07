using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoyaltyProgram.Api.Models
{
    public class LoyaltyProgramUser
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public int LoyaltyPoints { get; set; }
        public ICollection<LoyaltyProgramSettings> Settings { get; set; }
    }
}
