using MediatR;
using Microsoft.EntityFrameworkCore;
using N5.Application.Dtos;
using R = N5.Application.Servicios.Interfaces;
using N5.Application.Servicios.Repositorios;
using N5.Application.UseCases.Permisos;
using N5.Application.UseCases.TipoPermisos;
using N5.Domain;
using N5.Persistence;
using Nest;
using System.Reflection;
using Elasticsearch.Net;

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

builder.Services.AddScoped<R.IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(R.IRepository<>), typeof(Repository<>));

// Configuración del cliente Elasticsearch
var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));
var settings = new ConnectionSettings(pool)
    .DefaultIndex("permisos")
    .BasicAuthentication("elastic", "f+1i_ZIx=07mu=yLB3y1")  // Agrega autenticación básica
    .ServerCertificateValidationCallback(CertificateValidations.AllowAll); // Permitir todos los certificados

var client = new ElasticClient(settings);
builder.Services.AddSingleton<IElasticClient>(client);

builder.Services.AddScoped<IRequestHandler<ObtenerTipoPermisoPorId.ObtenerTipoPermisoPorIdRequest, Result<TipoPermisoDto>>, ObtenerTipoPermisoPorId.Handler>();
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

// Aplica la política CORS a todas las solicitudes
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
