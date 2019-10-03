import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BaseComponent } from 'src/app/common/base/base.component';
import { ConfigurationService } from '../core/services/configuration.service';


@Component({
    selector: 'app-application',
    templateUrl: './application.component.html',
    styleUrls: ['./application.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ApplicationComponent extends BaseComponent implements OnInit {

    // public selectedTabIndex: number;
    // private allConsoles: ConsoleData[] = [];
    // consoleDataList: ConsoleData[] = [];
    // dialoRef: MatDialogRef<ApplicationLoaderDialogComponent>;

    // private searchRequestPerProfile: Map<string, SearchRequest> = new Map<string, SearchRequest>();

    public mastDoLogIn:boolean = true;
    // public profileGroupEnum = ProfileGroup;

    constructor(
        private language: TranslateService,
        public configurationService: ConfigurationService
        //public dialog: MatDialog
    ) {
        super();
        // this.selectedTabIndex = 0;
        // this.allConsoles = this.getAllConsoles();
    }

    // private getAllConsoles() {
    //     return new Array<ConsoleData>(
    //         { label: this.language.instant('APPLICATION.CONSOLES.SUBMITTER'), profileGroup: ProfileGroup.Submitter },
    //         { label: this.language.instant('APPLICATION.CONSOLES.APPROVER'), profileGroup: ProfileGroup.Approver },
    //         { label: this.language.instant('APPLICATION.CONSOLES.VIEWER'), profileGroup: ProfileGroup.Viewer },
    //         { label: this.language.instant('APPLICATION.CONSOLES.OWNER'), profileGroup: ProfileGroup.Owner },
    //         { label: this.language.instant('APPLICATION.CONSOLES.ADMIN'), profileGroup: ProfileGroup.Admin }
    //     );
    // }

    ngOnInit() {
     //   this.mastDoLogIn= this.configurationService.getLogin();

        // for (let consoleData of this.allConsoles) {
        //     for (let userProfile of this.configurationService.getProfiles()) {
        //         if (consoleData.profileGroup.toString() === userProfile.name) {
        //             this.consoleDataList.push({ label: consoleData.label, profileGroup: consoleData.profileGroup });
        //             break;
        //         }
        //     }
        // }
    }

    // tabChanged(event: MatTabChangeEvent) {
    // }

    // onFilterChange(profile: string, request: SearchRequest) {
    //     this.searchRequestPerProfile.set(profile, Object.assign({}, request));
    // }

    // isOwnerOrAdmin(profile: string): boolean {
    //     return profile === ProfileGroup.Admin || profile === ProfileGroup.Owner;
    // }

    // trackByFn(index, item) {
    //     return item.profileGroup;
    // }
}
