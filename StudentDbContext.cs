using Microsoft.EntityFrameworkCore;
using StudentDetails;
public class StudentDbContext : DbContext
{
   public DbSet<Student> AllStudents { get; set; }
   public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }
}