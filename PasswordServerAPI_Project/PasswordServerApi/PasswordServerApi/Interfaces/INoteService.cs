using PasswordServerApi.Models.Requests.Note;
using PasswordServerApi.Models.Responces;
using System;

namespace PasswordServerApi.Interfaces
{
	public interface INoteService
	{
		NoteActionResponse NoteAction(NoteActionRequest request, Guid userID);
	}
}
