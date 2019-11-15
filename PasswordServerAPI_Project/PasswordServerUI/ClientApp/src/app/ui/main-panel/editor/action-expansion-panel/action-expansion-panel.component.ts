import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { BaseComponent } from "../../../../common/base/base.component";
import { ConfigurationService } from "../../../../core/services/configuration.service";
import { ApplicationAction } from "../../../../core/models/configuration/ApplicationAction";
import { ApplicationValidationMode } from "../../../../core/enums/application-validation-mode";


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
        private configurationService: ConfigurationService,
        //private language: TranslateService,
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