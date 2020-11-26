using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    internal class RulePresenterTypeDescriptorProvider : TypeDescriptionProvider
    {
        private static TypeDescriptionProvider defaultTypeProvider =
                       TypeDescriptor.GetProvider(typeof(RulePresenter));

        public RulePresenterTypeDescriptorProvider() : base(defaultTypeProvider)
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType,
                                                                object instance)
        {
            ICustomTypeDescriptor defaultDescriptor =
                                  base.GetTypeDescriptor(objectType, instance);

            return instance == null ? defaultDescriptor :
                new RulePresenterTypeDescriptor(defaultDescriptor, instance);
        }
    }
}