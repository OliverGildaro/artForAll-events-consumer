namespace ArtForAll.Events.Consumer.Entities.Helpers
{
    public class EventPatchOperation
    {
        public string Path { get; set; }
        public string Op { get; set; }
        public object Value { get; set; }
    }
}
