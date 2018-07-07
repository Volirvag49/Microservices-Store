using LoyaltyProgram.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyProgram.Api.Infrastructure.EF.Context
{
    public class LoyaltyProgramContext : DbContext
    {
        public LoyaltyProgramContext(DbContextOptions<LoyaltyProgramContext> options) : base(options)
        {
        }
        public DbSet<LoyaltyProgramUser> LoyaltyProgramUser { get; set; }
        public DbSet<LoyaltyProgramSettings> LoyaltyProgramSettings { get; set; }
    }
}
