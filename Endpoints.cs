using Microsoft.EntityFrameworkCore;
using StudentDetails;
public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("/students", async (Student student,StudentService service) =>
        {
            await service.AddStudent(student);
            return Results.Created($"/students/{student.Id}", student);
        });
        app.MapGet("/students", async (StudentService service,int page,int pageSize) =>
        {
            var students=await service.GetAllStudent(page,pageSize);
            return Results.Ok(students);

        });
        app.MapGet("/students/{id}", async (int id, StudentService service) =>
        {
            var student=await service.GetOneStudent(id); //Returns first student that matches the passed id
            if (student == null)
            return Results.NotFound($"No student having ID: {id}");
            return Results.Ok(student);
        });
        app.MapPut("/students/{id}", async (int id, Student updatedStudent, StudentService service) =>
        {
            var student = await service.UpdateAStudent(id,updatedStudent);
            if (student == null)
            return Results.NotFound("Student Not Found");
            return Results.Ok(student);

        });
        app.MapPatch("/students/{id}/age", async (int id, int age,StudentService service) =>
        {
            if(age<1&&age>120)
            {
                return Results.BadRequest($"Invalid age");
            }
            var student=await service.UpdateAStudentsAge(id,age);
            if (student == null)
            return Results.NotFound($"Student with {id} Not Found");
            else
                return Results.Ok(student);
        });
        app.MapDelete("/students/{id}", async (int id, StudentService service) =>
        {
            var deletedStudent = await service.DeleteAStudent(id);
            if (deletedStudent == null)
                return Results.NotFound($"Student with id {id} not found");
            return Results.Ok(deletedStudent);

        });
        app.MapGet("/students/search",async (string name,StudentService service)=>
        {
            var searchedStudent= await service.SearchAStudent(name);
            return Results.Ok(searchedStudent);
        });
        app.MapGet("/students/filter",async (int age,StudentService service)=>
        {
            var filteredStudent=await service.FilterStudents(age);
            return Results.Ok(filteredStudent);
        });
    }
}