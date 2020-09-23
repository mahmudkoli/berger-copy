//export enum ModeOfApproval {
//    ApprovalRequired =1,
//    Notified = 2
//}

import { MapObject } from './mapObject';

export class ModeOfApprovals {


    public static modeOfApproval: MapObject[] = [
        { id: 1, label: "Approval Required" },
        { id: 2, label: "Notified" }
    ];


    constructor() {

    }

}