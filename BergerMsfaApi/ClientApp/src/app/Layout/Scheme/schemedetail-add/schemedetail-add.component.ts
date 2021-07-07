import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { SaveSchemeDetail, SchemeDetail } from '../../../Shared/Entity/Scheme/SchemeMaster';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';

@Component({
  selector: 'app-schemedetail-add',
  templateUrl: './schemedetail-add.component.html',
  styleUrls: ['./schemedetail-add.component.css']
})
export class SchemedetailAddComponent implements OnInit, OnDestroy {

	schemeDetail: SchemeDetail;
	schemeDetailForm: FormGroup;
	schemeMasters: MapObject[] = [];
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload;

	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private schemeDetailService: SchemeService,
		private formatter:NgbDateParserFormatter
		) { }

	ngOnInit() {
		this.loadSchemeMasters();
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			if (id) {
				this.alertService.fnLoading(true);
				this.schemeDetailService.getSchemeDetailById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.schemeDetail = res.data as SchemeDetail;
							this.initSchemeDetails();
						}
					});
			} else {
				this.schemeDetail = new SchemeDetail();
				this.schemeDetail.clear();
				this.initSchemeDetails();
			}
		});
		this.subscriptions.push(routeSubscription);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	loadSchemeMasters() {
		this.alertService.fnLoading(true);
		const categorySubscription = this.schemeDetailService.getAllSchemeMastersForSelect()
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.schemeMasters = res.data;
			},
			error => {
				this.throwError(error);
			});
		this.subscriptions.push(categorySubscription);
	}

	initSchemeDetails() {
		this.createForm();
	}

	createForm() {
		console.log('hi there')
		console.log(this.schemeDetail.benefitStartDate)
		console.log(this.schemeDetail.benefitEndDate)
		this.schemeDetailForm = this.formBuilder.group({
			schemeMasterId: [this.schemeDetail.schemeMasterId, [Validators.required]],
			code: [this.schemeDetail.code],
			brand: [this.schemeDetail.brand],
			rateInLtrOrKg: [this.schemeDetail.rateInLtrOrKg],
			rateInDrum: [this.schemeDetail.rateInDrum],
			slab: [this.schemeDetail.slab],
			condition: [this.schemeDetail.condition],
			benefitStartDate: [],
			benefitEndDate: [],
			// benefitStartDate: [this.formatter.parse(this.schemeDetail.benefitStartDate.toString()),[Validators.required]],
			// benefitEndDate: [this.formatter.parse(this.schemeDetail.benefitEndDate? this.schemeDetail.benefitEndDate.toString():null)],
			schemeId: [this.schemeDetail.schemeId],
			material: [this.schemeDetail.material],
			targetVolume: [this.schemeDetail.targetVolume],
			benefit: [this.schemeDetail.benefit],
			status: [this.schemeDetail.status, [Validators.required]]
		});

		if (this.schemeDetail.benefitStartDate) {
			const fromDate = new Date(this.schemeDetail.benefitStartDate);
			this.schemeDetailForm.controls.benefitStartDate.setValue({
				year: fromDate.getFullYear(),
				month: fromDate.getMonth()+1,
				day: fromDate.getDate()
			});
		}

		if (this.schemeDetail.benefitEndDate) {
			const toDate = new Date(this.schemeDetail.benefitEndDate);
			this.schemeDetailForm.controls.benefitEndDate.setValue({
				year: toDate.getFullYear(),
				month: toDate.getMonth()+1,
				day: toDate.getDate()
			});
		}

		this.schemeDetailForm.controls.benefitStartDate.setValidators([Validators.required]);
		this.schemeDetailForm.controls.benefitStartDate.updateValueAndValidity();
	}

	get formControls() { return this.schemeDetailForm.controls; }

	onSubmit() {
		const controls = this.schemeDetailForm.controls;

		if (this.schemeDetailForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedSchemeDetails = this.prepareSchemeDetails();
		if (editedSchemeDetails.id) {
			this.updateSchemeDetails(editedSchemeDetails);
		}
		else {
			this.createSchemeDetails(editedSchemeDetails);
		}
	}

	prepareSchemeDetails(): SaveSchemeDetail {
		const controls = this.schemeDetailForm.controls;

		const _schemeDetail = new SaveSchemeDetail();
		_schemeDetail.clear();
		_schemeDetail.id = this.schemeDetail.id;
		_schemeDetail.schemeMasterId = controls['schemeMasterId'].value;
		_schemeDetail.code = controls['code'].value;
		_schemeDetail.brand = controls['brand'].value;
		_schemeDetail.rateInLtrOrKg = controls['rateInLtrOrKg'].value;
		_schemeDetail.rateInDrum = controls['rateInDrum'].value;
		_schemeDetail.slab = controls['slab'].value;
		_schemeDetail.condition = controls['condition'].value;

		// if (controls['benefitStartDate'].value) {
		// 	const value = controls['benefitStartDate'].value;
		// 	_schemeDetail.benefitStartDate =  this.formatter.format(value)
		//   }
		// if (controls['benefitEndDate'].value) {
		// 	const value = controls['benefitEndDate'].value;
		// 	_schemeDetail.benefitEndDate =  this.formatter.format(value)
		//   }

		const fromDate = controls['benefitStartDate'].value;
		if (fromDate && fromDate.year && fromDate.month && fromDate.day) {
			_schemeDetail.benefitStartDate = new Date(fromDate.year,fromDate.month-1,fromDate.day);
		} else {
			_schemeDetail.benefitStartDate = null;
		}

		const toDate = controls['benefitEndDate'].value;
		if (toDate && toDate.year && toDate.month && toDate.day) {
			_schemeDetail.benefitEndDate = new Date(toDate.year,toDate.month-1,toDate.day);
		} else {
			_schemeDetail.benefitEndDate = null;
		}

		_schemeDetail.schemeId = controls['schemeId'].value;
		_schemeDetail.material = controls['material'].value;
		_schemeDetail.targetVolume = controls['targetVolume'].value;
		_schemeDetail.benefit = controls['benefit'].value;
		_schemeDetail.status = controls['status'].value;

		return _schemeDetail;
	}

	createSchemeDetails(_schemeDetail: SaveSchemeDetail) {
		this.alertService.fnLoading(true);
		const createSubscription = this.schemeDetailService.createSchemeDetail(_schemeDetail)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Scheme Detail has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateSchemeDetails(_schemeDetail: SaveSchemeDetail) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.schemeDetailService.updateSchemeDetail(_schemeDetail)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Scheme Detail has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	getComponentTitle() {
		let result = 'Create Scheme Detail';
		if (!this.schemeDetail || !this.schemeDetail.id) {
			return result;
		}

		result = `Edit Scheme Detail - ${this.schemeDetail.schemeMasterName}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/scheme/detail-list`], { relativeTo: this.activatedRoute });
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