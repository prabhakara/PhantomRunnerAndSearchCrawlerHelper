using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PhantomRunner.Sample.Controllers
{
    public class ExampleController : Controller
    {
        // GET: Example
        public async Task<ActionResult> Index()
        {
            var runScript = new RunScript();

            try
            {
                var result = await runScript.RunAsync("scripts/sample.js");
                ViewBag.Result = result;
            }
            catch (ExternalException ex)
            {
                ViewBag.Result = ex.Message; ;
            }
            
            return View();
        }
    }
}