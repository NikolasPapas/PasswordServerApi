import { Injectable } from "@angular/core";
import { Meta } from "@angular/platform-browser";
import { Guid } from '../common/types/guid';

interface IWrapHeader {
    ID: string;
    Application: string;
    Company: string;
    Channel: string;
    Bank: string;
}

@Injectable()
export class MetaData {
    constructor(
        private meta: Meta
    ) { }
    get = (name: string): string => {
        if (this[name] == null) {
            this[name] = this.meta.getTag("name='" + name + "'").content;
        }
        return this[name];
    };

    private removeSlash = function (path: string) {
        if (path && path.length > 0) {
            return (path.charAt(0) == '/') ? path.slice(1) : path;
        }
        return '';
    }

    getContextPath = (path: string) => {
        return "http://localhost:53257/api/"+ this.removeSlash(path);
    }


    wrap2Request = (payload: any) => {

        var uuidStr = Guid.create().toString();//uuid();

        payload = payload || {};
        payload.userId = this.get('userId');
        return {
            header: <IWrapHeader>{
                ID: uuidStr,
                Application: this.get('appId'),
                Company: this.get('company'),
                Bank: 'NBG',
                Channel: this.get('channel')
            },
            payload: payload
        };
    }
}