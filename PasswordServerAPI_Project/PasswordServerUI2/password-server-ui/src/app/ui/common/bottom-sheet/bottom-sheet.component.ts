import { BaseComponent } from 'src/app/common/base/base.component';
import { OnInit, Component, Inject, Output } from '@angular/core';
import { ConfigurationService } from 'src/app/services/configuration.service';
import { MatBottomSheetRef, MAT_DIALOG_DATA, MAT_BOTTOM_SHEET_DATA } from '@angular/material';
import { Guid } from 'src/app/common/types/guid';
import { ApplicationAction } from 'src/app/models/configuration/ApplicationAction';
import { AccountService } from 'src/app/services/account-action.service';

@Component({
    selector: 'app-bottom-sheet',
    templateUrl: './bottom-sheet.component.html',
    styleUrls: ['./bottom-sheet.component.scss'],
})
export class BottomSheet extends BaseComponent implements OnInit {

    constructor(
        private _bottomSheetRef: MatBottomSheetRef<BottomSheet>,
        @Inject(MAT_BOTTOM_SHEET_DATA) public actions: ApplicationAction[],
    ) {
        super();
    }

    ngOnInit(): void {

    }

    cancel() {
        this._bottomSheetRef.dismiss(null);
    }

    executeAction(index: number) {
        this._bottomSheetRef.dismiss(this.actions[index]);
    }

}
