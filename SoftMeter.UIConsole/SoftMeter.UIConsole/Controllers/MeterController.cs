using SoftMeter.UIConsole.Models;
using SoftMeter.UIConsole.Services;
using System.Web.Mvc;

namespace SoftMeter.UIConsole.Controllers
{
    public class MeterController : Controller
    {

        private readonly IMeterService _meterService;

        public MeterController()
        {
            _meterService = new MeterService("localhost");
        }

        // GET: Meter
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StartNewMeter(StartNewMeterViewModel newMeter)
        {
            _meterService.StartNew(new Models.Messages.StartNewMeterRequest { WseId = newMeter.MeterId });
            return RedirectToAction(nameof(StartNewMeterConfirmation), newMeter);
        }

        public ActionResult StartNewMeterConfirmation(StartNewMeterViewModel newMeter)
        {
            return View(newMeter);
        }
    }
}