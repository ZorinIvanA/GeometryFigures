using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mindbox.Api.Domain;
using Mindbox.Api.Domain.Entities;
using Mindbox.Api.Domain.Interfaces;
using Moq;
using NUnit.Framework;

namespace Mindbox.UnitTests
{
    public class ServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SaveFigure_Success()
        {
            //Arrange
            Mock<IGeometryRepository> repositoryMock = new Mock<IGeometryRepository>();
            repositoryMock.Setup(x => x.SaveFigureAsync(It.IsAny<FigureBase>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            //Act
            GeometryService service = GetGeometryService(repositoryMock.Object);
            TestEntity model = new TestEntity();

            //Assert
            Assert.AreEqual(1, await service.SaveFigureAsync(model, CancellationToken.None));
        }

        [Test]
        public async Task SaveFigure_ThrowsNullArg_ModelIsEmpty()
        {
            //Arrange
            Mock<IGeometryRepository> repositoryMock = new Mock<IGeometryRepository>();

            //Act
            GeometryService service = GetGeometryService(repositoryMock.Object);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => service.SaveFigureAsync(null, CancellationToken.None));
        }

        [Test]
        public async Task SaveFigure_ThrowsException_SomeException()
        {
            //Arrange
            Mock<IGeometryRepository> repositoryMock = new Mock<IGeometryRepository>();
            repositoryMock.Setup(x => x.SaveFigureAsync(It.IsAny<FigureBase>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException());

            //Act
            GeometryService service = GetGeometryService(repositoryMock.Object);

            //Assert
            Assert.ThrowsAsync<NotImplementedException>(() => service.SaveFigureAsync(new TestEntity(), CancellationToken.None));
        }

        [Test]
        public async Task GetFigure_Success()
        {
            //Arrange
            var guid = Guid.NewGuid();
            Mock<IGeometryRepository> repositoryMock = new Mock<IGeometryRepository>();
            repositoryMock.Setup(x => x.GetFigureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TestEntity());

            //Act
            GeometryService service = GetGeometryService(repositoryMock.Object);

            //Assert
            var result = await service.FindFigureAsync(1, CancellationToken.None);
            Assert.AreEqual(3, result.GetArea());
        }

        [Test]
        public async Task GetFigure_ReturnNull_NoFigureFound()
        {
            //Arrange
            Mock<IGeometryRepository> repositoryMock = new Mock<IGeometryRepository>();
            repositoryMock.Setup(x => x.GetFigureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as TestEntity);

            //Act
            GeometryService service = GetGeometryService(repositoryMock.Object);

            //Assert
            var result = await service.FindFigureAsync(1, CancellationToken.None);
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task GetFigure_Throws_RepositoryThrows()
        {
            //Arrange
            Mock<IGeometryRepository> repositoryMock = new Mock<IGeometryRepository>();
            repositoryMock.Setup(x => x.GetFigureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotImplementedException());

            //Act
            GeometryService service = GetGeometryService(repositoryMock.Object);

            //Assert
            Assert.ThrowsAsync<NotImplementedException>(() => service.FindFigureAsync(1, CancellationToken.None));
        }

        private GeometryService GetGeometryService(IGeometryRepository repository)
        {
            return new GeometryService(repository);
        }
    }
}
