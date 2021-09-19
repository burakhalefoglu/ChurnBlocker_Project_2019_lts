using UnityEngine;

namespace Assets.Appneuron.Core.CoreServices.DontDestroyOnLoadServices
{
    class DontDestroyOnLoadServices : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad();
        }

        void DontDestroyOnLoad()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Appneuron");

            if (objs.Length > 1)
            {
                for (int i = 1; i < objs.Length; i++)
                {
                    Destroy(objs[i]);
                }

            }

            DontDestroyOnLoad(gameObject);
        }

    }
}
