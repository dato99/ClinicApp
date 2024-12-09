using ClinicApp.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ClinicApp.Filter
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        readonly IPKG_LOGS logs;

        public GlobalExceptionFilter(IPKG_LOGS logs)
        {
            this.logs = logs;
        }

        public override void OnException(ExceptionContext context)
        {
            var result = new ObjectResult("System Error Try Again")
            { 
                StatusCode = (int)HttpStatusCode.InternalServerError 
            };
            var error = context.Exception.Message;

            try
            {
                logs.add_log(error);
            }
            catch
            {

            }
            context.Result = result;
        }
    }
}
