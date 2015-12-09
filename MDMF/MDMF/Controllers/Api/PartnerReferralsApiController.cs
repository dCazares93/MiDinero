using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Referrals;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    //[Authorize]
    [RoutePrefix("api/ReferralForm")]
    public class PartnerReferralApiController : ApiController
    {
        private IUserService _userService;
        private IReferralService _referralService;

        public PartnerReferralApiController(IUserService userService, IReferralService referralService)
        {
            this._userService = userService;
            this._referralService = referralService;

        }

        [Route, HttpPost]
        public HttpResponseMessage ReferralInsert(AddReferral model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = _userService.GetCurrentUserId();

            ItemResponse<int> response = new ItemResponse<int>();
            response.Item = _referralService.Insert(model, userId);

            return Request.CreateResponse(response);
        }

        [Route("{referralId:int}"), HttpPut]
        public HttpResponseMessage ReferralUpdate(UpdateReferral model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = _userService.GetCurrentUserId();

            SuccessResponse response = new SuccessResponse();

            _referralService.Update(model, userId);

            return Request.CreateResponse(response);
        }

        [Route("referrals"), HttpGet]
        public HttpResponseMessage GetAllReferrals()
        {
            ItemsResponse<Referral> response = new ItemsResponse<Referral>();

            response.Items = _referralService.Get();

            return Request.CreateResponse(response);
        }

        [Route("referrals/{referralId:int}"), HttpGet]
        public HttpResponseMessage GetReferralById(int referralId)
        {
            ItemResponse<Referral> response = new ItemResponse<Referral>();

            response.Item = _referralService.GetById(referralId);

            return Request.CreateResponse(response);
        }

        [Route("delete/{referralId:int}"), HttpDelete]
        public HttpResponseMessage DeleteReferral(int referralId)
        {
            _referralService.Delete(referralId);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(response);
        }

        [Route("Type/{typeId:int}"), HttpGet]
        public HttpResponseMessage GetByType(int typeId)
        {
            ItemsResponse<Referral> response = new ItemsResponse<Referral>();

            response.Items = _referralService.GetByPartnerType(typeId);

            return Request.CreateResponse(response);
        }

    }
}



