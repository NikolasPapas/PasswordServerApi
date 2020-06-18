import { Component, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { BaseComponent } from '../common/base/base.component';
import { LoginModel } from './login/login.model';
import { ApplicationAction } from '../models/configuration/ApplicationAction';
import { ConfigurationService } from '../services/configuration.service';
import { AccountService } from '../services/account-action.service';
import { TokenRequest } from '../models/requests-responses/requests/token-request';


@Component({
    selector: 'app-application',
    templateUrl: './application.component.html',
    styleUrls: ['./application.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ApplicationComponent extends BaseComponent implements OnInit {

    ACTION_INDICATOR_ADD_ACCOUNT: string = "1086495e-fd61-4397-b3a9-87b737adeddd";

    @Output() ActionEvent: ApplicationAction = null;
    @Output() AddAccountEvent: number = null;
    @Output() AddPasswordEvent: number = null;

    opened: boolean;
    actions: ApplicationAction[];

    isActionAddAccountIsOn: boolean = false;
    isActionAddPasswordIsOn: number = -1;
    selectedAction: number = -1;

    public mastDoLogIn: boolean = true;
    loginForm: FormGroup;

    constructor(
        public configurationService: ConfigurationService,
        private accountService: AccountService,
    ) {
        super();
        this.mastDoLogIn = this.configurationService.needLogin();
    }

    ngOnInit() {

        this.loginForm = new LoginModel().fromModel().buildForm();

        this.configurationService.isLoginIn.subscribe(value => { this.mastDoLogIn = value });
    }

    LogOut(){
        this.configurationService.setToken(null);
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

    executeAction(index: number) {
        this.ActionEvent = null;
        this.ActionEvent = this.actions[index];
    }

    IsActionAddPasswordIsOnEvent(index: number) {
        this.isActionAddPasswordIsOn = index;
    }


    addAccount() {
        this.AddAccountEvent = null;
        this.AddAccountEvent = 1;
        
    }

    addPassword() {
        this.AddPasswordEvent = null;
        this.AddPasswordEvent = this.isActionAddPasswordIsOn;
    }
}
