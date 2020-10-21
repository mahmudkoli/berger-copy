import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { ModeOfApprovals } from '../../Enums/modeOfApproval';

export class JourneyPlan {

    public id: number;
    public dealers: number[];
    //public employeeRegId: string;
    //public approvedBy?: number;
    public visitDDate: NgbDate;
    public visitDate: string;
    //public approvedDate: string;
    //public approvedStatus: boolean;

    constructor() {
        this.id = 0;
        this.dealers=[]
        //this.code = 0,
        //this.employeeRegId = '';
        //this.approvedBy = null
        //this.approvedDate = '';
        //this.approvedStatus = false;
     
    }
   
}