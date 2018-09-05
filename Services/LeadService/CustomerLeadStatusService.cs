namespace LeadService
{
    using System;
    using System.Linq;

    using MeetUp.BLL;
    using MeetUp.Enumerations;
    using MeetUp.Model;

    using NLog;

    public class CustomerLeadStatusService : ICustomerLeadStatusService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICustomerLeadStatusRepository _customerLeadStatusRepository;

        public CustomerLeadStatusService(ICustomerLeadStatusRepository customerLeadStatusRepository)
        {
            _customerLeadStatusRepository = customerLeadStatusRepository;
        }

        public CustomerLeadServiceResponse SaveCustomerLead(CustomerLeadStatus customerLead)
        {
            Logger.Debug("BLL: SaveCustomerLead : customerId : " + customerLead?.FromCustomerId ?? "#NULL#");

            if (customerLead != null)
            {
                CustomerLeadStatus customerLeadModel = null;

                if (customerLead.LeadTrackingId > 0)
                    customerLeadModel = _customerLeadStatusRepository.All.FirstOrDefault(i => i.LeadTrackingId == customerLead.LeadTrackingId);

                if (customerLeadModel != null)
                {

                    customerLeadModel.ModifiedDateTimeUtc = DateTime.UtcNow;

                    if (customerLead.FromUserId != Guid.Empty)
                        customerLeadModel.FromUserId = customerLead.FromUserId;

                    if (customerLead.ForSupplierId > 0)
                        customerLeadModel.ForSupplierId = customerLead.ForSupplierId;

                    if (customerLead.FromCustomerId > 0)
                        customerLeadModel.FromCustomerId = customerLead.FromCustomerId;

                    if (customerLead.FromDepartmentId > 0)
                        customerLeadModel.FromDepartmentId = customerLead.FromDepartmentId;

                    _customerLeadStatusRepository.Update(customerLeadModel);
                    _customerLeadStatusRepository.SaveChanges();

                    return new CustomerLeadServiceResponse
                    {
                        IsSuccess = true,
                        TrackingLeadId = customerLead.LeadTrackingId,
                        Message = ResponseMessage.RecordUpdated
                    };
                }
                else
                {
                    customerLead.CreatedDateTimeUtc = DateTime.UtcNow;
                    customerLead.ModifiedDateTimeUtc = DateTime.UtcNow;

                    if (customerLead.CustomerLeadStatusDetail != null && customerLead.CustomerLeadStatusDetail.Count > 0)
                    {
                        foreach (var leadDetail in customerLead.CustomerLeadStatusDetail)
                        {
                            if (leadDetail.TransactionId == 0)
                            {
                                leadDetail.CreatedUTCDateTime = DateTime.UtcNow;
                                leadDetail.LastUpdatedUTCDateTime = DateTime.UtcNow;
                            }
                        }
                    }
                    var lead = _customerLeadStatusRepository.All.FirstOrDefault(d => d.OrderId == customerLead.OrderId && customerLead.OrderId != null);
                    if (lead == null)
                    {
                        _customerLeadStatusRepository.Insert(customerLead);
                        _customerLeadStatusRepository.SaveChanges();
                    }
                    else
                    {
                        return new CustomerLeadServiceResponse
                        {
                            TrackingLeadId = lead.LeadTrackingId,
                            IsSuccess = true,
                            Message = "Lead Exists"
                        };
                    }
                }

                return new CustomerLeadServiceResponse
                {
                    IsSuccess = true,
                    TrackingLeadId = customerLead.LeadTrackingId,
                    Message = ResponseMessage.RecordSaved
                };
            }


            // if parameters are null then sending error response
            return new CustomerLeadServiceResponse
            {
                IsSuccess = false,
                TrackingLeadId = customerLead.LeadTrackingId,
                Message = ResponseMessage.InvalidParam
            };
        }
    }
}
