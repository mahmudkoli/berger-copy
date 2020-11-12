import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';

@Component({
    selector: 'app-schemedetail-list',
    templateUrl: './schemedetail-list.component.html',
    styleUrls: ['./schemedetail-list.component.css']
})
export class SchemedetailListComponent implements OnInit {
    schemeDetailMasterList = [];
    constructor(
        private router: Router,
        private alertService: AlertService,
        private schemeService: SchemeService) { }

    ngOnInit() {
        this.getSchemeDetailMasterList();
    }
    getSchemeDetailMasterList() {
        this.alertService.fnLoading(true);
        this.schemeService.getSchemeDetailWithMaster().subscribe(
            (res) => {
                this.schemeDetailMasterList = res.data || [];
                this.alertService.fnLoading(false);
            },
            () => { },
            () => { this.alertService.fnLoading(false);}
        )
    }
    add() {
        this.router.navigate(["/scheme/detail-add"]);
    }
    edit(id) {
        this.router.navigate(["/scheme/detail-add", id]);

    }
    delete(id) {

        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.alertService.fnLoading(true);
            this.schemeService.deleteSchemeDetail(id).subscribe(
                (res: any) => {
                    //  console.log('res from del func', res);
                    this.alertService.tosterSuccess("Scheme master has been deleted successfully.");
                    this.getSchemeDetailMasterList();
                },
                (error) => {
                    console.log(error);
                }, () => () => this.alertService.fnLoading(false)
            );
        }, () => { });

    }
}
