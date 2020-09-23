import { MapObject } from './mapObject';

export enum WorkflowStatusEnum {
    Approved = 1,
    Pending = 2,
    Rejected = 3,
    ApprovalInProgress = 4,
    Completed = 5
}

export class WorkflowStatusEnumLabel {
    public static workflowStatusEnumLabel : MapObject[] = [
        { id : 1, label : "Approved" },
        { id : 3, label : "Rejected" }
    ];

    constructor() {
    }
}