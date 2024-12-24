using System;

namespace ServerManagerApp
{
    class Program
    {
        static ServerManager serverManager = new ServerManager();

        static void Main(string[] args)
        {
            serverManager.Logged += ServerManager_Logged;

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить сервер");
                Console.WriteLine("2. Удалить сервер");
                Console.WriteLine("3. Запустить сервер");
                Console.WriteLine("4. Остановить сервер");
                Console.WriteLine("5. Перезапустить сервер");
                Console.WriteLine("6. Запустить все серверы");
                Console.WriteLine("7. Остановить все серверы");
                Console.WriteLine("8. Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddServer();
                        break;
                    case "2":
                        RemoveServer();
                        break;
                    case "3":
                        StartServer();
                        break;
                    case "4":
                        StopServer();
                        break;
                    case "5":
                        RestartServer();
                        break;
                    case "6":
                        serverManager.StartAllServers();
                        break;
                    case "7":
                        serverManager.StopAllServers();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
        private static void ServerManager_Logged(object sender, string e)
        {
            Console.WriteLine(e);
        }

        static void AddServer()
        {
            Console.Write("Введите имя сервера: ");
            string name = Console.ReadLine();

            Console.Write("Введите путь к исполняемому файлу: ");
            string path = Console.ReadLine();

            Console.Write("Введите аргументы (необязательно): ");
            string arguments = Console.ReadLine();

            Server newServer = new Server(name, path, arguments);
            serverManager.AddServer(newServer);
        }

        static void RemoveServer()
        {
            Console.Write("Введите имя сервера для удаления: ");
            string serverName = Console.ReadLine();
            serverManager.RemoveServer(serverName);
        }

        static void StartServer()
        {
            Console.Write("Введите имя сервера для запуска: ");
            string serverName = Console.ReadLine();
            serverManager.StartServer(serverName);
        }
        static void StopServer()
        {
            Console.Write("Введите имя сервера для остановки: ");
            string serverName = Console.ReadLine();
            serverManager.StopServer(serverName);
        }
        static void RestartServer()
        {
            Console.Write("Введите имя сервера для перезапуска: ");
            string serverName = Console.ReadLine();
            serverManager.RestartServer(serverName);
        }
    }
}