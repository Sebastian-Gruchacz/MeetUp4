namespace OrderService.Tests
{
    using System.Linq;

    using NUnit.Framework;

    using TestingHelpers;

    public class ConsoleSimulation : BaseInit
    {
        [TestCase(123)]
        public void reprocess_specific_order(int orderId)
        {
            // arrange
            var service = Resolver.ResolveType<ICustomerOrder>();

            // act
            service.ResendEmail(orderId.BuildSingleItemArray().ToList());

            // yep, assert not explode
        }
    }
}
