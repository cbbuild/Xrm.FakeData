using CbBuild.Xrm.FakeData.Presenters;
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

        public void Configure(IExportRegistrationBlock registrationBlock)
        {
            registrationBlock.ExportInstance(serviceLocator).Lifestyle.Singleton();
            registrationBlock.Export<EventAggregator>().As<IEventAggregator>().Lifestyle.Singleton();

            registrationBlock.Export<TreeNodeView>().As<ITreeNodeView>();
            registrationBlock.Export<RuleFactory>().As<IRuleFactory>();
            registrationBlock.Export<RuleEditView>().As<IRuleEditView>().Lifestyle.Singleton();
            registrationBlock.Export<MessageBoxService>().As<IMessageBoxService>().Lifestyle.Singleton();
            registrationBlock.Export<RulePreviewView>().As<IRulePreviewView>().Lifestyle.Singleton();
            registrationBlock.Export<RuleExecutorFactory>().As<IRuleExecutorFactory>().Lifestyle.Singleton();

            // Views
            registrationBlock.Export<RulesTreeView>().As<IRulesTreeView>().Lifestyle.Singleton();
            registrationBlock.Export<RulesTreePresenter>().As<IRulesTreePresenter>().Lifestyle.Singleton();
        }
    }
}