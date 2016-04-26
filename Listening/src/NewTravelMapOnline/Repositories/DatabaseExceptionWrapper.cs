using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PostSharp.Aspects;

namespace WebListening.Repositories
{
    [Serializable]
    public class DatabaseExceptionWrapper : OnExceptionAspect
    {
        //[Dependency]
        public ILogger Logger { get; set; }
        //private readonly ILogger _logger;

        //public DatabaseExceptionWrapper(ILoggerFactory loggerFactory)
        //{
        //    _logger = loggerFactory.CreateLogger<DatabaseExceptionWrapper>();
        //}

        public override void OnException(MethodExecutionArgs args)
        {
            string msg = string.Format("{0} had an error @ {1}: {2}\n{3}",
                args.Method.Name, DateTime.Now,
                args.Exception.Message, args.Exception.StackTrace);

            Logger.LogError(msg);

            throw new Exception("Invalid operation");
        }
    }

    public class CustomLoggerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            // Here goes your logic
        }

        // ...

        //override exc
    }

}
