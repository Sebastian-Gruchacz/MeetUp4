namespace OrderService.Tests
{
    using StructureMap;

    using TestingHelpers;

    public abstract class BaseInit
    {
        protected BaseInit()
        {
            var container = new Container();
            Resolver = new Resolver(container);

            RegisterTypes(container);
        }

        private void RegisterTypes(Container container)
        {

        }

        protected IResolver Resolver { get; private set; }
    }
}