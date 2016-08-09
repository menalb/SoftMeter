using System.Web.Mvc;
using SoftMeter.UIConsole.Models.TopUpViewModels;
using SoftMeter.UIConsole.Services;

namespace SoftMeter.UIConsole.Controllers
{
    public class TopUpController : Controller
    {
        private readonly ITopUpSender _topUpSender;

        public TopUpController()
        {
            _topUpSender = new TopUpSender("localhost", "SoftMeter");
        }

        public TopUpController(ITopUpSender topUpSender)
        {
            _topUpSender = topUpSender;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TopUp(TopUpRequestViewModel request)
        {
            _topUpSender.TopUpAsync(new Models.TopUpRequest { WseId = request.WseId, Amount = request.Amount, RequesterId = "123456" });
            return RedirectToAction(nameof(TopUpConfirmation), request);
        }

        public ActionResult TopUpConfirmation(TopUpRequestViewModel request)
        {
            return View(request);
        }
    }
}