import { Injectable } from '@angular/core';
import { HttpPostService } from './http-post.service';
import { ApplicationAction } from '../models/configuration/ApplicationAction';
import { ConfigurationResponse } from '../models/configuration/configuration_response';
import { BaseService } from '../../common/base/base.service';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class ConfigurationService extends BaseService {
   
    private actions: ApplicationAction[] = [];
    private token: string;
    needLoginBool:boolean =false;
    isLoginIn: Subject<boolean> = new Subject<boolean>();  

    constructor() { 
        super(); 
        this.isLoginIn.subscribe((value) => {
            this.needLoginBool = value
        });
    }


    public getActions(): ApplicationAction[] {
        this.needLogin();
        return this.actions;
    }

    public needLogin(): any {
        this.needLoginBool = this.token != null && this.token != undefined;
        this.isLoginIn.next(!this.needLoginBool);
        return this.needLoginBool;
    }

    public getToken() {
        this.needLogin();
        return this.token;
    }
    public setToken(token: string) {
        this.token = token;
        this.needLogin();
       
    }

    public setLoginResponse(configuration: ConfigurationResponse): void {
        // this.profiles.splice(0, this.profiles.length);
        // for (let profile of configuration.profiles) {
        //     this.profiles.push(profile);
        // }
        this.actions = [];
        for (let action of configuration.actions) {
            this.actions.push(action);
        }
        this.token = configuration.token;
    }

}
