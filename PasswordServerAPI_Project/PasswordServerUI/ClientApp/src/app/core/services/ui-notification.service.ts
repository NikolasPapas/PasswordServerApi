import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { UiNotification } from '../models/ui-notification';
import { NotificationLevel } from '../models/enums/notification-level';

@Injectable()
export class UiNotificationService {

    private notificationSubject = new Subject<UiNotification>();

    constructor() {
    }

    public handleMessage(level: NotificationLevel, msg: string) {
        this.handleMessages(level, [msg]);
    }

    public handleMessages(level: NotificationLevel, messages: string[]) {
        const notification: UiNotification = {
            level: level,
            messages: messages,
            active: true
        };
        this.notificationSubject.next(notification);
    }

    public getNotificationObservable(): Observable<UiNotification> {
        return this.notificationSubject.asObservable();
    }
}

