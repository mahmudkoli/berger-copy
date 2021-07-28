import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { SaveSchemeMaster, SchemeMaster } from '../../../Shared/Entity/Scheme/SchemeMaster';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';

@Component({
    selector: 'app-schememaster-add',
    templateUrl: './schememaster-add.component.html',
    styleUrls: ['./schememaster-add.component.css']
})
export class SchememasterAddComponent implements OnInit, OnDestroy {
	plants:[]
	schemeMaster: SchemeMaster;
	schemeMasterForm: FormGroup;
	// actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload;

	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private schemeMasterService: SchemeService) { }

	ngOnInit() {
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			if (id) {
				this.alertService.fnLoading(true);
				this.schemeMasterService.getSchemeMasterById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.schemeMaster = res.data as SchemeMaster;
							this.initSchemeMasters();
						}
					});
			} else {
				this.schemeMaster = new SchemeMaster();
				this.schemeMaster.clear();
				this.initSchemeMasters();
			}
		});
		this.subscriptions.push(routeSubscription);

		this.commonService.getDepotList().subscribe((p) => {
            this.plants = p.data;


        }), (err: any) => console.log(err);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	initSchemeMasters() {
		this.createForm();
	}

	createForm() {
		this.schemeMasterForm = this.formBuilder.group({
			schemeName: [this.schemeMaster.schemeName, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
			condition: [this.schemeMaster.condition],
			businessArea: [this.schemeMaster.businessArea, [!this.checkrole()? Validators.required: Validators.nullValidator]],
			// status: [this.schemeMaster.status, [Validators.required]]
		});
	}

	get formControls() { return this.schemeMasterForm.controls; }

	onSubmit() {
		const controls = this.schemeMasterForm.controls;

		if (this.schemeMasterForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedSchemeMasters = this.prepareSchemeMasters();
		if (editedSchemeMasters.id) {
			this.updateSchemeMasters(editedSchemeMasters);
		}
		else {
			this.createSchemeMasters(editedSchemeMasters);
		}
	}

	prepareSchemeMasters(): SaveSchemeMaster {
		const controls = this.schemeMasterForm.controls;

		const _schemeMaster = new SaveSchemeMaster();
		_schemeMaster.clear();
		_schemeMaster.id = this.schemeMaster.id;
		_schemeMaster.schemeName = controls['schemeName'].value;
		_schemeMaster.condition = controls['condition'].value;
		_schemeMaster.businessArea = controls['businessArea'].value;
		// _schemeMaster.status = controls['status'].value;

		return _schemeMaster;
	}

	createSchemeMasters(_schemeMaster: SaveSchemeMaster) {
		this.alertService.fnLoading(true);
		const createSubscription = this.schemeMasterService.createSchemeMaster(_schemeMaster)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Scheme Category has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateSchemeMasters(_schemeMaster: SaveSchemeMaster) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.schemeMasterService.updateSchemeMaster(_schemeMaster)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Scheme Category has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	getComponentTitle() {
		let result = 'Create Scheme Category';
		if (!this.schemeMaster || !this.schemeMaster.id) {
			return result;
		}

		result = `Edit Scheme Category - ${this.schemeMaster.schemeName}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/scheme/master-list`], { relativeTo: this.activatedRoute });
	}

	stringToInt(value): number {
		return Number.parseInt(value);
	}

	private throwError(errorDetails: any) {
		// this.alertService.fnLoading(false);
		let errList = errorDetails.error.errors;
		if (errList.length) {
			console.log("error", errList, errList[0].errorList[0]);
			// this.alertService.tosterDanger(errList[0].errorList[0]);
		} else {
			// this.alertService.tosterDanger(errorDetails.error.message);
		}
	}

	checkrole(){
		console.log(this.commonService.getUserInfoFromLocalStorage())
		let user=this.commonService.getUserInfoFromLocalStorage()
		if(user.roleId==6){
			return true;
		}
		return false;
	}
}
