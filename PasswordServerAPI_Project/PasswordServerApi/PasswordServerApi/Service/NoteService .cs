using System;
using System.Collections.Generic;
using System.Linq;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Models;
using Newtonsoft.Json;
using PasswordServerApi.Models.Requests.Note;
using PasswordServerApi.Models.DTO;

namespace PasswordServerApi.Service
{
    public class NoteService : INoteService
    {
        private IBaseService _baseService;
        private ILoggingService _logger;

        public NoteService(IBaseService baseService, ILoggingService logger)
        {
            _baseService = baseService;
            _logger = logger;
        }

        #region Dictionary ActionId To Function

        private Dictionary<Guid, Func<List<NoteDto>, NoteDto, AccountDto, NoteActionRequest, NoteActionResponse>> _actionIdToFunction = null;

        private Dictionary<Guid, Func<List<NoteDto>, NoteDto, AccountDto, NoteActionRequest, NoteActionResponse>> ActionIdToFunction
        {
            get
            {
                return _actionIdToFunction ?? (_actionIdToFunction =
                    new Dictionary<Guid, Func<List<NoteDto>, NoteDto, AccountDto, NoteActionRequest, NoteActionResponse>>()
                    {
                        { StaticConfiguration.ActionGetNotesId, GetNotesFunc },
                        { StaticConfiguration.ActionUpdateOrAddNoteId, UpdateOrAddNoteFunc },
                        { StaticConfiguration.ActionRemoveNotedId, RemoveNoteFunc },
                        { StaticConfiguration.ActionCreateNoteId, CreateNoteFunc },
                    });
            }
        }

        #endregion

        public NoteActionResponse NoteAction(NoteActionRequest request, Guid userID)
        {
            AccountDto userAccount = _baseService.GetAccountById(userID, true);
            Guid accountToScann = userID;
            //if (userAccount.Role == "Admin")
            //    accountToScann = request.AccountId;

            AccountDto account = _baseService.GetAccountById(accountToScann, true);
            List<NoteDto> savedNotes = _baseService.FindUserNotes(userID);

            if (StaticConfiguration.GetAcrionByProfile(userAccount.Role) == null)
                throw new Exception("Invalid Profile");
            else
            {
                ApplicationAction actions = StaticConfiguration.GetAcrionByProfile(userAccount.Role).Find(x => x.Id == request.ActionId);
                if (actions == null)
                    throw new Exception("Invalid Action");
                Func<List<NoteDto>, NoteDto, AccountDto, NoteActionRequest, NoteActionResponse> func;
                if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το Id: " + request.ActionId);
                return func(savedNotes, request.Note, account, request);
            }

        }

        #region Actions 
        private NoteActionResponse GetNotesFunc(List<NoteDto> savedNotes, NoteDto requesNote, AccountDto account, NoteActionRequest request)
        {
            return new NoteActionResponse() { Notes = _baseService.FindUserNotes(account.AccountId).ToList() };
        }

        private NoteActionResponse UpdateOrAddNoteFunc(List<NoteDto> savedNotes, NoteDto requesNote, AccountDto account, NoteActionRequest request)
        {
            if (savedNotes.Find(x => x.NoteId == requesNote.NoteId) == null)
                requesNote.NoteId = Guid.NewGuid();
            _baseService.SaveNote(account.AccountId, requesNote.NoteId, requesNote.Note);
            return new NoteActionResponse() { Notes = _baseService.FindUserNotes(account.AccountId) };
        }

        private NoteActionResponse RemoveNoteFunc(List<NoteDto> savedNotes, NoteDto requesNote, AccountDto account, NoteActionRequest request)
        {
            _baseService.DeleteNote(account.AccountId, requesNote.NoteId);
            return new NoteActionResponse() { Notes = _baseService.FindUserNotes(account.AccountId) };
        }

        private NoteActionResponse CreateNoteFunc(List<NoteDto> savedNotes, NoteDto requesNote, AccountDto account, NoteActionRequest request)
        {
            _baseService.SaveNote(account.AccountId, Guid.NewGuid(), "New Note");
            return new NoteActionResponse() { Notes = _baseService.FindUserNotes(account.AccountId) };
        }

        #endregion

    }
}
