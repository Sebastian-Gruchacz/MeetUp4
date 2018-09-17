namespace OrderFormService.OtherServices
{
    using System;

    using MeetUp.Model;

    public interface IUserAccountService
    {
        AspNetUser GetUser(Guid userid);
    }
}