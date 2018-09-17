namespace OrderFormService
{
    using System;

    using MeetUp.Common;

    public class OrderFormService : IOrderFormService
    {
        /// <inheritdoc />
        public ServiceResponse ConvertToHTML(string customerOrderXml, int trackingId, Guid orderId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string GetOrderQuestionsHtml(Guid customerOrderId)
        {
            throw new NotImplementedException();
        }
    }
}