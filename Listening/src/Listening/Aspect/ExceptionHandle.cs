//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Mvc;
//using PostSharp.Aspects;
//using WebListening.Exceptions;

//namespace Listening.Aspect
//{
//    public class ExceptionHandle : OnExceptionAspect
//    {
//        public ExceptionHandle()
//        {

//        }

//        public override void OnException(MethodExecutionArgs args)
//        {
//            //var xxx = Json("");
//            Console.WriteLine("Inside OnException");
//            var controller = ((Controller)args.Instance);
//            Console.WriteLine(controller);

//            //object controller = ((Controller)args.Instance).ControllerContext.RouteData.Values["controller"];
//            if (args.Exception.GetType() == typeof(FileUploadException))
//            {
//                args.ReturnValue = controller.Json(args.Exception.Message);
//            }
//            else
//            {
//                //args.Instance.
//                args.ReturnValue = controller.Json("Invalid operation");
//            }
//            //args.ReturnValue=
//            //base.OnException(args);
//        }
//    }
//}
