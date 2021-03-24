using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Appneuron.Project.ChurnBlockerModule.Components.AdvDataComponent.UnityManager
{
    public interface IAdvEventUnityManager
    {
        void CheckAdvFileAndSendData();
        void SendAdvEventData(string Tag,
            string levelName,
            float GameSecond);
    }
}
