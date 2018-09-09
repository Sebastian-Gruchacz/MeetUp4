namespace MeetUp.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>The concept is fine, but why this is not generic? Another exerciser ;-)</remarks>
    public class ServiceResponse
    {
        public ServiceResponse()
        {
        }

        public ServiceResponse(bool isSuccess, string message, object obj = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = obj;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Data { get; set; } // TODO: made generic
    }
}