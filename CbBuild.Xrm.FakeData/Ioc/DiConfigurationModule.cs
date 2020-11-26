using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.RuleExecutors;
using CbBuild.Xrm.FakeData.Services;
using CbBuild.Xrm.FakeData.Views;
using Grace.DependencyInjection;
using Reactive.EventAggregator;

namespace CbBuild.Xrm.FakeData.Ioc
{
    public class DiConfigurationModule : IConfigurationModule
    {
        private readonly IServiceLocator serviceLocator;

        public DiConfigurationModule(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Configure(IExportRegistrationBlock c)
        {
            c.ExportInstance(serviceLocator).Lifestyle.Singleton();
            c.Export<EventAggregator>().As<IEventAggregator>().Lifestyle.Singleton();

            c.Export<TreeNodeView>().As<ITreeNodeView>();
            c.Export<RuleFactory>().As<IRuleFactory>();
            c.Export<RuleEditView>().As<IRuleEditView>().Lifestyle.Singleton();
            c.Export<MessageBoxService>().As<IMessageBoxService>().Lifestyle.Singleton();
            c.Export<RulePreviewView>().As<IRulePreviewView>().Lifestyle.Singleton();
            c.Export<RulesTreeView>().As<IRulesTreeView>().Lifestyle.Singleton();
            c.Export<RuleExecutorFactory>().As<IRuleExecutorFactory>().Lifestyle.Singleton();
        }
    }
}