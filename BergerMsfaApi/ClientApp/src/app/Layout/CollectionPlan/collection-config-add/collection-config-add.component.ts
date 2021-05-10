import { Component, OnDestroy, OnInit } from '@angular/core';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SaveSchemeMaster, SchemeMaster } from '../../../Shared/Entity/Scheme/SchemeMaster';
import { Subscription } from 'rxjs';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { finalize } from 'rxjs/operators';
import { CollectionConfig, SaveCollectionConfig } from 'src/app/Shared/Entity/KPI/CollectionPlan';
import { CollectionPlanService } from 'src/app/Shared/Services/KPI/CollectionPlanService';
import { formatDate } from '@angular/common';

@Component({
    selector: 'app-collection-config-add',
    templateUrl: './collection-config-add.component.html',
    styleUrls: ['./collection-config-add.component.css']
})
export class CollectionConfigAddComponent implements OnInit, OnDestroy {

	collectionConfig: CollectionConfig;
	collectionConfigForm: FormGroup;
	// startDate = formatDate(new Date(), 'yyyy-MM-dd', 'en');
	currentDate = new Date();
	startDate = {year: this.currentDate.getFullYear(),month:this.currentDate.getMonth()+1};
	
	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private collectionConfigService: CollectionPlanService) { }

	ngOnInit() {
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			console.log(id);
			if (id) {
				this.alertService.fnLoading(true);
				this.collectionConfigService.getCollectionConfigById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.collectionConfig = res.data as CollectionConfig;
							this.initCollectionConfigs();
						}
					});
			} else {
				this.collectionConfig = new CollectionConfig();
				this.collectionConfig.clear();
				this.initCollectionConfigs();
			}
		});
		this.subscriptions.push(routeSubscription);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	initCollectionConfigs() {
		this.createForm();
	}

	createForm() {
		this.collectionConfigForm = this.formBuilder.group({
			hiddenchunk: [],
			changeableMaxDate: []
		});

		if(this.collectionConfig.changeableMaxDate) {
			const dateStr = new Date(this.collectionConfig.changeableMaxDate);
			this.collectionConfigForm.controls.changeableMaxDate.setValue({
				year: dateStr.getFullYear(),
				month: dateStr.getMonth()+1,
				day: dateStr.getDate()
			});
		}
	}

	get formControls() { return this.collectionConfigForm.controls; }

	onSubmit() {
		const controls = this.collectionConfigForm.controls;

		if (this.collectionConfigForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedCollectionConfigs = this.prepareCollectionConfigs();
		if (editedCollectionConfigs.id) {
			this.updateCollectionConfigs(editedCollectionConfigs);
		}
		else {
			this.createCollectionConfigs(editedCollectionConfigs);
		}
	}

	prepareCollectionConfigs(): SaveCollectionConfig {
		const controls = this.collectionConfigForm.controls;
		const _collectionConfig = new SaveCollectionConfig();
		_collectionConfig.clear();
		_collectionConfig.id = this.collectionConfig.id;
		
		const date = controls['changeableMaxDate'].value;
		if(date && date.year && date.month && date.day) {
			_collectionConfig.changeableMaxDateDay = date.day;
		}
		
		return _collectionConfig;
	}

	createCollectionConfigs(_collectionConfig: SaveCollectionConfig) {
		this.alertService.fnLoading(true);
		const createSubscription = this.collectionConfigService.updateCollectionConfig(_collectionConfig)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Collection Config has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateCollectionConfigs(_collectionConfig: SaveCollectionConfig) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.collectionConfigService.updateCollectionConfig(_collectionConfig)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Collection Config has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	getComponentTitle() {
		let result = 'Create Collection Config';
		if (!this.collectionConfig || !this.collectionConfig.id) {
			return result;
		}

		result = `Edit Collection Config - ${this.collectionConfig.changeableMaxDateText}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/collection-plan/collection-config-list`], { relativeTo: this.activatedRoute });
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
