﻿import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { MetaData } from './urlBuilder';


@Injectable()
export class HttpPostService {
    constructor(
        private http: HttpClient,
        private metaData: MetaData,
        //private uiNotificationService: UiNotificationService
        ) { }

    private handleHttpError(error: any): Observable<any> {
        if (error instanceof HttpErrorResponse) {
           // this.uiNotificationService.handleMessage(NotificationLevel.Error, error.message);
            throw of(error);
        }
        if (error.value && error.value.desc) {
            //this.uiNotificationService.handleMessage(NotificationLevel.Error, error.value.desc);
            throw of(error);
        }
        //this.uiNotificationService.handleMessage(NotificationLevel.Error, error);
        throw of(error);
    }

    private handleBlobError(response: any): Observable<any> {
        throw of(response.value);
    }

    httpPost<T>(actionUrl: string, postData: any): Observable<T> {
        let url = this.metaData.getContextPath(actionUrl);
        let wrappedReq = this.metaData.wrap2Request(postData);
        let res = this.http.post(url, wrappedReq);
        return res.pipe(map(this.resolve), catchError((error) => this.handleHttpError(error)));
    }

    httpPostBlob(actionUrl: string, postData: any): Observable<HttpResponse<any>> {
        let url = this.metaData.getContextPath(actionUrl);
        let wrappedReq = this.metaData.wrap2Request(postData);
        let res = this.http.post(url, wrappedReq, { observe: 'response', responseType: 'blob' });
        return res.pipe(map(this.resolveBlob), catchError((error) => this.handleBlobError(error)));
    }

    private resolve<T>(response: any): any {
        if (response.exception) {
            let error = response.exception;
            throw of(error);
        }
        else {
            return response.payload as T;
        }
    }

    private resolveBlob(response: any): any {
        if (response.body.type === 'text/plain') {
            throw of(response);
        } else {
            return response;
        }
    }
}