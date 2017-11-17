using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SystemContracts.ServiceContracts
{
    public interface IInternalServiceEngine
    {
         Task<IEngineServiceRS> ProcessAsync(IEngineServiceRQ engineServiceRQ);
    }
}
