﻿using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Service.PaymentApi.Controllers
{
	[ApiController]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[SwaggerResponse(HttpStatusCode.Unauthorized, null, Description = "Unauthorized")]
	public abstract class BaseController : Controller
	{
		protected Guid? GetUserId() => Guid.TryParse(User.Identity?.Name, out Guid uid) ? (Guid?)uid : null;
	}
}
