import { Injectable } from "@angular/core";
import { HttpPostService } from "./http-post.service";
import { AccountActionRequest } from "../models/requests/account-request/account-action-request";
import { Observable } from "rxjs";
import { TokenRequest } from "../models/requests/token-request";
import { ConfigurationResponse } from "../models/configuration/configuration_response";
import { HttpHeaders } from '@angular/common/http';
import { ConfigurationService } from "./configuration.service";
import { AccountActionResponse } from "../models/response/account-response/account-action-response";
import { PasswordActionResponse } from "../models/response/password-response/password-action-response";
import { PasswordActionRequest } from "../models/requests/password-request/password-action-request";
import { StoreReportRequest } from "../models/requests/store-report-request";
import { StoreReportResponse } from "../models/response/store-report-response";
import { BaseRequest } from "../models/requests/base-request";
import { UiNotificationService } from "./ui-notification.service";

@Injectable()
export class AccountService {

    constructor(
        private httpPostService: HttpPostService,
        private configurationService: ConfigurationService,
        public uiNotificationService: UiNotificationService
        // private postNewWindowService: PostNewWindowService
    ) {
    }

    getHttpOption(blob: boolean): any {
        // if (blob) {
        //     return {
        //         headers: new HttpHeaders({
        //             'Content-Type': 'application/json',
        //             'Authorization': 'Bearer ' + this.configurationService.getToken()
        //         }),
        //         observe: 'response',
        //         responseType: 'blob'
        //     }
        // } 
        return {headers: new HttpHeaders({})};
    }

    accountAction(request: AccountActionRequest, path: string): Observable<AccountActionResponse> {
        return this.httpPostService.httpPost<AccountActionResponse>(path, request, this.getHttpOption(false));
    }

    passwordAction(request: PasswordActionRequest, path: string): Observable<PasswordActionResponse> {
        return this.httpPostService.httpPost<PasswordActionResponse>(path, request, this.getHttpOption(false));
    }

    getApplicationPdf(request: BaseRequest, path: string) {
        return this.httpPostService.httpPostBlob(path, request, this.getHttpOption(true));
    }

    storeApplication(request: StoreReportRequest): Observable<StoreReportResponse> {
        return this.httpPostService.httpPost<StoreReportResponse>("api/application/storeApplication", request, this.getHttpOption(false));
    }

    public login(request: TokenRequest): Observable<ConfigurationResponse> {
        return this.httpPostService.httpPost<ConfigurationResponse>("api/authentication/logIn", request, null);
    }

}
