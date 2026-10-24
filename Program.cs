using Microsoft.EntityFrameworkCore;
using StudentDetails;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddDbContext<StudentDbContext>(options => options.UseSqlite("Data Source=student.db"));
builder.Services.AddScoped<StudentService>();
builder.Services.AddProblemDetails();
var app = builder.Build();
app.UseExceptionHandler();
app.MapEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment()) { app.MapOpenApi(); }
app.Run();