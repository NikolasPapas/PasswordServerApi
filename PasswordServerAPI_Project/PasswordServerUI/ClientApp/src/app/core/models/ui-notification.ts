import { NotificationLevel } from "./enums/notification-level";


export class UiNotification {
    level: NotificationLevel;
    messages: string[];
    active: boolean;
}