using Cookbook.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cookbook.Services
{
    public class JsonFilePrzepisService
    {
        public JsonFilePrzepisService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "przepisy.json"); }
        }

        public IEnumerable<Przepis> GetPrzepisy()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return System.Text.Json.JsonSerializer.Deserialize<Przepis[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void AddRating(string przepisID, int rating)
        {
            var przepisy = GetPrzepisy();

            var query = przepisy.First(x => x.ID == przepisID);

            if (query.Ratings == null)
            {
                query.Ratings = new int[] { rating };
            }
            else
            {
                var ratings = query.Ratings.ToList();
                ratings.Add(rating);
                query.Ratings = ratings.ToArray();
            }

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                System.Text.Json.JsonSerializer.Serialize<IEnumerable<Przepis>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                przepisy
                );
            }
        }

        public IEnumerable<Przepis> GetPrzepisyFromList(List<Przepis> przepisy)
        {
            return przepisy;
        }

        public void AddPrzepis(Przepis przepis)
        {
            var first = GetPrzepisy().ToList();
            var sec = System.Text.Json.JsonSerializer.Serialize<Przepis>(przepis);
            var second = System.Text.Json.JsonSerializer.Deserialize<Przepis>(sec);
            first.Add(second);
            var przepisy = GetPrzepisyFromList(first);

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                System.Text.Json.JsonSerializer.Serialize<IEnumerable<Przepis>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                przepisy
                );
            }
        }
    }
}
