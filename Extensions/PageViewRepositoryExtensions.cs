using Microsoft.AspNetCore.Http;
using Penguin.Cms.Web.Analytics.Entities;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Security.Abstractions.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Cms.Web.Analytics.Extensions
{
    public static class PageViewRepositoryExtensions
    {
        public static int GetCountByUrl(this IRepository<PageView> repository, string Url)
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (Url is null)
            {
                throw new ArgumentNullException(nameof(Url));
            }

            return repository.Where(pv => pv.Path == Url).Count();
        }

        public static void Record(this IRepository<PageView> repository, HttpContext httpContext, IUserSession userSession = null)
        {
            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            PageView pageView = new PageView(httpContext.Request);

            if (userSession?.IsLoggedIn ?? false)
            {
                pageView.Creator = userSession.LoggedInUser.Guid;
            }

            pageView.DateCreated = DateTime.Now;

            using (IWriteContext context = repository.WriteContext())
            {
                repository.Add(pageView);
            }
        }
    }
}