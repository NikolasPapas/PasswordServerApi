import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfigurationService } from '../core/services/configuration.service';
import { TokenRequest } from '../core/models/requests/token-request';
import { takeUntil } from 'rxjs/operators';
import { LoginModel } from './login/login.model';
import { FormGroup } from '@angular/forms';
import { AccountService } from '../core/services/account-action.service';
import { BaseComponent } from '../common/base/base.component';


@Component({
    selector: 'app-application',
    templateUrl: './application.component.html',
    styleUrls: ['./application.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class ApplicationComponent extends BaseComponent implements OnInit  {

    // public selectedTabIndex: number;
    // private allConsoles: ConsoleData[] = [];
    // consoleDataList: ConsoleData[] = [];
    // dialoRef: MatDialogRef<ApplicationLoaderDialogComponent>;

    // private searchRequestPerProfile: Map<string, SearchRequest> = new Map<string, SearchRequest>();

    public mastDoLogIn:boolean = true;
    loginForm:FormGroup;
    // public profileGroupEnum = ProfileGroup;

    constructor(
        private language: TranslateService,
        public configurationService: ConfigurationService,
        private accountService :AccountService,
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
        this.loginForm = new LoginModel().fromModel().buildForm();
        // for (let consoleData of this.allConsoles) {
        //     for (let userProfile of this.configurationService.getProfiles()) {
        //         if (consoleData.profileGroup.toString() === userProfile.name) {
        //             this.consoleDataList.push({ label: consoleData.label, profileGroup: consoleData.profileGroup });
        //             break;
        //         }
        //     }
        // }
    }


    login(){
        let loginRequest : TokenRequest ={ username : this.loginForm.get('username').value , password : this.loginForm.get('password').value};
        this.accountService.login(loginRequest).pipe(takeUntil(this._destroyed)).subscribe(
            response => {
                this.configurationService.setLoginResponse(response)
                this.mastDoLogIn= this.configurationService.needLogin();
            });
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
