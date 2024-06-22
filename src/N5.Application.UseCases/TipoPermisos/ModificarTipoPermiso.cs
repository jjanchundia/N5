using MediatR;
using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Domain;

namespace N5.Application.UseCases.TipoPermisos
{
    public class ModificarTipoPermiso
    {
        public class ModificarTipoPermisoCommand : IRequest<Result<TipoPermisoDto>>
        {
            public int Id { get; set; }
            public string Descripcion { get; set; }
        }

        public class Handler : IRequestHandler<ModificarTipoPermisoCommand, Result<TipoPermisoDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<TipoPermisoDto>> Handle(ModificarTipoPermisoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var tipoPermisoDB = _unitOfWork.TipoPermisoRepository.ObtenerPorId(request.Id).Result;

                    if (tipoPermisoDB == null)
                    {
                        return Result<TipoPermisoDto>.Failure("No se encontró Tipo de Permiso para actualizar!");
                    }

                    tipoPermisoDB.Descripcion = request.Descripcion;

                    await _unitOfWork.TipoPermisoRepository.Modificar(tipoPermisoDB);
                    await _unitOfWork.SaveChangesAsync();

                    return Result<TipoPermisoDto>.Success(new TipoPermisoDto
                    {
                        Id = request.Id,
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