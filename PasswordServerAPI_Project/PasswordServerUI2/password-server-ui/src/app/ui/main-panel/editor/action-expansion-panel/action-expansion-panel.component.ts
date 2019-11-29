import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { BaseComponent } from "../../../../common/base/base.component";
import { ApplicationAction } from 'src/app/models/configuration/ApplicationAction';
import { ApplicationValidationMode } from 'src/app/models/enums/application-validation-mode';
import { ConfigurationService } from 'src/app/services/configuration.service';


@Component({
    selector: 'app-action-expansion-panel',
    templateUrl: './action-expansion-panel.component.html',
    styleUrls: ['./action-expansion-panel.component.scss'],
})
export class ActionExpansionPanelComponent extends BaseComponent implements OnInit  {
    
    @Input() action: ApplicationAction;
    @Output() onActionSelected = new EventEmitter<ApplicationAction>();
    validationMode=ApplicationValidationMode;

    constructor(
    ) {
        super();
    }

    ngOnInit(): void {
        
    }


    actionOpen(){

    }

    actionSelected(){
        this.onActionSelected.emit(this.action);
    }

}