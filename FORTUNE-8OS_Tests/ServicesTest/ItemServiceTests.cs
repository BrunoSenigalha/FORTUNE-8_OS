using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Exceptions;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS_Tests.ServicesTest
{
    public class ItemServiceTests
    {
        [Fact]
        public void GetItems_WhenDoesNotExist_ShoulReturnNull()
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(p => p.GetItemList()).Returns((IEnumerable<Item>)null);

            var itemService = new ItemService(itemGatewayMock.Object);

            //Act
            var result = itemService.GetItems();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetItems_ReturnTheItemData_WhenIsNotNull()
        {
            //Arrange
            var expectedItem = new Item("ItemTest", 800M);

            var itemGetwayMock = new Mock<IItemGateway>();
            itemGetwayMock.Setup(p => p.GetItemList()).Returns(new List<Item> { expectedItem });

            var itemService = new ItemService(itemGetwayMock.Object);

            //Act
            var result = itemService.GetItems().FirstOrDefault();

            //Assert
            Assert.Equal("ItemTest", result.Name);
        }

        [Fact]
        public void CreateNewItemObject_ShouldReturnValidItem()
        {
            // Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("ValidName", "10,5");

            // Act
            var result = itemService.CreateNewItemObject();

            // Assert
            Assert.Equal("ValidName", result.Name);
            Assert.Equal(10.5m, result.Credits);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Fact]
        public void CreateNewItemObject_WhenInvalidCredits_ShouldReturnThrowException()
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            var itemService = new ItemService(itemGatewayMock.Object);
            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("ValidName", "NotANumber");

            //Act
            var exception = Assert.Throws<InvalidOperationException>(() => itemService.CreateNewItemObject());

            //Assert
            Assert.Equal("Wrong value for credits, please type again.", exception.Message);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Theory]
        [InlineData("ValidName", "-5")]
        [InlineData("ValidName", "0")]
        public void CreateNewItemObject_WhenNegativeOrIqualZeroCredits_ShouldReturnDomainExceptionValidation(
            string inputName, string inputCredits)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            var itemService = new ItemService(itemGatewayMock.Object);
            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, inputCredits);

            //Act
            var exception = Assert.Throws<DomainExceptionValidation>(() => itemService.CreateNewItemObject());

            //Assert
            Assert.Equal("The field credits can't be less than or iqual zero.", exception.Message);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Fact]
        public void CreateNewItemObject_WhenInvalidName_ShouldReturnDomainExceptionValidation()
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            var itemService = new ItemService(itemGatewayMock.Object);
            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("", "10,5");

            //Act
            var exception = Assert.Throws<DomainExceptionValidation>(() => itemService.CreateNewItemObject());

            //Assert
            Assert.Equal("The field name can't be empty.", exception.Message);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Fact]
        public void UpdateItemFromDatabase_WhenValidNameAndConfirm_ShouldReturnConfirmationMessage()
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(1, "ValidValue", 10.5M) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("ValidValue", "CONFIRM", "NewValidName", "12,5");

            //Act
            var result = itemService.UpdateItem();

            //Assert
            Assert.Equal("Item NewValidName updated successfully", result);
            itemGatewayMock.Verify(g => g.UpdateItem(It.IsAny<Item>()), Times.Once);

        }
        private class ConsoleOutputCapture : IDisposable
        {
            private readonly System.IO.StringWriter stringWriter;
            private readonly System.IO.TextWriter originalOutput;

            public ConsoleOutputCapture()
            {
                stringWriter = new System.IO.StringWriter();
                originalOutput = Console.Out;
                Console.SetOut(stringWriter);
            }

            public void Dispose()
            {
                Console.SetOut(originalOutput);
                stringWriter.Dispose();
            }
        }

        private class ConsoleInputCapture : IDisposable
        {
            private readonly System.IO.StringReader stringReader;
            private readonly System.IO.TextReader originalInput;

            public ConsoleInputCapture(params string[] inputLines)
            {
                stringReader = new System.IO.StringReader(string.Join(Environment.NewLine, inputLines));
                originalInput = Console.In;
                Console.SetIn(stringReader);
            }

            public void Dispose()
            {
                Console.SetIn(originalInput);
                stringReader.Dispose();
            }
        }

    }
}
