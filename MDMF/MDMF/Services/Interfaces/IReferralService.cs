using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Referrals;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Services
{
    public interface IReferralService
    {
        void Delete(int Id);
        List<Referral> Get();
        Referral GetById(int Id);
        List<Referral> GetByPartnerType(int Type);
        int Insert(AddReferral model, string userId);
        void Update(UpdateReferral model, string userId);
    }
}
