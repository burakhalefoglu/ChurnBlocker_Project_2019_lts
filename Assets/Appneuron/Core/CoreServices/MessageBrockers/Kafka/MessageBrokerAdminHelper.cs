using Appneuron.Services;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class MessageBrokerAdminHelper
{
    public static  Task<int> GetPartitionCount(string topicName)
    {
        using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = ApacheKafkaConfigService.BootstrapServers }).Build())
        {
          
                var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(20));

               var partititonCount =  meta.Topics.Find(p => p.Topic == topicName).Partitions.Count();

            return Task.FromResult(partititonCount);
        }

    }
}

