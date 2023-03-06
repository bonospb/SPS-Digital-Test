using System;
using System.Diagnostics;
using System.Text;

namespace FreeTeam.BP.SourceControl
{
    public abstract class SourceControlVersion : IVersion
    {
        #region Protected
        protected abstract string Executable { get; }
        protected abstract string Command { get; }
        #endregion

        #region Public
        public int Version
        {
            get
            {
                // Start process
                string error = null;
                StringBuilder output = new StringBuilder();
                int version = 0;
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    // Set command
                    startInfo.FileName = Executable;
                    startInfo.Arguments = Command;
                    // Hide terminal
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    // Enable redirect
                    startInfo.RedirectStandardError = true;
                    startInfo.RedirectStandardOutput = true;


                    // Start process
                    Process process = new Process();
                    process.StartInfo = startInfo;
                    process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
                    process.Start();
                    process.BeginOutputReadLine();
                    error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (string.IsNullOrEmpty(error) && output.Length > 0)
                        version = Convert.ToInt32(output.ToString());
                }
                catch (Exception e)
                {
                    error = e.Message;
                }

                if (!string.IsNullOrEmpty(error))
                    UnityEngine.Debug.LogError($"Can not perform command {Executable} {Command}. Error: {error}");

                return version;
            }
        }
        #endregion
    }
}
