import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { BaseComponent } from "../../../../common/base/base.component";
import { ConfigurationService } from "../../../../core/services/configuration.service";
import { ApplicationAction } from "../../../../core/models/configuration/ApplicationAction";
import { Guid } from "../../../../common/types/guid";


@Component({
    selector: 'app-editor-expansion-panel',
    templateUrl: './editor-expansion-panel.component.html',
    styleUrls: ['./editor-expansion-panel.component.scss'],
})
export class EditorExpansionPanelComponent extends BaseComponent implements OnInit  {
    
    @Input() action: ApplicationAction;
    @Output() onActionSelected = new EventEmitter<ApplicationAction>();

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