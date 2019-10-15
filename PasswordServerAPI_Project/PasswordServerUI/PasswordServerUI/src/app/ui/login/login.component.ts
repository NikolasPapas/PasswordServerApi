import { ViewEncapsulation, Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../common/base/base.component';
import { FormGroup } from '@angular/forms';
import { LoginModel } from './login.model';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class LoginComponent extends BaseComponent implements OnInit {


    public formGroup: FormGroup;

    constructor(
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.formGroup = new LoginModel().fromModel().buildForm();
    }

}