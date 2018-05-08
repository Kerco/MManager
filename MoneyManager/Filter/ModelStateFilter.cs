using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoneyManager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Filter
{
    public class ModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext Context)
        {
            if (!Context.ModelState.IsValid)
            {
                Context.Result = new BadRequestObjectResult(Context.ModelState);

            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is NotFoundException)
            {

            }
            base.OnActionExecuted(context);
        }
    }
}
