using UnityEngine;
using static Assets.Appneuron.Core.CoreServices.IdConfigService.IdConfigServices;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.DataAccessBase;
using AppneuronUnity.Core.UnityManager;

namespace Assets.Appneuron.Core.UnityManager
{
    public class IdUnityController : MonoBehaviour
    {
        private IdUnityManager ıdUnityManager;
        private async void Awake()
        {
            ıdUnityManager = new IdUnityManager();
            CreateFileDirectories();
            await ıdUnityManager.SaveIdOnLocalStorage();
        }
    }
}
