using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cookbook.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.AspNetCore.Hosting;
using Cookbook.Services;

namespace Cookbook.Pages
{
    public class Nowy_przepisModel : PageModel
    {
        [BindProperty] public IFormFile Image { get; set; }
        public void OnGet()
        {
        }
        public Nowy_przepisModel(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public IActionResult OnPost()
        {
            var allGood = true;

            var ID = Guid.NewGuid().ToString().Substring(0, 5);
            var Category = Request.Form["Category"];
            var PrzepisTitle = Request.Form["PrzepisTitle"];
            var Description = Request.Form["Description"];
            var Ingredients = Request.Form["Ingredients"];
            var HowTo = Request.Form["HowTo"];


            if (PrzepisTitle.Equals(""))
            {
                allGood = false;
                ViewData["Error1"] = "Pole Tytuł nie może pozostać puste!";
            }
            if (Description.Equals(""))
            {
                allGood = false;
                ViewData["Error2"] = "Pole Opis nie może pozostać puste!";
            }
            if (Ingredients.Equals(""))
            {
                allGood = false;
                ViewData["Error3"] = "Pole Składniki nie może pozostać puste!";
            }
            if (HowTo.Equals(""))
            {
                allGood = false;
                ViewData["Error4"] = "Pole Sposób przygotowania nie może pozostać puste!";
            }

            if (this.Image != null && allGood==true)
            {
                var fileName = GetUniqueName(this.Image.FileName);
                ViewData["Imagename"] = fileName;
                var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "data", "images", fileName);
                var fs = new FileStream(filePath, FileMode.Create);
                this.Image.CopyTo(fs);
                fs.Close();
            }
            else
            {
                allGood = false;
                ViewData["Error"] = "Musisz dodać zdjęcie!";
            }

            if (allGood)
            {
                string[] Ingr = Ingredients.First().Split("; ", StringSplitOptions.RemoveEmptyEntries);
                if (Ingr[0].Contains(';'))
                    Ingr = Ingredients.First().Split(';', StringSplitOptions.RemoveEmptyEntries);

                Przepis przepis = new Przepis();
                przepis.ID = ID;
                przepis.Category = Category;
                przepis.Title = PrzepisTitle;
                przepis.Image = (string)ViewData["Imagename"];
                przepis.Description = Description;
                przepis.Ingredients = Ingr;
                przepis.HowTo = HowTo;

                JsonFilePrzepisService service = new JsonFilePrzepisService(WebHostEnvironment);
                service.AddPrzepis(przepis);

                ViewData["Thanks"] = "Przepis został dodany, dziękujemy!";
            }
            else
            {
                ViewData["ID"] = ID;
                ViewData["Category"] = Category;
                ViewData["PrzepisTitle"] = PrzepisTitle;
                ViewData["Description"] = Description;
                ViewData["Ingredients"] = Ingredients;
                ViewData["HowTo"] = HowTo;
            }

            return Page();
        }

        private string GetUniqueName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
    }
}