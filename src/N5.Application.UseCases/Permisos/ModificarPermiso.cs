using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;

namespace N5.Application.UseCases.Permisos
{
    public class ModificarPermiso
    {
        public class ModificarPermisoCommand : IRequest<Result<PermisoDto>>
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

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<PermisoDto>> Handle(ModificarPermisoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var permisoDB = _unitOfWork.PermisoRepository.ObtenerPermisoPorId(request.Id).Result;

                    if (permisoDB == null)
                    {
                        return Result<PermisoDto>.Failure("No se encontró Permiso para actualizar!");
                    }

                    permisoDB.NombreEmpleado = request.NombreEmpleado;
                    permisoDB.ApellidoEmpleado = request.ApellidoEmpleado;
                    permisoDB.TipoPermisoId = request.TipoPermisoId;
                    permisoDB.FechaPermiso = request.FechaPermiso;

                    await _unitOfWork.PermisoRepository.ModificarPermiso(permisoDB);
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