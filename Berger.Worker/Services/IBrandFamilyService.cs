using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Worker.Services
{
    public interface IBrandFamilyService
    {
         Task<int> GetBrandFamilyData();
    }
}
