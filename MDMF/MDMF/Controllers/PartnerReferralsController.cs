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
    [RoutePrefix("Partner")]
    public class PartnerReferralsController : BaseController
    {
        public override ViewType ViewType
        {
            get
            {
                return ViewType.PartnerReferrals;
            }
        }

        public PartnerReferralsController(IUserService uService)
            : base(uService)
        {

        }


        [Route]
        public ActionResult ReferralsPage()
        {
            BaseViewModel model = GetViewModel<BaseViewModel>();

            return View(model);
        }


        [Route("referral/{referralId:int}")]
        public ActionResult ViewById(int referralId)
        {
            ItemViewModel<int> model = GetViewModel<ItemViewModel<int>>();
            model.Item = referralId;
            return View(model);
        }


        [Route("update/{referralId:int}")]
        public ActionResult UpdateReferral(int referralId)
        {
            ItemViewModel<int> model = GetViewModel<ItemViewModel<int>>();
            model.Item = referralId;
            return View(model);
        }


        [Route("add")]
        public ActionResult AddReferral()
        {
            BaseViewModel model = GetViewModel<BaseViewModel>();

            return View(model);
        }


        [Route("Type/{typeId:int}")]
        public ActionResult ViewByPartnerType(int typeId)
        {
            ItemViewModel<int> model = GetViewModel<ItemViewModel<int>>();
            model.Item = typeId;
            return View(model);
        }

    }
}
