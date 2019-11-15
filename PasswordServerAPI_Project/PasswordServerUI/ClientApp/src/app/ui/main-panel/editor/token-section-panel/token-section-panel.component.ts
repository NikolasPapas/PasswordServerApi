import { Component, Input, OnInit } from "@angular/core";
import { BaseComponent } from "../../../../common/base/base.component";
import { ConfigurationService } from "../../../../core/services/configuration.service";
import { LoginToken } from "../../../../core/models/LoginToken";
import { Guid } from "../../../../common/types/guid";
import { AccountService } from "../../../../core/services/account-action.service";
import { takeUntil } from "rxjs/operators";
import { TokenActionRequest } from "../../../../core/models/requests/token-action-request";
import { TokenRequestActionEnum } from "../../../../core/models/enums/token-request-action";

@Component({
    selector: 'app-token-section-panel',
    templateUrl: './token-section-panel.component.html',
    styleUrls: ['./token-section-panel.component.scss'],
})
export class TokenSectionPanel extends BaseComponent implements OnInit {

    loginTokens: LoginToken[] = [];

    constructor(
        private accountService: AccountService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.refresh();
    }


    deleteToken(index: number) {
        let request: TokenActionRequest={
            action :TokenRequestActionEnum.Delete,
            token:this.loginTokens[index]
        };
        this.tokenAction(request);
    }

    refresh() {
        let request: TokenActionRequest={
            action :TokenRequestActionEnum.Get,
            token:null
        };
        this.tokenAction(request);
    }


    tokenAction(request: TokenActionRequest){
        this.accountService.getTokensAction(request, "/accounts/tokenActions")
        .pipe(takeUntil(this._destroyed)).subscribe(
            res => this.onGetTokensActionSuccess(res)
            , error => this.onGetTokensActionError(error)
        );
    }

    clearAll() {
        this.loginTokens = [];
    }


    onGetTokensActionSuccess(res: any) {
        this.clearAll();
        res.forEach(token => {
            this.loginTokens.push(token);
        });
    }

    onGetTokensActionError(error: any) {
        this.clearAll();
        //Notification For Error
    }
}