namespace OrderFormService
{
    using System.Collections.Generic;

    using global::OrderFormService.ExternalModel;

    public class ExternalOrderFormServiceResponseDto
    {
        public string http_status { get; set; }

        public Stat stats { get; set; }

        public List<Question> questions { get; set; }

        public List<UserResponse> responses { get; set; }
    }
}