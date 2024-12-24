using System;
using System.Collections.Generic;

namespace ServerManagerApp
{
    public class ServerManager
    {
        private List<Server> servers = new List<Server>();
        public event EventHandler<string> Logged;

        public void AddServer(Server server)
        {
            servers.Add(server);
            server.Logged += Server_Logged;
            OnLogged($"Сервер {server.Name} добавлен.");
        }

        public void RemoveServer(string serverName)
        {
            Server serverToRemove = servers.Find(s => s.Name == serverName);
            if (serverToRemove != null)
            {
                serverToRemove.Stop();
                serverToRemove.Logged -= Server_Logged;
                servers.Remove(serverToRemove);
                OnLogged($"Сервер {serverName} удален.");
            }
            else
            {
                OnLogged($"Сервер {serverName} не найден");
            }
        }

        public void StartServer(string serverName)
        {
            Server server = servers.Find(s => s.Name == serverName);
            if (server != null)
                server.Start();
            else
                OnLogged($"Сервер {serverName} не найден");

        }

        public void StopServer(string serverName)
        {
            Server server = servers.Find(s => s.Name == serverName);
            if (server != null)
                server.Stop();
            else
                OnLogged($"Сервер {serverName} не найден");
        }

        public void RestartServer(string serverName)
        {
            Server server = servers.Find(s => s.Name == serverName);
            if (server != null)
                server.Restart();
            else
                OnLogged($"Сервер {serverName} не найден");
        }

        public void StartAllServers()
        {
            foreach (Server server in servers)
                server.Start();
            OnLogged("Запуск всех серверов.");
        }

        public void StopAllServers()
        {
            foreach (Server server in servers)
                server.Stop();
            OnLogged("Остановка всех серверов.");
        }

        private void Server_Logged(object sender, string e)
        {
            OnLogged(e);
        }
        protected virtual void OnLogged(string message)
        {
            Logged?.Invoke(this, message);
        }
    }
}