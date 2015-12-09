using Sabio.Web.Models.Requests.Plans;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Tags;

namespace Sabio.Web.Controllers.Api
{
    //[Authorize]
    [RoutePrefix("api/plans")]
    public class PlansApiController : ApiController
    {
        private IUserService _userService;
        private IPlansService _plansService;
        private ITagsService _tagsService;

        public PlansApiController(IUserService userService, IPlansService plansService, ITagsService tagsService)
        {
            this._userService = userService;
            this._plansService = plansService;
            this._tagsService = tagsService;
        }

        [Route("addPlan"), HttpPost]
        public HttpResponseMessage AddPlan(PlansAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (model.Tag != null)
            {
                TagsAddRequest tagRequest = new TagsAddRequest(model.Tag);
                int tagId = _tagsService.Add(tagRequest);
                model.TagId = tagId;
            }

            string UserId = _userService.GetCurrentUserId();
            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _plansService.Insert(model, UserId);

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage UpdatePlan(PlansUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (model.Tag != null)
            {
                TagsAddRequest tagRequest = new TagsAddRequest(model.Tag);
                int tagId = _tagsService.Add(tagRequest);
                model.TagId = tagId;
            }

            _plansService.Update(model);

            SuccessResponse response = new SuccessResponse();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("getAllPlans"), HttpGet]
        public HttpResponseMessage GetPlans()
        {
            ItemsResponse<Plan> response = new ItemsResponse<Plan>();

            response.Items = _plansService.GetAllPlans();

            return Request.CreateResponse(response);
        }

        [Route("getPlanById/{Id:int}"), HttpGet]
        public HttpResponseMessage GetPlanById(int Id)
        {
            ItemResponse<Plan> response = new ItemResponse<Plan>();

            response.Item = _plansService.GetPlan(Id);

            return Request.CreateResponse(response);
        }

        [Route("Delete/{Id:int}"), HttpDelete]
        public HttpResponseMessage DeletePlan(int Id)
        {
            _plansService.DeletePlan(Id);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("{planId:int}/features"), HttpPost]
        public HttpResponseMessage AddPlanFeature(PlanFeatureAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            string userId = _userService.GetCurrentUserId();

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _plansService.Insert(model, userId);

            return Request.CreateResponse(response);
        }

        [Route("{planId:int}/features/{featureId:int}"), HttpPut]
        public HttpResponseMessage UpdatePlanFeature(PlanFeatureUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            string userId = _userService.GetCurrentUserId();

            _plansService.Update(model, userId);
            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("features"), HttpGet]
        public HttpResponseMessage GetFeatures()
        {
            ItemsResponse<PlanFeature> response = new ItemsResponse<PlanFeature>();

            response.Items = _plansService.GetAllPlanFeatures();

            return Request.CreateResponse(response);
        }

        [Route("{planId:int}/features"), HttpGet]
        public HttpResponseMessage GetFeatures(int planId)
        {
            ItemsResponse<PlanFeature> response = new ItemsResponse<PlanFeature>();

            response.Items = _plansService.GetPlanFeaturesById(planId);

            return Request.CreateResponse(response);
        }

        [Route("features/{featureId:int}"), HttpGet]
        public HttpResponseMessage GetFeature(int featureId)
        {
            ItemResponse<PlanFeature> response = new ItemResponse<PlanFeature>();

            response.Item = _plansService.GetPlanFeature(featureId);

            return Request.CreateResponse(response);
        }

        [Route("features/{featureId:int}"), HttpDelete]
        public HttpResponseMessage DeleteFeature(int featureId)
        {
            _plansService.DeletePlanFeature(featureId);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("addType"), HttpPost]
        public HttpResponseMessage InsertPlanType(PlanTypeAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _plansService.InsertPlanType(model);

            return Request.CreateResponse(response);

        }

        [Route("type/{typeId:int}/edit"), HttpPut]
        public HttpResponseMessage UpdatePlanType(PlanTypeUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _plansService.UpdatePlanType(model);
            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [Route("getType/{typeId:int}"), HttpGet]
        public HttpResponseMessage GetPlanTypesById(int typeId)
        {
            ItemResponse<PlanType> response = new ItemResponse<PlanType>();

            response.Item = _plansService.GetPlanTypesById(typeId);

            if (response == null)
            {
                ErrorResponse er = new ErrorResponse("NOPE, NADA, YOU FAIL!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            return Request.CreateResponse(response);
        }

        [Route("headers"), HttpGet]
        public HttpResponseMessage GetAllPlanTypes()
        {
            ItemsResponse<PlanType> response = new ItemsResponse<PlanType>();

            response.Items = _plansService.GetAllPlanTypes();

            if (response == null)
            {
                ErrorResponse er = new ErrorResponse("NOPE, NADA, YOU FAIL!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            return Request.CreateResponse(response);
        }

        [Route("planswithfeatures"), HttpGet]
        public HttpResponseMessage GetAllPlansWithFeatures()
        {
            ItemsResponse<Plan> response = new ItemsResponse<Plan>();

            response.Items = _plansService.GetAllPlansWithFeatures();

            return Request.CreateResponse(response);
        }

        [Route("PlansWithFeaturesNotSubscribed"), HttpGet]
        public HttpResponseMessage PlansWithFeaturesNotSubscribed()
        {
            ItemsResponse<Plan> response = new ItemsResponse<Plan>();
            string userId = _userService.GetCurrentUserId();
            response.Items = _plansService.GetPlansWithFeaturesNotSubscribed(userId);

            return Request.CreateResponse(response);
        }

        [Route("Type/{type:int}"), HttpGet]
        public HttpResponseMessage GetPlansByType(int type)
        {
            ItemsResponse<Plan> response = new ItemsResponse<Plan>();

            response.Items = _plansService.GetPlansByType(type);

            if (response == null)
            {
                ErrorResponse er = new ErrorResponse("NOPE, NADA, YOU FAIL!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            return Request.CreateResponse(response);
        }

        [Route("DeletePlanWithFeatures/{Id:int}"), HttpDelete]
        public HttpResponseMessage DeletePlanWithFeatures(int Id)
        {
            _plansService.DeletePlanWithFeatures(Id);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("DeletePlanType/{typeId:int}"), HttpDelete]
        public HttpResponseMessage DeletePlanType(int typeId)
        {
            _plansService.DeletePlanType(typeId);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("usersubcription/{PlanId:int}"), HttpPost]
        public HttpResponseMessage UserSubcriptionToPlan(UserPlanSubscribe model, int PlanId)
        {
            string UserId = _userService.GetCurrentUserId();
            bool Subscribed = true;
            _plansService.UserSubcriptionToPlan(model, UserId, Subscribed, PlanId);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("getPriorityLevels"), HttpGet]
        public HttpResponseMessage GetPriorityLevels()
        {
            ItemsResponse<PriorityLevel> response = new ItemsResponse<PriorityLevel>();

            response.Items = _plansService.GetPriorityLevels();

            if (response == null)
            {
                ErrorResponse er = new ErrorResponse("NOPE, NADA, YOU FAIL!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            return Request.CreateResponse(response);
        }

        [Route("GetPlanCommentsByUserId"), HttpGet]
        public HttpResponseMessage GetPlanCommentsByUserId()
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string UserId = _userService.GetCurrentUserId();
            ItemResponse<List<UserComment>> response = new ItemResponse<List<UserComment>>();

            response.Item = _plansService.SelectPlanCommentsByUserId(UserId);

            return Request.CreateResponse(response);
        }
    }
}