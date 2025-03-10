﻿using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Service.PaymentApi.Controllers
{
	/// <summary>
	///     to-do: remove
	/// </summary>
	[OpenApiTag("PaymentTest", Description = "payment test")]
	[Route("/api/v1/payment/test")]
	public class PaymentTestController : BaseController
	{
		[HttpGet("index")]
		public ViewResult PaymentTest() => View("PaymentTest");
	}
}