namespace OrderService
{
    using System;

    using MeetUp.Common;

    public interface IOrderFormService
    {
        ServiceResponse ConvertToHTML(string customerOrderXml, int trackingId, Guid orderId);
        string GetOrderQuestionsHtml(Guid customerOrderId);
    }
}