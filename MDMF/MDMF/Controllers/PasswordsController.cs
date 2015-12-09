using Sabio.Web.Models.ViewModels;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    [RoutePrefix("account")]
    public class PasswordsController : BaseController
    {
        public override ViewType ViewType
        {
            get
            {
                return ViewType.Passwords;
            }
        }

        public PasswordsController(IUserService uService)
            : base(uService)
        {

        }

        [Route("Forgotpassword")]
        public ActionResult ForgotPassword()
        {
            BaseViewModel model = GetViewModel<BaseViewModel>();
            return View(model);
        }

        [Route("Forgotpassword/{resetToken:guid}")]
        public ActionResult ResetPassword(Guid resetToken)
        {
            bool exists = TokensService.Exists(resetToken);

            if (!exists)
            {
                return View("Error_404");
            }

            ItemViewModel<Guid> model = new ItemViewModel<Guid>();
            model.Item = resetToken;
            return View(model);
        }

        public ActionResult Error_404()
        {
            return View();
        }
    }


}