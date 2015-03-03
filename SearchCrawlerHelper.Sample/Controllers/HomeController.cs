using PhantomRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SearchCrawlerHelper.Sample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            if (Request.QueryString["_escaped_fragment_"] != null)
            {                
                //We´ll crawl the normal url without _escaped_fragment_
                var crawler = new PageRenderer();
                var result = await crawler.RenderAsync(new Uri(Request.Url.AbsoluteUri.Replace("?_escaped_fragment_=", "")));
                
                return Content(result);                
            }
            else
            {                
                return View();
            }            
        }
    }
}
