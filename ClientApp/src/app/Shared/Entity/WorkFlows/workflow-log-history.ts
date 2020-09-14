

export class WorkflowLogHistory {

    public  id : number;
    public  userId : number;
    public  userName : string;
    public  workflowLogId : number;
    public  workflowStatus : number; 
    public  workflowTitle : string;
    public  comments : string;
    public  isSeen : boolean;
    public  createdTime: Date;

    constructor()
    {
        this.id = 0;
    }
}
