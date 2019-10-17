import { Injectable } from "@angular/core";
import { HttpPostService } from "./http-post.service";
import { AccountActionRequest } from "../models/requests/account-request/account-action-request";
import { Observable } from "rxjs";
import { TokenRequest } from "../models/requests/token-request";
import { ConfigurationResponse } from "../models/configuration/configuration_response";
import { HttpHeaders } from '@angular/common/http';
import { ConfigurationService } from "./configuration.service";
import { AccountActionResponse } from "../models/response/account-response/account-action-response";

@Injectable()
export class AccountService {

    constructor(
        private httpPostService: HttpPostService,
        private configurationService: ConfigurationService,
        // private postNewWindowService: PostNewWindowService
    ) {
    }

    accountAction(request: AccountActionRequest): Observable<AccountActionResponse> {
        const httpOptions = {
            headers: new HttpHeaders({
              'Content-Type':  'application/json',
              'Authorization': 'Bearer '+this.configurationService.getToken()
            })
          };
        return this.httpPostService.httpPost<AccountActionResponse>("api/accounts/accountAction", request,httpOptions);
    }
    
    public login(request: TokenRequest): Observable<ConfigurationResponse> {
        return this.httpPostService.httpPost<ConfigurationResponse>("api/authentication/logIn", request,null);
    }
}
