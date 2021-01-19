namespace AdsPortal.WebPortal.Controllers
{
    using AdsPortal.WebPortal.Configurations;
    using AdsPortal.WebPortal.Models.Manifest;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [Route("")]
    public class ManifestController : BaseApiController
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        public ManifestController(IOptions<ApplicationConfiguration> applicationConfiguration) : base()
        {
            _applicationConfiguration = applicationConfiguration.Value;
        }

        [HttpGet("manifest.json")]
        public IActionResult GetManifest()
        {
            ApplicationManifest manifest = new()
            {
                Name = _applicationConfiguration.Name,
                ShortName = _applicationConfiguration.Name,
                StartUrl = "/",
                Display = "standalone",
                BackgroundColor = "#080a0a",
                ThemeColor = "#00d5ff",
                Icons = new ApplicationManifestIcon[]
                {
                    new() { Src = "assets/images/icon-512px.png", Type = "image/png", Sizes =  "512x512" }
                }
            };

            return Ok(manifest);
        }
    }
}
