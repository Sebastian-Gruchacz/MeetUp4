namespace OrderFormService.ExternalModel
{
    using System.Collections.Generic;

    public class UserResponse
    {
        public string completed { get; set; }

        public string token { get; set; }

        public HiddenFields hidden { get; set; }

        public Dictionary<string, string> answers { get; set; }
    }
}