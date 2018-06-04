using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Win10NoUp.Library.Hosts;

namespace Win10NoUp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMyAutofacSingletonService _autofacSingletonService;
        private readonly IMyAutofacTransientService _autofacTransientService;
        private readonly IMyCoreSingletonService _coreSingletonService;
        private readonly IMyCoreTransientService _coreTransientService;

        public HomeController(IMyAutofacSingletonService autofacSingletonService,
            IMyAutofacTransientService autofacTransientService,
            IMyCoreSingletonService coreSingletonService,
            IMyCoreTransientService coreTransientService)
        {
            _autofacSingletonService = autofacSingletonService;
            _autofacTransientService = autofacTransientService;
            _coreSingletonService = coreSingletonService;
            _coreTransientService = coreTransientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
