using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniQR2.Models;
using UniQR2.Providers;

namespace UniQR2.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly ITemplateHelper _templateHelper;

        public TemplatesController(ITemplateHelper helper)
        {
            _templateHelper = helper;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _templateHelper.GetTemplateHtmlAsStringAsync<List<Floor>>("Templates/Content", new List<Floor> {  });
            return View("Index", response);
        }
    }
}
