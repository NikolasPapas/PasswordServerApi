import { Component, OnInit, Input} from "@angular/core";
import { BaseComponent } from "../../../../common/base/base.component";
import { Password } from "../../../../core/models/password-model";
import { Account } from "../../../../core/models/account-model";
import { FormGroup, FormArray } from "@angular/forms";

@Component({
    selector: 'app-request-editor-panel',
    templateUrl: './request-editor.component.html',
    styleUrls: ['./request-editor.component.scss'],
})
export class RequestEditorComponent extends BaseComponent implements OnInit  {
    
    @Input() account:FormGroup;
    constructor(
        //private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {
        
    }


    actionOpen(){

    }

}