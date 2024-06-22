using Elasticsearch.Net;
using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;
using Nest;

namespace N5.Application.UseCases.Permisos
{
    public class ObtenerPermiso
    {
        public class ObtenerPermisoRequest : MediatR.IRequest<Result<List<PermisoDto>>>
        {
        }

        public class Handler : IRequestHandler<ObtenerPermisoRequest, Result<List<PermisoDto>>>
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

            public async Task<Result<List<PermisoDto>>> Handle(ObtenerPermisoRequest request, CancellationToken cancellationToken)
            {
                var permisos = await _unitOfWork.PermisoRepository.ObtenerTodo();
                var permisosDto = new List<PermisoDto>();

                foreach (var item in permisos)
                {
                    var p = await _unitOfWork.TipoPermisoRepository.ObtenerPorId(item.TipoPermisoId);
                    var permisoDto = new PermisoDto
                    {
                        PermisoId = item.PermisoId,
                        NombreEmpleado = item.NombreEmpleado,
                        ApellidoEmpleado = item.ApellidoEmpleado,
                        FechaPermiso = item.FechaPermiso,
                        TipoPermisoId = item.TipoPermisoId,
                        IdPermisoE = item.IdPermisoE,
                        NombreTipoPermiso = p.Descripcion
                    };

                    permisosDto.Add(permisoDto);
                }

                var searchResponse = await _client.SearchAsync<PermisoDto>(s => s
                        .Index("permisos")
                        .MatchAll()
                    );

                if (searchResponse.IsValid)
                {
                    var documents = searchResponse.Documents;
                    Console.WriteLine($"Found {documents.Count} documents.");
                }
                Console.WriteLine(searchResponse);

                return Result<List<PermisoDto>>.Success(permisosDto);
            }
        }
    }
}