using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using NHibernate.Context;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Models;
using Plateforme.AlloTabib.DomainLayer.Models.Base;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace Plateforme.AlloTabib.ServiceLayer.Filters
{
    /// <summary>
    /// Action Filter to implement the Nhibernate session per request
    /// </summary>
    /// <summary>
    /// Action Filter to implement the Nhibernate session per request
    /// </summary>
    public class NhSessionManagementAttribute : ActionFilterAttribute
    {
        public ISessionFactoriesManager SessionFactories { get; set; }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            SessionFactories = (ISessionFactoriesManager)GlobalConfiguration.Configuration
                                .DependencyResolver.GetService(typeof(ISessionFactoriesManager));
            foreach (var sessionFactory in SessionFactories.GetSessionFactories())
            {
                var session = sessionFactory.Value.OpenSession();
                CurrentSessionContext.Bind(session);
                session.BeginTransaction();
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            foreach (var sessionFactory in SessionFactories.GetSessionFactories())
            {
                var session = CurrentSessionContext.Unbind(sessionFactory.Value);
                var transaction = session.Transaction;

                if (actionExecutedContext.Exception != null && transaction != null && transaction.IsActive)
                {
                    transaction.Rollback();
                    session.Close();
                    return;
                }

                if (actionExecutedContext.Response != null && actionExecutedContext.Response.StatusCode == HttpStatusCode.InternalServerError &&
                    transaction != null && transaction.IsActive)
                {
                    transaction.Rollback();
                    session.Close();
                    return;
                }

                if (transaction != null && transaction.IsActive)
                {
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        // TODO: You have to choose if you want to rollback in all the sessions in case of exception!
                        transaction.Rollback();
                        var result = new Plateforme.AlloTabib.DomainLayer.Models.ContentResult
                        {
                            Errors = new System.Collections.Generic.List<ErrorBase>
                        {
                            new InternalError
                            {
                                Exception = exception
                               
                            }
                        },
                            Status = "Error",
                            StatusDetail = "InternalServerError"
                        };

                        actionExecutedContext.ActionContext.Response.StatusCode =
                            System.Net.HttpStatusCode.InternalServerError;
                        var objectContent = actionExecutedContext.ActionContext.Response.Content as ObjectContent;
                        if (objectContent != null)
                            objectContent.Value = result;
                    }
                }

                session.Close();
            }
        }
    }
}