using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;
using Nest;

namespace N5.Application.UseCases.TipoPermisos
{
    public class CrearTipoPermiso
    {
        public class CrearTipoPermisoCommand : MediatR.IRequest<Result<TipoPermisoDto>>
        {
            public string Descripcion { get; set; }
        }

        public class Handler : IRequestHandler<CrearTipoPermisoCommand, Result<TipoPermisoDto>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ElasticClient _client;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<TipoPermisoDto>> Handle(CrearTipoPermisoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var tipoPermiso = new TipoPermiso
                    {
                        Descripcion = request.Descripcion
                    };

                    await _unitOfWork.TipoPermisoRepository.Crear(tipoPermiso);
                    await _unitOfWork.SaveChangesAsync();

                    var idTipoP = tipoPermiso.Id;

                    return Result<TipoPermisoDto>.Success(new TipoPermisoDto
                    {
                        Id = idTipoP,
                        Descripcion = request.Descripcion
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