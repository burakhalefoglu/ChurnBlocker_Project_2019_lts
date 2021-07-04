using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamTest : MonoBehaviour
{
    string Message0 = "This is The New Unity Message0";
    string Message1 = "This is The New Unity Message1";
    string TopicName = "test-topic";
    float timer = 0;

    async void Update()
    {
        timer += Time.deltaTime;
        if (timer <= 1.0f)
        {
            return;
        }
        timer = 0;

        var config = new ProducerConfig
        {
            BootstrapServers = "192.168.1.35:9092",

        };

        using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = "192.168.1.35:9092" }).Build())
        {
            try
            {
                await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                        new TopicSpecification { Name = TopicName, ReplicationFactor = 1, NumPartitions = 2 } });
                
            }
            catch (CreateTopicsException e)
            {
                Debug.Log($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }

        using (var p = new ProducerBuilder<Null, string>(config).Build())
        {
            try
            {
                var dr = await p.ProduceAsync(TopicName, new Message<Null, string> { 
                    Value = Message0

                });
                dr.Partition = 0;
                Debug.Log($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Debug.LogError($"Delivery failed: {e.Error.Reason}");
            }
        }

        using (var p = new ProducerBuilder<Null, string>(config).Build())
        {
            try
            {
                var dr = await p.ProduceAsync(TopicName, new Message<Null, string>
                {
                    Value = Message1

                });
                dr.Partition = 1;

                Debug.Log($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Debug.LogError($"Delivery failed: {e.Error.Reason}");
            }
        }

    }

}
