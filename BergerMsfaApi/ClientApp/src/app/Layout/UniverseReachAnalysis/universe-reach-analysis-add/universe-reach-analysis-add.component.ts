import { Component, OnDestroy, OnInit } from '@angular/core';
import { Status } from '../../../Shared/Enums/status';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin, pipe, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { UniverseReachAnalysis, SaveUniverseReachAnalysis } from 'src/app/Shared/Entity/KPI/UniverseReachAnalysis';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UniverseReachAnalysisService } from 'src/app/Shared/Services/KPI/UniverseReachAnalysisService';

@Component({
  selector: 'app-universe-reach-analysis-add',
  templateUrl: './universe-reach-analysis-add.component.html',
  styleUrls: ['./universe-reach-analysis-add.component.css']
})
export class UniverseReachAnalysisAddComponent implements OnInit, OnDestroy {

	universeReachAnalysis: UniverseReachAnalysis;
	universeReachAnalysisForm: FormGroup;
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
		private universeReachAnalysisService: UniverseReachAnalysisService) { }

	ngOnInit() {
		this.populateDropdwonDataList();
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			console.log(id);
			if (id) {
				this.alertService.fnLoading(true);
				this.universeReachAnalysisService.getUniverseReachAnalysisById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.universeReachAnalysis = res.data as UniverseReachAnalysis;
							this.initUniverseReachAnalysiss();
						}
					});
			} else {
				this.universeReachAnalysis = new UniverseReachAnalysis();
				this.universeReachAnalysis.clear();
				this.universeReachAnalysis.fiscalYear = this.getCurrentFiscalYear();
				this.initUniverseReachAnalysiss();
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

	initUniverseReachAnalysiss() {
		this.createForm();
	}

	createForm() {
		this.universeReachAnalysisForm = this.formBuilder.group({
			businessArea: [this.universeReachAnalysis.businessArea, [Validators.required]],
			territory: [this.universeReachAnalysis.territory, [Validators.required]],
			outletNumber: [this.universeReachAnalysis.outletNumber],
			directCovered: [this.universeReachAnalysis.directCovered],
			indirectCovered: [this.universeReachAnalysis.indirectCovered],
			directTarget: [this.universeReachAnalysis.directTarget],
			indirectTarget: [this.universeReachAnalysis.indirectTarget],
			indirectManual: [this.universeReachAnalysis.indirectManual],
		});
	}

	get formControls() { return this.universeReachAnalysisForm.controls; }

	onSubmit() {
		const controls = this.universeReachAnalysisForm.controls;

		if (this.universeReachAnalysisForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedUniverseReachAnalysiss = this.prepareUniverseReachAnalysiss();
		if (editedUniverseReachAnalysiss.id) {
			this.updateUniverseReachAnalysiss(editedUniverseReachAnalysiss);
		}
		else {
			this.createUniverseReachAnalysiss(editedUniverseReachAnalysiss);
		}
	}

	prepareUniverseReachAnalysiss(): SaveUniverseReachAnalysis {
		const controls = this.universeReachAnalysisForm.controls;

		const _universeReachAnalysis = new SaveUniverseReachAnalysis();
		_universeReachAnalysis.clear();
		_universeReachAnalysis.id = this.universeReachAnalysis.id;
		_universeReachAnalysis.businessArea = controls['businessArea'].value;
		_universeReachAnalysis.territory = controls['territory'].value;
		_universeReachAnalysis.outletNumber = +controls['outletNumber'].value;
		_universeReachAnalysis.directCovered = +controls['directCovered'].value;
		_universeReachAnalysis.indirectCovered = +controls['indirectCovered'].value;
		_universeReachAnalysis.directTarget = +controls['directTarget'].value;
		_universeReachAnalysis.indirectTarget = +controls['indirectTarget'].value;
		if (this.universeReachAnalysis.id)
			_universeReachAnalysis.indirectManual = controls['indirectManual'].value;
		
		return _universeReachAnalysis;
	}

	createUniverseReachAnalysiss(_universeReachAnalysis: SaveUniverseReachAnalysis) {
		this.alertService.fnLoading(true);
		const createSubscription = this.universeReachAnalysisService.createUniverseReachAnalysis(_universeReachAnalysis)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Universe Reach Plan has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateUniverseReachAnalysiss(_universeReachAnalysis: SaveUniverseReachAnalysis) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.universeReachAnalysisService.updateUniverseReachAnalysis(_universeReachAnalysis)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Universe Reach Plan has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	onChangeDepot() {
	  this.callTerritories();
	  const controls = this.universeReachAnalysisForm.controls;
	  controls['territory'].setValue(null);
	}
  
	callTerritories () {
		const controls = this.universeReachAnalysisForm.controls;
		const depot = controls['businessArea'].value;
		
		  this.commonService.getTerritoryListByDepot({'depots':[depot]}).subscribe(res => {
			this.territories = res.data;
		  });
	}

	getComponentTitle() {
		let result = `Create Universe Reach Plan - (${this.universeReachAnalysis.fiscalYear})`;
		if (!this.universeReachAnalysis || !this.universeReachAnalysis.id) {
			return result;
		}

		result = `Edit Universe Reach Plan - (${this.universeReachAnalysis.fiscalYear})`;
		return result;
	}

	goBack() {
		this.router.navigate([`/universe-reach-analysis/universe-reach-analysis-list`], { relativeTo: this.activatedRoute });
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

	getCurrentFiscalYear(): string {
		// return `${this.getCFYFD().getFullYear()}-${this.getCFYLD().getFullYear().toString().substring(2)}`;
		return `${this.getCFYFD().getFullYear()}-${(this.getCFYFD().getFullYear()%100)+1}`;
	}

	getCFYFD(): Date {
		let startMonth = 4;
		let currentDate = new Date();
		return new Date((currentDate.getMonth()+1) >= startMonth ? currentDate.getFullYear() : (currentDate.getFullYear()-1), startMonth, 1);
	}

	getCFYLD(): Date {
		let startMonth = 4;
		let currentDate = new Date();
		return this.addDays(new Date((currentDate.getMonth()+1) >= startMonth ? (currentDate.getFullYear()+1) : currentDate.getFullYear(), startMonth, 1), -1);
	}

	addDays(date, days): Date {
		var result = new Date(date);
		result.setDate(result.getDate() + days);
		return result;
	}
}