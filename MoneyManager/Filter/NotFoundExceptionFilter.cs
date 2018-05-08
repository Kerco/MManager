using Microsoft.AspNetCore.Mvc.Filters;
using MoneyManager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Filter
{
    public class NotFoundExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
               
            }
        }
    }
}
