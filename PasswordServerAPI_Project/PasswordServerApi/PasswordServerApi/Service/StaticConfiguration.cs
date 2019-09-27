using PasswordServerApi.Models;
using PasswordServerApi.Models.Enums;
using PasswordServerApi.Security.SecurityModels;
using System;
using System.Collections.Generic;

namespace PasswordServerApi.Service
{
	public static class StaticConfiguration
	{
		#region ActionID 

		//Accoun
		public static readonly Guid ActionGetAccountId = new Guid("C25B9787-8751-4FBD-BC6C-C63A48026D30");
		public static readonly Guid ActionSaveAccountId = new Guid("1086495E-FD61-4397-B3A9-87B737ADEDDD");

		//Password
		public static readonly Guid ActionGetPasswordId = new Guid("AF897163-6642-4C27-8084-DB99788E77E9");
		public static readonly Guid ActionSavePasswordId = new Guid("887B5253-A8F5-462E-8200-330C3D60D62A");

		#endregion

		#region AcrionConfiguration

		//Account
		public static readonly ApplicationAction ActionGetAccounts = new ApplicationAction { Id = ActionGetAccountId, Name = "Αναζήτηση", ToolTipText = "Αναζήτηση λογαριασμού", NeedsComment = false, SendApplicationData = true, Icon = "refresh", ValidationMode = ApplicationValidationMode.Full };
		public static readonly ApplicationAction ActionSaveAccount = new ApplicationAction { Id = ActionSaveAccountId, Name = "Αποθήκευση", ToolTipText = "Αποθήκευση λογαριασμού", NeedsComment = false, SendApplicationData = true, Icon = "save", ValidationMode = ApplicationValidationMode.Full };


		//Password
		public static readonly ApplicationAction ActionGetPassword = new ApplicationAction { Id = ActionGetPasswordId, Name = "Αναζήτηση", ToolTipText = "Αναζήτηση κωδικου", NeedsComment = false, SendApplicationData = true, Icon = "refresh", ValidationMode = ApplicationValidationMode.Full };
		public static readonly ApplicationAction ActionSavePassword = new ApplicationAction { Id = ActionSavePasswordId, Name = "Αποθήκευση", ToolTipText = "Αποθήκευση κωδικου", NeedsComment = false, SendApplicationData = true, Icon = "save", ValidationMode = ApplicationValidationMode.Full };


		#endregion


		public static readonly Dictionary<string, List<ApplicationAction>> GetAcrionByRole = new Dictionary<string, List<ApplicationAction>>()
		{
			//Admin Actions
			{ Role.Admin, new List<ApplicationAction>(){ ActionGetAccounts,ActionSaveAccount, ActionGetPassword } },

			//User Actions
			{ Role.User, new List<ApplicationAction>(){ ActionGetPassword, ActionSavePassword } },

			//Viewer Actions
			{ Role.Viewer, new List<ApplicationAction>(){ ActionGetAccounts, ActionGetPassword } },
		};


	}
}
