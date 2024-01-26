﻿using Farfetch.LoadShedding.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Farfetch.LoadShedding.AspNetCore.Resolvers
{
    internal class HttpHeaderPriorityResolver : IPriorityResolver
    {
        private const string Separator = "-";

        internal const string DefaultPriorityHeaderName = "X-Priority";

        private readonly string _headerName;

        public HttpHeaderPriorityResolver()
            : this(DefaultPriorityHeaderName)
        {
        }

        public HttpHeaderPriorityResolver(string headerName)
        {
            this._headerName = headerName;
        }

        public Task<Priority> ResolveAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(this._headerName, out var values))
            {
                return Task.FromResult(Priority.Normal);
            }

            var normalizedValue = this.NormalizeHeaderValue(values);

            if (!Enum.TryParse(normalizedValue, true, out Priority priority))
            {
                priority = Priority.Normal;
            }

            return Task.FromResult(priority);
        }

        private string NormalizeHeaderValue(string headerValue)
        {
            return headerValue
                .Replace(Separator, String.Empty)
                .ToLower();
        }
    }
}