import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfigurationService } from '../core/services/configuration.service';
import { TokenRequest } from '../core/models/requests/token-request';
import { takeUntil } from 'rxjs/operators';
import { LoginModel } from './login/login.model';
import { FormGroup } from '@angular/forms';
import { AccountService } from '../core/services/account-action.service';
import { BaseComponent } from '../common/base/base.component';


@Component({
    selector: 'app-application',
    templateUrl: './application.component.html',
    styleUrls: ['./application.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ApplicationComponent extends BaseComponent implements OnInit  {

    public mastDoLogIn:boolean = true;
    loginForm:FormGroup;

    constructor(
        private language: TranslateService,
        public configurationService: ConfigurationService,
        private accountService :AccountService,
    ) {
        super();
    }

    ngOnInit() {    
        this.loginForm = new LoginModel().fromModel().buildForm();
    }


    login(){
        let loginRequest : TokenRequest ={ username : this.loginForm.get('username').value , password : this.loginForm.get('password').value};
        this.accountService.login(loginRequest).pipe(takeUntil(this._destroyed)).subscribe(
            response => {
                this.configurationService.setLoginResponse(response)
                this.mastDoLogIn= this.configurationService.needLogin();
            });
    }
}
