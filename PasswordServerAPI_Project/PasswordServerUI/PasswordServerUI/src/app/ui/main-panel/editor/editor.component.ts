import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "../../../common/base/base.component";
import { ConfigurationService } from "../../../core/services/configuration.service";
import { ApplicationAction } from "../../../core/models/configuration/ApplicationAction";


@Component({
    selector: 'app-editor',
    templateUrl: './editor.component.html',
    styleUrls: ['./editor.component.scss'],
})
export class EditorComponent extends BaseComponent implements OnInit  {
    
    actions:ApplicationAction[];


    constructor(
        private configurationService: ConfigurationService,
        //private language: TranslateService,
    ) {
        super();
    }

    ngOnInit(): void {
        this.actions = this.configurationService.getActions();
    }


    actionOpen(){

    }

}