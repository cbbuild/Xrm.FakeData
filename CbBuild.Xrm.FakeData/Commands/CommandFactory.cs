using CbBuild.Xrm.FakeData.Ioc;

namespace CbBuild.Xrm.FakeData.Commands
{
    public interface ICommandFactory
    {
        T Create<T>()
            where T : CommandBase;
    }

    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceLocator serviceLocator;

        public CommandFactory(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public T Create<T>() where T : CommandBase
        {
            return this.serviceLocator.Get<T>();
        }
    }
}