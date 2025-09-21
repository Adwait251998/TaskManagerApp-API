using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Application.DTOs;
using TaskManager.Core.Entities;

namespace TaskManager.Filters
{
    public class UserSessionFilter:IAsyncActionFilter
    {
        private readonly UserSession _userSession;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserSessionFilter(UserSession userSession, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okResult
                && okResult.Value is LoginResponseDto loginResponse)
            {
                var user = loginResponse.User;
                _userSession.SetUser(user.Id, user.Name, user.Email);
            }
        }

       
    }
}
