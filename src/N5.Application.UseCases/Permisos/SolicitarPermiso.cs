using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;

namespace N5.Application.UseCases.Permisos
{
    public class SolicitarPermiso
    {
        public class SolicitarPermisoCommand : IRequest<Result<PermisoDto>>
        {
            public int Id { get; set; }
            public string NombreEmpleado { get; set; }
            public string ApellidoEmpleado { get; set; }
            public int TipoPermisoId { get; set; }
            public DateTime FechaPermiso { get; set; }
        }

        public class Handler : IRequestHandler<SolicitarPermisoCommand, Result<PermisoDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<PermisoDto>> Handle(SolicitarPermisoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var permiso = new Permiso
                    {
                        NombreEmpleado = request.NombreEmpleado,
                        ApellidoEmpleado = request.ApellidoEmpleado,
                        TipoPermisoId = request.TipoPermisoId,
                        FechaPermiso = request.FechaPermiso
                    };

                    await _unitOfWork.PermisoRepository.SolicitarPermiso(permiso);
                    await _unitOfWork.SaveChangesAsync();
                    
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