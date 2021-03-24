using Assets.Appneuron.CoreServices.CryptoServices.Absrtact;
using Effortless.Net.Encryption;
using System;
using System.Linq;

namespace Assets.Appneuron.CoreServices.CryptoServices.Concrete.WithEffortless
{
    public class EfforlessCryptoServices : ICryptoServices
    {
        static Random random = new Random();


        public string DeCrypto(string value)
        {
            return null;

        }

        public string EnCrypto(int LongSstring)
        {
            return null;
        }

        public string GetRandomHexNumber(int digits)
        {
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }
        public string GenerateStringName(int LongSstring)
        {
            string salt = Strings.CreateSalt(LongSstring);
            return salt;
        }
    }
}
