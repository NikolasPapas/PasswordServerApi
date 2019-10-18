import { Component, OnInit, Input } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { BaseComponent } from "../../../../../common/base/base.component";

@Component({
    selector: 'app-password-editor-panel',
    templateUrl: './password-editor.component.html',
    styleUrls: ['./password-editor.component.scss'],
})
export class PasswordEditorComponent extends BaseComponent implements OnInit {

    @Input() password: FormGroup;
    constructor(
        //private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {

    }

}