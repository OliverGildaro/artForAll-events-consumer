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
                Pk = @message.Id,
                Name = @message.Name,
                Date = @message.Date,
                CreatedAt = @message.CreatedAt,
                State = @message.StateEvent,
                Description = @message.Description,
                Type = @message.Type
            };
        }


        public static Event EventPublishedToDynamoEvent(this EventPublished @message)
        {
            return new Event
            {
                Pk = @message.Id,
                State = @message.StateEvent
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
                Pk = @message.Id,
                CreatedAt = @message.CreatedAt,
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
