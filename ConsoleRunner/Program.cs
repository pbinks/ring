using System;
using System.Threading;
using Kerv;

namespace ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var requestHandler = new ConsoleRequestHandler();
            Console.WriteLine("Username: ");
            var username = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();
            var loginHandler = new LoginHandler(username, password, requestHandler);
            var result = loginHandler.Login(new HtmlParser());
            while (!result.IsCompleted) {
                Thread.Sleep(100);
            }

            Console.ReadKey();
        }
    }
}
