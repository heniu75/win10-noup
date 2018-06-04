using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Win10NoUp.Library.Hosts;

namespace Win10NoUp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMyAutofacSingletonService _autofacSingletonService;
        private readonly IMyAutofacTransientService _autofacTransientService;
        private readonly IMyCoreSingletonService _coreSingletonService;
        private readonly IMyCoreTransientService _coreTransientService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMyAutofacSingletonService autofacSingletonService,
            IMyAutofacTransientService autofacTransientService,
            IMyCoreSingletonService coreSingletonService,
            IMyCoreTransientService coreTransientService,
            ILogger<HomeController> logger)
        {
            _autofacSingletonService = autofacSingletonService;
            _autofacTransientService = autofacTransientService;
            _coreSingletonService = coreSingletonService;
            _coreTransientService = coreTransientService;
            _logger = logger;
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
