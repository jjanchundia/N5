using Elasticsearch.Net;
using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;
using Nest;

namespace N5.Application.UseCases.Permisos
{
    public class ModificarPermiso
    {
        public class ModificarPermisoCommand : MediatR.IRequest<Result<PermisoDto>>
        {
            public int Id { get; set; }
            public required string NombreEmpleado { get; set; }
            public string ApellidoEmpleado { get; set; }
            public int TipoPermisoId { get; set; }
            public DateTime FechaPermiso { get; set; }
        }

        public class Handler : IRequestHandler<ModificarPermisoCommand, Result<PermisoDto>>
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

            public async Task<Result<PermisoDto>> Handle(ModificarPermisoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var tipoPermisoDB = _unitOfWork.PermisoRepository.ObtenerPorId(request.Id).Result;

                    if (tipoPermisoDB == null)
                    {
                        return Result<PermisoDto>.Failure("No se encontró Permiso para actualizar!");
                    }

                    tipoPermisoDB.NombreEmpleado = request.NombreEmpleado;
                    tipoPermisoDB.ApellidoEmpleado = request.ApellidoEmpleado;
                    tipoPermisoDB.TipoPermisoId = request.TipoPermisoId;
                    tipoPermisoDB.FechaPermiso = request.FechaPermiso;

                    await _unitOfWork.PermisoRepository.Modificar(tipoPermisoDB);
                    await _unitOfWork.SaveChangesAsync();

                    var indexName = "permisos"; 

                    // Realizar la consulta por el campo PermisoId
                    var searchResponse = await _client.SearchAsync<PermisoDto>(s => s
                        .Index(indexName)
                        .Query(q => q
                            .Term(t => t.Field(f => f.PermisoId).Value(tipoPermisoDB.PermisoId))
                        )
                    );

                    if (!searchResponse.IsValid || !searchResponse.Documents.Any())
                    {
                        Console.WriteLine($"No se encontró el documento con PermisoId: {request.Id}");
                    }

                    var nombrePermiso = await _unitOfWork.TipoPermisoRepository.ObtenerPorId(request.TipoPermisoId);

                    var permisoDt = new PermisoDto()
                    {
                        NombreEmpleado = request.NombreEmpleado,
                        ApellidoEmpleado = request.ApellidoEmpleado,
                        TipoPermisoId = request.TipoPermisoId,
                        FechaPermiso = request.FechaPermiso,
                        NombreTipoPermiso = nombrePermiso.Descripcion,
                        PermisoId = request.Id,
                        IdPermisoE = tipoPermisoDB.IdPermisoE ?? ""
                    };

                    foreach (var document in searchResponse.Documents)
                    {
                        var documentPath = new DocumentPath<PermisoDto>(permisoDt.IdPermisoE).Index(indexName);

                        var responses = await _client.UpdateAsync<PermisoDto, object>(documentPath, u => u
                            .Doc(permisoDt) // Los campos a actualizar en el documento
                            .RetryOnConflict(3)   // Opcional: Número de intentos en caso de conflictos
                            .Refresh(Refresh.True)
                        );
                    }

                    return Result<PermisoDto>.Success(new PermisoDto
                    {
                        NombreEmpleado = request.NombreEmpleado,
                        ApellidoEmpleado = request.ApellidoEmpleado,
                        TipoPermisoId = request.TipoPermisoId,
                        FechaPermiso = request.FechaPermiso
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