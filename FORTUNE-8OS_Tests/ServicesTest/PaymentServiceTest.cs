using FORTUNE_8OS.Entities;
using FORTUNE_8OS.Interfaces;
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
        [Fact]
        public void ValidatePurchase_WhenInputValueProductAndQuantity_ShouldReturnPositiveMessage()
        {
            var shipGatewayMock = new Mock<IShipGateway>();
            var productGatewayMock = new Mock<IProductGateway>();
            var ship = new Ship("ShipOne", 60);
            shipGatewayMock.Setup(p => p.GetShipList()).Returns([ship]);


        }
    }
}
