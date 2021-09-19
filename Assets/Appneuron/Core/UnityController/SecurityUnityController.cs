using AppneuronUnity.Core.UnityManager;
using UnityEngine;


namespace Assets.Appneuron.Core.UnityManager
{
    public class SecurityUnityController : MonoBehaviour
    {
        SecurityUnityManager securityUnityManager;
        private async void Start()
        {
            securityUnityManager = new SecurityUnityManager();
            await securityUnityManager.Login();
        }
    }

}
