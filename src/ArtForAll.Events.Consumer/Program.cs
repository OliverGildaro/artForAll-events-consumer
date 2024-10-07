using Amazon.SQS;
using ArtForAll.Events.Consumer.utils;
using ArtForAll.Events.Consumer;
using ArtForAll.Events.Consumer.repositories;
using Amazon.DynamoDBv2;
using ArtForAll.Shared.Contracts.CQRS;
using ArtForAll.Events.Consumer.Messages;
using ArtForAll.Shared.ErrorHandler;
using ArtForAll.Events.Consumer.handlers;
using ArtForAll.Events.Infrastructure.DynamoRepositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.AddSingleton<IAmazonSQS>(sp =>
{
    var sqsClient = new AmazonSQSClient(Amazon.RegionEndpoint.SAEast1);
    return sqsClient;
});
builder.Services.AddHostedService<EventQueueConsumerService>();
builder.Services.AddSingleton<CommandDispatcher>();
builder.Services.AddTransient<ICommandHandler<EventCreated, Result>, EventCreatedHandler>();
builder.Services.AddTransient<ICommandHandler<EventPatched, Result>, EventPatchedHandler>();
builder.Services.AddTransient<ICommandHandler<ImageAdded, Result>, ImageAddedHandler>();
builder.Services.AddTransient<ICommandHandler<EventNameUpdated, Result>, EventNameUpdatedHandler>();
builder.Services.AddTransient<ICommandHandler<EventPublished, Result>, EventPublishedHandler>();
builder.Services.AddTransient<ICommandHandler<EventDeleted, Result>, EventDeletedHandler>();
builder.Services.AddTransient<IEventsRepository, EventsDynameLowLevelRepository>();
builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();

var app = builder.Build();

app.Run();
