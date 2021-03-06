﻿using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    internal class RuleParameterPropertyDescriptor : PropertyDescriptor
    {
        public RuleParameter CustomField { get; private set; }

        public RuleParameterPropertyDescriptor(RuleParameter customField)
            : base(customField.Name, new Attribute[] { new CategoryAttribute(customField.Category) })
        {
            CustomField = customField;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(IRulePresenter);
            }
        }

        public override object GetValue(object component)
        {
            IRulePresenter rule = (IRulePresenter)component;
            return rule[CustomField.Name] ?? (CustomField.DataType.IsValueType ?
                (Object)Activator.CreateInstance(CustomField.DataType) : null);
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return CustomField.DataType;
            }
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            var rule = (IRulePresenter)component;
            rule[CustomField.Name] = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}