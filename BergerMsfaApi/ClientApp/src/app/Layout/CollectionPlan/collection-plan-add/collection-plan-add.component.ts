import { Component, OnDestroy, OnInit } from '@angular/core';
import { Status } from '../../../Shared/Enums/status';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { forkJoin, pipe, Subscription } from 'rxjs';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { finalize } from 'rxjs/operators';
import { CollectionPlanService } from 'src/app/Shared/Services/KPI/CollectionPlanService';
import { CollectionPlan, CollectionPlanSlippageAmount, SaveCollectionPlan } from 'src/app/Shared/Entity/KPI/CollectionPlan';

@Component({
  selector: 'app-collection-plan-add',
  templateUrl: './collection-plan-add.component.html',
  styleUrls: ['./collection-plan-add.component.css']
})
export class CollectionPlanAddComponent implements OnInit, OnDestroy {

	collectionPlan: CollectionPlan;
	collectionPlanForm: FormGroup;
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload; 

    plants: any[] = [];
    // saleOffices: any[] = [];
    // areaGroups: any[] = [];
    // zones: any[] = [];
    territories:any[]=[]
	
	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private collectionPlanService: CollectionPlanService) { }

	ngOnInit() {
		this.populateDropdwonDataList();
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			// console.log(id);
			if (id) {
				this.alertService.fnLoading(true);
				this.collectionPlanService.getCollectionPlanById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.collectionPlan = res.data as CollectionPlan;
							this.initCollectionPlans();
						}
					});
			} else {
				this.collectionPlan = new CollectionPlan();
				this.collectionPlan.clear();
				this.initCollectionPlans();
			}
		});
		this.subscriptions.push(routeSubscription);
	}

    populateDropdwonDataList() {
        forkJoin([
            this.commonService.getDepotList(),
            // this.commonService.getSaleOfficeList(),
            // this.commonService.getSaleGroupList(),
            this.commonService.getTerritoryList(),
            // this.commonService.getZoneList()
        // ]).subscribe(([plants, salesOffices, areaGroups, territories, zones]) => {
        ]).subscribe(([plants, territories]) => {
            this.plants = plants.data;
            // this.saleOffices = salesOffices.data;
            // this.areaGroups = areaGroups.data;
            this.territories = territories.data;
            // this.zones = zones.data;
        }, (err) => { }, () => { });
    }

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	initCollectionPlans() {
		this.createForm();
	}

	createForm() {
		this.collectionPlanForm = this.formBuilder.group({
			hiddenchunk: [],
			businessArea: [this.collectionPlan.businessArea, [Validators.required]],
			territory: [this.collectionPlan.territory, [Validators.required]],
			collectionTargetAmount: [this.collectionPlan.collectionTargetAmount, [Validators.required]],
			slippageAmount: [this.collectionPlan.slippageAmount, [Validators.required]],
		});
	}

	get formControls() { return this.collectionPlanForm.controls; }

	onSubmit() {
		const controls = this.collectionPlanForm.controls;

		if (this.collectionPlanForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedCollectionPlans = this.prepareCollectionPlans();
		if (editedCollectionPlans.id) {
			this.updateCollectionPlans(editedCollectionPlans);
		}
		else {
			this.createCollectionPlans(editedCollectionPlans);
		}
	}

	prepareCollectionPlans(): SaveCollectionPlan {
		const controls = this.collectionPlanForm.controls;

		const _collectionPlan = new SaveCollectionPlan();
		_collectionPlan.clear();
		_collectionPlan.id = this.collectionPlan.id;
		_collectionPlan.businessArea = controls['businessArea'].value;
		_collectionPlan.territory = controls['territory'].value;
		_collectionPlan.collectionTargetAmount = controls['collectionTargetAmount'].value;
		_collectionPlan.slippageAmount = controls['slippageAmount'].value;
		
		return _collectionPlan;
	}

	createCollectionPlans(_collectionPlan: SaveCollectionPlan) {
		this.alertService.fnLoading(true);
		const createSubscription = this.collectionPlanService.createCollectionPlan(_collectionPlan)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Collection Plan has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateCollectionPlans(_collectionPlan: SaveCollectionPlan) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.collectionPlanService.updateCollectionPlan(_collectionPlan)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Collection Plan has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	onChangeArea() {
		const controls = this.collectionPlanForm.controls;
		const businessArea: string = controls['businessArea'].value;
		const territory: string = controls['territory'].value;
		let amount = 0;

		if (!businessArea || !territory) {
			amount = 0;
			controls['slippageAmount'].setValue(amount);
			return;
		}

		const model = new CollectionPlanSlippageAmount({businessArea: businessArea, territory: territory});

		this.alertService.fnLoading(true);
		const slippageAmountSubscription = this.collectionPlanService.getCollectionPlanSlippageAmount(model)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				amount = res.data as number;
				controls['slippageAmount'].setValue(amount);
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(slippageAmountSubscription);
	}

	getComponentTitle() {
		let result = 'Create Collection Plan';
		if (!this.collectionPlan || !this.collectionPlan.id) {
			return result;
		}

		result = `Edit Collection Plan - ${this.collectionPlan.yearMonthText}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/collection-plan/collection-plan-list`], { relativeTo: this.activatedRoute });
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