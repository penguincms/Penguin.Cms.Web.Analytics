using Microsoft.AspNetCore.Http;
using Penguin.Cms.Entities;
using System;

namespace Penguin.Cms.Web.Analytics.Entities
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
                throw new System.ArgumentNullException(nameof(thisRequest));
            }

            this.Path = thisRequest.Path.Value;
            this.Referrer = thisRequest.Headers["Referer"];
        }

        public PageView()
        {
        }
    }
}