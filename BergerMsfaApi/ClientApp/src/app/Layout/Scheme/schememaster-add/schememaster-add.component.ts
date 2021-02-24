import { Component, OnDestroy, OnInit } from '@angular/core';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SaveSchemeMaster, SchemeMaster } from '../../../Shared/Entity/Scheme/SchemeMaster';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-schememaster-add',
    templateUrl: './schememaster-add.component.html',
    styleUrls: ['./schememaster-add.component.css']
})
export class SchememasterAddComponent implements OnInit, OnDestroy {

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
			console.log(id);
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
		// _schemeMaster.status = controls['status'].value;
			
		return _schemeMaster;
	}

	createSchemeMasters(_schemeMaster: SaveSchemeMaster) {
		this.alertService.fnLoading(true);
		const createSubscription = this.schemeMasterService.createSchemeMaster(_schemeMaster)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Scheme Master has been added successfully.`);
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
				this.alertService.tosterSuccess(`Scheme Master has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	getComponentTitle() {
		let result = 'Create Scheme Master';
		if (!this.schemeMaster || !this.schemeMaster.id) {
			return result;
		}

		result = `Edit Scheme Master - ${this.schemeMaster.schemeName}`;
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
		console.log("error", errorDetails);
		let errList = errorDetails.error.errors;
		if (errList.length) {
			console.log("error", errList, errList[0].errorList[0]);
			// this.alertService.tosterDanger(errList[0].errorList[0]);
		} else {
			// this.alertService.tosterDanger(errorDetails.error.message);
		}
	}
}
