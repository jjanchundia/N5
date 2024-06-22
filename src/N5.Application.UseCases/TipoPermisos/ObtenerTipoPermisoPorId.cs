using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;

namespace N5.Application.UseCases.TipoPermisos
{
    public class ObtenerTipoPermisoPorId
    {
        public class ObtenerTipoPermisoPorIdRequest : IRequest<Result<TipoPermisoDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<ObtenerTipoPermisoPorIdRequest, Result<TipoPermisoDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<TipoPermisoDto>> Handle(ObtenerTipoPermisoPorIdRequest request, CancellationToken cancellationToken)
            {
                var tipoPermiso = await _unitOfWork.TipoPermisoRepository.ObtenerPorId(request.Id);

                var tipoPermisosDto = new TipoPermisoDto
                {
                    Id = tipoPermiso.Id,
                    Descripcion = tipoPermiso.Descripcion
                };

                return Result<TipoPermisoDto>.Success(tipoPermisosDto);
            }
        }
    }
}