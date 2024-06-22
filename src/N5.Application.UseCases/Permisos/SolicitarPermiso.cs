using Elasticsearch.Net;
using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;
using Nest;

namespace N5.Application.UseCases.Permisos
{
    public class SolicitarPermiso
    {
        public class SolicitarPermisoCommand : MediatR.IRequest<Result<PermisoDto>>
        {
            public string NombreEmpleado { get; set; }
            public string ApellidoEmpleado { get; set; }
            public int TipoPermisoId { get; set; }
            public DateTime FechaPermiso { get; set; }
        }

        public class Handler : IRequestHandler<SolicitarPermisoCommand, Result<PermisoDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ElasticClient _client;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));
                var settings = new ConnectionSettings(pool)
                    .DefaultIndex("permisos")
                        .BasicAuthentication("elastic", "f+1i_ZIx=07mu=yLB3y1")  // Agrega autenticación básica
                        .DisableDirectStreaming()
                        .ServerCertificateValidationCallback(CertificateValidations.AllowAll);

                _client = new ElasticClient(settings);
            }

            public async Task<Result<PermisoDto>> Handle(SolicitarPermisoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var nombrePermiso = await _unitOfWork.TipoPermisoRepository.ObtenerPorId(request.TipoPermisoId);

                    var permiso = new Permiso
                    {
                        NombreEmpleado = request.NombreEmpleado,
                        ApellidoEmpleado = request.ApellidoEmpleado,
                        TipoPermisoId = request.TipoPermisoId,
                        FechaPermiso = request.FechaPermiso,
                        IdPermisoE = ""
                    };

                    await _unitOfWork.PermisoRepository.Crear(permiso);
                    await _unitOfWork.SaveChangesAsync();

                    var idP = permiso.PermisoId;

                    //Para insertar en Index de ElasticSearch
                    var permisoE = new PermisoDto
                    {
                        PermisoId = idP,
                        NombreEmpleado = request.NombreEmpleado,
                        ApellidoEmpleado = request.ApellidoEmpleado,
                        TipoPermisoId = request.TipoPermisoId,
                        FechaPermiso = request.FechaPermiso,
                        NombreTipoPermiso = nombrePermiso.Descripcion
                    };

                    var indexResponse = await _client.Indices.ExistsAsync("permisos");

                    if (!indexResponse.Exists)
                    {
                        var createIndexResponse = await _client.Indices.CreateAsync("permisos", i => i.Map<PermisoDto>(x => x.AutoMap()));
                    }

                    var response = await _client.IndexDocumentAsync(permisoE);

                    if (response.IsValid)
                    {
                        Console.WriteLine($"Index document with ID {response.Id} succeeded.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to index document: {response.ServerError}");
                    }

                    var idE = response.Id;

                    var tipoPermisoDB = _unitOfWork.PermisoRepository.ObtenerPorId(idP).Result;
                    tipoPermisoDB.IdPermisoE = idE;
                    await _unitOfWork.PermisoRepository.Modificar(tipoPermisoDB);
                    await _unitOfWork.SaveChangesAsync();

                    return Result<PermisoDto>.Success(new PermisoDto
                    {
                        PermisoId = idP,
                        NombreEmpleado = request.NombreEmpleado,
                        ApellidoEmpleado = request.ApellidoEmpleado,
                        TipoPermisoId = request.TipoPermisoId,
                        FechaPermiso = request.FechaPermiso,
                        NombreTipoPermiso = nombrePermiso.Descripcion,
                        IdPermisoE = idE
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error", e.Message);
                    throw;
                }
            }
        }
    }
}