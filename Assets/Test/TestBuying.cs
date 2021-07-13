using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Assets.Appneuron.Core.UnityManager;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.BuyingDataComponent.DataModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public class TestBuying : MonoBehaviour
{
    private int timer = 0;
    private IdUnityManager idUnityManager;
    private IKafkaMessageBroker _kafkaMessageBroker;

    private string projectId;
    private string customerId;

    private List<string> storePageList = new List<string> { "main-page",
        "sword-page",
        "skill-page",
        "defence-page",
        "mine-page"
    };

    private void Awake()
    {
        using (var kernel = new StandardKernel())
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
        customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();
        idUnityManager = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<IdUnityManager>();

    }

    async void Update()
    {

        var startedDate = new DateTime(DateTime.Now.Year,
        month: 3, day: UnityEngine.Random.Range(1, 6),
        hour: UnityEngine.Random.Range(1, 23),
        minute: UnityEngine.Random.Range(1, 59),
        0, 0);
        var id = idUnityManager.GenerateId();
        var difficultyLevel = UnityEngine.Random.Range(1, 11);
        timer += 1;

        if (timer >= 1000)
            return;
        if (UnityEngine.Random.Range(1, 10) != 5)
            return;
      
        for (int i = 0; i < storePageList.ToArray().Length; i++)
        {
            startedDate.AddHours(UnityEngine.Random.Range(1, 5));
            startedDate.AddDays(UnityEngine.Random.Range(0, 3));
            startedDate.AddSeconds(UnityEngine.Random.Range(0, 59));

            await CalculateBuyingEvent(storePageList.ToArray()[i], id, storePageList.ToArray()[i], difficultyLevel,
               startedDate);
        }
    }


    private async Task CalculateBuyingEvent(string levelName,
       string clientId,
       string productType,
       int difficultyLevel,
       DateTime trigerdTime)
    {
        var payingCount = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < payingCount; i++)
        {

            BuyingEventDataModel dataModel = new BuyingEventDataModel
            {
                ClientId = clientId,
                ProjectID = projectId,
                CustomerID = customerId,
                TrigersInlevelName = levelName,
                ProductType = productType,
                DifficultyLevel = difficultyLevel,
                InWhatMinutes = UnityEngine.Random.Range(50, 600),
                TrigerdTime = trigerdTime

            };

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            Debug.Log(result.Success);

        }
    }

}
