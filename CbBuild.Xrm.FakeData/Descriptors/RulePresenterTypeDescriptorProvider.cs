using CbBuild.Xrm.FakeData.Presenter.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    class RulePresenterTypeDescriptorProvider : TypeDescriptionProvider
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
