using Microsoft.AspNetCore.Http;
using Penguin.Cms.Entities;
using System;

namespace Penguin.Cms.Web.Analytics
{
    public class PageView : Entity
    {
        public Guid Creator { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Referrer { get; set; } = string.Empty;

        public PageView(HttpRequest thisRequest)
        {
            if (thisRequest is null)
            {
                throw new ArgumentNullException(nameof(thisRequest));
            }

            Path = thisRequest.Path.Value;
            Referrer = thisRequest.Headers["Referer"];
        }

        public PageView()
        {
        }
    }
}