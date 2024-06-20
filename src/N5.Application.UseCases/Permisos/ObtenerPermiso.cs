using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;

namespace N5.Application.UseCases.Permisos
{
    public class ObtenerPermiso
    {
        public class ObtenerPermisoRequest : IRequest<Result<List<PermisoDto>>>
        {
        }

        public class Handler : IRequestHandler<ObtenerPermisoRequest, Result<List<PermisoDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<List<PermisoDto>>> Handle(ObtenerPermisoRequest request, CancellationToken cancellationToken)
            {
                var permisos = await _unitOfWork.PermisoRepository.ObtenerPermisos();
                var permisosDto = new List<PermisoDto>();

                foreach (var item in permisos)
                {
                    var permisoDto = new PermisoDto
                    {
                        Id = item.Id,
                        NombreEmpleado = item.NombreEmpleado,
                        ApellidoEmpleado = item.ApellidoEmpleado,
                        FechaPermiso = item.FechaPermiso,
                        TipoPermisoId = item.TipoPermisoId,
                    };

                    permisosDto.Add(permisoDto);
                }

                return Result<List<PermisoDto>>.Success(permisosDto);
            }
        }
    }
}