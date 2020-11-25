import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

export class FocusDealer {

    public id: number;
    public code: string;
    public employeeId: string;
    public validFrom: string;
    public validTo: string;
    public employeeName: string;
    public dealerName: string;

    public validFromNgbDate: NgbDate
    public validToNgbDate: NgbDate
  
    



    constructor() {
        this.id = 0;
        this.code = '',
        this.employeeId = '';
        this.validFrom = '';
        this.validTo=''
      

    }
}