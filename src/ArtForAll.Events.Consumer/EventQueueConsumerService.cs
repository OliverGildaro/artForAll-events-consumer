namespace ArtForAll.Events.Consumer
{
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using ArtForAll.Events.Consumer.utils;
    using ArtForAll.Shared.Contracts.CQRS;
    using Microsoft.Extensions.Options;

    public class EventQueueConsumerService : BackgroundService
    {
        private readonly IAmazonSQS _sqs;
        private readonly IOptions<QueueSettings> _queueSettings;
        private readonly CommandDispatcher _mediator;

        public EventQueueConsumerService(
            IAmazonSQS sqs,
            IOptions<QueueSettings> queueSettings,
            CommandDispatcher mediator)
        {
            _sqs = sqs;
            _queueSettings = queueSettings;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            GetQueueUrlResponse queueUrlResponse = null;
            try
            {
                //We ask for our SQS URL by sending the SQS service name
                queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken);
            }
            catch (Exception ex)
            {
                return;
            }

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrlResponse.QueueUrl,
                AttributeNames = new List<string> { "All" },//For performance SQS doesn't return the atributes you need to setup to get them
                MessageAttributeNames = new List<string> { "All" },//For performance SQS doesn't return the message atributes you need to setup to get them
                MaxNumberOfMessages = 1//we can increase for batch process
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _sqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
                foreach (var message in response.Messages)
                {
                    var messageType = message.MessageAttributes["MessageType"].StringValue;
                    var type = Type.GetType($"ArtForAll.Events.Consumer.Messages.{messageType}");
                    if (type is null)
                    {
                        //_logger.LogWarning("Unknown message type: {MessageType}", messageType);
                        continue;
                    }

                    try
                    {
                        var sqsMessage = (ICommand)Newtonsoft.Json.JsonConvert.DeserializeObject(message.Body, type)!;
                        await _mediator.Dispatch(sqsMessage);
                    }
                    catch (Exception ex)
                    {
                        //if the message fails here, the standard SQS will be able to keep procesing other messages
                        //and failing one and again the message that can not process. But in a FIFO SQS
                        //If a message fails that will be blocking the rest of the procesing messages
                        //So we implement a DLQ (death letter queue) after three times the message fails, we drive the message
                        //from the DLQ
                        //_logger.LogError(ex, "Message failed during processing");
                        continue;
                    }

                    //We need to delete the message or the message will remain in SQS
                    await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }

}
