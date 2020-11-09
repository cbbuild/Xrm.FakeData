using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbBuild.Xrm.FakeData.Model
{
//    DynamicTypeDescriptor dt = new DynamicTypeDescriptor(typeof(Question));

//    Question q = new Question(); // initialize question the way you want    
//    if (q.Element == 0)
//    {
//        dt.RemoveProperty("Element");
//    }
//propertyGrid1.SelectedObject = dt.FromComponent(q);
//https://www.codeproject.com/Articles/22717/Using-PropertyGrid

    internal class RuleProperties
    {
        public RuleOperator Operator { get; set; }
    }
}
