using Newtonsoft.Json;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Enums;
using PasswordServerApi.Models.Responces;
using System;
using System.Linq;
using System.Net.Http;
using tepixPetitions.common.Types.Exceptions;

namespace PasswordServerApi.Service
{
	public class ExceptionHandler : IExceptionHandler
	{
		private readonly Type[] _businessExceptions;

		public ExceptionHandler()
		{
			_businessExceptions = new Type[] { typeof(BusinessException) };
		}

		public Response<T> HandleException<T>(Func<T> action, Guid requestId) where T : class
		{
			return HandleException(action, null, requestId);
		}

		private Response<T> HandleException<T>(Func<T> action, T nullValue, Guid requestId)
		{
			try
			{
				return new Response<T>()
				{
					Payload = action(),
					SelectedAction = requestId,
				};
			}
			catch (AggregateException aggEx)    // Handle all inner exceptions, but return only the result of the 1st handling
			{
				return HandleAggregateException<T>(aggEx, nullValue, requestId);
			}
			catch (Exception ex)
			{
				return HandleException<T>(ex, nullValue, requestId);
			}
		}

		private Response<T> HandleAggregateException<T>(AggregateException aggEx, T nullValue, Guid requestId)
		{
			int exCount = 0;
			Response<T> result = default(Response<T>);
			foreach (var innerEx in aggEx.Flatten().InnerExceptions)
			{
				Response<T> innerExHandlingResult = HandleException<T>(innerEx, nullValue, requestId);
				if (exCount == 0)
					result = innerExHandlingResult;
				exCount++;
			}
			return result;
		}

		private Response<T> HandleException<T>(Exception ex, T nullValue, Guid requestId)
		{
			var respMessage = new Response<T>()
			{
				Error = ex.Message,
				SelectedAction = requestId,
				Category = (_businessExceptions.FirstOrDefault(t => t.IsInstanceOfType(ex)) != null) ? ErrorCategory.Business : ErrorCategory.Technical,
			};

			return respMessage;
		}
	}
}

