using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    internal class RulePresenterTypeDescriptor : CustomTypeDescriptor
    {
        public RulePresenterTypeDescriptor() : base()
        {

        }
        public RulePresenterTypeDescriptor(ICustomTypeDescriptor parent, object instance) : base(parent)
        {
            var rule = (RulePresenter)instance;

            customFields.Add(new RuleParameterPropertyDescriptor(new RuleParameter("datka", typeof(DateTime))));
            // generate some fields
    //        customFields.AddRange(CustomFieldsGenerator.GenerateCustomFields(title.Category)
    //.Select(f => new CustomFieldPropertyDescriptor(f)).Cast<PropertyDescriptor>());
        }

        private List<PropertyDescriptor> customFields = new List<PropertyDescriptor>();
        public override PropertyDescriptorCollection GetProperties()
        {
            var x = base.GetProperties();
            return new PropertyDescriptorCollection(base.GetProperties()
                .Cast<PropertyDescriptor>().Union(customFields).ToArray());
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var x = base.GetProperties(attributes);
            return new PropertyDescriptorCollection(base.GetProperties(attributes)
                .Cast<PropertyDescriptor>().Union(customFields).ToArray());
        }
    }
}

//internal static IEnumerable<CustomField> GenerateCustomFields(TitleCategory category)
//{
//    List<CustomField> customFields = new List<CustomField>();

//    switch (category)
//    {
//        case TitleCategory.Book:
//            customFields.Add(new CustomField("Author", typeof(String)));
//            customFields.Add(new CustomField("HardCover", typeof(bool)));
//            customFields.Add(new CustomField("Amazon Rank", typeof(int)));
//            break;

//        case TitleCategory.Movie:
//            customFields.Add(new CustomField("Director", typeof(String)));
//            customFields.Add(new CustomField("Rating", typeof(MovieRating)));
//            customFields.Add(new CustomField("Duration", typeof(TimeSpan)));
//            customFields.Add(new CustomField("Release Date", typeof(DateTime)));
//            break;
//    }

//    return customFields;
//}