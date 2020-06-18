import { Component, Input, OnInit } from "@angular/core";
import { BaseComponent } from 'src/app/common/base/base.component';
import { LoginToken } from 'src/app/models/login-token';
import { AccountService } from 'src/app/services/account-action.service';
import { TokenActionRequest } from 'src/app/models/requests-responses/requests/token-action-request';
import { TokenRequestActionEnum } from 'src/app/models/enums/token-request-action';
import { takeUntil } from 'rxjs/operators';

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