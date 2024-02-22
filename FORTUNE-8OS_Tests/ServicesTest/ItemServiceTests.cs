using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Exceptions;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using FORTUNE_8OS_Tests.Utilitaries;
using Moq;
using static FORTUNE_8OS_Tests.ServicesTest.ScrapServiceTest;

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

        [Theory]
        [InlineData("ValidValue", "CONFIRM", "NewValidName", "12,5")]
        [InlineData("validvaLUE", "confirm", "NewValidName", "12,5")]
        public void UpdateItemFromDatabase_WhenValidNameAndConfirm_ShouldReturnConfirmationMessage(string inputName,
            string inputOption, string inputNewName, string inputDecimal)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(1, "ValidValue", 10.5M) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, inputOption, inputNewName, inputDecimal);

            //Act
            var result = itemService.UpdateItem();

            //Assert
            Assert.Equal("Item NewValidName updated successfully", result);
            itemGatewayMock.Verify(g => g.UpdateItem(It.IsAny<Item>()), Times.Once);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();

        }

        [Theory]
        [InlineData("ValidValue", "DENY")]
        [InlineData("ValidValue", "AnotherValue")]
        [InlineData("ValidValue", "")]

        public void UpdateItemFromDatabase_WhenValidNameAndDenyOrAnotherValue_ShouldReturnDeniedMessage(string inputName, string option)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(1, "ValidValue", 10.5M) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, option);

            //Act
            var result = itemService.UpdateItem();

            //Assert
            Assert.Equal("You denied the change", result);
            itemGatewayMock.Verify(g => g.UpdateItem(It.IsAny<Item>()), Times.Never);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Theory]
        [InlineData("InvalidName")]
        [InlineData("")]
        public void UpdateItemFromDatabase_WhenInvalidName_ShouldReturnNotFoundMessage(string inputName)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(1, "ValidValue", 10.5M) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName);

            //Act
            var result = itemService.UpdateItem();

            //Assert
            Assert.Equal("Item not found", result);
            itemGatewayMock.Verify(g => g.UpdateItem(It.IsAny<Item>()), Times.Never);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Theory]
        [InlineData("ValidValue", "CONFIRM")]
        [InlineData("validvalue", "Confirm")]
        [InlineData("VALIDVALUE", "confirm")]
        public void DeleteItemFromDatabase_WhenValidNameAndConfirm_ShouldReturnConfirmationMessage(string inputName, string inputOption)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(1, "ValidValue", 10.5M) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, inputOption);

            //Act
            var result = itemService.DeleteItem();

            //Assert
            Assert.Equal("Item ValidValue deleted", result);
            itemGatewayMock.Verify(d => d.DeleteItem(It.IsAny<Item>()), Times.Once);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Theory]
        [InlineData("ValidValue", "DENY")]
        [InlineData("ValidValue", "AnotherOption")]
        [InlineData("ValidValue", "")]
        public void DeleteItemFromDatabase_WhenValidNameAndDeny_ShouldReturnDeniedMessage(string inputName, string option)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(1, "ValidValue", 10.5M) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, option);

            //Act
            var result = itemService.DeleteItem();

            //Assert
            Assert.Equal("You denied the delectation", result);
            itemGatewayMock.Verify(d => d.DeleteItem(It.IsAny<Item>()), Times.Never);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Theory]
        [InlineData("InvalidValue")]
        [InlineData("")]
        public void DeleteItemFromDatabase_WhenInvalidName_ShouldReturnNotFoundMessage(string inputName)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(1, "ValidValue", 10.5M) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName);

            //Act
            var result = itemService.DeleteItem();

            //Assert
            Assert.Equal("Item not found", result);
            itemGatewayMock.Verify(d => d.DeleteItem(It.IsAny<Item>()), Times.Never);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Fact]
        public void ReadItemsFromFile_WhenPathIsValidAndHasFourItems_ShouldReturnConfirmedMessage()
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item>());
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("C:\\Users\\brsen\\Desktop\\Curso C#\\list_of_items.txt");

            //Act
            var result = itemService.ReadItemsFromFile();

            //Asset
            Assert.Equal("Items were read successfully", result);
            itemGatewayMock.Verify(r => r.PostItem(It.IsAny<Item>()), Times.Exactly(4));
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Theory]
        [InlineData("Key", 3)]
        [InlineData("key", 5)]
        public void ReadItemsFromFile_WhenPathIsValidAndHasOneExistentItem_ShouldReturnConfirmedMessageAndPostItemThreeTimes(string inputName, decimal inputValue)
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item> { new Item(inputName, inputValue) });
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("C:\\Users\\brsen\\Desktop\\Curso C#\\list_of_items.txt");

            //Act
            var result = itemService.ReadItemsFromFile();

            //Asset
            Assert.Equal("Items were read successfully", result);
            itemGatewayMock.Verify(r => r.PostItem(It.IsAny<Item>()), Times.Exactly(3));
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Fact]
        public void ReadItemsFile_WhenPathIsInvalid_ShouldReturnErrorMessage()
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item>());
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("C:\\");

            //Act
            var result = itemService.ReadItemsFromFile();

            //Asset
            Assert.Equal("Access to the path 'C:\\' is denied.", result);
            itemGatewayMock.Verify(r => r.PostItem(It.IsAny<Item>()), Times.Never);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        [Fact]
        public void ReadItemsFile_WhenPathIsNotInformed_ShouldReturnErrorMessage()
        {
            //Arrange
            var itemGatewayMock = new Mock<IItemGateway>();
            itemGatewayMock.Setup(g => g.GetItemList()).Returns(new List<Item>());
            var itemService = new ItemService(itemGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture("");

            //Act
            var result = itemService.ReadItemsFromFile();

            //Asset
            Assert.Equal("Informe the path needed", result);
            itemGatewayMock.Verify(r => r.PostItem(It.IsAny<Item>()), Times.Never);
            consoleOutputCapture.Dispose();
            consoleInputCapture.Dispose();
        }

        

        

    }
}
