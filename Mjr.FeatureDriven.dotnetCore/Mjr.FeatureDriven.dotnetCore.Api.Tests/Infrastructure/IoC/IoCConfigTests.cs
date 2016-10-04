using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using StructureMap;
using Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.IoC;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.IoC
{
    public class IoCConfigTests
    {
        public void AssertConfigurationIsValid()
        {
            IoCConfig.Container.AssertConfigurationIsValid();
        }

        public void AssertConfigurationIsInValid()
        {
            var container = new Container(x => x.For<IWidget>().Use<NamedWidget>());
            Should.Throw<StructureMapConfigurationException>(() => container.AssertConfigurationIsValid());
        }


        public interface IWidget
        {
            void DoSomething();
        }
        public class NamedWidget : IWidget
        {
            private readonly string _name;

            public void DoSomething()
            {
                throw new NotImplementedException();
            }
            //Name cannot be resolved by the IoC
            public NamedWidget(string name)
            {
                _name = name;
            }

            public string Name
            {
                get { return _name; }
            }
        }
    }
}
