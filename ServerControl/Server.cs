using System;
using System.Diagnostics;
using System.Threading;

namespace ServerManagerApp
{
    public class Server
    {
        public string Name { get; private set; }
        public string ExecutablePath { get; private set; }
        public bool IsRunning { get; private set; }
        public string Arguments { get; private set; }
        private Process serverProcess;
        public event EventHandler<string> Logged;

        public Server(string name, string executablePath, string arguments = "")
        {
            Name = name;
            ExecutablePath = executablePath;
            Arguments = arguments;
            IsRunning = false;
        }

        public void Start()
        {
            if (IsRunning)
            {
                OnLogged($"Сервер {Name} уже запущен");
                return;
            }
            try
            {
                serverProcess = new Process();
                serverProcess.StartInfo.FileName = ExecutablePath;
                serverProcess.StartInfo.Arguments = Arguments;
                serverProcess.StartInfo.RedirectStandardOutput = true;
                serverProcess.StartInfo.RedirectStandardError = true;
                serverProcess.StartInfo.UseShellExecute = false;
                serverProcess.StartInfo.CreateNoWindow = true;
                serverProcess.EnableRaisingEvents = true;
                serverProcess.OutputDataReceived += ServerProcess_OutputDataReceived;
                serverProcess.ErrorDataReceived += ServerProcess_ErrorDataReceived;

                serverProcess.Start();
                serverProcess.BeginOutputReadLine();
                serverProcess.BeginErrorReadLine();

                IsRunning = true;
                OnLogged($"Сервер {Name} запущен.");
            }
            catch (Exception ex)
            {
                OnLogged($"Ошибка запуска сервера {Name}: {ex.Message}");
                return;
            }

        }
        private void ServerProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                OnLogged($"[{Name} Error]: " + e.Data);
        }
        private void ServerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                OnLogged($"[{Name}]: " + e.Data);
        }
        public void Stop()
        {
            if (!IsRunning)
            {
                OnLogged($"Сервер {Name} уже остановлен.");
                return;
            }
            try
            {
                if (serverProcess != null && !serverProcess.HasExited)
                {
                    serverProcess.Kill();
                    serverProcess.WaitForExit();
                    IsRunning = false;
                    OnLogged($"Сервер {Name} остановлен.");
                }
            }
            catch (Exception ex)
            {
                OnLogged($"Ошибка остановки сервера {Name}: {ex.Message}");
                return;
            }
        }

        public void Restart()
        {
            Stop();
            Thread.Sleep(1000); // Пауза перед запуском
            Start();
            OnLogged($"Сервер {Name} перезапущен.");
        }
        protected virtual void OnLogged(string message)
        {
            Logged?.Invoke(this, message);
        }
    }
}