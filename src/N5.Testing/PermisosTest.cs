using N5.Application.Dtos;
using N5.Application.Servicios.Interfaces;
using N5.Application.UseCases.Permisos;
using N5.Domain;
using Nest;
using Moq;
using Elasticsearch.Net;
using Elastic.Clients.Elasticsearch.Nodes;


namespace N5.Testing
{
    public class PermisosTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<Application.Servicios.Interfaces.IRepository<Permiso>> _mockPermisoRepository;
        private readonly Mock<Application.Servicios.Interfaces.IRepository<TipoPermiso>> _mockTipoPermisoRepository;
        private readonly Mock<IElasticClient> _mockElasticClient;
        //public readonly ElasticClient _client;
        private readonly ObtenerPermiso.Handler _handler;
        
        public PermisosTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPermisoRepository = new Mock<Application.Servicios.Interfaces.IRepository<Permiso>>();
            _mockTipoPermisoRepository = new Mock<Application.Servicios.Interfaces.IRepository<TipoPermiso>>();
            _mockElasticClient = new Mock<IElasticClient>();

            // Configuración del cliente Elasticsearch mock
            var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));
            var settings = new ConnectionSettings(pool)
                .DefaultIndex("permisos")
                .BasicAuthentication("elastic", "f+1i_ZIx=07mu=yLB3y1")  // Agrega autenticación básica
                .DisableDirectStreaming()
                .ServerCertificateValidationCallback(CertificateValidations.AllowAll);

            var _client = new ElasticClient(settings);
            _mockElasticClient.Setup(client => client.SearchAsync<PermisoDto>(It.IsAny<Func<SearchDescriptor<PermisoDto>, ISearchRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SearchResponse<PermisoDto>()); // Devuelve una respuesta simulada

            _mockUnitOfWork.Setup(uow => uow.PermisoRepository).Returns(_mockPermisoRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.TipoPermisoRepository).Returns(_mockTipoPermisoRepository.Object);
            _client = new ElasticClient(settings);

        }


        [Fact]
        public async Task Handle_ObtenerPermisos()
        {
            // Arrange
            var permisos = new List<Permiso>
            {
                new Permiso { PermisoId = 1, NombreEmpleado = "John", ApellidoEmpleado = "Doe", FechaPermiso = DateTime.Now, TipoPermisoId = 1}
            };

            var tipoPermiso = new TipoPermiso { Id = 1, Descripcion = "Tipo 1" };

            _mockPermisoRepository.Setup(repo => repo.ObtenerTodo()).ReturnsAsync(permisos);
            _mockTipoPermisoRepository.Setup(repo => repo.ObtenerPorId(1)).ReturnsAsync(tipoPermiso);

            var handler = new ObtenerPermiso.Handler(_mockUnitOfWork.Object)
            {
                // Asignar el cliente Elasticsearch mock
                //_client = _mockElasticClient.Object
            };

            var request = new ObtenerPermiso.ObtenerPermisoRequest();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
            Assert.Equal("John", result.Value[0].NombreEmpleado);
            Assert.Equal("Doe", result.Value[0].ApellidoEmpleado);
            Assert.Equal("Tipo 1", result.Value[0].NombreTipoPermiso);
        }

        [Fact]
        public async Task Handle_SolicitarPermisoCommand_ReturnsSuccessResult()
        {
            // Arrange
            var command = new SolicitarPermiso.SolicitarPermisoCommand
            {
                NombreEmpleado = "John",
                ApellidoEmpleado = "Doe",
                TipoPermisoId = 1,
                FechaPermiso = DateTime.Now
            };

            var handler = new SolicitarPermiso.Handler(_mockUnitOfWork.Object)
            {
                // Asignar el cliente Elasticsearch mock
                //_client = _mockElasticClient.Object
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal("John", result.Value.NombreEmpleado);
            Assert.Equal("Doe", result.Value.ApellidoEmpleado);
            Assert.Equal(1, result.Value.TipoPermisoId);
            Assert.Equal("Tipo 1", result.Value.NombreTipoPermiso);
            Assert.Equal("1", result.Value.IdPermisoE);
        }
    }
}