namespace OrderService
{
    using System;

    public interface IOrderFormService
    {
        void ConvertToHTML(string customerOrderXml, int trackingId, Guid orderId);
        string GetOrderQuestionsHtml(Guid customerOrderId);
    }
}