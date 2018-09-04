namespace OrderService.Tests
{
    using NUnit.Framework;

    public class ServiceSimulation : BaseInit
    {
        [TestCase(24, @"New Order,Nyt Bestil", 1440, false, "orders@mycompany.fake", @"support@mycompany.fake")]
        public void reprocess_specific_order(int dayTotalHours, string emailSubject, int dayTotalMinutes, bool isHourBased, string orderFromEmail, string supportEmail)
        {
            // arrange
            var service = Resolver.ResolveType<ICustomerOrder>();

            // act - these numbers are normally taken from "App.config"
            service.SendCustomerOrders(dayTotalHours, emailSubject, dayTotalMinutes, isHourBased, orderFromEmail, supportEmail);

            // yep, assert not explode
        }
    }
}
