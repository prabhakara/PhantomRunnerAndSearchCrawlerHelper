﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PhantomRunner
{
    public class PageRenderer
    {
        private string workingDirectory, createPageScript;        

        public PageRenderer():this("")
        {            
        }

        public PageRenderer(string workingDirectory): this("",@"scripts\createPage.js")
        {            
        }

        public PageRenderer(string workingDirectory, string createPageScript)
        {
            this.workingDirectory = workingDirectory;
            this.createPageScript = createPageScript;
        }

        public Task<string> RenderAsync(Uri url)
        {            
            return RenderAsync(url, string.Empty);
        }

        public Task<string> RenderAsync(Uri url, string optionsFileName)
        {
            var runScript = String.IsNullOrEmpty(this.workingDirectory) ? new RunScript() : new RunScript(this.workingDirectory);

            return runScript.RunAsync(createPageScript, url.AbsoluteUri, optionsFileName);         
        }

        public string CreatePageScript { get; set; }        
    }
}
