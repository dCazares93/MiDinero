using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Users;
using Sabio.Web.Models.Requests.LogIn;
using Sabio.Web.Exceptions;
using Sabio.Web.Models.Requests.ResetPassword;
using Sabio.Web.Models;

namespace Sabio.Web.Controllers.Api
{

    [RoutePrefix("api/users")]
    public class UsersApiController : ApiController
    {
        private IUserService _userService;
        private IPlansService _plansService;

        public UsersApiController(IUserService service, IPlansService service2)
        {
            this._userService = service;
            this._plansService = service2;
        }

        [Route("forgotPassword"), HttpPost]
        public HttpResponseMessage forgotPassword(VerifyEmailRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SuccessResponse response = new SuccessResponse();
            ApplicationUser userVerification = _userService.GetUser(model.Email);

            if (userVerification == null)
            {
                ErrorResponse er = new ErrorResponse("Cannot find a user with that email.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            string userId = userVerification.Id;
            TokensRequest request = new TokensRequest();
            request.UserId = userId;
            request.Token = Guid.NewGuid();
            request.TokenTypeId = 1;

            ItemResponse<int> tokenAddResponse = new ItemResponse<int>();
            tokenAddResponse.Item = TokensService.Insert(request, userId);

            if (tokenAddResponse.Item <= 0)
            {
                ErrorResponse er = new ErrorResponse("A new token was not inserted.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            Guid uniqueId = request.Token;
            EmailRequest emailRequest = new EmailRequest();
            emailRequest.Email = model.Email;
            emailRequest.Subject = "Password Reset";
            bool emailSent = MailService.resetPasswordEmail(emailRequest, uniqueId);

            if (!emailSent)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "The reset password email failed to send.");
            }

            return Request.CreateResponse(response);
        }

        [Route("resetPassword/{ResetToken:Guid}"), HttpPut]
        public HttpResponseMessage PasswordReset(NewPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SuccessResponse response = new SuccessResponse();
            UserTokens thisToken = TokensService.GetById(model.ResetToken);
            string userId = thisToken.UserId;
            bool changed = _userService.ChangePassWord(userId, model.ConfirmPassword);

            if (!changed)
            {
                ErrorResponse er = new ErrorResponse("There was something wrong with your password format.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            bool deleted = TokensService.DeleteToken(model.ResetToken);

            if (!deleted)
            {
                ErrorResponse er = new ErrorResponse("Your token wasn't deleted.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, er);
            }

            return Request.CreateResponse(response);
        }

    }
}