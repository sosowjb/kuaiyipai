using Microsoft.AspNetCore.Antiforgery;

namespace Kuaiyipai.Web.Controllers
{
    public class AntiForgeryController : KuaiyipaiControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
