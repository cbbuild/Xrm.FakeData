using Grace.DependencyInjection;

namespace CbBuild.Xrm.FakeData.Common
{
    public interface IContainerGetter
    {
        T Get<T>();
    }

    public class ContainerGetter : IContainerGetter
    {
        private readonly DependencyInjectionContainer container;

        public ContainerGetter(DependencyInjectionContainer container)
        {
            this.container = container;
        }

        public T Get<T>()
        {
            return container.Locate<T>();
        }
    }
}