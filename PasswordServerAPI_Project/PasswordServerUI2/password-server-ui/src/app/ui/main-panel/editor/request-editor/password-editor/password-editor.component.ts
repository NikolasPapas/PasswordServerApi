import { Component, OnInit, Input } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { BaseComponent } from "../../../../../common/base/base.component";
import { Strength } from 'src/app/models/enums/strength';
import { Sensitivity } from 'src/app/models/enums/sensitivity';

@Component({
    selector: 'app-password-editor-panel',
    templateUrl: './password-editor.component.html',
    styleUrls: ['./password-editor.component.scss'],
})
export class PasswordEditorComponent extends BaseComponent implements OnInit {

    @Input() password: FormGroup;

    
    strength = Strength;
    sensitivity = Sensitivity;


    constructor(
        //private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {

    }

}