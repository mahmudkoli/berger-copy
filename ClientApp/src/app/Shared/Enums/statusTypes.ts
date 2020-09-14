import { MapObject } from './mapObject';

export class StatusTypes {


    public static statusType: MapObject[] = [
        { id: 0, label: "Inactive" },
        { id: 1, label: "Active" },
        { id: 2, label: "Pending"},
        { id: 3, label: "Revert"},
        { id: 4, label: "Rejected"},
        { id: 5, label: "Completed"},
        { id: 6, label: "NotCompleted"},
        { id: 7, label: "InCompleted"},
        { id: 8, label: "InPlace"},
        { id: 9, label: "NotInPlace"}
    ];


    constructor() {

    }

}