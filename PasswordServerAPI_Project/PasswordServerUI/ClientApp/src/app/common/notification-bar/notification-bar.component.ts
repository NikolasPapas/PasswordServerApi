import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { interval } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { BaseComponent } from '../base/base.component';
import { NotificationLevel } from 'src/app/models/enums/notification-level';
import { UiNotificationService } from 'src/app/services/ui-notification.service';
import { UiNotification } from './ui-notification';

@Component({
    selector: 'app-notification-bar',
    templateUrl: './notification-bar.component.html',
    styleUrls: ['./notification-bar.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class NotificationBarComponent extends BaseComponent implements OnInit {

    notifications: Array<UiNotification> = new Array<UiNotification>();
    notificationLevelEnum = NotificationLevel;

    constructor(
        private notificationService: UiNotificationService
    ) { super(); }

    ngOnInit() {
        this.notificationService.getNotificationObservable().pipe(takeUntil(this._destroyed))
            .pipe(takeUntil(this._destroyed)).subscribe(x => {
                this.addNotification(x);
            });
    }

    private close(i: number) {

        if (!this.notifications[i])
            return;

        this.notifications[i].active = false;

        interval(300).pipe(take(1), takeUntil(this._destroyed)).subscribe(
            res => {
                if (this.notifications[i].active == false)
                    this.notifications.splice(i, 1);
            });
    }

    private addNotification(notification: UiNotification) {

        this.notifications.unshift(notification);
        if (notification.level == NotificationLevel.Success)
            interval(4000).pipe(take(1), takeUntil(this._destroyed)).subscribe(
                res => {
                    this.close(this.notifications.indexOf(notification));
                });
    }

    notificationTrackByFn(index, item: UiNotification) { return index; }
}