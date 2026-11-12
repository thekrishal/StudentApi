using Microsoft.EntityFrameworkCore;
using StudentDetails;
public class StudentService
{
    private readonly StudentDbContext Db;
    private readonly ILogger<StudentService> _logger;

    public StudentService(StudentDbContext db,ILogger<StudentService> logger)
    {
        Db=db;
        _logger=logger;
    }
    public async Task AddStudent(Student newStudent)
    { 
        _logger.LogInformation("Adding a new student");
        Db.AllStudents.Add(newStudent);
        await Db.SaveChangesAsync();
    }
    public async Task<List<Student>> GetAllStudent(int page,int pageSize)
    {
       int skipCount=(page-1)*pageSize;
       return await Db.AllStudents.OrderBy(s=>s.Id).Skip(skipCount).Take(pageSize).ToListAsync();
    }
    public async Task<Student?> GetOneStudent(int id)
    {
        _logger.LogInformation("Looking up the student");
         return await Db.AllStudents.FirstOrDefaultAsync(s => s.Id == id);
    }
    public async Task<Student?> UpdateAStudent(int id,Student uStudent)
    {
        var student=await Db.AllStudents.FirstOrDefaultAsync(s => s.Id == id);
        if (student == null)
        return null;
        student.Name = uStudent.Name;
        student.Age  = uStudent.Age;
        await Db.SaveChangesAsync();
        return student;
    }
    public async Task<Student?> UpdateAStudentsAge(int id,int age)
    {
        var student = await Db.AllStudents.FirstOrDefaultAsync(s => s.Id == id);
        if (student == null)
            return null;
        else if (age >= 1 && age <= 120)
        {
            student.Age = age;
            await Db.SaveChangesAsync();
        }
        return student;
    }
    public async Task<Student?> DeleteAStudent(int id)
    {
        var studentToDelete= await Db.AllStudents.FirstOrDefaultAsync(s=>s.Id==id);
        if(studentToDelete==null)
            return null;
        Db.AllStudents.Remove(studentToDelete);
        await Db.SaveChangesAsync();
        return studentToDelete;
        
    }
    public async Task<List<Student>> SearchAStudent (string name)
    {
        var SearchedStudent= await Db.AllStudents.Where(s=>s.Name.Contains(name)).ToListAsync();
        return SearchedStudent;
    }
    public async Task<List<Student>> FilterStudents(int age)
    {
        var filteredStudent=await Db.AllStudents.Where(s=>s.Age>=age).ToListAsync();
        return filteredStudent;
    }


}
