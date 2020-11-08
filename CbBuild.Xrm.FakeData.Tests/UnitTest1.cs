using CbBuild.Xrm.FakeData.Presenter;
using CbBuild.Xrm.FakeData.View.Controls;
using System;
using System.Linq;
using Xunit;

namespace CbBuild.Xrm.FakeData.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            RulePresenter rootRule = new RulePresenter();
            TreeViewRuleNode rootNode = new TreeViewRuleNode(rootRule);

            var child = new RulePresenter();
            rootRule.Add(child);
            rootRule.Name = "new name";

            child.Name = "child name";
            //rootRule.
        }
    }
}
