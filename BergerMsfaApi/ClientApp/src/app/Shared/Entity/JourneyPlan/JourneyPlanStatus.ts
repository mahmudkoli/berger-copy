import { Status } from '../../Enums/status';
import { PlanStatus } from '../../Enums/PlanStatus';


export class JourneyPlanStatus {
    planId: number;
    status: PlanStatus;
    comment: string;
    constructor() {
        this.status = PlanStatus.Pending;

    }
}