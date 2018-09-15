namespace CustomerCaseService
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using MeetUp.Common;
    using MeetUp.DalBase;
    using MeetUp.Enumerations;
    using MeetUp.Model;

    using NLog;

    public static class CustomerServiceLogic
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CustomerCaseServiceResponse SaveCustomerCase(IUnitOfWork work, CustomerCase customerCase)
        {
            Logger.Debug("BLL: SaveCustomerCase : customerId : " + customerCase?.FromCustomerId ?? "#NULL#");

            if (customerCase != null) // TODO: throw ArgumentNulLException instead
            {
                CustomerCase customerCaseModel = null;

                if (customerCase.CaseId > 0)
                {
                    customerCaseModel = work.Context.CustomerCases.FirstOrDefault(i => i.CaseId == customerCase.CaseId);
                }

                if (customerCaseModel != null)
                {
                    customerCaseModel.Tracking.UpdateTracking(SystemUsers.CustomerCaseService);

                    if (customerCase.FromUserId != Guid.Empty)
                        customerCaseModel.FromUserId = customerCase.FromUserId;

                    if (customerCase.ForSupplierId > 0)
                        customerCaseModel.ForSupplierId = customerCase.ForSupplierId;

                    if (customerCase.FromCustomerId > 0)
                        customerCaseModel.FromCustomerId = customerCase.FromCustomerId;

                    if (customerCase.FromDepartmentId > 0)
                        customerCaseModel.FromDepartmentId = customerCase.FromDepartmentId;

                    work.Context.CustomerCases.Update(customerCaseModel);

                    // no SaveChanges! service do not own UOF object!

                    return new CustomerCaseServiceResponse
                    {
                        IsSuccess = true,
                        CaseId = customerCase.CaseId,
                        Message = ResponseMessage.RecordUpdated
                    };
                }
                else
                {
                    customerCase.Tracking = EntityTracker.StartTracking(SystemUsers.CustomerCaseService);

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

                    var newCase = work.Context.CustomerCases.FirstOrDefault(d => d.OrderId == customerCase.OrderId && customerCase.OrderId != null);
                    if (newCase == null)
                    {
                        work.Context.CustomerCases.Add(customerCase); // Insert()?

                        // no SaveChanges! service do not own UOF object!
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