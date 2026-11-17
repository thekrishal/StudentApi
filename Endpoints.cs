using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StudentDetails;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost("/students", async (Student student,StudentService service) =>
        {
            await service.AddStudent(student);
            return Results.Created($"/students/{student.Id}", student);
        }).RequireAuthorization();
        app.MapGet("/students", async (StudentService service,int page,int pageSize) =>
        {
            var students=await service.GetAllStudent(page,pageSize);
            return Results.Ok(students);

        }).RequireAuthorization();
        app.MapGet("/students/{id}", async (int id, StudentService service) =>
        {
            var student=await service.GetOneStudent(id); //Returns first student that matches the passed id
            if (student == null)
            return Results.NotFound($"No student having ID: {id}");
            return Results.Ok(student);
        }).RequireAuthorization();
        app.MapPut("/students/{id}", async (int id, Student updatedStudent, StudentService service) =>
        {
            var student = await service.UpdateAStudent(id,updatedStudent);
            if (student == null)
            return Results.NotFound("Student Not Found");
            return Results.Ok(student);

        }).RequireAuthorization();
        app.MapPatch("/students/{id}/age", async (int id, int age,StudentService service) =>
        {
            if(age<1||age>120)
            {
                return Results.BadRequest($"Invalid age");
            }
            var student=await service.UpdateAStudentsAge(id,age);
            if (student == null)
            return Results.NotFound($"Student with {id} Not Found");
            else
                return Results.Ok(student);
        }).RequireAuthorization();
        app.MapDelete("/students/{id}", async (int id, StudentService service) =>
        {
            var deletedStudent = await service.DeleteAStudent(id);
            if (deletedStudent == null)
                return Results.NotFound($"Student with id {id} not found");
            return Results.Ok(deletedStudent);

        }).RequireAuthorization().RequireRole("Admin");
        
        app.MapGet("/students/search",async (string name,StudentService service)=>
        {
            var searchedStudent= await service.SearchAStudent(name);
            return Results.Ok(searchedStudent);
        }).RequireAuthorization();
        app.MapGet("/students/filter",async (int age,StudentService service)=>
        {
            var filteredStudent=await service.FilterStudents(age);
            return Results.Ok(filteredStudent);
        }).RequireAuthorization();
        app.MapPost("/auth/register", async (RegisterRequest request, UserManager<ApplicationUser> manager) =>
        {
            ApplicationUser user = new ApplicationUser
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email
            };
            IdentityResult status = await manager.CreateAsync(user, request.Password);
            if (!status.Succeeded)
            {
                return Results.BadRequest(status.Errors.Select(errorType => errorType.Description));
            }
            IdentityResult roleStatus = await manager.AddToRoleAsync(user, "Student");
            if (!roleStatus.Succeeded)
            {
                await manager.DeleteAsync(user);

                return Results.Problem(
                    title: "Registration failed",
                    detail: "Could not assign the default Student role.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
            return Results.Created("/auth/login", "Sign up successful. You can now login");
        });
       app.MapPost("/auth/login" , async(LoginRequest request, UserManager<ApplicationUser> manager,IConfiguration config)=>
       {
        var Email=request.Email;
        ApplicationUser? existingUser  = await  manager.FindByEmailAsync(Email);
        if(existingUser==null)
           {
               return Results.Unauthorized();
           }
        bool isPasswordCorrect =await manager.CheckPasswordAsync(existingUser,request.Password);
        if(!isPasswordCorrect)
           {
               return Results.Unauthorized();
           }
        IList<string> getRole = await manager.GetRolesAsync(existingUser);
        Claim claimForId=new Claim(ClaimTypes.NameIdentifier,existingUser.Id);
        Claim claimForEmail=new Claim(ClaimTypes.Email,existingUser.Email);
        List<Claim> claims=new List<Claim>
        {
            claimForId,
            claimForEmail
        };
        foreach (string role in getRole)
        {
            Claim claimForRole = new Claim(ClaimTypes.Role, role);
            claims.Add(claimForRole);
        }
        string? key=config.GetValue<string>("Jwt:Key");
        byte[] keyBytes=Encoding.UTF8.GetBytes(key);
        SymmetricSecurityKey securityKey= new SymmetricSecurityKey(keyBytes);
        SigningCredentials algo= new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var issuer=config.GetValue<string>("Jwt:Issuer");
        var audience=config.GetValue<string>("Jwt:Audience");
        JwtSecurityToken jwtToken=new JwtSecurityToken(issuer,audience,claims,DateTime.UtcNow,DateTime.UtcNow.AddMinutes(30),algo);
        JwtSecurityTokenHandler tokenHandler=new JwtSecurityTokenHandler();
        string jwtToken_string=tokenHandler.WriteToken(jwtToken);
        return Results.Ok(jwtToken_string);
       });
    }
}