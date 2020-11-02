import { Status } from '../../Enums/status';


export class JourneyPlanStatus {
    planId: number;
    status: Status;
    constructor() {
        this.status = Status.Inactive;
    }
}