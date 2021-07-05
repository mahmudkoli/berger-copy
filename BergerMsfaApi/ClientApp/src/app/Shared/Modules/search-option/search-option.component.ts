import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin, of, Subscription } from 'rxjs';
import { APIResponse } from 'src/app/Shared/Entity';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { EnumEmployeeRole } from 'src/app/Shared/Enums/employee-role';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from './search-option';
import { Enums } from '../../Enums/enums';
import { AlertService } from '../alert/alert.service';

@Component({
    selector: 'app-search-option',
    templateUrl: './search-option.component.html',
    styleUrls: ['./search-option.component.css']
})
export class SearchOptionComponent implements OnInit, OnDestroy {

	@Input() searchOptionQuery: SearchOptionQuery;
	@Input() searchOptionSettings: SearchOptionSettings;
	@Output() searchOptionQueryCallbackFn: EventEmitter<SearchOptionQuery> =
											new EventEmitter<SearchOptionQuery>() || null;

	searchOptionForm: FormGroup;
	employeeRole: EnumEmployeeRole;

    depots: any[] = [];
    salesGroups: any[] = [];
    territories: any[]=[]
    zones: any[] = [];
    users: any[] = [];
	dealers: any[] = [];
	creditControlAreas: any[] = [];
    paintingStages: any[] = [];
    projectStatuses: any[] = [];
    painters: any[] = [];
    painterTypes: any[] = [];
    paymentMethods: any[] = [];
    brands: any[] = [];
    materialCodes: any[] = [];
	months: any[] = [];
	years: any[] = [];
	activitySummaries: any[] = [];

    valueVolumeResultTypes: any[] = Enums.valueVolumeResultType;
    customerClassificationTypes: any[] = Enums.customerClassificationType;

	private _allDealers: any[] = []

	private subscriptions: Subscription[] = [];

	constructor(
		private alertService: AlertService,
		private formBuilder: FormBuilder,
		private commonService: CommonService,
		private dynamicDropdownService: DynamicDropdownService) {
	}

	ngOnInit() {
		this.employeeRole = this._loggedUser.employeeRole;
		this.populateDropdownDataList();
		this.createForm();
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	createForm() {
		this.searchOptionForm = this.formBuilder.group({
			depot: [this.searchOptionQuery.depot],
			salesGroups: [this.searchOptionQuery.salesGroups],
			territories: [this.searchOptionQuery.territories],
			zones: [this.searchOptionQuery.zones],
			fromDate: [],
			toDate: [],
			userId: [this.searchOptionQuery.userId],
			dealerId: [this.searchOptionQuery.dealerId],
			creditControlArea: [this.searchOptionQuery.creditControlArea],
			paintingStageId: [this.searchOptionQuery.paintingStageId],
			projectStatusId: [this.searchOptionQuery.projectStatusId],
			painterId: [this.searchOptionQuery.painterId],
			painterTypeId: [this.searchOptionQuery.painterTypeId],
			paymentMethodId: [this.searchOptionQuery.paymentMethodId],
			brands:[this.searchOptionQuery.brands],
			materialCodes:[this.searchOptionQuery.materialCodes],
			fromMonth: [this.searchOptionQuery.fromMonth],
			toMonth: [this.searchOptionQuery.toMonth],
			fromYear: [this.searchOptionQuery.fromYear],
			toYear: [this.searchOptionQuery.toYear],
			month: [this.searchOptionQuery.month],
			year: [this.searchOptionQuery.year],
			text1: [this.searchOptionQuery.text1],
			text2: [this.searchOptionQuery.text2],
			text3: [this.searchOptionQuery.text3],
			activitySummary: [this.searchOptionQuery.activitySummary],
			valueVolumeResultType: [this.searchOptionQuery.valueVolumeResultType],
			customerClassificationType: [this.searchOptionQuery.customerClassificationType],
		});

		if (this.searchOptionQuery.fromDate) {
			const fromDate = new Date(this.searchOptionQuery.fromDate);
			this.searchOptionForm.controls.fromDate.setValue({
				year: fromDate.getFullYear(),
				month: fromDate.getMonth()+1,
				day: fromDate.getDate()
			});
		}

		if (this.searchOptionQuery.toDate) {
			const toDate = new Date(this.searchOptionQuery.toDate);
			this.searchOptionForm.controls.toDate.setValue({
				year: toDate.getFullYear(),
				month: toDate.getMonth()+1,
				day: toDate.getDate()
			});
		}

		this.addValidation();
	}

	addValidation() {
		const requiredOptionsBasedOnEmployeeRole = this.searchOptionSettings.searchOptionDef
													.filter(x => x.isRequiredBasedOnEmployeeRole)
													.map(x => x.searchOption);
		let conditionalRequiredOptions = [];
		switch(this.employeeRole) {
			case EnumEmployeeRole.Admin:
			case EnumEmployeeRole.GM:
			case EnumEmployeeRole.DSM:
			case EnumEmployeeRole.RSM:
			case EnumEmployeeRole.BM_BSI:
				conditionalRequiredOptions = [EnumSearchOption.Depot];
				break;
			case EnumEmployeeRole.AM:
				conditionalRequiredOptions = [EnumSearchOption.Depot, EnumSearchOption.SalesGroup, EnumSearchOption.Territory];
				break;
			case EnumEmployeeRole.TM_TO:
				conditionalRequiredOptions = [EnumSearchOption.Depot, EnumSearchOption.Territory];
				break;
		}
		const requiredOptions = this.searchOptionSettings.searchOptionDef.filter(x => x.isRequired)
																.map(x => x.searchOption);

		let finalRequiredOptions = [];
		finalRequiredOptions = requiredOptionsBasedOnEmployeeRole.filter(x => conditionalRequiredOptions.includes(x));
		finalRequiredOptions = [...new Set([...finalRequiredOptions, ...requiredOptions])];
		// let intersection = arrA.filter(x => arrB.includes(x));
		// let union = [...new Set([...arrA, ...arrB)];

		finalRequiredOptions.forEach(option => {
			this.searchOptionForm.controls[option].setValidators([Validators.required]);
			this.searchOptionForm.controls[option].updateValueAndValidity();
		});
	}

	get formControls() { return this.searchOptionForm.controls; }

	private get _loggedUser() { return this.commonService.getUserInfoFromLocalStorage(); }

    populateDropdownDataList() {
        const forkJoinSubscription1 = forkJoin([
            this.hasSearchOption(EnumSearchOption.Depot)?this.commonService.getDepotList():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.SalesGroup)?this.commonService.getSaleGroupList():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.Territory)?this.commonService.getTerritoryList():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.Zone)?this.commonService.getZoneList():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.CreditControlArea)?this.commonService.getCreditControlAreaList():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.UserId)?this.commonService.getUserInfoListByCurrentUser():of(APIResponse),
        ]).subscribe(([plants, areaGroups, territories, zones, creditControlAreas, users]) => {
            this.depots = plants.data;
            this.salesGroups = areaGroups.data;
            this.territories = territories.data;
            this.zones = zones.data;
            this.creditControlAreas = creditControlAreas.data;
            this.users = users.data;
        }, (err) => { }, () => { });

        const forkJoinSubscription2 = forkJoin([
            this.hasSearchOption(EnumSearchOption.DealerId)?this.commonService.getDealerList(this._loggedUser.userCategory, this._loggedUser.userCategoryIds):of(APIResponse),
            this.hasSearchOption(EnumSearchOption.PaintingStageId)?this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.PaintingStage):of(APIResponse),
            this.hasSearchOption(EnumSearchOption.ProjectStatusId)?this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.ProjectStatus):of(APIResponse),
            this.hasSearchOption(EnumSearchOption.PainterId)?this.commonService.getPainterList():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.PainterTypeId)?this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Painter):of(APIResponse),
            this.hasSearchOption(EnumSearchOption.PaymentMethodId)?this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Payment):of(APIResponse)

        ]).subscribe(([dealers, paintingStages, projectStatuses, painters, painterTypes, paymentMethods]) => {
            this.dealers = dealers.data;
            this.paintingStages = paintingStages.data;
            this.projectStatuses = projectStatuses.data;
            this.painters = painters.data;
            this.painterTypes = painterTypes.data;
            this.paymentMethods = paymentMethods.data;
			this._allDealers = dealers.data;
			this.updateDealerSubDealerShow();
        }, (err) => { }, () => { });

        const forkJoinSubscription3 = forkJoin([
            this.hasSearchOption(EnumSearchOption.Brand)?this.commonService.getBrandDropDown():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.MaterialCode)?this.commonService.getMaterialGroupOrBrand():of(APIResponse),
            this.hasSearchOption(EnumSearchOption.ActivitySummary)?this.commonService.getActivitySummaryDropDown():of(APIResponse),
        ]).subscribe(([brands,materialCodes,activitySummary]) => {
            this.brands = brands.data;
            this.materialCodes = materialCodes.data;
            this.activitySummaries = activitySummary.data;
			this.updateDealerSubDealerShow();
        }, (err) => { }, () => { });

		this.subscriptions.push(forkJoinSubscription1);
		this.subscriptions.push(forkJoinSubscription2);
		this.subscriptions.push(forkJoinSubscription3);

		this.months = this.getMonths();
		this.years = this.getYears();
    }

	onSubmitSearch() {
		const controls = this.searchOptionForm.controls;
		this.searchOptionQuery.page = 1;
		this.searchOptionQuery.depot = controls['depot'].value;
		this.searchOptionQuery.salesGroups = controls['salesGroups'].value;
		this.searchOptionQuery.territories = controls['territories'].value;
		this.searchOptionQuery.zones = controls['zones'].value;
		this.searchOptionQuery.userId = controls['userId'].value;
		this.searchOptionQuery.dealerId = controls['dealerId'].value;
		this.searchOptionQuery.creditControlArea = controls['creditControlArea'].value;
		this.searchOptionQuery.paintingStageId = controls['paintingStageId'].value;
		this.searchOptionQuery.projectStatusId = controls['projectStatusId'].value;
		this.searchOptionQuery.painterId = controls['painterId'].value;
		this.searchOptionQuery.painterTypeId = controls['painterTypeId'].value;
		this.searchOptionQuery.paymentMethodId = controls['paymentMethodId'].value;
		this.searchOptionQuery.fromMonth = controls['fromMonth'].value;
		this.searchOptionQuery.toMonth = controls['toMonth'].value;
		this.searchOptionQuery.fromYear = controls['fromYear'].value;
		this.searchOptionQuery.toYear = controls['toYear'].value;
		this.searchOptionQuery.month = controls['month'].value;
		this.searchOptionQuery.year = controls['year'].value;
		this.searchOptionQuery.text1 = controls['text1'].value;
		this.searchOptionQuery.text2 = controls['text2'].value;
		this.searchOptionQuery.text3 = controls['text3'].value;
		this.searchOptionQuery.brands = controls['brands'].value;
		this.searchOptionQuery.materialCodes = controls['materialCodes'].value;
		this.searchOptionQuery.activitySummary = controls['activitySummary'].value;
		this.searchOptionQuery.valueVolumeResultType = controls['valueVolumeResultType'].value;
		this.searchOptionQuery.customerClassificationType = controls['customerClassificationType'].value;
		
		if (this._allDealers && this.searchOptionQuery.dealerId)
			this.searchOptionQuery.customerNo = this._allDealers.find(x => x.id == this.searchOptionQuery.dealerId).customerNo;

		const fromDate = controls['fromDate'].value;
		if (fromDate && fromDate.year && fromDate.month && fromDate.day) {
			this.searchOptionQuery.fromDate = new Date(fromDate.year,fromDate.month-1,fromDate.day);
		} else {
			this.searchOptionQuery.fromDate = null;
		}

		const toDate = controls['toDate'].value;
		if (toDate && toDate.year && toDate.month && toDate.day) {
			this.searchOptionQuery.toDate = new Date(toDate.year,toDate.month-1,toDate.day);
		} else {
			this.searchOptionQuery.toDate = null;
		}

		if (!this.checkSearchOptionValidity()) return;

		this.searchOptionQueryCallbackFn.emit(this.searchOptionQuery);
	}

	updateDealerSubDealerShow() {
		if (this.searchOptionForm)
			this.searchOptionForm.controls.dealerId.setValue(null);

		if (this.searchOptionSettings.isDealerShow) {
			this.dealers = this._allDealers.filter(x => !x.isSubdealer);
		} else if (this.searchOptionSettings.isSubDealerShow) {
			this.dealers = this._allDealers.filter(x => x.isSubdealer);
		} else {
			this.searchOptionSettings.isDealerShow = true;
			this.searchOptionSettings.isSubDealerShow = false;
		}
	}

	changeDealerSubDealerShow() {
		this.searchOptionSettings.isDealerShow = !this.searchOptionSettings.isDealerShow;
		this.searchOptionSettings.isSubDealerShow = !this.searchOptionSettings.isSubDealerShow;
		this.updateDealerSubDealerShow();
	}

	checkSearchOptionValidity() : boolean | true {
		let validity = true;
		if (this.searchOptionSettings.hasMonthDifference &&
			!this.checkDifferenceMonth(this.searchOptionQuery.fromYear, this.searchOptionQuery.fromMonth,
				this.searchOptionQuery.toYear, this.searchOptionQuery.toMonth, this.searchOptionSettings.monthDifferenceCount)) {
			this.alertService.alert(`Month difference must be ${this.searchOptionSettings.monthDifferenceCount} months.`);
			validity = false;
		}
		return validity;
	}

	hasSearchOption(searchOption) {
		return this.searchOptionSettings.searchOptionDef.some(x => x.searchOption == searchOption);
	}

	getSearchOption(searchOption): SearchOptionDef | null {
		return this.searchOptionSettings.searchOptionDef.find(x => x.searchOption == searchOption) || null;
	}

	getSearchOptionLabel(searchOption): string | '' {
		return this.searchOptionSettings.searchOptionDef.find(x => x.searchOption == searchOption).textLabel || '';
	}

	isSearchOptionRequired(searchOption): boolean | false {
		const searchOptionDef = this.searchOptionSettings.searchOptionDef.find(x => x.searchOption == searchOption);
		let isRequired = false;
		switch(this.employeeRole) {
			case EnumEmployeeRole.Admin:
			case EnumEmployeeRole.GM:
			case EnumEmployeeRole.DSM:
			case EnumEmployeeRole.RSM:
			case EnumEmployeeRole.BM_BSI:
				isRequired = searchOptionDef.searchOption==EnumSearchOption.Depot;
				break;
			case EnumEmployeeRole.AM:
				isRequired = searchOptionDef.searchOption==EnumSearchOption.Depot ||
							searchOptionDef.searchOption==EnumSearchOption.SalesGroup ||
							searchOptionDef.searchOption==EnumSearchOption.Territory;
				break;
			case EnumEmployeeRole.TM_TO:
				isRequired = searchOptionDef.searchOption==EnumSearchOption.Depot ||
							searchOptionDef.searchOption==EnumSearchOption.Territory;
				break;
		}
		isRequired = isRequired && searchOptionDef.isRequiredBasedOnEmployeeRole;
		isRequired = isRequired || searchOptionDef.isRequired;
		return isRequired || false;
	}

	ngbDateToDate(date: NgbDate) : Date | null {
		return date && date.year && date.month && date.day ?
				new Date(date.year,date.month-1,date.day) :
				null;
	}

	getMonths(): any[] {
		const months = [{'id':1,'name':'January'}, {'id':2,'name':'February'}, {'id':3,'name':'March'}, {'id':4,'name':'April'},
				{'id':5,'name':'May'}, {'id':6,'name':'June'}, {'id':7,'name':'July'}, {'id':8,'name':'August'},
				{'id':9,'name':'September'}, {'id':10,'name':'October'}, {'id':11,'name':'November'}, {'id':12,'name':'December'}];
		return months;
	}

	getYears(): any[] {
		let years: any[] = [];
		let currentYear: number = new Date().getFullYear();
		for(let i = (currentYear - 10); i < (currentYear + 3); i++) {
			years.push({'id':i,'name':i.toString()});
		}
		return years;
	}

	checkDifferenceMonth(fromYear:number,fromMonth:number,toYear:number,toMonth:number,monthDiffCount:number) : boolean | false {
		var fromDate = new Date(fromYear, fromMonth-1);
		var toDate = new Date(toYear, toMonth-1);
		// let monthCount = 0;
		// 	monthCount = (toDate.getFullYear() - fromDate.getFullYear()) * 12;
		// 	monthCount -= fromDate.getMonth()+1;
		// 	monthCount += toDate.getMonth()+1;
		// 	monthCount += 1;
		// return (monthCount <= 0 ? 0 : monthCount)===monthDiffCount;
		return ((toDate.getMonth()+1) - (fromDate.getMonth()+1) + (12 * (toDate.getFullYear() - fromDate.getFullYear())) + 1)===monthDiffCount;
	}
}
