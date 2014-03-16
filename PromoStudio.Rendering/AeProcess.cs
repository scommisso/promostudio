using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PromoStudio.Rendering.Properties;

namespace PromoStudio.Rendering
{
    public class AeProcess
    {
        private Process _process;
        private SemaphoreSlim _processExitedSignal;

        public AeProcess()
        {
            ExePath = Settings.Default.AfterEffectsExePath;
            RunUnattended = true;
        }

        public string ExePath { get; set; }
        public bool RunUnattended { get; set; }

        public string ExecuteProcess(string scriptPath)
        {
            try
            {
                _process = CreateProcess(scriptPath);
                _process.Start();
                _process.WaitForExit();

                if (RunUnattended)
                {
                    return _process.StandardOutput.ReadToEnd();
                }
                return null;
            }
            finally
            {
                if (_process != null)
                {
                    _process.Dispose();
                }
            }
        }

        public async Task<string> ExecuteProcessAsync(string scriptPath)
        {
            try
            {
                _processExitedSignal = new SemaphoreSlim(0, 1);
                _process = CreateProcess(scriptPath);
                _process.EnableRaisingEvents = true;
                _process.Exited += Process_Exited;
                _process.Start();

                await _processExitedSignal.WaitAsync(); // semaphore will fire when process exit event is raised

                if (RunUnattended)
                {
                    return _process.StandardOutput.ReadToEnd();
                }
                return null;
            }
            finally
            {
                if (_process != null)
                {
                    try
                    {
                        _process.Exited -= Process_Exited;
                    }
                    catch
                    {
                    }
                    _process.Dispose();
                }
                if (_processExitedSignal != null)
                {
                    _processExitedSignal.Dispose();
                }
            }
        }

        private Process CreateProcess(string scriptPath)
        {
            return new Process
            {
                StartInfo =
                {
                    UseShellExecute = !RunUnattended,
                    LoadUserProfile = !RunUnattended,
                    RedirectStandardOutput = RunUnattended,
                    CreateNoWindow = RunUnattended,
                    FileName = ExePath,
                    Arguments = string.Format("{0}-r {1}", RunUnattended ? "-noui " : "", scriptPath),
                }
            };
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if (_processExitedSignal != null)
            {
                _processExitedSignal.Release();
            }
        }
    }
}