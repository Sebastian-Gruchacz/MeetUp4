namespace OrderFormService
{
    using System;

    using MeetUp.Common;

    public interface IOrderFormService
    {
        ServiceResponse ConvertToHTML(string customerOrderXml, int caseId, Guid orderId);

        string GetOrderQuestionsHtml(Guid customerOrderId);
    }
}