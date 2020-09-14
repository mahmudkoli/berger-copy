import { WorkflowLogHistory } from './workflow-log-history';

export class WorkflowLog {

    public  id : number;
    public  rowId : number; 
    public  workFlowFor : number;
    public  masterWorkFlowId : number;
    public  workflowStatus : number;
    public  pendingWorkflowCount : number;
    public  workflowMessage: string;
    public  createdTime: Date;
    public  data: any;
    public  logHistories : WorkflowLogHistory[];

    constructor()
    {
        this.logHistories = [];
    }

}
