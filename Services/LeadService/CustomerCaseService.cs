namespace CustomerCaseService
{
    using System;
    using System.Linq;

    using MeetUp.DAL;
    using MeetUp.Enumerations;
    using MeetUp.Model;

    using NLog;

    public class CustomerCaseService : ICustomerCaseService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICustomerCaseRepository _customerCaseRepository;

        public CustomerCaseService(ICustomerCaseRepository customerCaseRepository)
        {
            this._customerCaseRepository = customerCaseRepository;
        }

        public CustomerCaseServiceResponse SaveCustomerCase(CustomerCase customerCase)
        {
            Logger.Debug("BLL: SaveCustomerCase : customerId : " + customerCase?.FromCustomerId ?? "#NULL#");

            if (customerCase != null) // TODO: throw ArgumentNulLException instead
            {
                CustomerCase customerCaseModel = null;

                if (customerCase.CaseId > 0)
                    customerCaseModel = this._customerCaseRepository.All.FirstOrDefault(i => i.CaseId == customerCase.CaseId);

                if (customerCaseModel != null)
                {

                    customerCaseModel.Tracking.ModifiedDateTimeUtc = DateTime.UtcNow; // TODO: TASK: update tracking object with user id

                    if (customerCase.FromUserId != Guid.Empty)
                        customerCaseModel.FromUserId = customerCase.FromUserId;

                    if (customerCase.ForSupplierId > 0)
                        customerCaseModel.ForSupplierId = customerCase.ForSupplierId;

                    if (customerCase.FromCustomerId > 0)
                        customerCaseModel.FromCustomerId = customerCase.FromCustomerId;

                    if (customerCase.FromDepartmentId > 0)
                        customerCaseModel.FromDepartmentId = customerCase.FromDepartmentId;

                    this._customerCaseRepository.Update(customerCaseModel);
                    this._customerCaseRepository.SaveChanges();

                    return new CustomerCaseServiceResponse
                    {
                        IsSuccess = true,
                        CaseId = customerCase.CaseId,
                        Message = ResponseMessage.RecordUpdated
                    };
                }
                else
                {
                    customerCase.Tracking.CreatedDateTimeUtc = DateTime.UtcNow;  // ??? this one is bad
                    customerCase.Tracking.ModifiedDateTimeUtc = DateTime.UtcNow; // TODO: TASK: update tracking object with user id

                    if (customerCase.CaseHistory != null && customerCase.CaseHistory.Count > 0)
                    {
                        foreach (var caseStatusEntry in customerCase.CaseHistory)
                        {
                            if (caseStatusEntry.EntryId == 0)
                            {
                                caseStatusEntry.Tracking.CreatedDateTimeUtc = DateTime.UtcNow;  // ??? this one is bad
                                caseStatusEntry.Tracking.ModifiedDateTimeUtc = DateTime.UtcNow; // TODO: TASK: update tracking object with user id
                            }
                        }
                    }

                    // TODO: this code is again quite bad and doing some magic with entities

                    var newCase = this._customerCaseRepository.All.FirstOrDefault(d => d.OrderId == customerCase.OrderId && customerCase.OrderId != null);
                    if (newCase == null)
                    {
                        this._customerCaseRepository.Insert(customerCase);
                        this._customerCaseRepository.SaveChanges();
                    }
                    else
                    {
                        return new CustomerCaseServiceResponse
                        {
                            CaseId = newCase.CaseId,
                            IsSuccess = true,
                            Message = "Case Exists"
                        };
                    }
                }

                return new CustomerCaseServiceResponse
                {
                    IsSuccess = true,
                    CaseId = customerCase.CaseId,
                    Message = ResponseMessage.RecordSaved
                };
            }

            // WARN: this will throw null-ref...
            // TODO: throw ArgumentNulLException instead on top of the function, this is not recoverable bug

            // if parameters are null then sending error response
            return new CustomerCaseServiceResponse
            {
                IsSuccess = false,
                CaseId = customerCase.CaseId,
                Message = ResponseMessage.InvalidParam
            };
        }
    }
}
