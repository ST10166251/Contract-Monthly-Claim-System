using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Contract_Monthly_Claim_System.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Claim> Claims { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Manager> Managers { get; set; }
}

