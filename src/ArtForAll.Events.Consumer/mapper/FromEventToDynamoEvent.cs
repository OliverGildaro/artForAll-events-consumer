namespace ArtForAll.Events.Consumer.mapper
{
    using ArtForAll.Events.Consumer.Entities;
    using ArtForAll.Events.Consumer.Messages;
    using ArtForAll.Events.Infrastructure.DynamoRepositories.Entities;

    public static class FromEventToDynamoEvent
    {
        public static Event EventCreatedToDynamoEvent(this EventCreated @message)
        {
            return new Event
            {
                Id = @message.Id,
                Name = @message.Name,
                StartDate = @message.StartDate,
                EndDate = message.EndDate,
                CreatedAt = @message.CreatedAt,
                State = @message.StateEvent,
                Description = @message.Description,
                Type = @message.Type,
                Addres = new Address
                {
                    City = message.Addres.City,
                    Country = message.Addres.Country,
                    Number = message.Addres.Number,
                    Street = message.Addres.Street,
                    ZipCode = message.Addres.ZipCode    
                },
                Capacity = message.Capacity,
                Price = new Price
                {
                    CurrencyExchange = message.Price.CurrencyExchange,
                    MonetaryValue = message.Price.MonetaryValue,
                }
            };
        }


        public static Event EventPublishedToDynamoEvent(this EventPublished @message)
        {
            return new Event
            {
                Id = @message.Id,   
                Name = @message.Name,
                StartDate = @message.StartDate,
                EndDate = message.EndDate,
                CreatedAt = @message.CreatedAt,
                State = @message.StateEvent,
                Description = @message.Description,
                Type = @message.Type,
                Addres = new Address
                {
                    City = message.Addres.City,
                    Country = message.Addres.Country,
                    Number = message.Addres.Number,
                    Street = message.Addres.Street,
                    ZipCode = message.Addres.ZipCode
                },
                Capacity = message.Capacity,
                Price = new Price
                {
                    CurrencyExchange = message.Price.CurrencyExchange,
                    MonetaryValue = message.Price.MonetaryValue,
                }
            };
        }

        public static EventPatch EventPatchedToDynamoEvent(this EventPatched @message)
        {
            var patchOperations = @message.PatchOperations;
            foreach (var operation in patchOperations)
            {
                operation.Path = operation.Path.Replace("/", "");
            }
            return new EventPatch
            {
                State = @message.State,
                Name = @message.Name,
                PatchOperations = patchOperations,
            };
        }

        public static Image EventImageToDynamoEventImage(this ImageAdded @message)
        {
            return new Image
            {
                Id = @message.Id,
                contentType = message.contentType,
                fileName = @message.fileName,
            };
        }
    }
}
