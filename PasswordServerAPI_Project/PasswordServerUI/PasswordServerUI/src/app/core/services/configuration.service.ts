import { Injectable } from '@angular/core';
import { HttpPostService } from './http-post.service';
import { ApplicationAction } from '../models/configuration/ApplicationAction';
import { ConfigurationResponse } from '../models/configuration/configuration_response';
import { BaseService } from '../../common/base/base.service';

@Injectable()
export class ConfigurationService extends BaseService {

    constructor(
        private httpPostService: HttpPostService,
    ) { super(); }

    private actions: ApplicationAction[] = [];
    private token: string;
    private _isLogin: boolean = this.token != null && this.token != undefined;


    public getActions(): ApplicationAction[] {
        return this.actions;
    }

    public needLogin(): boolean {
        return this._isLogin;
    }

    public getToken() {
        return this.token;
    }

    public setLoginResponse(configuration: ConfigurationResponse): void {
        // this.profiles.splice(0, this.profiles.length);
        // for (let profile of configuration.profiles) {
        //     this.profiles.push(profile);
        // }
        this.actions.slice(0, this.actions.length);
        for (let action of configuration.actions) {
            this.actions.push(action);
        }
        this.token = configuration.token;
    }

}
