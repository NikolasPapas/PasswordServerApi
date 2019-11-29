import { Injectable } from "@angular/core";
import { HttpPostService } from "./http-post.service";
import { Observable } from "rxjs";
import { HttpHeaders } from '@angular/common/http';
import { ConfigurationService } from "./configuration.service";
import { UiNotificationService } from "./ui-notification.service";
import { AccountActionRequest } from '../models/requests-responses/requests/account-action-request';
import { AccountActionResponse } from '../models/requests-responses/responses/account-action-response';
import { PasswordActionRequest } from '../models/requests-responses/requests/password-action-request';
import { PasswordActionResponse } from '../models/requests-responses/responses/password-action-response';
import { TokenActionRequest } from '../models/requests-responses/requests/token-action-request';
import { LoginToken } from '../models/login-token';
import { BaseRequest } from '../models/requests-responses/requests/base-request';
import { TokenRequest } from '../models/requests-responses/requests/token-request';
import { ConfigurationResponse } from '../models/configuration/configuration_response';

@Injectable()
export class AccountService {

    constructor(
        private httpPostService: HttpPostService,
        public uiNotificationService: UiNotificationService
    ) {
    }

    getHttpOption(blob: boolean): any {
        return {headers: new HttpHeaders({})};
    }

    accountAction(request: AccountActionRequest, path: string): Observable<AccountActionResponse> {
        return this.httpPostService.httpPost<AccountActionResponse>(path, request, this.getHttpOption(false));
    }

    passwordAction(request: PasswordActionRequest, path: string): Observable<PasswordActionResponse> {
        return this.httpPostService.httpPost<PasswordActionResponse>(path, request, this.getHttpOption(false));
    }

    getTokensAction(request: TokenActionRequest, path: string): Observable<LoginToken> {
        return this.httpPostService.httpPost<LoginToken>(path, request, this.getHttpOption(false));
    }

    getApplicationPdf(request: BaseRequest, path: string) {
        return this.httpPostService.httpPostBlob(path, request, this.getHttpOption(true));
    }

    public login(request: TokenRequest): Observable<ConfigurationResponse> {
        return this.httpPostService.httpPost<ConfigurationResponse>("authentication/logIn", request, null);
    }

}
