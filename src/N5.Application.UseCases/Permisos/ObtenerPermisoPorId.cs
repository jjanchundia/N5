using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;

namespace N5.Application.UseCases.Permisos
{
    public class ObtenerPermisoPorId
    {
        public class ObtenerPermisoPorIdRequest : MediatR.IRequest<Result<PermisoDto>>
        {
            public int PermisoId { get; set; }
        }

        public class Handler : IRequestHandler<ObtenerPermisoPorIdRequest, Result<PermisoDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<PermisoDto>> Handle(ObtenerPermisoPorIdRequest request, CancellationToken cancellationToken)
            {
                var tipoPermisoDB = await _unitOfWork.PermisoRepository.ObtenerPorId(request.PermisoId);
                var tipoPermiso = await _unitOfWork.TipoPermisoRepository.ObtenerPorId(tipoPermisoDB.TipoPermisoId);

                var permisoDto = new PermisoDto
                {
                    PermisoId = tipoPermisoDB.PermisoId,
                    NombreEmpleado = tipoPermisoDB.NombreEmpleado,
                    ApellidoEmpleado = tipoPermisoDB.ApellidoEmpleado,
                    TipoPermisoId = tipoPermisoDB.TipoPermisoId,
                    NombreTipoPermiso = tipoPermiso.Descripcion,
                    FechaPermiso = tipoPermisoDB.FechaPermiso
                };

                return Result<PermisoDto>.Success(permisoDto);
            }
        }
    }
}