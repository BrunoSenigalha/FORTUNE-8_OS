using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Exceptions;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using FORTUNE_8OS_Tests.Utilitaries;
using Moq;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;

namespace FORTUNE_8OS_Tests.ServicesTest
{
    public class ProductServicesTests
    {
        [Fact]
        public void GetProducts_WhenExistProducts_ShouldReturnAListOfProducts()
        {
            //Arrange

            var expectedProduct = new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0) };
            var procutMockGateway = new Mock<IProductGateway>();
            procutMockGateway.Setup(p => p.GetProducts()).Returns(expectedProduct);

            var productServices = new ProductServices(procutMockGateway.Object);

            //Act
            var result = productServices.GetProducts();

            //Assert
            Assert.Equal(expectedProduct, result);
        }

        [Fact]
        public void GetProducts_WhenDoesNotExistProducts_ShouldReturnEmpty()
        {
            var procutMockGateway = new Mock<IProductGateway>();
            procutMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product>());
            var productServices = new ProductServices(procutMockGateway.Object);

            //Act
            var result = productServices.GetProducts();

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("Valid Product")]
        [InlineData("valid product")]
        [InlineData("VALID PRODUCT")]
        public void FindProduct_WhenTheProductNameInputedExists_ShouldReturnTheProduct(string inputProductName)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });

            var productServices = new ProductServices(productMockGateway.Object);

            //Act
            var result = productServices.FindProduct(inputProductName);

            //Assert
            Assert.NotNull(result);
            productMockGateway.Verify(p => p.GetProducts(), Times.Once);
        }

        [Theory]
        [InlineData("Invalid Product")]
        [InlineData("")]
        [InlineData(" ")]
        public void FindProduct_WhenTheProductNameInputedDoenstExist_ShouldReturnNull(string inputProductName)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });

            var productServices = new ProductServices(productMockGateway.Object);

            //Act
            var result = productServices.FindProduct(inputProductName);

            //Assert
            Assert.Null(result);
            productMockGateway.Verify(p => p.GetProducts(), Times.Once);
        }

        [Theory]
        [InlineData("First Valid Product", "New Product Description", "10", "12.5", "Tool")]
        [InlineData("Second Valid Product", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("Third Valid Product", "New Product Description", "10", "12.5", "Decor")]
        public void CreadNewProduct_WhenInputValidValue_ShouldReturnPositiveMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductServices(productMockGateway.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal($"The product: {productName} was successfully created", result);
            productMockGateway.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Once);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Valid Product", "New Product Description", "10", "12.5", "Tool")]
        [InlineData("Another Product", "New Product Description", "10", "12.5", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputtingAnExistingName_ShouldReturnProducDoesExistMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductServices(productMockGateway.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal($"The product {productName} does exist", result);
            productMockGateway.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
        }

        [Theory]
        [InlineData(" va", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("v", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("    va   ", "New Product Description", "10", "12.5", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputtingAnNameLassThanThreeCharacters_ShouldReturnAnDomainExceptionValidation(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductServices(productMockGateway.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var exception = Assert.Throws<DomainExceptionValidation>(() => productServices.CreateNewProduct()) ;

            //Assert
            Assert.Equal("The name must have a minimum of three characters.", exception.Message);
            productMockGateway.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
        }

        [Theory]
        [InlineData("", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("    ", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("        ", "New Product Description", "10", "12.5", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputtingAnEmptyNameField_ShouldReturnAnDomainExceptionValidation(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductServices(productMockGateway.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var exception = Assert.Throws<DomainExceptionValidation>(() => productServices.CreateNewProduct());

            //Assert
            Assert.Equal("The field name can't be empty.", exception.Message);
            productMockGateway.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
        }

        [Theory]
        [InlineData("Fist Valid Product", "New Product Description", "Wrong Value", "12.5", "Tool")]
        [InlineData("Second Valid Product", "New Product Description", "10", "Wrong Value", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputWrongNumeralValue_ShouldReturnAFormatErrorMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductServices(productMockGateway.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal("Format error: The input string 'Wrong Value' was not in a correct format.", result);
            productMockGateway.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
        }

        [Theory]
        [InlineData("Fist Valid Product", "New Product Description", "10", "12.5", "")]
        public void CreateNewProduct_WhenInputEmptyEnumValue_ShouldReturnAFormatErrorMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductServices(productMockGateway.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal("Argument error: Value cannot be null. (Parameter 'value')", result);
            productMockGateway.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
        }

        [Theory]
        [InlineData("Fist Valid Product", "New Product Description", "10", "12.5", "Invalid Enum")]
        public void CreateNewProduct_WhenInputWrongEnumValue_ShouldReturnAFormatErrorMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productMockGateway = new Mock<IProductGateway>();
            productMockGateway.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductServices(productMockGateway.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal($"Argument error: Requested value '{category}' was not found.", result);
            productMockGateway.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
        }
    }
}