using FORTUNE_8OS.Entities;
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
    public class ScrapServiceTest
    {
        [Theory]
        [InlineData("Valid Value 2", 10)]
        [InlineData("Valid Value item 10", 50)]
        [InlineData("valid VALUE item 10", 50)]
        [InlineData("Valid 3", 15)]

        public void PostScrapItems_WhenInputValueData_ShouldReturnAStringAndDecimal(string inputValue, decimal resultAccount)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(p => p.GetItemList()).Returns(new List<Item> { new Item(1, "Valid Value", 5M), new Item(2, "Valid Value Item", 5M), new Item(3, "Valid", 5M) });

            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(p => p.GetShipList()).Returns(new List<Ship> { new Ship(1, "ShipOne", 60M) });

            var scrapGatewayMock = new Mock<IScrapGateway>();
            var scrapServices = new ScrapService(scrapGatewayMock.Object, itemGatewayMock.Object, shipGatewayMock.Object);

            //Act
            (string result1, decimal result2) = scrapServices.PostScrapItems(inputValue);

            //Arrange
            Assert.Equal("Successfully scraped!\n", result1);
            Assert.Equal(resultAccount, result2);
            scrapGatewayMock.Verify(p => p.PostScrap(It.IsAny<Scrap>()), Times.Once);
        }

        [Theory]
        [InlineData("Invalid Value 2", 0)]
        [InlineData("1", 0)]
        [InlineData(" 1", 0)]

        public void PostScrapItems_WhenInputInvalidValueData_ShouldReturnNotFoundMessage(string inputValue, decimal resultAccount)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(p => p.GetItemList()).Returns(new List<Item> { new Item(1, "Valid Value", 5M), new Item(2, "Valid Value Item", 5M), new Item(3, "Valid", 5M) });

            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(p => p.GetShipList()).Returns(new List<Ship> { new Ship(1, "ShipOne", 60M) });

            var scrapGatewayMock = new Mock<IScrapGateway>();
            var scrapServices = new ScrapService(scrapGatewayMock.Object, itemGatewayMock.Object, shipGatewayMock.Object);

            //Act
            (string result1, decimal result2) = scrapServices.PostScrapItems(inputValue);

            //Arrange
            Assert.Equal("Item not found!\n", result1);
            Assert.Equal(resultAccount, result2);
            scrapGatewayMock.Verify(p => p.PostScrap(It.IsAny<Scrap>()), Times.Never);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(" ", 0)]
        public void PostScrapItems_WhenDontInputNumberOrInputNull_ShouldReturnWrongValueInformed(string inputValue, decimal resultAccount)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(p => p.GetItemList()).Returns(new List<Item> { new Item(1, "Valid Value", 5M), new Item(2, "Valid Value Item", 5M), new Item(3, "Valid", 5M) });

            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(p => p.GetShipList()).Returns(new List<Ship> { new Ship(1, "ShipOne", 60M) });

            var scrapGatewayMock = new Mock<IScrapGateway>();
            var scrapServices = new ScrapService(scrapGatewayMock.Object, itemGatewayMock.Object, shipGatewayMock.Object);

            //Act
            (string result1, decimal result2) = scrapServices.PostScrapItems(inputValue);

            //Arrange
            Assert.Equal("Wrong value informed!\n", result1);
            Assert.Equal(resultAccount, result2);
            scrapGatewayMock.Verify(p => p.PostScrap(It.IsAny<Scrap>()), Times.Never);
        }

        [Theory]
        [InlineData("Valid Value 2", "NO")]
        [InlineData("valid 2", "NO")]
        public void InputScrapInfo_WhenInputRightValueDataAndNotContiue_ShouldReturnAConfirmationMessage(string inputData, string inputOption)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(p => p.GetItemList()).Returns(new List<Item> { new Item(1, "Valid Value", 5M), new Item(2, "Valid Value Item", 5M), new Item(3, "Valid", 5M) });

            var shipGatewayMock = new Mock<IShipGateway>();
            shipGatewayMock.Setup(p => p.GetShipList()).Returns(new List<Ship> { new Ship(1, "ShipOne", 60M) });

            var scrapGatewayMock = new Mock<IScrapGateway>();
            var scrapServices = new ScrapService(scrapGatewayMock.Object, itemGatewayMock.Object, shipGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputData, inputOption);

            //Act
            var result = scrapServices.ImputScrapInfo();

            //Arrange
            Assert.Equal("Total payment for your services: 10", result);
            scrapGatewayMock.Verify(p => p.PostScrap(It.IsAny<Scrap>()), Times.Once);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }
    }
}