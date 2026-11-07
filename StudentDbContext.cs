using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentDetails;
public class StudentDbContext : IdentityDbContext
{
   public DbSet<Student> AllStudents { get; set; }
   public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }
}