using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
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
    }
}
