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
            var loginHandler = new OldLoginHandler(username, password, requestHandler);
            var parser = new HtmlParser();
            var result = loginHandler.Login(parser);
            while (!result.IsCompleted) {
                Thread.Sleep(100);
            }
            var balanceHandler = new BalanceHandler();
            var balance = balanceHandler.GetBalance(requestHandler, parser);
            while (!balance.IsCompleted) {
                Thread.Sleep(100);
            }
            Console.WriteLine(
                String.Format("Your balance is £{0}", balance.Result));

            Console.ReadKey();
        }
    }
}
