﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Web.Framework
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapHttpHandler<THttpHandler>(this IEndpointRouteBuilder builder, Action<HttpModel> configure = null)
        {
            var endpoints = HttpHandler.Build<THttpHandler>(configure);

            var dataSource = builder.EndpointDataSources.OfType<HandlerEndpointsDataSource>().SingleOrDefault();
            if (dataSource == null)
            {
                dataSource = new HandlerEndpointsDataSource();
                builder.EndpointDataSources.Add(dataSource);
            }

            dataSource.AddEndpoints(endpoints);
        }

        private class HandlerEndpointsDataSource : EndpointDataSource
        {
            private readonly List<Endpoint> _endpoints = new List<Endpoint>();

            public void AddEndpoints(IList<Endpoint> endpoints)
            {
                _endpoints.AddRange(endpoints);
            }

            public override IReadOnlyList<Endpoint> Endpoints => _endpoints;

            public override IChangeToken GetChangeToken()
            {
                return NullChangeToken.Singleton;
            }
        }
    }
}
