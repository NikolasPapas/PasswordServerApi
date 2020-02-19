using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Service;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PasswordServerApi.ActionFilters
{
    public class VerificationActionFilter : IAsyncActionFilter
    {
        private readonly IAccountService _accountService;
        private readonly IExceptionHandler _exceptionHandler;

        public VerificationActionFilter(IAccountService accountService, IExceptionHandler exceptionHandler)
        {
            _accountService = accountService;
            _exceptionHandler = exceptionHandler;
        }
     
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            AccountActionResponse results = _accountService.ExecuteAction(new Models.Account.Requests.AccountActionRequest()
            {
                AccountId = Guid.Parse(context.HttpContext.User.Identity.Name),
                ActionId = StaticConfiguration.ActionGetIfHackedId

            }, Guid.Parse(context.HttpContext.User.Identity.Name));
            if (results != null && results.WarningMessages.Count > 1)
            {
                context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(results));
                return;
            }
            else
            {
                var result = await next();
            }
        }
    }
}
