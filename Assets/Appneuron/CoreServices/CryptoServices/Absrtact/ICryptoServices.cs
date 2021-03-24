using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Appneuron.CoreServices.CryptoServices.Absrtact
{
    public interface ICryptoServices
    {

        string GetRandomHexNumber(int digits);

        string EnCrypto(int LongSstring);

        string DeCrypto(string value);

        string GenerateStringName(int LongString);

    }
}
