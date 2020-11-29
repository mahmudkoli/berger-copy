import { Component, OnInit } from '@angular/core';
import { SchemeDetail, SchemeMaster } from '../../../Shared/Entity/Scheme/SchemeMaster';
import { Status } from '../../../Shared/Enums/status';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-schemedetail-add',
  templateUrl: './schemedetail-add.component.html',
  styleUrls: ['./schemedetail-add.component.css']
})
export class SchemedetailAddComponent implements OnInit {
    schemeDetailModel: SchemeDetail = new SchemeDetail();
    changeStatus = Status;
    statusKeys: any[] = [];
    schemeMasterList=[]
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        public formatter: NgbDateParserFormatter,
        private alertService: AlertService,
        private schemeService: SchemeService) { }

    ngOnInit() {
        this.getSchemeMasterList();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this.getSchemeDetailById(id);
        }

        this.statusKeys = Object.keys(this.changeStatus).filter(k => !isNaN(Number(k)));
    }

    getSchemeDetailById(id) {
        this.schemeService.getSchemeDetailById(id).subscribe(
            (res) => {
                let schemeDetailData = res.data as SchemeDetail || new SchemeDetail()
                schemeDetailData.vDate = NgbDate.from(this.formatter.parse(schemeDetailData.date));
                this.editForm(schemeDetailData);
                //this.schemeDetailModel = res.data || new SchemeDetail()

            },
            () => { },
            () => { }
        )
    }
    private editForm(schemeDetail: SchemeDetail) {
        this.schemeDetailModel = schemeDetail;
    }

    getSchemeMasterList() {
        this.alertService.fnLoading(true);
        this.schemeService.getSchemeMasterList().subscribe((res) => {
            this.schemeMasterList = res.data || [];
        }, () => { }, () => this.alertService.fnLoading(false));
    }
    fnSave() {
        this.schemeDetailModel.date = this.schemeDetailModel.vDate.year.toString() + "-" + this.schemeDetailModel.vDate.month.toString() + "-" + this.schemeDetailModel.vDate.day.toString();
        this.schemeDetailModel.id == 0 ? this.insert(this.schemeDetailModel) : this.update(this.schemeDetailModel);
    }
    insert(schemeDetailModel) {
        this.alertService.fnLoading(true);
        this.schemeService.createSchemeDetail(schemeDetailModel).subscribe((res) => {
            this.router.navigate(['/scheme/detail-list']).then(() => {
                this.alertService.tosterSuccess("Scheme has been created successfully.");
            });
        }, (error) => {
            console.log(error);
            this.displayError(error);
        }, () => {
            this.alertService.fnLoading(false);
        });
    }
    update(schemeDetailModel) {
        this.alertService.fnLoading(true);
        this.schemeService.updateSchemeDetail(schemeDetailModel).subscribe((res) => {
            this.router.navigate(['/scheme/detail-list']).then(() => {
                this.alertService.tosterSuccess("Scheme has been update successfully.");
            });
        }, (error) => {
            console.log(error);
            this.displayError(error);
        }, () => {
            this.alertService.fnLoading(false);
        });
    }
    fnRouteList() {
        this.router.navigate(['/scheme/detail-list']);
    }
    private displayError(errorDetails: any) {
        this.alertService.fnLoading(false);
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }
   
}
