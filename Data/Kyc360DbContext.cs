using KYC360.Models;
using Microsoft.EntityFrameworkCore;

public class Kyc360DbContext : DbContext
{
    public Kyc360DbContext(DbContextOptions<Kyc360DbContext> options) : base(options) { }

    public DbSet<Entity> Entities { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Date> Dates { get; set; }
    public DbSet<Name> Names { get; set; }
}
