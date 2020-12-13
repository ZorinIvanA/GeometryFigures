using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mindbox.Api.Domain.Entities;
using Mindbox.Api.Domain.Exceptions;
using Mindbox.Api.Domain.Interfaces;
using Mindbox.Api.Presentation;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Mindbox.UnitTests
{
    public class ControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CreateFigure_Success()
        {
            //Arrange
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();
            serviceMock.Setup(x => x.SaveFigureAsync(It.IsAny<FigureBase>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);
            TestModel model = new TestModel();

            //Assert
            var result = await controller.CreateFigure(new JsonElement(),  CancellationToken.None);
            var resultAsType = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status201Created, resultAsType.StatusCode);
            Assert.IsNotNull(resultAsType);
            Assert.IsNotNull(resultAsType.Value);
            Assert.AreEqual("1", resultAsType.Value.ToString());
        }

        [Test]
        public async Task CreateFigure_400_ModelIsEmpty()
        {
            //Arrange
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);

            //Assert
            var result = await controller.CreateFigure(new JsonElement(), CancellationToken.None);
            var resultAsType = result as BadRequestObjectResult;
            var valueAsType = resultAsType?.Value as ProblemDetails;
            Assert.NotNull(valueAsType);
        }

        [Test]
        public async Task CreateFigure_400_BusinessValidationError()
        {
            //Arrange
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();
            serviceMock.Setup(x => x.SaveFigureAsync(It.IsAny<FigureBase>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new BusinessValidationException("ошибка", "ошибка"));

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);

            //Assert
            var result = await controller.CreateFigure(new JsonElement(), CancellationToken.None);
            var resultAsType = result as BadRequestObjectResult;
            var valueAsType = resultAsType?.Value as ProblemDetails;
            Assert.NotNull(valueAsType);
        }

        [Test]
        public async Task CreateFigure_500_ServerError()
        {
            //Arrange
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();
            serviceMock.Setup(x => x.SaveFigureAsync(It.IsAny<FigureBase>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException());

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);

            //Assert
            var result = await controller.CreateFigure(new JsonElement(), CancellationToken.None);
            var resultAsType = result as ObjectResult;
            Assert.NotNull(resultAsType);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, resultAsType.StatusCode);
        }

        [Test]
        public async Task GetFigure_Success()
        {
            //Arrange
            var guid = Guid.NewGuid();
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();
            serviceMock.Setup(x => x.FindFigureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestEntity());

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);

            //Assert
            var result = await controller.GetFigureArea(1, CancellationToken.None);
            var resultAsType = result as OkObjectResult;
            Assert.IsNotNull(resultAsType);
            Assert.IsNotNull(resultAsType.Value);
            Assert.AreEqual(3, resultAsType.Value);
        }

        [Test]
        public async Task GetFigure_400_FigureNotFound()
        {
            //Arrange
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();
            serviceMock.Setup(x => x.FindFigureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as TestEntity);

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);

            //Assert
            var result = await controller.GetFigureArea(1, CancellationToken.None);
            var resultAsType = result as BadRequestObjectResult;
            var valueAsType = resultAsType?.Value as ProblemDetails;
            Assert.NotNull(valueAsType);
        }

        [Test]
        public async Task GetFigure_400_BusinessValidationError()
        {
            //Arrange
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();
            serviceMock.Setup(x => x.FindFigureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new BusinessValidationException("ошибка", "ошибка"));

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);

            //Assert
            var result = await controller.GetFigureArea(1, CancellationToken.None);
            var resultAsType = result as BadRequestObjectResult;
            var valueAsType = resultAsType?.Value as ProblemDetails;
            Assert.NotNull(valueAsType);
        }

        [Test]
        public async Task GetFigure_500_ServerError()
        {
            //Arrange
            Mock<IGeometryService> serviceMock = new Mock<IGeometryService>();
            serviceMock.Setup(x => x.FindFigureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException());

            //Act
            GeometryController controller = GetGeometryController(serviceMock.Object);

            //Assert
            var result = await controller.GetFigureArea(1, CancellationToken.None);
            var resultAsType = result as ObjectResult;
            Assert.NotNull(resultAsType);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, resultAsType.StatusCode);
        }

        private GeometryController GetGeometryController(IGeometryService service)
        {
            return new GeometryController(service);
        }
    }
}