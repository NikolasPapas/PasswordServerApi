import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, ViewEncapsulation } from "@angular/core";
import { BaseComponent } from "../../../common/base/base.component";
import { ApplicationAction } from 'src/app/models/configuration/ApplicationAction';
import { FormGroup, FormArray } from '@angular/forms';
import { ConfigurationService } from 'src/app/services/configuration.service';
import { AccountService } from 'src/app/services/account-action.service';
import { FileSaveService } from 'src/app/services/file-save.service';
import { UiNotificationService } from 'src/app/services/ui-notification.service';
import { AccountForm } from './request-editor/account-editor/account-form.model';
import { PasswordForm } from './request-editor/password-editor/password-form.model';
import { AccountActionRequest } from 'src/app/models/requests-responses/requests/account-action-request';
import { Guid } from 'src/app/common/types/guid';
import { takeUntil } from 'rxjs/operators';
import { AccountActionResponse } from 'src/app/models/requests-responses/responses/account-action-response';
import { NotificationLevel } from 'src/app/models/enums/notification-level';
import { PasswordActionRequest } from 'src/app/models/requests-responses/requests/password-action-request';
import { PasswordActionResponse } from 'src/app/models/requests-responses/responses/password-action-response';
import { Account } from 'src/app/models/account-model';
import { BaseRequest } from 'src/app/models/requests-responses/requests/base-request';
import { MatBottomSheet } from '@angular/material';
import { BottomSheet } from '../../common/bottom-sheet/bottom-sheet.component';
import { DataNeeded } from 'src/app/models/enums/data-needed';

@Component({
    selector: 'app-editor',
    templateUrl: './editor.component.html',
    styleUrls: ['./editor.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class EditorComponent extends BaseComponent implements OnInit {

    activeActions: ApplicationAction[];
    actions: ApplicationAction[];
    ACTION_INDICATOR_ACCOUNT_CONTROLLER: string = "Account";
    ACTION_INDICATOR_PASSWORD_CONTROLLER: string = "Password";
    ACTION_INDICATOR_REPORT: string = "Report";
    ACTION_INDICATOR_ADD_ACCOUNT: string = "1086495e-fd61-4397-b3a9-87b737adeddd";

    @Input() selectedAction: number = -1;
    @Input() ActionEvent: ApplicationAction;
    @Input() AddAccountEvent: number;
    @Input() AddPasswordEvent: number;
    @Input() isActionAddAccountIsOn: boolean = false;

    @Output() IsActionAddPasswordIsOnEvent = new EventEmitter<number>();

    accountModels: FormGroup[] = [];

    expandedIndex: number = -1;
    public selectedAccountIndex: number = -1;
    selectedPasswordIndex: number = -1;



    constructor(
        private configurationService: ConfigurationService,
        private accountService: AccountService,
        private fileSaveService: FileSaveService,
        public uiNotificationService: UiNotificationService,
        public dialog: MatBottomSheet,
    ) {
        super();
    }

    ngOnInit(): void {
        this.actions = this.configurationService.getActions();
        this.activeActions = this.actions.filter(x => x.sendApplicationData == false);
    }

    ngOnChanges(changes: SimpleChanges) {
        for (let propName in changes) {
            if (propName === 'ActionEvent' && this.ActionEvent != null) {
                this.onActionSelected(this.ActionEvent);
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
        this.FieldActiveActions();
    }

    closeAccount(index: number) {
        if (this.selectedAccountIndex == index)
            this.collapseAllAccounts();
    }

    collapseAllAccounts() {
        this.selectedAccountIndex = -1;
        this.selectedPasswordIndex = -1;
        this.IsActionAddPasswordIsOnEvent.emit(this.selectedAccountIndex);
        this.FieldActiveActions();
    }

    selectedPasswordIndexEvent(passwordIndex: number) {
        this.selectedPasswordIndex = passwordIndex;
        this.FieldActiveActions();
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
        this.FieldActiveActions();
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


    FieldActiveActions() {
        this.activeActions = [];
        this.activeActions.push(... this.actions.filter(x => x.sendApplicationData == false));
        this.activeActions.push(... this.actions.filter(x => this.selectedAccountIndex != null && this.selectedAccountIndex >= 0 && x.dataNeeded == DataNeeded.Account));
        this.activeActions.push(... this.actions.filter(x => this.selectedPasswordIndex != null && this.selectedPasswordIndex >= 0 && x.dataNeeded == DataNeeded.Password));
        this.activeActions.push(... this.actions.filter(x => this.selectedAccountIndex != null && this.selectedAccountIndex >= 0 && this.selectedPasswordIndex != null && this.selectedPasswordIndex > 0 && x.dataNeeded == DataNeeded.All));
    }


    openBottomSheet(): void {
        this.FieldActiveActions();
        const dialogRef = this.dialog.open(BottomSheet, {
            data: this.activeActions,
        });

        dialogRef.afterDismissed().subscribe(res => {
            if (res) {
                this.onActionSelected(res);
                this.dialog.dismiss();
            }
            else
                this.dialog.dismiss();
        });
    }
    //#endregion
}