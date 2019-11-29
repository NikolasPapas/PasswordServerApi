import { NotificationLevel } from 'src/app/models/enums/notification-level';

export class UiNotification {
    level: NotificationLevel;
    messages: string[];
    active: boolean;
}