using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataServiceInterface.Models;
using Swashbuckle.Application;
using System.Web.Http.ModelBinding;
using System.Text;
using DataServiceInterface.Controllers.Functions;

namespace DataServiceInterface.Controllers
{

    [RoutePrefix("api")]
    public class CPUTipusInterfaceController : ApiController
    {
        [HttpPost]
        [Route("getCPUTipus/v1")]
        public List<CPUTipusInterfaceOutput> getCPUTipus
            ([FromBody] CPUTipusInterfaceInput InterfaceInputData)
        {


            string DataBaseCommand = System.Configuration.ConfigurationManager.AppSettings["GV_DBCommandGetCPUTipus_GV"];
            var MicroserviceOutputControllerInstance = new MicroserviceOutputController<CPUTipusInterfaceOutput, CPUTipusInterfaceInput>();
            return MicroserviceOutputControllerInstance.Post(InterfaceInputData, DataBaseCommand);

        }
    }
}