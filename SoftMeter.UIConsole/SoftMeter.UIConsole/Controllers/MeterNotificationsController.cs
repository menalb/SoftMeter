using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SoftMeter.UIConsole.Hubs;
using SoftMeter.UIConsole.Models;
using System.Web.Http;

namespace SoftMeter.UIConsole.Controllers
{
    [RoutePrefix("api")]
    public class MeterNotificationsController : ApiController
    {
        [Route("Incoming")]
        [HttpPost]
        public IHttpActionResult MeterNotification(SoftMeterNotification notification)
        {
            GetMessageHub().Clients.Group("Meter" + notification.MeterId).notify("App", notification);

            return Ok();
        }

        private IHubContext GetMessageHub()
        {
            return GlobalHost.ConnectionManager.GetHubContext<MeterHub>();
        }
    }
}