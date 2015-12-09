using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Plans;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Services
{
    public interface IPlansService
    {
        int Insert(PlansAddRequest model, string UserId);
        void Update(PlansUpdateRequest model);
        List<Plan> GetAllPlans();
        List<Plan> GetAllPlansWithFeatures();
        List<Plan> GetPlansWithFeaturesNotSubscribed(string userId);
        List<Plan> GetPlansByType(int type);
        List<UserPlans> GetPlanByUserId(string userId);
        Plan GetPlan(int Id);
        void DeletePlan(int id);

        int Insert(PlanFeatureAddRequest model, string userId);
        void Update(PlanFeatureUpdateRequest model, string userId);
        List<PlanFeature> GetAllPlanFeatures();
        List<PlanFeature> GetPlanFeaturesById(int planId);
        PlanFeature GetPlanFeature(int id);
        void DeletePlanFeature(int id);

        int InsertPlanType(PlanTypeAddRequest model);
        void UpdatePlanType(PlanTypeUpdateRequest model);
        List<PlanType> GetAllPlanTypes();
        PlanType GetPlanTypesById(int typeId);
        void DeletePlanWithFeatures(int id);
        void DeletePlanType(int typeId);

        void UserSubcriptionToPlan(UserPlanSubscribe model, string UserId, bool Subscribed, int PlanId);
        List<PriorityLevel> GetPriorityLevels();
        List<UserComment> SelectPlanCommentsByUserId(string UserId);

    }
}
