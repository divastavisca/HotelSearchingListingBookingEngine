using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SystemContracts.ServiceContracts
{
    public interface IEngineServiceProvider
    {
        Task<IEngineServiceRS> GetServiceRS(IEngineServiceRQ serviceRQ);
    }
}
