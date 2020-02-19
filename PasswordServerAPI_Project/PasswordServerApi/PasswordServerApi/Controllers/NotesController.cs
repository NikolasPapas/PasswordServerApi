using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Requests.Note;
using PasswordServerApi.Models.Responces;

namespace PasswordServerApi.Controllers
{
    //api/notes
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _notesService;
        private readonly ILoggingService _logger;
        private readonly IExceptionHandler _exceptionHandler;

        public NotesController(INoteService notesService, ILoggingService logger, IExceptionHandler exceptionHandler)
        {
            _notesService = notesService;
            _logger = logger;
            _exceptionHandler = exceptionHandler;
        }

        [Authorize]
        [HttpPost("noteAction")]
        public Response<NoteActionResponse> GetMyNote([FromBody] NoteActionRequest request)
        {
            return _exceptionHandler.HandleException(() => _notesService.NoteAction(request, Guid.Parse(HttpContext.User.Identity.Name)), request.ActionId);
        }

    }
}
