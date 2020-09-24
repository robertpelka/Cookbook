using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    public class Przepis
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        public string Description { get; set; }
        public string[] Ingredients { get; set; }
        public string HowTo { get; set; }
        public int[] Ratings { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Przepis>(this);
    }
}
