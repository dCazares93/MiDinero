using Sabio.Web.Models.ViewModels;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    //[Authorize]
    [RoutePrefix("plans")]
    public class PlansController : BaseController
    {
        public override ViewType ViewType
        {
            get
            {
                return ViewType.Plans;
            }
        }

        public PlansController(IUserService uService)
            : base(uService)
        {

        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("AddPlan")]
        [Route("{Id:int}/edit")]
        public ActionResult AddPlan(int? Id = 0)
        {
            ItemViewModel<int?> model = GetViewModel<ItemViewModel<int?>>();
            model.Item = Id;
            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("{planId:int}/tasks/add")]
        [Route("{planId:int}/tasks/{featureId:int}/edit")]
        public ActionResult AddFeatureIndex(int planId, int? featureId = 0)
        {
            PairItemViewModel<int, int?> model = GetViewModel<PairItemViewModel<int, int?>>();

            model.PrimaryItem = planId;
            model.SecondaryItem = featureId;

            return View(model);
        }

        public ActionResult Index()
        {
            BaseViewModel model = GetViewModel<BaseViewModel>();

            return View("Plans", model);
        }

        [Route("tasks")]
        public ActionResult AllTasks()
        {
            BaseViewModel model = GetViewModel<BaseViewModel>();

            return View(model);
        }

        [Route("{planId:int}/tasks")]
        public ActionResult Features(int planId)
        {
            ItemViewModel<int> model = GetViewModel<ItemViewModel<int>>();

            model.Item = planId;

            return View(model);

        }

        [Route("{planId:int}/tasks/{featureId:int}")]
        public ActionResult Feature(int planId, int featureId)
        {
            PairItemViewModel<int, int?> model = GetViewModel<PairItemViewModel<int, int?>>();

            model.PrimaryItem = planId;
            model.SecondaryItem = featureId;

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("Dashboard")]
        public ActionResult PlansDashboard()
        {
            return View("PlansDashboardV2");
        }

        [Route("{PlanType}")]
        public ActionResult PlansByType(string PlanType)
        {
            ItemViewModel<string> model = GetViewModel<ItemViewModel<string>>();
            model.Item = PlanType;

            return View(model);
        }

        [Route("types")]
        public ActionResult PlanCategories()
        {
            return View();
        }

    }
}