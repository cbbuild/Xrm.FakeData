using CbBuild.Xrm.FakeData.Ioc;
using Grace.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace CbBuild.Xrm.FakeData
{
    // ICONS LICENCE PAST TO REDME
    //<div>Icons made by<a href="https://www.flaticon.com/authors/freepik" title="Freepik"> Freepik</a> from<a href="https://www.flaticon.com/" title="Flaticon"> www.flaticon.com</a></div>

    //https://github.com/MscrmTools/XrmToolBox/wiki/Develop-your-own-custom-plugin-for-XrmToolBox#Common-logic

    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Fake Data"),
        ExportMetadata("Description", "Xrm fake data generator"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAA7AAAAOwBeShxvQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAWGSURBVFiF7ZdrbBRVFMd/d2a2+2qXlr6kgF2aPtBS5FEQqlbwRWNCCIGo0aj4yUcC0cToJ83GSIxRY0RDiBpFjBoeitqQKFQrKkHASltKNdDSB6ULSG27lN3udGavH7a7230VBY1f/CebzD33nPP/3zP3nrkruALIpsZSCN2CFDMIXKxFEadQ5DaxdNWBv5tL/E3iOpAvAgujxuELMQfV4kPTnhM3rdz0jwqQjY0aU+RrSDYkTU4UEIFmbcE8Wi2We4zL5VYuSy6lwCW3piRPByN4A9r8pr/iGq3A++vq3FJRkwTdtXLtA67s7BciY81iwW53xBwmVODiyCUMM7ZovxH64N2tH75AIhQMT/2PvQBaVIlU9wuTaxN9Gz7flRQ/bca13LFqDRG9+pjB1p2f0uc9m+j68PgvDtKkFbghrCVm/i6JKQ28fb30n+6Ojn/r6EhFnhYKsjHyHK2Aw+98zO+41DRiKrVBM1QAkJuXW2Gz2QtSJRnRY6UeGTOZWnhNSjLDNC/1nPH+AmBVFa/DojQqjuH3kgTcs3NnANi0YsWKYinlGgAGzqVdxbOzq5gdMkGanPYFeO/IiXSuTuAWACmlp62tbcvESS1lCDDn5uXkzShOsp9q/YXe9lZyRTcc3wJtTkpdJQC4q2uYUbUgKebciV85eeCblDxpBZRX11C+aCkAPkOSoQhsCuiBAL3trRSLIxAaBSOTSr0bgJlVC6le+xAAg7rEkJJ8q8LxffVpBcQdO9m4o9SdnxN3EoYNSYvPpGXYwJRhm1UN4cILhA02fwibGosZ1CW7vEE+O6vjM2TUfnuFe2XPm55nul73ZCcJkN9sfxXJyQXu6WsjNr8JrT4TU0KRTUEd7xpVBaMomDHGkOBud/gxYMIubxC/CWUOlSwt1mwLsxwLJbysaHwdJ0DW1zsQ4kkAqxZ+K3pwFLsK+RmCWQ6FmXYlap9/TSAcPaGR1xUpGHoQuwpVLo05LpXl+RYEYARHw5Uazy1hcc/ml3JiFZgScAIqQPm0PACa9tZzceACFZkqxePkZ7s6+PXgfuYX+knE9Q6V4/vqGeg9xZIcjdvywuRD3j5a9oSbWUXB1Ki/bkgNIpvQJACEAGVJWTHzioto7jzB5g3rkohc1hAlOXp4IGMlcJkWNF8/2x6/NykGYIm7iMrxxQH+YWvuUFwR5bc7fgBuDqsz2f5TCwdPndH9o8GMiE9Btov7F2Qwz/JVLHNLfvSxOWcmbxwNcG7oYtSW68oMzcufoqyeW4aqRLfcW+71nvXxAvbvmIXJTiZ+611T+xFKUWQ4NDBA8PQXFCoHARk+BK3jAoSkx+nELLmbEveE/qGooz3HjtkAE+gEse3SgHyl0uPR4wQASI9HoXbOdchQOQiN7KkbQZR1/tbOscOHGB76A4Asu05F0RDTskfI6nPiNSV7+y30+sJn0WqxsHjuHO5cVguqOiJuXZOV8r0kCkiEbPr26wMNX93V0X48rU/nhaG0c9ML8nj0kXWnRe3qpK9sBJNeSI78+H3TZOR2ZyZa8hUiijPnL7D7yz2dk3GkbcUAbc2HGmbOKq3Jyc2rJaFamqbhLqugureLw0ebGTOSb1+6rg90dnfuvSIB0rPMxvCgk6HDvViKg9SsteHKS/KbooTiNx2AlLDvHej82SDDmv/0E/mZYvPvI6l4EjYhCoM31iB4ELgPcMWkWmB+Hcyuic+QeCkdPg+fPA9+X8ymqBKrs4sM68dYrBvF1u7ROAHyqUWVIB5EioeAaemqAkBhCZQvhsxcEAIUDWQIAiNwaDe0NEwajmYxsdrb0Gxv81H/FiGfvPEkUDp51CTo+EuX39SwZIwpV0V+tRjTLZf9X/Bv438B/7kADSFfvqoMmTm3Xk34n0pr/Y2rmdp2AAAAAElFTkSuQmCC"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class FakeDataPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            var container = CreateContainer();
            return container.Locate<FakeDataPluginControl>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FakeDataPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to
            // hook into the event that will fire when an Assembly fails to resolve
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        private DependencyInjectionContainer CreateContainer()
        {
            var container = new DependencyInjectionContainer();
            var serviceLocator = new ServiceLocator(container);

            container.Configure(new DiConfigurationModule(serviceLocator));

            return container;
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.FirstOrDefault(a => a.Name == argName);

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
#pragma warning disable S3885 // "Assembly.Load" should be used
                    loadAssembly = Assembly.LoadFrom(assmbPath);
#pragma warning restore S3885 // "Assembly.Load" should be used
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}