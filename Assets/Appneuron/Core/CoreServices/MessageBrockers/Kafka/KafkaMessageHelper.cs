using Appneuron.Services;
using Assets.Appneuron.Core.CoreServices.ResultService;
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka
{
    public class KafkaMessageHelper : IKafkaMessageBroker
    {
        public Task<IDataResult<T>> getMessageAsync<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> SendMessageAsync<T>(T messageModel) where T :
         class, new()
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = ApacheKafkaConfigService.BootstrapServers
            };

            var message = JsonConvert.SerializeObject(messageModel);
            var topicName = typeof(T).Name;
            using (var p = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                try
                {
                    await p.ProduceAsync(new TopicPartition(topicName,
                        new Partition(new System.Random().Next(0, await MessageBrokerAdminHelper.GetPartitionCount(topicName))))
                    , new Message<Null, string>
                    {
                        Value = message

                    });
                    return new SuccessResult();

                }

                catch (ProduceException<Null, string> e)
                {
                    return new ErrorResult();
                }
            }
        }
    }
}
