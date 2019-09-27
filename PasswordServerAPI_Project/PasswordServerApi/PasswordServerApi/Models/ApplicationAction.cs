﻿using PasswordServerApi.Models.Enums;
using System;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models
{
	[DataContract]
	public class ApplicationAction
	{
		[DataMember(Name = "id")]
		public Guid Id { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "needsComment")]
		public bool NeedsComment { get; set; }

		[DataMember(Name = "tooltip")]
		public string ToolTipText { get; set; }

		[DataMember(Name = "sendApplicationData")]
		public bool SendApplicationData { get; set; }

		[DataMember(Name = "validationMode")]
		public ApplicationValidationMode ValidationMode { get; set; }

		[DataMember(Name = "refreshAfterAction")]
		public bool RefreshAfterAction { get; set; }

		[DataMember(Name = "collapseApplication")]
		public bool CollapseApplication { get; set; }

		[DataMember(Name = "icon")]
		public string Icon { get; set; }

		[DataMember(Name = "needsConfirmation")]
		public bool NeedsConfirmation { get; set; }
	}
}