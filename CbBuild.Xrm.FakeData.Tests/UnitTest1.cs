﻿using Xunit;

namespace CbBuild.Xrm.FakeData.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            // Niektóre generatory będą miały parametry, jak to rozwiązać? w linku ponizej trzeba zaimplementować
            // customowy typedescriptor który dla generatora zwróci odpowiednie parametry
            //https://www.codeproject.com/Articles/26992/Using-a-TypeDescriptionProvider-to-support-dynamic
            //https://stackoverflow.com/questions/11892064/propertygrid-with-custom-propertydescriptor

            // bedzie oparte to o dictionary z ktorego bedzie korzystal executor
            // np. Index powinien mieć startIndex itp.
            // gdzieś statyczna konfiguracja
            // na zmiane generatora property grid powienien lapac refresha (ale zachowac selected?):

            // serializacja bedzie raczej tylko dla preview, xml/csv

            //import na początek po service, moze kiedys datamaps
            Assert.True(true);
        }
    }
}