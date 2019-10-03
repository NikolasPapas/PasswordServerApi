import { ViewEncapsulation, Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../common/base/base.component';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class LoginComponent extends BaseComponent implements OnInit {


    constructor(
        //private language: TranslateService,
    ){
        super();
    }

    ngOnInit(): void {
       
    }



}