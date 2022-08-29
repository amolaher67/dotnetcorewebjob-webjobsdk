using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebJob
{
    public class Functions
    {
        private readonly someRepo _someRepo;
        public Functions(ISomeRepo someRepo)
        {
            _someRepo = someRepo;
        }

        public async Task ProcessWorkItem_ServiceBus([ServiceBusTrigger("rm2bcp", Connection = "ServiceBusConnection")] string queueItem, ILogger logger)
        {
            var wamMessage = JsonConvert.DeserializeObject<somecalss>(queueItem);

            //process messahere here
        }
    }
}
