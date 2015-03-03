using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PhantomRunner
{
    public class RunScript
    {
        private string workingDirectory, fileName;

        public RunScript():this("")
        {
            
        }

        public RunScript(string workingDirectory)
        {
            if(string.IsNullOrEmpty(workingDirectory))
            {
                this.workingDirectory = AppDomain.CurrentDomain.RelativeSearchPath;                
            }
            else
            {
                this.workingDirectory = workingDirectory;
            }
            
            this.fileName = Path.Combine(this.workingDirectory, "phantomjs.exe");
        }

        public Task<string> RunAsync(string scriptName)
        {
            return RunAsync(scriptName, "");
        }

        public Task<string> RunAsync(string scriptName, string commandLineArguments)
        {
            return RunAsync(scriptName, commandLineArguments, "");
        }

        public Task<string> RunAsync(string scriptName, string commandLineArguments, string optionsFileName)
        {

            if (string.IsNullOrEmpty(scriptName))
            {
                throw new ArgumentNullException("valid scriptName expected");
            }

            var arguments = CreateArguments(scriptName, commandLineArguments, optionsFileName);

            var tcs = new TaskCompletionSource<string>();
            string standardOutput = string.Empty;
            string errorOutput = string.Empty;

            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                Arguments = arguments,
                FileName = fileName,
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8
            };

            var process = new Process() { StartInfo = startInfo, EnableRaisingEvents = true };

                process.OutputDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        standardOutput += args.Data;
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    if (args.Data != null)
                    {
                        errorOutput += args.Data;
                    }
                };

                process.Exited += (sender, args) =>
                {
                    if (process.ExitCode != 0)
                    {
                        tcs.TrySetException(new ExternalException("Error running PhantomJS. " + errorOutput, process.ExitCode));
                    }
                    else
                    {
                        tcs.SetResult(standardOutput);
                    }                    
                };

            
                if (process.Start() == false)
                {
                    tcs.TrySetException(new ExternalException("Process failed to start."));
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            
            
            return tcs.Task;
        }

        private string CreateArguments(string scriptName, string commandLineArguments, string optionsFileName)
        {
            if (scriptName.Split('.').Length == 1)
            {
                scriptName += ".js";
            }

            StringBuilder arguments = new StringBuilder();

            if (!string.IsNullOrEmpty(optionsFileName))
            {
                arguments.AppendFormat("--config={0} ", optionsFileName);
            }

            arguments.Append(scriptName);

            if (!string.IsNullOrEmpty(commandLineArguments))
            {
                arguments.AppendFormat(" {0}", commandLineArguments);
            }

            return arguments.ToString();
        }
    }
}
