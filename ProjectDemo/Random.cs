using System;
namespace ProjectDemo
{
    static class Random
    {
        private static System.Random _random = new System.Random();

        public static int NextInt()
        {
            return _random.Next(10);
        }
    }
}
