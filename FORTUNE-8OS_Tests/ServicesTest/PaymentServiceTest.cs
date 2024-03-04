using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Entities.Enums;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using FORTUNE_8OS_Tests.Utilitaries;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS_Tests.ServicesTest
{
    public class PaymentServiceTest
    {
        [Theory]
        [InlineData(1, 50, "CONFIRM")]
        [InlineData(2, 40, "Confirm")]
        [InlineData(3, 30, "confirm")]
        [InlineData(6, 0, "confirm")]
        [InlineData(0, 60, "CONFIRM")]
        public void ValidatePurchase_WhenInputValidProductAndQuantity_ShouldReturnPositiveMessage(int quantity, decimal newBalance, string inputConfirm)
        {
            //Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            var productGatewayMock = new Mock<IProductGateway>();
            var ship = new Ship("ShipOne", 60);
            var requiredProduct = new Product(1, "Valid Product", "Requested Product", 20, 10, CategoryEnum.Tool);

            shipGatewayMock.Setup(p => p.GetShipList()).Returns([ship]);
            var paymentService = new PaymentService(shipGatewayMock.Object, productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("CONFIRM");

            //Act
            var result = paymentService.ValidatePurchase(requiredProduct, quantity);

            //Assert
            Assert.Equal($"You ordered {requiredProduct.Name} and your new balance is {newBalance}", result);
            shipGatewayMock.Verify(p => p.UpdateShip(It.IsAny<Ship>()), Times.Once);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Once);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("DENY")]
        [InlineData("deny")]
        [InlineData("")]
        [InlineData("AnoterInput")]

        public void ValidatePurchase_WhenInputDenyOnConfirmationOption_ShouldReturnCacenledMessage(string inputDeny)
        {
            //Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            var productGatewayMock = new Mock<IProductGateway>();
            var ship = new Ship("ShipOne", 60);
            var requiredProduct = new Product(1, "Valid Product", "Requested Product", 20, 10, CategoryEnum.Tool);

            shipGatewayMock.Setup(p => p.GetShipList()).Returns([ship]);
            var paymentService = new PaymentService(shipGatewayMock.Object, productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputDeny);

            //Act
            var result = paymentService.ValidatePurchase(requiredProduct, 2);

            //Assert
            Assert.Equal("You canceled the purchase", result);
            shipGatewayMock.Verify(p => p.UpdateShip(It.IsAny<Ship>()), Times.Never);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Fact]
        public void ValidatePurchase_WhenInputNegativeQuantity_ShouldReturnErrorMessage()
        {
            //Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            var productGatewayMock = new Mock<IProductGateway>();
            var ship = new Ship("ShipOne", 60);
            var requiredProduct = new Product(1, "Valid Product", "Requested Product", 20, 10, CategoryEnum.Tool);

            shipGatewayMock.Setup(p => p.GetShipList()).Returns([ship]);
            var paymentService = new PaymentService(shipGatewayMock.Object, productGatewayMock.Object);

            //Act
            var result = paymentService.ValidatePurchase(requiredProduct, -3);

            //Assert
            Assert.Equal("The quantity cannot be negative", result);
            shipGatewayMock.Verify(p => p.UpdateShip(It.IsAny<Ship>()), Times.Never);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void ValidatePurchase_WhenInputMoreQuantityOfProductThenExist_ShoulRetornAnErrorMessage()
        {
            //Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            var productGatewayMock = new Mock<IProductGateway>();
            var ship = new Ship("ShipOne", 60);
            var requiredProduct = new Product(1, "Valid Product", "Requested Product", 20, 10, CategoryEnum.Tool);

            shipGatewayMock.Setup(p => p.GetShipList()).Returns([ship]);
            var paymentService = new PaymentService(shipGatewayMock.Object, productGatewayMock.Object);

            //Act
            var result = paymentService.ValidatePurchase(requiredProduct, 30);

            //Assert
            Assert.Equal("More quantity then you can buy", result);
            shipGatewayMock.Verify(p => p.UpdateShip(It.IsAny<Ship>()), Times.Never);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void ValidatePurchase_WhenDontHaveEnothCreditsToBuyAProduct_ShouldReturnAnErrorMessa()
        {
            //Arrange
            var shipGatewayMock = new Mock<IShipGateway>();
            var productGatewayMock = new Mock<IProductGateway>();
            var ship = new Ship("ShipOne", 60);
            var requiredProduct = new Product(1, "Valid Product", "Requested Product", 20, 10, CategoryEnum.Tool);

            shipGatewayMock.Setup(p => p.GetShipList()).Returns([ship]);
            var paymentService = new PaymentService(shipGatewayMock.Object, productGatewayMock.Object);

            //Act
            var result = paymentService.ValidatePurchase(requiredProduct, 20);

            //Assert
            Assert.Equal("You could not afford these items!\n" +
                $"Your balance is \\{ship.Credits:F2}. Total cost of these items is \\200,00", result);
            shipGatewayMock.Verify(p => p.UpdateShip(It.IsAny<Ship>()), Times.Never);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Never);
        }
    }
}
