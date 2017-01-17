using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace jForum
{
    //Source: http://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/
    public class ValidateModel : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> validate;

        public ValidateModel() : this(arguments => arguments.ContainsValue(null)) { }

        public ValidateModel(Func<Dictionary<string, object>, bool> checkCondition)
        {
            validate = checkCondition;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (validate(actionContext.ActionArguments))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The argument cannot be null");
            } else if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}