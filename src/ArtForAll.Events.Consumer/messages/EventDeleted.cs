﻿using ArtForAll.Shared.Contracts.CQRS;
namespace ArtForAll.Events.Consumer.Messages
{
    public class EventDeleted : ICommand
    {
        public string Id { get; set; }
        public string CreatedAt { get; set; }
        public string StateEvent { get; set; }

    }
}