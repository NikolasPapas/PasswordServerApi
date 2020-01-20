import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { BaseComponent } from "../../../../common/base/base.component";
import { ApplicationAction } from 'src/app/models/configuration/ApplicationAction';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ConfigurationService } from 'src/app/services/configuration.service';
import { AccountService } from 'src/app/services/account-action.service';
import { DataNeeded } from 'src/app/models/enums/data-needed';
import { BottomSheet } from 'src/app/ui/common/bottom-sheet/bottom-sheet.component';
import { MatBottomSheet } from '@angular/material';
import { Note } from 'src/app/models/note-model';
import { NoteService } from 'src/app/services/note-servece';
import { NoteActionRequest } from 'src/app/models/requests-responses/requests/note-action-request';
import { takeUntil } from 'rxjs/operators';
import { UiNotificationService } from 'src/app/services/ui-notification.service';
import { NotificationLevel } from 'src/app/models/enums/notification-level';
import { NoteActionResponse } from 'src/app/models/requests-responses/responses/note-action-responce';
import { Guid } from 'src/app/common/types/guid';


@Component({
    selector: 'app-note-tab',
    templateUrl: './note-tab.component.html',
    styleUrls: ['./note-tab.component.scss'],
})
export class NoteTabComponent extends BaseComponent implements OnInit {

    editorConfig: AngularEditorConfig = {
        editable: true,
        spellcheck: true,
        height: 'auto',
        minHeight: '5em',
        maxHeight: 'auto',
        width: 'auto',
        minWidth: '3em',
        translate: 'yes',
        enableToolbar: true,
        showToolbar: true,
        placeholder: 'Enter text here...',
        defaultParagraphSeparator: '',
        defaultFontName: '',
        defaultFontSize: '',
        fonts: [
            { class: 'arial', name: 'Arial' },
            { class: 'times-new-roman', name: 'Times New Roman' },
            { class: 'calibri', name: 'Calibri' },
            { class: 'comic-sans-ms', name: 'Comic Sans MS' }
        ],
        customClasses: [
            {
                name: 'quote',
                class: 'quote',
            },
            {
                name: 'redText',
                class: 'redText'
            },
            {
                name: 'titleText',
                class: 'titleText',
                tag: 'h1',
            },
        ],
        sanitize: true,
        toolbarPosition: 'top',
        toolbarHiddenButtons: [
            ['bold', 'italic'],
            ['subscript', 'superscript'],
            ['link', 'insertImage', 'unlink', 'insertVideo', 'insertHorizontalRule', 'clearFormatting']
        ]
    };

    noteActions: ApplicationAction[];
    stableActions: ApplicationAction[];
    actions: ApplicationAction[];
    notes: Note[];
    selectedNoteIndex: number = -1;
    ACTION_INDICATOR_NOTE_CONTROLLER: string = "Note";

    constructor(
        private configurationService: ConfigurationService,
        private noteService: NoteService,
        public dialog: MatBottomSheet,
        public uiNotificationService: UiNotificationService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.notes =[];
        this.actions = this.configurationService.getActions();
        this.noteActions = [];
        this.noteActions.push(... this.actions.filter(x => x.sendApplicationData == true && x.dataNeeded == DataNeeded.Note));
        this.stableActions = [];
        this.stableActions.push(... this.actions.filter(x => x.sendApplicationData == false && x.dataNeeded == DataNeeded.Note));
    }


    actionSelected(noteIndex: number, action: ApplicationAction) {
        let note: Note = {
            noteId: noteIndex >= 0 ? this.notes[noteIndex].noteId  : Guid.create(),
            userId: noteIndex >= 0 ? this.notes[noteIndex].userId : Guid.create(),
            note: noteIndex >= 0 ? this.notes[noteIndex].note :"",
            lastEdit:noteIndex>=0 ? this.notes[noteIndex].lastEdit :  null
        }

        let request: NoteActionRequest = {
            actionId: action.id,
            note: note,
            accountId: Guid.create(),
        }

        this.noteService.NoteAction(request, action.controllerPath)
            .pipe(takeUntil(this._destroyed)).subscribe(
                res => this.onActionPasswordSuccess(res, action)
                , error => this.onActionError(error)
            );
    }


    onActionPasswordSuccess(res: NoteActionResponse, action: ApplicationAction) {
        if (res.warningMessages)
            this.uiNotificationService.handleMessages(NotificationLevel.Warning, res.warningMessages);
            else{
                this.notes =[];
                this.notes = res.notes;
            }
    }

    onActionError(error: any) {
        if (error.warningMessages)
            this.uiNotificationService.handleMessages(NotificationLevel.Warning, error.warningMessages);
        //Notification For Error
    }

}
