import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { HttpPostService } from './http-post.service';
import { Profile } from '../models/configuration/profiles';
import { ConfigurationResponse } from '../models/configuration/configuration_response';
import { BaseService } from '../../common/base/base.service';

@Injectable()
export class ConfigurationService extends BaseService {

    constructor(
        private httpPostService: HttpPostService,
    ) { super(); }

    private profiles: Profile[] = [];
    private token: string;
    private _isLogin: boolean = this.token != null && this.token != undefined;

    public getProfiles(): Profile[] {
        return this.profiles;
    }

    public getLogin():boolean{
        return this._isLogin;
    }

    public getToken(){
        return this.token;
    }

    private setConfiguration(configuration: ConfigurationResponse): void {
        this.profiles.splice(0, this.profiles.length);
        for (let profile of configuration.profiles) {
            this.profiles.push(profile);
        }
        this.token = configuration.token;
    }

    private getConfigurations(): Observable<ConfigurationResponse> {
        return this.httpPostService.httpPost<ConfigurationResponse>("api/configuration/getConfiguration", {});
    }
}
