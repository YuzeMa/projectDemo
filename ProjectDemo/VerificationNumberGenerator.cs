using System;
namespace ProjectDemo
{
    public class VerificationNumberGenerator
    {
        public static string Generate()
        {
            string verificationNumber = "";
            for (int i = 0; i < 4; i++)
            {
                verificationNumber += Random.NextInt().ToString();
            }
            return verificationNumber;
        }
    }
}
