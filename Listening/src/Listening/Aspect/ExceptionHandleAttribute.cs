using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using PostSharp.Aspects;
using WebListening.Exceptions;

namespace Listening.Aspect
{
    [Serializable]
    public class ExceptionHandleAttribute : OnExceptionAspect
    {
        private Type[] AllowedExceptions;

        // probably it would be better to read this 
        // http://codereview.stackexchange.com/questions/20341/inject-dependency-into-postsharp-aspect
        public Type AbstractFactoryType { get; set; }

        private ILogger Logger { get; set; }

        public ExceptionHandleAttribute()
        {
            AllowedExceptions = new Type[] { typeof(FileUploadException) };
        }

        public override void OnException(MethodExecutionArgs args)
        {
            var controller = ((Controller)args.Instance);
            controller.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            if (AllowedExceptions.Contains(args.Exception.GetType()))
                args.ReturnValue = controller.Json(args.Exception.Message);
            else
                args.ReturnValue = controller.Json("Invalid operation");
        }
    }
}
