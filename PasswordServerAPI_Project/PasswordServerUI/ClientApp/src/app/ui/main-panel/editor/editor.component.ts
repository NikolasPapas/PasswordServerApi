import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges } from "@angular/core";
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
import { BaseRequest } from "../../../core/models/requests/base-request";
import { FileSaveService } from "../../../core/services/file-save.service";
import { UiNotificationService } from "../../../core/services/ui-notification.service";
import { NotificationLevel } from "../../../core/models/enums/notification-level";


@Component({
    selector: 'app-editor',
    templateUrl: './editor.component.html',
    styleUrls: ['./editor.component.scss'],
})
export class EditorComponent extends BaseComponent implements OnInit {

    actions: ApplicationAction[];
    ACTION_INDICATOR_ACCOUNT_CONTROLLER: string = "Account";
    ACTION_INDICATOR_PASSWORD_CONTROLLER: string = "Password";
    ACTION_INDICATOR_REPORT: string = "Report";
    ACTION_INDICATOR_ADD_ACCOUNT: string = "1086495e-fd61-4397-b3a9-87b737adeddd";

    @Input() selectedAction: number = -1;
    @Input() ActionEvent: ApplicationAction;
    @Input() AddAccountEvent: number;
    @Input() AddPasswordEvent: number;

    @Output() IsActionAddPasswordIsOnEvent = new EventEmitter<number>();

    accountModels: FormGroup[] = [];

    expandedIndex: number = -1;
    selectedAccountIndex: number = -1;
    selectedPasswordIndex: number = -1;
    isActionAddAccountIsOn: boolean = false;


    constructor(
        private configurationService: ConfigurationService,
        private accountService: AccountService,
        private fileSaveService: FileSaveService,
        public uiNotificationService: UiNotificationService
    ) {
        super();
    }

    ngOnInit(): void {
        this.actions = this.configurationService.getActions();
        //this.accountModels.push(new AccountForm().fromModel(null).buildForm());
    }

    ngOnChanges(changes: SimpleChanges) {
        for (let propName in changes) {
            if (propName === 'ActionEvent' && this.ActionEvent) {
                this.onActionSelected(this.ActionEvent);
                this.ActionEvent = null;
            }
            if (propName === 'AddAccountEvent' && this.AddAccountEvent != null) {
                this.addAccount();
                this.AddAccountEvent = null;
            }
            if (propName === 'AddPasswordEvent' && this.AddPasswordEvent != null) {
                this.addPassword();
                this.AddPasswordEvent = null;
            }
        }
    }

    open(index: number) {
        this.collapseAll();
        this.expandedIndex = index;
    }

    close(index: number) {
        this.collapseAll();
    }

    collapseAll() {
        this.expandedIndex = -1;
    }

    openAccount(index: number) {
        this.collapseAllAccounts();
        this.selectedAccountIndex = index;
        this.IsActionAddPasswordIsOnEvent.emit(this.selectedAccountIndex);
    }

    closeAccount(index: number) {
        if (this.selectedAccountIndex == index)
            this.collapseAllAccounts();
    }

    collapseAllAccounts() {
        this.selectedAccountIndex = -1;
        this.IsActionAddPasswordIsOnEvent.emit(this.selectedAccountIndex);
    }

    selectedPasswordIndexEvent(passwordIndex: number) {
        this.selectedPasswordIndex = passwordIndex;
    }

    onActionSelected(action: ApplicationAction) {
        if (action.controllerSend == this.ACTION_INDICATOR_ACCOUNT_CONTROLLER) {
            this.onActionAccountSelected(action);
        } else if (action.controllerSend == this.ACTION_INDICATOR_PASSWORD_CONTROLLER) {
            this.onActionPasswordSelected(action);
        } else if (action.controllerSend == this.ACTION_INDICATOR_REPORT) {
            this.onSentExportPDFApplication(action)
        }

    }

    addAccount() {
        this.accountModels.push(new AccountForm().fromModel(null).buildForm());
    }

    addPassword() {
        if (this.accountModels[this.selectedAccountIndex] != null && this.accountModels[this.selectedAccountIndex].get('passwords') != null) {
            (this.accountModels[this.selectedAccountIndex].get('passwords') as FormArray).push(new PasswordForm().fromModel(null).buildForm());
            this.selectedPasswordIndex = (this.accountModels[this.selectedAccountIndex].get('passwords') as FormArray).length - 1;
        }
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
        if (res.warningMessages)
            this.uiNotificationService.handleMessages(NotificationLevel.Warning, res.warningMessages);
        res.accounts.forEach(account => {
            this.accountModels.push(new AccountForm().fromModel(account).buildForm());
        });
    }

    onActionError(error: any) {
        if (error.warningMessages)
            this.uiNotificationService.handleMessages(NotificationLevel.Warning, error.warningMessages);
        //Notification For Error
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
        if (res.warningMessages)
            this.uiNotificationService.handleMessages(NotificationLevel.Warning, res.warningMessages);
        if (this.selectedAccountIndex >= 0 && this.accountModels[this.selectedAccountIndex] != null) {
            (this.accountModels[this.selectedAccountIndex].get('passwords') as FormArray).clear();
            res.passwords.forEach(pass => {
                (this.accountModels[this.selectedAccountIndex].get('passwords') as FormArray).push(new PasswordForm().fromModel(pass).buildForm());
            })
        } else {
            let account: Account = new Account();
            if (res.passwords != null)
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


    //#region Export 
    private onSentExportPDFApplication(action: ApplicationAction) {
        let request: BaseRequest = {
            actionId: action.id,
            accountId: action.id
        };
        this.accountService.getApplicationPdf(request, action.controllerPath)
            .pipe(takeUntil(this._destroyed)).subscribe(
                res => { this.onExportPDFSuccess(res) },
                error => { this.onExportPDFError(error) }
            );
    }

    private onExportPDFSuccess(res: any) {
        if (res.warningMessages)
            this.uiNotificationService.handleMessages(NotificationLevel.Warning, res.warningMessages);
        this.fileSaveService.saveBlob(res);
    }

    private onExportPDFError(error: any) {
        if (error.warningMessages)
            this.uiNotificationService.handleMessages(NotificationLevel.Warning, error.warningMessages);
        this.fileSaveService.handleError(error);
    }


    // this.applicationService.getApplicationListExcel(this.search)
    // .pipe(takeUntil(this._destroyed)).subscribe(res => this.fileSaveService.saveBlob(res),
    //     error => this.fileSaveService.handleError(error, this.uiNotificationService));

    //#endregion
}