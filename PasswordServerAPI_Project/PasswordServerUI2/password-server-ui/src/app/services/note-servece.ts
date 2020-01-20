import { HttpHeaders } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { NoteActionRequest } from '../models/requests-responses/requests/note-action-request';
import { AccountActionResponse } from '../models/requests-responses/responses/account-action-response';
import { NoteActionResponse } from '../models/requests-responses/responses/note-action-responce';
import { HttpPostService } from "./http-post.service";
import { UiNotificationService } from "./ui-notification.service";

@Injectable()
export class NoteService {

    constructor(
        private httpPostService: HttpPostService,
        public uiNotificationService: UiNotificationService
    ) {
    }

    getHttpOption(blob: boolean): any {
        return { headers: new HttpHeaders({}) };
    }

    NoteAction(request: NoteActionRequest, path: string): Observable<NoteActionResponse> {
        return this.httpPostService.httpPost<NoteActionResponse>(path, request, this.getHttpOption(false));
    }
}
