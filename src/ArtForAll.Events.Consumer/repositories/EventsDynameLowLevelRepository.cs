namespace ArtForAll.Events.Infrastructure.DynamoRepositories
{
    using ArtForAll.Events.Consumer.repositories;
    using Amazon.DynamoDBv2;
    using Amazon.DynamoDBv2.DocumentModel;
    using Amazon.DynamoDBv2.Model;
    using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;
    using ArtForAll.Shared.Contracts.DDD;
    using ArtForAll.Shared.ErrorHandler;
    using System.Text.Json;
    using ArtForAll.Events.Consumer.Entities;

    public class EventsDynameLowLevelRepository : IEventsRepository
    {
        private readonly string _tableName = "eventsDB";
        private readonly IAmazonDynamoDB client;

        public EventsDynameLowLevelRepository(IAmazonDynamoDB client)
        {
            this.client = client;
        }

        public async Task<Result<Event, Error>> Update(Event @event)
        {
            var seToInsertAsJason = JsonSerializer.Serialize(@event);
            var customerAttributes = Document.FromJson(seToInsertAsJason).ToAttributeMap();

            PutItemRequest createItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = customerAttributes,
                //ConditionExpression = "attribute_not_exist(pk) and attribute_not_exist(sk)"
            };
            PutItemResponse response = null;
            try
            {
                response = await client.PutItemAsync(createItemRequest);
            }
            catch (Exception ex)
            {
                throw;
            }

            if (response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result<Event, Error>.Success(@event);
            }

            return Result<Event, Error>.Failure(new Error("", ""));
        }

        public async Task<Result> UpdateState(string pk, string createdAt, string newState)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = pk } },
                { "sk", new AttributeValue { S = createdAt.ToString() } }
            };

            var updateItemRequest = new UpdateItemRequest
            {
                TableName = _tableName,
                Key = key,
                UpdateExpression = "SET #state = :newState",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#state", "State" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":newState", new AttributeValue { S = newState } }
                }
            };

            UpdateItemResponse response = null;
            try
            {
                response = await client.UpdateItemAsync(updateItemRequest);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw;
            }

            if (response != null && response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result.Success();
            }

            return Result.Failure("Error");
        }

        public async Task<Result> PatchEvent(EventPatch @event)
        {
            var atrributeUpdates = new Dictionary<string, AttributeValueUpdate>();
            foreach (var patchOp in @event.PatchOperations)
            {
                var key = "";
                var value = new AttributeValueUpdate();
                if (patchOp.Op == "add" || patchOp.Op == "replace")
                {
                    key = CapitalizeFirstLetter(patchOp.Path);
                    value = new AttributeValueUpdate()
                    {
                        Action = AttributeAction.PUT,
                        Value = GetAttributeValue(patchOp.Value)
                    };
                }
                else if (patchOp.Op == "remove")
                {
                    key = CapitalizeFirstLetter(patchOp.Path);
                    value = new AttributeValueUpdate()
                    {
                        Action = AttributeAction.DELETE,
                        Value = GetAttributeValue(patchOp.Value)
                    };
                }
                atrributeUpdates.Add(key, value);
            }

            var updateItemRequest = new UpdateItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = @event.State } },
                    { "sk", new AttributeValue { S = @event.Name } }
                },
                AttributeUpdates = atrributeUpdates,
            };

            try
            {
                var response = await client.UpdateItemAsync(updateItemRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var updatedEventJson = Document.FromAttributeMap(response.Attributes).ToJson();
                    var updatedEvent = JsonSerializer.Deserialize<Event>(updatedEventJson);
                    return Result.Success();
                }
                return Result.Failure("Unknown error occurred");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating item: {ex.Message}");
            }
        }

        public async Task<Result> DeleteASync(string state, string name)
        {
            var deletedItemRequest = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = state } },
                    { "sk", new AttributeValue { S = name } }
                },
            };

            try
            {
                var response = await client.DeleteItemAsync(deletedItemRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Result.Success();
                }
                return Result.Failure("Unknown error occurred");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating item: {ex.Message}");
            }
        }

        public async Task<Result<Event, Error>> FindAsync(string state, string name)
        {
            var queryRequest = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = state } },
                    { "sk", new AttributeValue { S = name } }

                }
            };

            var response = await client.GetItemAsync(queryRequest);

            if (response.Item.Count == 0)
            {
                return Result<Event, Error>.Failure(new Error("", ""));
            }

            var itemAsDocument = Document.FromAttributeMap(response.Item);
            var simpleDynamoEvent = JsonSerializer.Deserialize<Event>(itemAsDocument.ToJson());

            if (simpleDynamoEvent is null)
            {
                return Result<Event, Error>.Failure(new Error("", ""));
            }

            return Result<Event, Error>.Success(simpleDynamoEvent);
        }

        public async Task<Result<Event, Error>> AddImageAsync(string eventId, string createdAt, Image image)
        {
            // Fetch the existing event from DynamoDB
            var getRequest = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = eventId } },
                    { "sk", new AttributeValue { S = createdAt.ToString() } } // Assuming pk = sk
                }
            };

            var getResponse = await client.GetItemAsync(getRequest);
            if (getResponse.Item == null)
            {
                return Result<Event, Error>.Failure(new Error("EventNotFound", "Event not found"));
            }

            var eventItem = Document.FromAttributeMap(getResponse.Item).ToJson();
            var existingEvent = JsonSerializer.Deserialize<Event>(eventItem);

            // Update the event with the new image
            existingEvent.Image = image;

            // Save the updated event back to DynamoDB
            var updatedEventJson = JsonSerializer.Serialize(existingEvent);
            var updatedAttributes = Document.FromJson(updatedEventJson).ToAttributeMap();

            var updateRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = updatedAttributes
            };

            var updateResponse = await client.PutItemAsync(updateRequest);

            if (updateResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result<Event, Error>.Success(existingEvent);
            }

            return Result<Event, Error>.Failure(new Error("UpdateFailed", "Failed to update event with image"));
        }

        private AttributeValue GetAttributeValue(object value)
        {
            switch (value)
            {
                case string s:
                    return new AttributeValue { S = s };
                case int i:
                    return new AttributeValue { N = i.ToString() };
                case long l:
                    return new AttributeValue { N = l.ToString() };
                case float f:
                    return new AttributeValue { N = f.ToString() };
                case double d:
                    return new AttributeValue { N = d.ToString() };
                case bool b:
                    return new AttributeValue { BOOL = b };
                case DateTime dt:
                    return new AttributeValue { S = dt.ToString("o") }; // ISO 8601 format
                case DateTimeOffset dto:
                    return new AttributeValue { S = dto.ToString("o") }; // ISO 8601 format
                default:
                    throw new ArgumentException($"Unsupported value type: {value.GetType()}");
            }
        }

        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
