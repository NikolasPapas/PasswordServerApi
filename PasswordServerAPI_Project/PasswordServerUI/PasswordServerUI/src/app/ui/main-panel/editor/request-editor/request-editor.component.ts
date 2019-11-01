import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormArray, FormGroup } from "@angular/forms";
import { BaseComponent } from "../../../../common/base/base.component";
import { Strength } from "../../../../core/models/enums/strength";
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

    getColor(passwordStrength: Strength) {
        return passwordStrength == Strength.Danger ? 'red' : passwordStrength == Strength.VeryStrong ? 'green' : 'white';
    }

    addPassword() {
        (this.account.get('passwords') as FormArray).push(new PasswordForm().fromModel(null).buildForm());
        this.setStep((this.account.get('passwords') as FormArray).length - 1);
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