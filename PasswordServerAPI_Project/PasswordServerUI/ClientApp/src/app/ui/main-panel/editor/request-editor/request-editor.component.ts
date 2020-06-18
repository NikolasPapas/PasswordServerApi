import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from "@angular/core";
import { FormArray, FormGroup } from "@angular/forms";
import { BaseComponent } from "../../../../common/base/base.component";
import { PasswordForm } from "./password-editor/password-form.model";
import { Strength } from 'src/app/models/enums/strength';

@Component({
    selector: 'app-request-editor-panel',
    templateUrl: './request-editor.component.html',
    styleUrls: ['./request-editor.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class RequestEditorComponent extends BaseComponent implements OnInit {

    @Input() account: FormGroup;
    @Output() selectedPasswordIndexEvent = new EventEmitter<number>();
    step = -1;

    constructor(
        //private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {

    }

    getColor(passwordStrength: Strength) {
        return passwordStrength == Strength.Danger ? '$warn' : '$primary';
    }

    addPassword() {
        (this.account.get('passwords') as FormArray).push(new PasswordForm().fromModel(null).buildForm());
        this.setStep((this.account.get('passwords') as FormArray).length - 1);
    }

    setStep(index: number) {
        this.selectedPasswordIndexEvent.emit(index);
        this.step = index;
    }

    closePassword(index: number) {
        if (this.step == index) {
            this.step = -1;
            this.selectedPasswordIndexEvent.emit(-1);
        }
    }

    nextStep() {
        this.step++;
    }

    prevStep() {
        this.step--;
    }
}