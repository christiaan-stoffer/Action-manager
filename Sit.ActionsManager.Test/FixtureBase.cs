using NUnit.Framework;
using Sit.ActionsManager.Externals;
using StructureMap;

namespace Sit.ActionsManager.Test
{
    public abstract class FixtureBase
    {
        [TestFixtureSetUp]
        public void SetupStructureMap()
        {
            TestContext.Current = new TestContext();

            ObjectFactory
                .Initialize(InitStructureMap);
        }

        private static void InitStructureMap(IInitializationExpression x)
        {
            x.For<IFileSystem>().Use(() => TestContext.Current.FileSystemMoq.Object);
        }
    }
}