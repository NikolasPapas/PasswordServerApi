﻿using PasswordServerApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Responces
{
	public class Response<T>
	{
		public T Payload { get; set; }

		public Guid SelectedAction { get; set; }

		public List<string> Warnnings { get; set; }

		public string Error { get; set; }

		public ErrorCategory Category { get; set; }
	}
}
