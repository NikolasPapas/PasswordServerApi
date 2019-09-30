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
		public static readonly Guid ActionAddNewAccountId = new Guid("3CB81D8A-C477-440A-81CD-36745E6E79D8");

		//Comon
		public static readonly Guid ActionGetAccountAndPasswordId = new Guid("AF897163-6642-4C27-8084-DB99788E77E9");

		//Password
		public static readonly Guid ActionGetPasswordsId = new Guid("3CF704FB-6D37-4661-A543-7C7A1BAC3284");
		public static readonly Guid ActionSavePasswordId = new Guid("887B5253-A8F5-462E-8200-330C3D60D62A");

		#endregion

		#region AcrionConfiguration

		//Account
		public static readonly ApplicationAction ActionGetAccounts = new ApplicationAction { Id = ActionGetAccountId, Name = "Αναζήτηση", ToolTipText = "Αναζήτηση στοιχείων λογαριασμού", NeedsComment = false, SendApplicationData = true, Icon = "refresh", ValidationMode = ApplicationValidationMode.Full };
		public static readonly ApplicationAction ActionSaveAccount = new ApplicationAction { Id = ActionSaveAccountId, Name = "Αποθήκευση", ToolTipText = "Αποθήκευση λογαριασμού", NeedsComment = false, SendApplicationData = true, Icon = "save", ValidationMode = ApplicationValidationMode.Full };
		public static readonly ApplicationAction ActionAddNewAccount = new ApplicationAction { Id = ActionAddNewAccountId, Name = "Προσθήκη", ToolTipText = "Προσθήκη λογαριασμού", NeedsComment = false, SendApplicationData = true, Icon = "save", ValidationMode = ApplicationValidationMode.Full };

		//Comon
		public static readonly ApplicationAction ActionGetAccountAndPassword = new ApplicationAction { Id = ActionGetAccountAndPasswordId, Name = "Αναζήτηση όλων τον λογαριασμών και κωδικών τους", ToolTipText = "Αναζήτηση όλων τον λογαριασμών και κωδικών τους", NeedsComment = false, SendApplicationData = true, Icon = "refresh", ValidationMode = ApplicationValidationMode.Full };

		//Password
		public static readonly ApplicationAction ActionGetPasswords = new ApplicationAction { Id = ActionGetPasswordsId, Name = "Αναζήτηση", ToolTipText = "Αναζήτηση κωδικου", NeedsComment = false, SendApplicationData = true, Icon = "refresh", ValidationMode = ApplicationValidationMode.Full };
		public static readonly ApplicationAction ActionSavePassword = new ApplicationAction { Id = ActionSavePasswordId, Name = "Αποθήκευση", ToolTipText = "Αποθήκευση κωδικου", NeedsComment = false, SendApplicationData = true, Icon = "save", ValidationMode = ApplicationValidationMode.Full };


		#endregion


		public static readonly Dictionary<string, List<ApplicationAction>> GetAcrionByRole = new Dictionary<string, List<ApplicationAction>>()
		{
			//Admin Actions
			{ Role.Admin, new List<ApplicationAction>(){ ActionGetAccounts, ActionSaveAccount, ActionAddNewAccount, ActionGetAccountAndPassword, } },

			//User Actions
			{ Role.User, new List<ApplicationAction>(){ ActionGetAccountAndPassword, ActionSavePassword, ActionGetPasswords } },

			//Viewer Actions
			{ Role.Viewer, new List<ApplicationAction>(){ ActionGetAccounts, ActionGetAccountAndPassword } },
		};


	}
}
