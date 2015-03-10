using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {                                
                Trace.WriteLine("RunScript working directory: " + AppDomain.CurrentDomain.RelativeSearchPath, "Information");

                var runScript = new RunScript();
                
                var result = await runScript.RunAsync("../scripts/sample.js");
                ViewBag.Result = result;
            }
            catch (ExternalException ex)
            {
                Trace.WriteLine("Error running script " + ex.GetBaseException().Message,"Error");
                ViewBag.Result = ex.Message; ;
            }
            
            return View();
        }
    }
}