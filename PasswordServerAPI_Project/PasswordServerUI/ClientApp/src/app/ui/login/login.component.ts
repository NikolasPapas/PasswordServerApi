import { Component, Input, OnInit, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BaseComponent } from '../../common/base/base.component';
import { ConfigurationService } from '../../core/services/configuration.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class LoginComponent extends BaseComponent implements OnInit {

    @Input() formGroup: FormGroup;
    @Output() LoginEvent = new EventEmitter();

    constructor(
        private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {

    }

    login(){
        this.LoginEvent.emit();
    }

}