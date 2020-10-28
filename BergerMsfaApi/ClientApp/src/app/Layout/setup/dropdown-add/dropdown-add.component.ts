import { Component, OnInit } from '@angular/core';
import { Dropdown } from '../../../Shared/Entity/Setup/dropdown';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DynamicDropdownService } from '../../../Shared/Services/Setup/dynamic-dropdown.service';


@Component({
    selector: 'app-dropdown-add',
    templateUrl: './dropdown-add.component.html',
    styleUrls: ['./dropdown-add.component.css']
})
export class DropdownAddComponent implements OnInit {

    dropdownModel: Dropdown = new Dropdown();

    dropdownTypeList: any[] = [];

    constructor(
        private alertService: AlertService,
        private route: ActivatedRoute,
        private dynamicDropdownService: DynamicDropdownService,
        private router: Router) { }

    ngOnInit() {
    
        this.getDropdownTypeList();
        console.log("param", this.route.snapshot.params, Object.keys(this.route.snapshot.params).length);

        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let dropdownId = this.route.snapshot.params.id;
            this.getDropdownById(dropdownId);
        }
        
    }

     onChange(typeId) {

        this.dynamicDropdownService.getLastSquence(this.dropdownModel.id, typeId).subscribe(
            (result: any) => {

                this.dropdownModel.sequence =  result.data;
            },
            (err: any) => console.log(err)
        );
    }
    private getDropdownTypeList() {
        this.dynamicDropdownService.getDropdownTypeList().subscribe(
            (result: any) => {
                this.dropdownTypeList = result.data;
            },
            (err: any) => console.log(err)
        );
    }

    private getDropdownById(id) {
        this.dynamicDropdownService.GetDropdownById(id).subscribe(
            (result: any) => {
                this.editForm(result.data);
            },
            (err: any) => console.log(err)
        );
    };


    public fnRouteList() {
        this.router.navigate(['/setup/dropdown-list']);
    }

    private editForm(dropdown: Dropdown) {
        this.dropdownModel = dropdown;
    }

    public fnSave() {
        this.dropdownModel.id == 0 ? this.insert(this.dropdownModel) : this.update(this.dropdownModel);
    }

    private insert(model: Dropdown) {
        this.dynamicDropdownService.create(model).subscribe(res => {
            console.log("Dropdown res: ", res);
            this.router.navigate(['/setup/dropdown-list']).then(() => {
                this.alertService.tosterSuccess("dropdown has been created successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }

    private update(model: Dropdown) {
        this.dynamicDropdownService.update(model).subscribe(res => {
            console.log("Dropdown update res: ", res);
            this.router.navigate(['/setup/dropdown-list']).then(() => {
                this.alertService.tosterSuccess("dropdown has been edited successfully.");
            });
        },
            (error) => {
                console.log(error);
                this.displayError(error);
            }, () => this.alertService.fnLoading(false)
        );
    }
    private displayError(errorDetails: any) {
        // this.alertService.fnLoading(false);
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
