import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../../../common/base/base.component";
import { ConfigurationService } from "../../../core/services/configuration.service";
import { ApplicationAction } from "../../../core/models/configuration/ApplicationAction";
import { Account } from "../../../core/models/account-model";
import { FormGroup, FormArray } from "@angular/forms";
import { Guid } from "../../../common/types/guid";
import { AccountService } from "../../../core/services/account-action.service";
import { takeUntil } from "rxjs/operators";
import { AccountActionRequest } from "../../../core/models/requests/account-request/account-action-request";
import { AccountActionResponse } from "../../../core/models/response/account-response/account-action-response";
import { AccountForm } from "./request-editor/account-editor/account-form.model";


@Component({
    selector: 'app-editor',
    templateUrl: './editor.component.html',
    styleUrls: ['./editor.component.scss'],
})
export class EditorComponent extends BaseComponent implements OnInit {

    actions: ApplicationAction[];
    isOpen: boolean = false;

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

    onActionSelected(id: Guid) {
        let request: AccountActionRequest = {
            account: this.accountModel.getRawValue(),
            actionId: id,
            accountId: Guid.createEmpty()           
        }

        this.accountService.accountAction(request)
            .pipe(takeUntil(this._destroyed)).subscribe(
                res => this.onActionSuccess(res, request)
                , error => this.onActionError(error)
            );
    }

    onActionSuccess(res:AccountActionResponse ,request:AccountActionRequest){
        this.accountModel =null;
        res.accounts.forEach(account => {
        this.accountModel = new AccountForm().fromModel(account).buildForm();
        });
    }

    onActionError(error:any){

    }
}