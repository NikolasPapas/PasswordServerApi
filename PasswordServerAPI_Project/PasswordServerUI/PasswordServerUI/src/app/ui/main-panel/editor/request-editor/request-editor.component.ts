import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { BaseComponent } from "../../../../common/base/base.component";
import { FormGroup, FormArray } from "@angular/forms";
import { PasswordForm } from "./password-editor/password-form.model";

@Component({
    selector: 'app-request-editor-panel',
    templateUrl: './request-editor.component.html',
    styleUrls: ['./request-editor.component.scss'],
})
export class RequestEditorComponent extends BaseComponent implements OnInit {

    @Input() account: FormGroup;
    @Output() selectedPasswordIndexEvent = new EventEmitter<number>();

    step = 0;

    constructor(
        //private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {

    }

    addPassword() {
        (this.account.get('passwords')as FormArray).push(new PasswordForm().fromModel(null).buildForm());
    }

    

    setStep(index: number) {
        this.selectedPasswordIndexEvent.emit(index);
        this.step = index;
    }

    nextStep() {
        this.step++;
    }

    prevStep() {
        this.step--;
    }

}