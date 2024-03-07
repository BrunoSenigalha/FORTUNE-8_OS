using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Entities.Enums;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using FORTUNE_8OS_Tests.Utilitaries;
using Moq;


namespace FORTUNE_8OS_Tests.ServicesTest
{
    public class ShoppingServiceTests
    {
        [Fact]
        public void ProcessInformation_WhenInputValidValueWithInfo_ShouldReturnProductInfo()
        {
            //Arrange
            var expectedProduct = new Product(1, "Valid Product", "Description", 20, 5, CategoryEnum.Tool);
            string productName = "Valid Product Info";
            List<Product> productsList = new List<Product> { expectedProduct };
            ShoppingService service = new ShoppingService();

            //Act
            var result = service.ProcessInformation(productName, productsList);

            //Assert
            Assert.Equal($"{expectedProduct.Description}", result);
        }

        //[Fact]
        //public void ProcessInformation_WhenInputValidValueWithQuantity_ShouldReturnConfirmMessage()
        //{
        //    //Arrange
        //    var expectedProduct = new Product(1, "Valid Product", "Description", 20, 5, CategoryEnum.Tool);
        //    string productName = "Valid Product 2";
        //    List<Product> productsList = new List<Product> { expectedProduct };
        //    ShoppingService service = new ShoppingService();

        //    //Act
        //    var result = service.ProcessInformation(productName, productsList);

        //    //Assert
        //    Assert.Equal($"Message", result);
        //}
    }
}
