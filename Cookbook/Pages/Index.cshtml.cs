using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cookbook.Models;
using Cookbook.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cookbook.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public JsonFilePrzepisService PrzepisService;
        public IEnumerable<Przepis> Przepisy { get; private set; }

        public IndexModel(ILogger<IndexModel> logger,
            JsonFilePrzepisService przepisService)
        {
            _logger = logger;
            PrzepisService = przepisService;
        }

        public void OnGet()
        {
            Przepisy = PrzepisService.GetPrzepisy();
        }
    }
}
