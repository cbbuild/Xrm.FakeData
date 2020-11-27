using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    internal class RulePresenterTypeDescriptor : CustomTypeDescriptor
    {
        public RulePresenterTypeDescriptor() : base()
        {
        }

        public RulePresenterTypeDescriptor(ICustomTypeDescriptor parent, object instance) : base(parent)
        {
            var rule = (IRulePresenter)instance;
            customFields = RulePropertiesGenerator.Generate(rule).ToList();
        }

        private readonly List<PropertyDescriptor> customFields = new List<PropertyDescriptor>();

        public override PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(base.GetProperties()
                .Cast<PropertyDescriptor>().Union(customFields).ToArray());
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(base.GetProperties(attributes)
                .Cast<PropertyDescriptor>().Union(customFields).ToArray());
        }
    }
}