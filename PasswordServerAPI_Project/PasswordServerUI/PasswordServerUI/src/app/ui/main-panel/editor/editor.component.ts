import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../../../common/base/base.component";
import { ConfigurationService } from "../../../core/services/configuration.service";
import { ApplicationAction } from "../../../core/models/configuration/ApplicationAction";
import { FormGroup, FormArray } from "@angular/forms";
import { Guid } from "../../../common/types/guid";
import { AccountService } from "../../../core/services/account-action.service";
import { takeUntil } from "rxjs/operators";
import { AccountActionRequest } from "../../../core/models/requests/account-request/account-action-request";
import { AccountActionResponse } from "../../../core/models/response/account-response/account-action-response";
import { AccountForm } from "./request-editor/account-editor/account-form.model";
import { PasswordActionRequest } from "../../../core/models/requests/password-request/password-action-request";
import { PasswordActionResponse } from "../../../core/models/response/password-response/password-action-response";
import { PasswordForm } from "./request-editor/password-editor/password-form.model";
import { Account } from "../../../core/models/account-model";


@Component({
    selector: 'app-editor',
    templateUrl: './editor.component.html',
    styleUrls: ['./editor.component.scss'],
})
export class EditorComponent extends BaseComponent implements OnInit {

    actions: ApplicationAction[];
    ACTION_INDICATOR_ACCOUNT_CONTROLLER: string = "Account";
    ACTION_INDICATOR_PASSWORD_CONTROLLER: string = "Password";

    expandedIndex: number = -1;
    accountModels: FormGroup[] = [];
    selectedAccountIndex: number = -1;
    selectedPasswordIndex: number = -1;

    constructor(
        private configurationService: ConfigurationService,
        private accountService: AccountService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.actions = this.configurationService.getActions();
        //this.accountModels.push(new AccountForm().fromModel(null).buildForm());
    }

    open(index: number) {
        this.collapseAll();
        this.expandedIndex = index;
    }

    close(index: number) {
        this.expandedIndex = -1;
        this.collapseAll();
    }

    collapseAll() {
        this.expandedIndex = -1;
    }

    openAccount(index: number) {
        this.collapseAllAccounts();
        this.selectedAccountIndex = index;
    }

    closeAccount(index: number) {
        this.selectedAccountIndex = -1;
        this.collapseAllAccounts();
    }

    collapseAllAccounts() {
        this.selectedAccountIndex = -1;
    }

    selectedPasswordIndexEvent(passwordIndex: number) {
        this.selectedPasswordIndex = passwordIndex;
    }

    onActionSelected(action: ApplicationAction) {
        if (action.controllerSend == this.ACTION_INDICATOR_ACCOUNT_CONTROLLER) {
            this.onActionAccountSelected(action);
        } else if (action.controllerSend == this.ACTION_INDICATOR_PASSWORD_CONTROLLER) {
            this.onActionPasswordSelected(action);
        }

    }

    addAccount(){
        this.accountModels.push(new AccountForm().fromModel(null).buildForm());
    }

    //#region Account Action

    onActionAccountSelected(action: ApplicationAction) {
        let request: AccountActionRequest = {
            account: action.sendApplicationData ? this.selectedAccountIndex == -1 ? new AccountForm().fromModel(null).buildForm().getRawValue() : this.accountModels[this.selectedAccountIndex].getRawValue() : null,
            actionId: action.id,
            accountId: action.sendApplicationData && this.selectedAccountIndex != -1 ? this.accountModels[this.selectedAccountIndex].get('accountId').value : Guid.createEmpty()
        }

        this.accountService.accountAction(request, action.controllerPath)
            .pipe(takeUntil(this._destroyed)).subscribe(
                res => this.onActionAccountSuccess(res, action)
                , error => this.onActionError(error)
            );
    }

    onActionAccountSuccess(res: AccountActionResponse, action: ApplicationAction) {
        this.clearAll();
        res.accounts.forEach(account => {
            this.accountModels.push(new AccountForm().fromModel(account).buildForm());
        });
    }

    onActionError(error: any) {

    }
    //#endregion

    //#region Password Action

    onActionPasswordSelected(action: ApplicationAction) {
        let request: PasswordActionRequest = {
            password: action.sendApplicationData ? this.selectedAccountIndex == -1 ? new PasswordForm().fromModel(null).buildForm().getRawValue() : this.selectedPasswordIndex != -1 ? ((this.accountModels[this.selectedAccountIndex].get('passwords') as FormArray).controls[this.selectedPasswordIndex] as FormGroup).getRawValue() : new PasswordForm().fromModel(null).buildForm() : null,
            actionId: action.id,
            accountId: this.selectedAccountIndex == -1 ? Guid.createEmpty() : this.accountModels[this.selectedAccountIndex].get('accountId').value
        }

        this.accountService.passwordAction(request, action.controllerPath)
            .pipe(takeUntil(this._destroyed)).subscribe(
                res => this.onActionPasswordSuccess(res, action)
                , error => this.onActionError(error)
            );
    }

    onActionPasswordSuccess(res: PasswordActionResponse, action: ApplicationAction) {
        // if (action.refreshAfterAction) {
        //     this.clearAll();
        //     return;
        // }
        if (this.selectedAccountIndex >= 0 && this.accountModels[this.selectedAccountIndex] != null) {
            (this.accountModels[this.selectedAccountIndex].get('passwords') as FormArray).clear();
            res.passwords.forEach(pass => {
                (this.accountModels[this.selectedAccountIndex].get('passwords') as FormArray).push(new PasswordForm().fromModel(pass).buildForm());
            })
        } else {
            let account: Account = new Account();
            account.passwords = res.passwords;
            this.accountModels.push(new AccountForm().fromModel(account).buildForm())
        }
    }

    clearAll() {
        this.accountModels = [];
        this.selectedAccountIndex = -1;
        this.selectedPasswordIndex = -1;
    }

    //#endregion
}