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


@Component({
    selector: 'app-editor',
    templateUrl: './editor.component.html',
    styleUrls: ['./editor.component.scss'],
})
export class EditorComponent extends BaseComponent implements OnInit {

    actions: ApplicationAction[];
    isOpen: boolean = false;
    ACTION_INDICATOR_ACCOUNT_CONTROLLER: string ="Account";
    ACTION_INDICATOR_PASSWORD_CONTROLLER: string ="Password";

    expandedIndex: number = -1;
    lastOpened: number;
    accountModel: FormGroup;

    constructor(
        private configurationService: ConfigurationService,
        private accountService: AccountService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.actions = this.configurationService.getActions();
        this.accountModel = new AccountForm().fromModel(null).buildForm();
    }

    open(index: number) {
        this.isOpen = true;
        this.collapseAll();
        this.expandedIndex = index;
    }

    close(index: number) {
        this.isOpen = false;
        if (this.expandedIndex === index)
            this.lastOpened = index;
        this.collapseAll();
    }

    collapseAll() {
        this.expandedIndex = -1;
    }

    onActionSelected(action: ApplicationAction) {
        if (action.controllerSend == this.ACTION_INDICATOR_ACCOUNT_CONTROLLER) {
            this.onActionAccountSelected(action);
        } else if(action.controllerSend == this.ACTION_INDICATOR_PASSWORD_CONTROLLER){
            this.onActionPasswordSelected(action);
        }

    }

    //#region Account Action

    onActionAccountSelected(action: ApplicationAction) {
        let request: AccountActionRequest = {
            account: this.accountModel.getRawValue(),
            actionId: action.id,
            accountId: Guid.createEmpty()
        }

        this.accountService.accountAction(request, action.controllerPath)
            .pipe(takeUntil(this._destroyed)).subscribe(
                res => this.onActionAccountSuccess(res, request)
                , error => this.onActionError(error)
            );
    }

    onActionAccountSuccess(res: AccountActionResponse, request: AccountActionRequest) {
        this.accountModel = null;
        res.accounts.forEach(account => {
            this.accountModel = new AccountForm().fromModel(account).buildForm();
        });
    }

    onActionError(error: any) {

    }
    //#endregion

    //#region Password Action

    onActionPasswordSelected(action: ApplicationAction) {
        let request: PasswordActionRequest = {
            password: this.accountModel.getRawValue(),
            actionId: action.id,
            accountId: Guid.createEmpty()
        }

        this.accountService.passwordAction(request, action.controllerPath)
            .pipe(takeUntil(this._destroyed)).subscribe(
                res => this.onActionPasswordSuccess(res, request)
                , error => this.onActionError(error)
            );
    }

    onActionPasswordSuccess(res: PasswordActionResponse, request: PasswordActionRequest) {
        (this.accountModel.get('passwords') as FormArray).clear();
        res.passwords.forEach(pass => {
            (this.accountModel.get('passwords') as FormArray).push(new PasswordForm().fromModel(pass).buildForm());
        })
    }

    //#endregion
}