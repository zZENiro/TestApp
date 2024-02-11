using System.ComponentModel;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server.Test();
        }
    }

    public static class Server
    {
        private static int _value = default;
        private static readonly ReaderWriterLockSlim _rwLock = new();

        public static void Test()
        {
            new Thread(() => AddToCount(1)).Start();

            new Thread(() => GetCount()).Start();
            new Thread(() => GetCount()).Start();
            new Thread(() => GetCount()).Start();

        }

        public static int GetCount()
        {
            _rwLock.EnterWriteLock();

            try
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} -> {_value}");
                return _value;
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public static void AddToCount(int value)
        {
            _rwLock.EnterReadLock();

            try
            {
                Thread.Sleep(5000);
                _value = value;
            }
            finally
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Executed");
                _rwLock.ExitReadLock();
            }
        }
    }
}
