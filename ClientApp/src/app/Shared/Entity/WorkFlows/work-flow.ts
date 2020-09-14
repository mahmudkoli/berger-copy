//import { WorkFlowConfiguration } from '../WorkFlows/workflowconfiguration';
import { Status } from '../../Enums/status';
import { WorkFlowConfiguration } from './workflowconfiguration';

export class WorkFlow {

    public  id : number;
       
       
        public  name : string;

       
        public  action : string;

        public workflowType : number;
        public workflowTypeName : string;

        public  workflowStep : number;

        public  code : string;
        public configList : WorkFlowConfiguration[];
        public checked : boolean;
        public status : number;
        public status2 : string;

        constructor()
        {
            this.status = Status.Active;
            this.configList = [];
        }
}


