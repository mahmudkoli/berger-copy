import { Component, OnInit } from '@angular/core';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-schememaster-list',
    templateUrl: './schememaster-list.component.html',
    styleUrls: ['./schememaster-list.component.css']
})
export class SchememasterListComponent implements OnInit {

    constructor(
        private schemeService: SchemeService,
        private alertService: AlertService,
        private router: Router
    ) { }

    schemeMasterList: any[] = [];

    ngOnInit() {
        this.getSchemeMasterList();
    }

    getSchemeMasterList() {
        this.alertService.fnLoading(true);
        this.schemeService.getSchemeMasterList().subscribe((res) => {
            this.schemeMasterList = res.data || [];
        }, () => { }, () => this.alertService.fnLoading(false));
    }

    add() {
        this.router.navigate(["scheme/master-add",]);
    }
    edit(id) {
        this.router.navigate(["scheme/master-add",id]);

    }
    delete(id) {

        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.alertService.fnLoading(true);
            this.schemeService.DeleteSchemeMaster(id).subscribe(
                (res: any) => {
                  //  console.log('res from del func', res);
                    this.alertService.tosterSuccess("Scheme master has been deleted successfully.");
                    this.getSchemeMasterList();
                },
                (error) => {
                    console.log(error);
                }, () => () => this.alertService.fnLoading(false)
            );
        }, () => { });

    }
    detail(id) {
        this.router.navigate(["scheme/master-add", id]);
    }
}
