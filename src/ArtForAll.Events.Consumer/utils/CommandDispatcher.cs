using ArtForAll.Shared.Contracts.CQRS;
using ArtForAll.Shared.ErrorHandler;

namespace ArtForAll.Events.Consumer.utils
{
    public class CommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<Result> Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<,>);
            Type[] typeArgs = { command.GetType(), typeof(Result) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = this.serviceProvider.GetService(handlerType);
            Result result = await handler.HandleAsync((dynamic)command);

            return result;
        }
    }
}
