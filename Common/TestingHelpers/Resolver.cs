namespace TestingHelpers
{
    using StructureMap;

    public class Resolver : IResolver
    {
        private readonly Container _container;

        public Resolver(Container container)
        {
            _container = container;
        }

        public T ResolveType<T>()
        {
            return _container.GetInstance<T>();
        }
    }
}