using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Kuaiyipai.Web.Controllers
{
    public class HomeController : KuaiyipaiControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
