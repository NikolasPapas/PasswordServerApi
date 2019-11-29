import { Component, Input, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { BaseComponent } from "../../../../../common/base/base.component";
import { Sex } from 'src/app/models/enums/sex-enum';
import { Role } from 'src/app/models/enums/role';

@Component({
    selector: 'app-account-editor-panel',
    templateUrl: './account-editor.component.html',
    styleUrls: ['./account-editor.component.scss'],
})
export class AccountEditorComponent extends BaseComponent implements OnInit {

    @Input() account: FormGroup;
    sex = Sex;
    role = Role;
    constructor(
        //private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {

    }


}