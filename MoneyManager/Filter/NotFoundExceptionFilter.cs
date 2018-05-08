using Microsoft.AspNetCore.Mvc.Filters;
using MoneyManager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MoneyManager.Filter
{
    public class NotFoundExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Exception = null;
            }
            base.OnException(context);
        }
    }
}
