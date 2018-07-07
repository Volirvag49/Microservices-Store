using System.ComponentModel.DataAnnotations;

namespace LoyaltyProgram.Api.Models
{
    public class LoyaltyProgramSettings
    {
        [Required]
        public int Id { get; set; }
        public string Interests { get; set; }

        public int? LoyaltyProgramUserId { get; set; }
        public LoyaltyProgramUser LoyaltyProgramUser { get; set; }
    }
}
