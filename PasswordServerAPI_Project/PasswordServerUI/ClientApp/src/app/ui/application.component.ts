import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfigurationService } from '../core/services/configuration.service';
import { TokenRequest } from '../core/models/requests/token-request';
import { takeUntil } from 'rxjs/operators';
import { LoginModel } from './login/login.model';
import { FormGroup, FormArray } from '@angular/forms';
import { AccountService } from '../core/services/account-action.service';
import { BaseComponent } from '../common/base/base.component';
import { ApplicationAction } from '../core/models/configuration/ApplicationAction';
import { AccountForm } from './main-panel/editor/request-editor/account-editor/account-form.model';
import { PasswordForm } from './main-panel/editor/request-editor/password-editor/password-form.model';


@Component({
    selector: 'app-application',
    templateUrl: './application.component.html',
    styleUrls: ['./application.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ApplicationComponent extends BaseComponent implements OnInit {

    ACTION_INDICATOR_ADD_ACCOUNT: string = "1086495e-fd61-4397-b3a9-87b737adeddd";

    opened: boolean;
    actions: ApplicationAction[];
    accountModels: FormGroup[] = [];

    isActionAddAccountIsOn: boolean = false;
    isActionAddPasswordIsOn: number = -1;
    selectedAction: number = -1;


    public mastDoLogIn: boolean = true;
    loginForm: FormGroup;

    constructor(
        private language: TranslateService,
        public configurationService: ConfigurationService,
        private accountService: AccountService,
    ) {
        super();
    }

    ngOnInit() {
        this.loginForm = new LoginModel().fromModel().buildForm();
    }

    login() {
        let loginRequest: TokenRequest = { username: this.loginForm.get('username').value, password: this.loginForm.get('password').value };
        this.accountService.login(loginRequest).pipe(takeUntil(this._destroyed)).subscribe(
            response => {
                this.configurationService.setLoginResponse(response)
                this.actions = this.configurationService.getActions();
                if (this.actions.length > 0)
                    this.selectedAction = 0;
                if (this.actions.filter(x => x.id.toString() == this.ACTION_INDICATOR_ADD_ACCOUNT.toString()).length > 0)
                    this.isActionAddAccountIsOn = true;
                this.mastDoLogIn = this.configurationService.needLogin();
            });
    }

    selectMenuAction(index: number) {
        this.selectedAction = index;
    }

    IsActionAddPasswordIsOnEvent(index: number) {
        this.isActionAddPasswordIsOn = index;
    }

    addAccount() {
        this.accountModels.push(new AccountForm().fromModel(null).buildForm());
    }

    addPassword() {
        if (this.accountModels[this.isActionAddPasswordIsOn] != null && this.accountModels[this.isActionAddPasswordIsOn].get('passwords') != null)
            (this.accountModels[this.isActionAddPasswordIsOn].get('passwords') as FormArray).push(new PasswordForm().fromModel(null).buildForm());
    }
}
