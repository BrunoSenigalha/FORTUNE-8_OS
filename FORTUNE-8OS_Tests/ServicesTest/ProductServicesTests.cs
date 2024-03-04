using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Entities.Enums;
using FORTUNE_8OS.Exceptions;
using FORTUNE_8OS.Interfaces;
using FORTUNE_8OS.Services;
using FORTUNE_8OS_Tests.Utilitaries;
using Moq;
using System.Diagnostics;
using System.Numerics;
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
                "Valid Product", "Valid Product Description", 20, 15.5M, CategoryEnum.ShipUpgrade) };
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(expectedProduct);

            var productServices = new ProductService(productGatewayMock.Object);

            //Act
            var result = productServices.GetProducts();

            //Assert
            Assert.Equal(expectedProduct, result);
        }

        [Fact]
        public void GetProducts_WhenDoesNotExistProducts_ShouldReturnEmpty()
        {
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product>());
            var productServices = new ProductService(productGatewayMock.Object);

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
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });

            var productServices = new ProductService(productGatewayMock.Object);

            //Act
            var result = productServices.FindProduct(inputProductName);

            //Assert
            Assert.NotNull(result);
            productGatewayMock.Verify(p => p.GetProducts(), Times.Once);
        }

        [Theory]
        [InlineData("Invalid Product")]
        [InlineData("")]
        [InlineData(" ")]
        public void FindProduct_WhenTheProductNameInputedDoenstExist_ShouldReturnNull(string inputProductName)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });

            var productServices = new ProductService(productGatewayMock.Object);

            //Act
            var result = productServices.FindProduct(inputProductName);

            //Assert
            Assert.Null(result);
            productGatewayMock.Verify(p => p.GetProducts(), Times.Once);
        }

        [Theory]
        [InlineData("First Valid Product", "New Product Description", "10", "12.5", "Tool")]
        [InlineData("Second Valid Product", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("Third Valid Product", "New Product Description", "10", "12.5", "Decor")]
        public void CreadNewProduct_WhenInputValidValue_ShouldReturnPositiveMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal($"The product: {productName} was successfully created", result);
            productGatewayMock.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Once);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Valid Product", "New Product Description", "10", "12.5", "Tool")]
        [InlineData("Another Product", "New Product Description", "10", "12.5", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputtingAnExistingName_ShouldReturnProducDoesExistMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal($"The product {productName} does exist", result);
            productGatewayMock.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData(" va", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("v", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("    va   ", "New Product Description", "10", "12.5", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputtingAnNameLassThanThreeCharacters_ShouldReturnAnDomainExceptionValidation(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var exception = Assert.Throws<DomainExceptionValidation>(() => productServices.CreateNewProduct());

            //Assert
            Assert.Equal("The name must have a minimum of three characters.", exception.Message);
            productGatewayMock.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("    ", "New Product Description", "10", "12.5", "ShipUpgrade")]
        [InlineData("        ", "New Product Description", "10", "12.5", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputtingAnEmptyNameField_ShouldReturnAnDomainExceptionValidation(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var exception = Assert.Throws<DomainExceptionValidation>(() => productServices.CreateNewProduct());

            //Assert
            Assert.Equal("The field name can't be empty.", exception.Message);
            productGatewayMock.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Fist Valid Product", "New Product Description", "Wrong Value", "12.5", "Tool")]
        [InlineData("Second Valid Product", "New Product Description", "10", "Wrong Value", "ShipUpgrade")]
        public void CreateNewProduct_WhenInputWrongNumeralValue_ShouldReturnAFormatErrorMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal("Format error: The input string 'Wrong Value' was not in a correct format.", result);
            productGatewayMock.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Fist Valid Product", "New Product Description", "10", "12.5", "")]
        public void CreateNewProduct_WhenInputEmptyEnumValue_ShouldReturnAFormatErrorMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal("Argument error: Value cannot be null. (Parameter 'value')", result);
            productGatewayMock.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Fist Valid Product", "New Product Description", "10", "12.5", "Invalid Enum")]
        public void CreateNewProduct_WhenInputWrongEnumValue_ShouldReturnAFormatErrorMessage(string productName, string productDescription, string quantity, string price, string category)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(productName, productDescription, quantity, price, category);

            //Act
            var result = productServices.CreateNewProduct();

            //Assert
            Assert.Equal($"Argument error: Requested value '{category}' was not found.", result);
            productGatewayMock.Verify(p => p.PostProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Valid Product", "5")]
        [InlineData("  Valid Product    ", "5")]
        [InlineData("Valid Product", "0")]
        [InlineData("Valid Product", "")]
        public void UpdateProductQuantity_WhenInputValidNameAndQuantity_ShouldReturnConfirmationMessage(string inputName, string inputQuantity)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            var product = new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0);
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { product });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, inputQuantity);

            //Act
            var result = productServices.UpdateProductQuantity();

            //Assert
            Assert.Equal($"Product Valid Product quantity was update to: {product.Quantity}", result);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Once);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Invalid Product", "5")]
        [InlineData("", "5")]
        public void UpdateProductQuantity_WhenInputInvalidName_ShouldReturnNotFoundMessage(string inputName, string inputQuantity)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, inputQuantity);

            //Act
            var result = productServices.UpdateProductQuantity();

            //Assert
            Assert.Equal($"Product not found", result);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Valid Product", "-5")]
        public void UpdateProductQuantity_WhenInputLessThanZeroQuantity_ShouldReturnErrorMessage(string inputName, string inputQuantity)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, inputQuantity);

            //Act
            var exception = Assert.Throws<DomainExceptionValidation>(() => productServices.UpdateProductQuantity());

            //Assert
            Assert.Equal($"The quantity can't be less than 0", exception.Message);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Valid Product", "WrongValue")]
        [InlineData("Valid Product", " 6  7 ")]
        public void UpdateProductQuantity_WhenInputWrongValueQuantity_ShouldReturnErrorMessage(string inputName, string inputQuantity)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { new Product(1,
                "Valid Product", "Valid Product Description", 20, 15.5M, 0),
                new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0) });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName, inputQuantity);

            //Act
            var result = productServices.UpdateProductQuantity();

            //Assert
            Assert.Equal($"Format error: The input string '{inputQuantity}' was not in a correct format.", result);
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Fact]
        public void UpdateProdut_WhenInputProduct()
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            var product = new Product(1, "Valid Product", "Valid Product Description", 20, 15.5M, 0);
            var updatedProduct = new Product(1, "New Value", "Valid Product Description", 12, 15.5M, 0);
            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product>() { product });
            var productService = new ProductService(productGatewayMock.Object);

            //Act
            productService.UpdateProduct(updatedProduct);

            //Assert
            productGatewayMock.Verify(p => p.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Theory]
        [InlineData("Valid Product")]
        [InlineData("  Valid Product ")]
        public void DeleteProduct_WhenInputValidName_ShouldReturnPositiveMessage(string inputName)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            var product1 = new Product(1, "Valid Product", "Valid Product Description", 20, 15.5M, 0);

            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { product1 });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName);

            //Act
            var result = productServices.DeleteProduct();

            //Assert
            Assert.Equal($"Product {product1.Name}", result);
            productGatewayMock.Verify(p => p.DeleteProduct(It.IsAny<Product>()), Times.Once);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("Invalid Value")]
        [InlineData("15")]
        [InlineData(" ")]
        public void DeleteProduct_WhenInputInvalidValue_ShouldReturnNotFound(string inputName)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            var product1 = new Product(1, "Valid Product", "Valid Product Description", 20, 15.5M, 0);
            var product2 = new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0);

            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { product1, product2 });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName);

            //Act
            var result = productServices.DeleteProduct();

            //Assert
            Assert.Equal($"Product not found", result);
            productGatewayMock.Verify(p => p.DeleteProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }

        [Theory]
        [InlineData("")]
        public void DeleteProduct_WhenInputNullValue_ShouldReturnNameNotInformedMessage(string inputName)
        {
            //Arrange
            var productGatewayMock = new Mock<IProductGateway>();
            var product1 = new Product(1, "Valid Product", "Valid Product Description", 20, 15.5M, 0);
            var product2 = new Product(2, "Another Product", "Another Product Description", 20, 15.5M, 0);

            productGatewayMock.Setup(p => p.GetProducts()).Returns(new List<Product> { product1, product2 });
            var productServices = new ProductService(productGatewayMock.Object);

            var consoleOutputCapture = new ConsoleOutputCapture();
            var consoleInputCapture = new ConsoleInputCapture(inputName);

            //Act
            var result = productServices.DeleteProduct();

            //Assert
            Assert.Equal($"The name of the product was not informed", result);
            productGatewayMock.Verify(p => p.DeleteProduct(It.IsAny<Product>()), Times.Never);
            consoleInputCapture.Dispose();
            consoleOutputCapture.Dispose();
        }
    }
}