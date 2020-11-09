import { Component, OnInit } from '@angular/core';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SchemeMaster } from '../../../Shared/Entity/Scheme/SchemeMaster';

@Component({
    selector: 'app-schememaster-add',
    templateUrl: './schememaster-add.component.html',
    styleUrls: ['./schememaster-add.component.css']
})
export class SchememasterAddComponent implements OnInit {
    schemeMasterModel: SchemeMaster = new SchemeMaster();
    constructor(
        private schemeService: SchemeService,
        private alertService: AlertService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit() {
   

        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this.getSchemeMasterById(id);
        }

    }

    getSchemeMasterById(id) {
        this.schemeService.getSchemeMasterById(id).subscribe(
            (res) => { this.schemeMasterModel = res.data || new SchemeMaster() },
            () => { },
            () => { }
        )
    }
    fnSave() {
        this.schemeMasterModel.id == 0 ? this.insert(this.schemeMasterModel) : this.update(this.schemeMasterModel);
    }
    insert(schemeMasterModel) {
        this.alertService.fnLoading(true);
        this.schemeService.createSchemeMaster(schemeMasterModel).subscribe((res) => {
            this.router.navigate(['/scheme/master-list']).then(() => {
                this.alertService.tosterSuccess("Scheme has been created successfully.");
            });
        }, (error) => {
            console.log(error);
            this.displayError(error);
        }, () => {
            this.alertService.fnLoading(false);
        });
    }
    update(schemeMasterModel) {
        this.alertService.fnLoading(true);
        this.schemeService.UpdateSchemeMaster(schemeMasterModel).subscribe((res) => {
            this.router.navigate(['/scheme/master-list']).then(() => {
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
        this.router.navigate(['/scheme/master-list']);
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
