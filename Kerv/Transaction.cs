using System;
namespace Kerv
{
    public class Transaction
    {
        public DateTime Date { get; }
        public String Device { get; }
        public String Description { get; }
        public Money Amount { get; }
        public Money Balance { get; }
    }
}
