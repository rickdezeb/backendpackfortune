using Microsoft.AspNetCore.Http;

namespace Packfortune.API
{
    public class CreateCrateRequest
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public IFormFile Picture { get; set; }
    }
}
