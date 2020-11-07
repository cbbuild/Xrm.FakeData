using CbBuild.Xrm.FakeData.Presenter;
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
            FakeDataPresenter p = new FakeDataPresenter();
            p.x();
        }
    }
}
