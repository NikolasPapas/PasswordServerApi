import { Component, Input, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { BaseComponent } from "../../../../../common/base/base.component";
import { Role } from "../../../../../core/models/enums/role";
import { Sex } from "../../../../../core/models/enums/sex-enum";

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