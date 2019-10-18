import { Component, OnInit, Input } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { BaseComponent } from "../../../../../common/base/base.component";

@Component({
    selector: 'app-account-editor-panel',
    templateUrl: './account-editor.component.html',
    styleUrls: ['./account-editor.component.scss'],
})
export class AccountEditorComponent extends BaseComponent implements OnInit {

    @Input() account: FormGroup;
    constructor(
        //private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {

    }


}