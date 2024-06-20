using MediatR;
using Microsoft.EntityFrameworkCore;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Application.Servicios.Repositorios;
using N5.Application.UseCases.Permisos;
using N5.Domain;
using N5.Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Agregamos politicas CORS para uso de endpoint localmente
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDB")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRequestHandler<ObtenerPermiso.ObtenerPermisoRequest, Result<List<PermisoDto>>>, ObtenerPermiso.Handler>();
builder.Services.AddScoped<IRequestHandler<SolicitarPermiso.SolicitarPermisoCommand, Result<PermisoDto>>, SolicitarPermiso.Handler>();
builder.Services.AddScoped<IRequestHandler<ModificarPermiso.ModificarPermisoCommand, Result<PermisoDto>>, ModificarPermiso.Handler>();


builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

//services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
