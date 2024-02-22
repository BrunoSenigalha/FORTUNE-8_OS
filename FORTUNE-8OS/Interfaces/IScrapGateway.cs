using FORTUNE_8OS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTUNE_8OS.Interfaces
{
    public interface IScrapGateway
    {
        void PostScrap(Scrap scrap);
        IEnumerable<Scrap> GetScraps();

    }
}
