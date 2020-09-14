import { Status } from "../../Enums/status";
import { YesNo } from "../../Enums/yesno";

export class WorkFlowConfiguration {
    public id: number;
    public orgRoleId: number;
    public orgRoleName: string;
    public masterWorkFlowId: number;
    public masterWorkFlowName: string;
    //public workFlow: WorkFlow;
    public modeOfApproval: number;
    public approvalStatus: number;
    public rejectedStatus: number;
    public notificationStatus: number;
    public status: number;
    public sequence : number;
    constructor() {
        this.id = 0;
        this.status = Status.Active;
    }
}