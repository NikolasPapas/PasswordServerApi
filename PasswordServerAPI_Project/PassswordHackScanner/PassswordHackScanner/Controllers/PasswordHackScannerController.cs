using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PassswordHackScanner.Models;

namespace PassswordHackScanner.Controllers
{
	[Route("api/hackScanner")]
	[ApiController]
	public class PasswordHackScannerController : ControllerBase
	{
		[HttpGet("isHacked")]
		public ActionResult<IsHackedResponce> IsHacked(string email)
		{
			if (!string.IsNullOrWhiteSpace(email))
				return new IsHackedResponce()
				{
					Email = email,
					IsHacked = true
				};
			else
				return BadRequest();
		}

		[HttpPost("getHackTimes")]
		public ActionResult<HackTimeResponce> GetHackTimes([FromBody] EmailHackedInfoRequest request)
		{
			if (!string.IsNullOrWhiteSpace(request.Email))
				return new HackTimeResponce()
				{
					Email = request.Email,
					FromSites=new List<FromSite> (){ new FromSite() { } }
				};
			else
				return BadRequest();
		}
	}
}