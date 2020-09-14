import { MapObject } from './mapObject';

export class Enums {
    

    public static WorkflowType :  MapObject[] = [{ id : 0, label : "Workflow For POSM" },

    { id : 1, label : "Workflow For CM Task" },
    { id : 2, label : "Workflow For FMD" },
    { id : 3, label : "Workflow For User" }
    
    ];

    // public static workflowStatusType : MapObject[] = [{ id : 0, label : "In active" },

    // { id : 1, label : "Active" },
    // { id : 2, label : "Pending" },
    // { id : 3, label : "Rejected" },
    // { id : 4, label : "Approved" },
    // { id : 5, label : "Completed" }
    // ];


    public static hierarchyType : MapObject[] = [

    { id : 1, label : "National" },
    { id : 2, label : "Region" },
    { id : 3, label : "Area" },
    { id : 4, label : "Territory" }
    
    ];


    constructor() {

    }

}