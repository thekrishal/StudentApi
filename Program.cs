using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddDbContext<StudentDbContext>(options => options.UseSqlite("Data Source=student.db"));
builder.Services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<StudentDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        string jwtKey = builder.Configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key is missing.");
        string issuer = builder.Configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("JWT issuer is missing.");
        string audience = builder.Configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("JWT audience is missing.");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddScoped<StudentService>();
builder.Services.AddProblemDetails();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment()) { app.MapOpenApi(); }
app.Run();