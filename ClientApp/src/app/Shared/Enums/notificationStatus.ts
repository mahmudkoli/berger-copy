//export enum NotificationStatus {
//    NotificationNotSent,
//    NotificationSent
//} 

import { MapObject } from './mapObject';

export class NotificationStatuses {

    public static notificationStatus: MapObject[] = [
        { id: 0, label: "Notification Not Sent" },
        { id: 1, label: "Notification Sent" }
    ];

    constructor() {

    }

}