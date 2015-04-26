using RdsServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BoardClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup_1(object sender, StartupEventArgs e)
        {
            try
            {
                Server server = new Server();
                server.StartServer();
                Console.WriteLine("RDS Запущен");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при запуске RDS");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}