using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using Moq;
using Xunit;

namespace FORTUNE_8OS_Tests.ServicesTest
{
    public class ShipServiceTests
    {
        [Fact]
        public void CreateShip_WhenShipDoesNotExists_ShoulCreateAndPostShip()
        {
            // Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(g => g.GetShipList()).Returns(new List<Ship>());

            var shipService = new ShipService(shipGatewayMock.Object);

            // Act
            shipService.CreateShip();

            // Assert
            shipGatewayMock.Verify(g => g.PostShipDatabase(It.IsAny<Ship>()), Times.Once);
        }

        [Fact]

        public void CreateShip_WhenShipExists_ShouldNotCreateShip()
        {
            // Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(g => g.GetShipList()).Returns(new List<Ship> { new Ship("ShipExistents", 60M) });

            var shipService = new ShipService(shipGatewayMock.Object);

            // Act
            shipService.CreateShip();

            //Assert
            shipGatewayMock.Verify(g => g.PostShipDatabase(It.IsAny<Ship>()), Times.Never);
        }

        [Fact]
        public void GetShip_WhenShipExists_ShouldReturnShip()
        {
            // Arrange
            var expectedShip = new Ship("ShipOne", 60M);

            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(p => p.GetShipList()).Returns(new List<Ship> { expectedShip });

            var shipService = new ShipService(shipGatewayMock.Object);

            // Act
            var result = shipService.GetShip();

            // Assert
            Assert.Equal(expectedShip, result);
        }

        [Fact]
        public void GetShip_WhenShipDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(p => p.GetShipList()).Returns(new List<Ship>());

            var shipService = new ShipService(shipGatewayMock.Object);

            // Act
            var result = shipService.GetShip();

            // Assert
            Assert.Null(result);
        }
    }
}