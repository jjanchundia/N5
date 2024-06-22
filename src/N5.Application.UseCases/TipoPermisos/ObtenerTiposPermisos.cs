using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;

namespace N5.Application.UseCases.TipoPermisos
{
    public class ObtenerTiposPermisos
    {
        public class ObtenerTiposPermisosRequest : IRequest<Result<List<TipoPermisoDto>>>
        {
        }

        public class Handler : IRequestHandler<ObtenerTiposPermisosRequest, Result<List<TipoPermisoDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<List<TipoPermisoDto>>> Handle(ObtenerTiposPermisosRequest request, CancellationToken cancellationToken)
            {
                var tipotipoPermisoDB = await _unitOfWork.TipoPermisoRepository.ObtenerTodo();
                var tipoPermisosDto = new List<TipoPermisoDto>();

                foreach (var item in tipotipoPermisoDB)
                {
                    var permisoDto = new TipoPermisoDto
                    {
                        Id = item.Id,
                        Descripcion = item.Descripcion
                    };

                    tipoPermisosDto.Add(permisoDto);
                }

                return Result<List<TipoPermisoDto>>.Success(tipoPermisosDto);
            }
        }
    }
}