// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

[ServiceContract]
[OpenApiBasePath("/api")]
public interface IWebApi
{
	[OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/command")]
    [OpenApiTag("Tag")]
    [OpenApiResponse(ContentTypes = new[] { "application/json" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(CommandContract)) ]
    void BodyQuery(
            [OpenApiParameter(ContentTypes = new[] { "application/json" }, Description = "param description.")]
			CommandContract param
	);
}

