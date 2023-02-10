using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Penguin.Cms.Web.Analytics.Extensions;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Security.Abstractions.Interfaces;
using Penguin.Web.Abstractions.Interfaces;
using System.Threading.Tasks;

namespace Penguin.Cms.Web.Analytics.Middleware
{
    //https://exceptionnotfound.net/using-middleware-to-log-requests-and-responses-in-asp-net-core/
    public class RequestLogging : IPenguinMiddleware
    {
        private readonly RequestDelegate _next;

        //TODO: Learn what this is
        public RequestLogging(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            await _next(context).ConfigureAwait(true);

            //Open a new service scope to ensure there is no collision with anything running on the request
            IRepository<PageView> pageViewRepository = context.RequestServices.GetService<IRepository<PageView>>();

            using (pageViewRepository.WriteContext())
            {
                pageViewRepository.Record(context, context.RequestServices.GetService<IUserSession>());
            }
        }
    }
}