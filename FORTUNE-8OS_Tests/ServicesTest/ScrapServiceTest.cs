using FORTUNE_8OS.Interfaces;
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
        [InlineData("Valid Value 2")]
        public void InputScrapInfo_WhenInformNameAndQuantityAndNoOption_ShouldReturnATotalMessage(string inputData)
        {
            var scrapGatewayMock = new Mock<IScrapGateway>();
        }

        
    }
}
