﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Facebook;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.AlloTabibUserServices;
using Plateforme.AlloTabib.ServiceLayer.Models;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
    [RoutePrefix("account")]
  
    public class UserAccountController : ApiController
    {
            #region Private Properties
            private readonly IAlloTabibUserAppServices _userApiApplicationServices;
            private readonly IFacebookUserAppServices _facebookUserAppServices;

            #endregion


            #region Constructor

                public UserAccountController(IAlloTabibUserAppServices userApiApplicationServices, IFacebookUserAppServices facebookUserAppServices)
                {
                    if (userApiApplicationServices == null)
                        throw new ArgumentNullException("userApiApplicationServices");
                    _userApiApplicationServices = userApiApplicationServices;
                    if (facebookUserAppServices ==null)
                        throw new ArgumentNullException("facebookUserAppServices");
                    _facebookUserAppServices = facebookUserAppServices;
                }

            #endregion


                [Route("ispraticien")]
                [HttpPost]
                public HttpResponseMessage PostIsPraticienAuthenticated(UserDTO user)
                {
                    var statusCode = HttpStatusCode.OK;
                    if (user == null)
                    {
                        user = new UserDTO();
                    }
                    var result = _userApiApplicationServices.IsPraticienAuthenticated(user.UserName, user.Password);

                    return Request.CreateResponse(statusCode, result);
                }


                [Route("ispatient")]
                [HttpPost]
                public HttpResponseMessage PostIsPatientAuthenticated(UserDTO user)
                {
                    if (user == null)
                    {
                        user = new UserDTO();
                    }
                    var result = _userApiApplicationServices.IsPatientAuthenticated(user.UserName, user.Password);

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                [Route("motdepasseoublie")]
                [HttpGet]
                public HttpResponseMessage SendPasswordByEmail(string username)
                {

                    var statusCode = HttpStatusCode.OK;
                    var result = _userApiApplicationServices.SendPasswordByEmail(username);
                    if (result != null)
                        statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

                    return Request.CreateResponse(statusCode, result);
                }

                [Route("mailmotdepasse")]
                [HttpPost]
                public HttpResponseMessage SendPasswordByEmailPost(string username)
                {

                    var statusCode = HttpStatusCode.OK;
                    var result = _userApiApplicationServices.SendPasswordByEmail(username);
                    if (result != null)
                        statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

                    return Request.CreateResponse(statusCode, result);
               
                }


                [Route("login")]
                [HttpGet]
                public HttpResponseMessage IsPraticienAuthenticated(UserDTO user)
                {
                    var statusCode = HttpStatusCode.OK;
                    var result = _userApiApplicationServices.IsPraticienAuthenticated(user.UserName, user.Password);
                  

                    return Request.CreateResponse(statusCode, result);
                }

             
                [Route("new")]
            
                public HttpResponseMessage PostAddUser(UserDTO user)
                {
                    var statusCode = HttpStatusCode.OK;

                    var result = _userApiApplicationServices.AddAlloTabibUser(user.UserName, user.Password,user.Type);
                    if (result != null)
                        statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

                    return Request.CreateResponse(statusCode, result);
                }


                [Route("facebookuserprofile")]

                public HttpResponseMessage GetFacebookUserProfile(UserDTO user)
                {
                    var statusCode = HttpStatusCode.OK;
                    var facebook = new FacebookClient();
                    var result = _facebookUserAppServices.GetFacebookUserProfile(facebook);
                    if (result != null)
                        statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

                    return Request.CreateResponse(statusCode, result);
                }

         
                [Route("getuserbyemail")]
                [HttpPost]
                public HttpResponseMessage GetUserByEmail(string email)
                {

                    var statusCode = HttpStatusCode.OK;
                    var result = _userApiApplicationServices.GetUserByEmail(email);
                    if (result != null)
                        statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

                    return Request.CreateResponse(statusCode, result);

                }

                [Route("getallemailsubscriber")]
                [HttpGet]
                public HttpResponseMessage GetAllEmailsSubscriber()
                {

                    var statusCode = HttpStatusCode.OK;
                    var result = _userApiApplicationServices.GetAllEmailsSubscriber();
                    if (result != null)
                        statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

                    return Request.CreateResponse(statusCode, result);

                }

    }
}
