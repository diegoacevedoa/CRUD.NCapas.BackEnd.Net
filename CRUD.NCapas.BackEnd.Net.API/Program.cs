using CRUD.NCapas.BackEnd.Net.Data.Context;
using CRUD.NCapas.BackEnd.Net.Data.Mapping;
using CRUD.NCapas.BackEnd.Net.Service.Implementation;
using CRUD.NCapas.BackEnd.Net.Service.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Personas.CORS", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddTransient<IPersonaService, PersonaService>();
builder.Services.AddAutoMapper(typeof(Mapeos));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Personas.CORS");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
