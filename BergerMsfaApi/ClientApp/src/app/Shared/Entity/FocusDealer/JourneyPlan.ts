import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

export class FocusDealer {

    public id: number;
    public code: string;
    public employeeRegId: string;
    public validFrom: string;
    public validTo: string;

    public validFromNgbDate: NgbDate
    public validToNgbDate: NgbDate
  
    



    constructor() {
        this.id = 0;
        this.code = '',
        this.employeeRegId = '';
        this.validFrom = '';
        this.validTo=''
      

    }
}