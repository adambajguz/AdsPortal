namespace AdsPortal.WebApi.Infrastructure.Mailing.Services.Renderer
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentEmail.Core.Interfaces;
    using Microsoft.Extensions.Logging;
    using RazorLight;

    /// <summary>
    /// Based on: https://github.com/lukencode/FluentEmail/blob/master/src/Renderers/FluentEmail.Razor/RazorRenderer.cs
    /// + doc https://github.com/toddams/RazorLight#embeddedresource-source
    ///
    /// After install Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
    /// FluentEmail.Razor stop working (not support core 3.0)
    /// So we remove reference to FluentEmail.Razor and use RazorLight >= 2.0.0-rc3 directly and PreMailer.Net
    /// https://github.com/lukencode/FluentEmail/issues/184
    /// https://github.com/lukencode/FluentEmail/pull/186
    /// </summary>
    public class RazorRenderer : ITemplateRenderer
    {
        private readonly RazorLightEngine _engine;
        private readonly ILogger _logger;

        public bool InlineCss { get; }
        public bool InlineHtml { get; }

        public RazorRenderer(string root, ILogger<RazorRenderer> logger, bool inlineCss, bool inlineHtml)
        {
            _logger = logger;

            InlineCss = inlineCss;
            InlineHtml = inlineHtml;

            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(root ?? Directory.GetCurrentDirectory())
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> ParseAsync<T>(string path, T model, bool isHtml = true)
        {
            dynamic viewBag = (model as IViewBagModel)?.ViewBag;

            string html = await _engine.CompileRenderAsync<T>(path, model, viewBag);

            if (InlineCss)
            {
                using (var pm = new PreMailer.Net.PreMailer(html))
                {
                    PreMailer.Net.InlineResult result = pm.MoveCssInline(false);

                    html = result.Html;

                    if (result?.Warnings?.Any() ?? false)
                        _logger.LogWarning("PreMailer CSS inline resulted in warnings for {Path}: {Warnings}", path, result.Warnings);
                }
            }

            if (InlineHtml)
            {
                return html.Replace("\n", string.Empty)
                           .Replace("\r", string.Empty)
                           .Replace("\t", string.Empty);
            }

            return html;
        }

        public string Parse<T>(string path, T model, bool isHtml)
        {
            return ParseAsync(path, model, isHtml).GetAwaiter().GetResult();
        }
    }
}
