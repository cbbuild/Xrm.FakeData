using Grace.DependencyInjection;
using Microsoft.Xrm.Sdk;
using System;

namespace CbBuild.Xrm.FakeData.Ioc
{
    public interface IServiceLocator
    {
        T Get<T>();
        T Get<T>(object extraData);
        void RegisterOrganizationServiceFactory(Func<IOrganizationService> service);
    }

    internal class ServiceLocator : IServiceLocator
    {
        private readonly DependencyInjectionContainer container;

        public ServiceLocator(DependencyInjectionContainer container)
        {
            this.container = container;
        }

        public T Get<T>()
        {
            return container.Locate<T>();
        }

        public T Get<T>(object extraData)
        {
            return container.Locate<T>(extraData);
        }

        public void RegisterOrganizationServiceFactory(Func<IOrganizationService> service)
        {
            container.Configure(c =>
            {
                c.ExportFuncWithContext((scope, sctx, ctx) => service());
            });
        }
    }
}