import { ViewEncapsulation, Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../common/base/base.component';
import { FormGroup } from '@angular/forms';
import { LoginModel } from './login.model';
import { ConfigurationService } from '../../core/services/configuration.service';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class LoginComponent extends BaseComponent implements OnInit {


    public formGroup: FormGroup;

    constructor(
        private configurationService:ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.formGroup = new LoginModel().fromModel().buildForm();
    }


    login(){
        this.configurationService.login(this.formGroup.getRawValue()).pipe(takeUntil(this._destroyed)).subscribe(
            response => this.configurationService.setLoginResponse(response));
    }
}