import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { AlertService } from '../alert/alert.service';
import { forkJoin, of, Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { NgbDate, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { LeadSummaryQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { ReportService } from 'src/app/Shared/Services/Report/ReportService';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { EnumEmployeeRole, EnumEmployeeRoleLabel } from 'src/app/Shared/Enums/employee-role';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from '.';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { APIResponse } from 'src/app/Shared/Entity';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';

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
	months: any[] = [];
	years: any[] = [];

	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private reportService: ReportService,
		private modalService: NgbModal,
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
			fromDate: [this.searchOptionQuery.fromDate],
			toDate: [this.searchOptionQuery.toDate],
			userId: [this.searchOptionQuery.userId],
			dealerId: [this.searchOptionQuery.dealerId],
			creditControlArea: [this.searchOptionQuery.creditControlArea],
			paintingStageId: [this.searchOptionQuery.paintingStageId],
			projectStatusId: [this.searchOptionQuery.projectStatusId],
			painterId: [this.searchOptionQuery.painterId],
			painterTypeId: [this.searchOptionQuery.painterTypeId],
			paymentMethodId: [this.searchOptionQuery.paymentMethodId],
			fromMonth: [this.searchOptionQuery.fromMonth],
			toMonth: [this.searchOptionQuery.toMonth],
			fromYear: [this.searchOptionQuery.fromYear],
			toYear: [this.searchOptionQuery.toYear],
			month: [this.searchOptionQuery.month],
			year: [this.searchOptionQuery.year],
			text1: [this.searchOptionQuery.text1],
			text2: [this.searchOptionQuery.text2],
			text3: [this.searchOptionQuery.text3],
		});
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
			case EnumEmployeeRole.DIC:
			case EnumEmployeeRole.RSM:
			case EnumEmployeeRole.BIC:
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
            this.hasSearchOption(EnumSearchOption.UserId)?this.commonService.getUserInfoListByLoggedInManager():of(APIResponse),
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
            this.hasSearchOption(EnumSearchOption.PaymentMethodId)?this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Payment):of(APIResponse),
        ]).subscribe(([dealers, paintingStages, projectStatuses, painters, painterTypes, paymentMethods]) => {
            this.dealers = dealers.data;
            this.paintingStages = paintingStages.data;
            this.projectStatuses = projectStatuses.data;
            this.painters = painters.data;
            this.painterTypes = painterTypes.data;
            this.paymentMethods = paymentMethods.data;
        }, (err) => { }, () => { });
		
		this.subscriptions.push(forkJoinSubscription1);
		this.subscriptions.push(forkJoinSubscription2);

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
		const fromDate = controls['fromDate'].value;
		if(fromDate && fromDate.year && fromDate.month && fromDate.day) {
			this.searchOptionQuery.fromDate = new Date(fromDate.year,fromDate.month-1,fromDate.day);
		}
		const toDate = controls['toDate'].value;
		if(toDate && toDate.year && toDate.month && toDate.day) {
			this.searchOptionQuery.toDate = new Date(toDate.year,toDate.month-1,toDate.day);
		}
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

		this.searchOptionQueryCallbackFn.emit(this.searchOptionQuery);
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
			case EnumEmployeeRole.DIC:
			case EnumEmployeeRole.RSM:
			case EnumEmployeeRole.BIC:
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
		const years = [{'id':2020,'name':'2020'}, {'id':2021,'name':'2021'}, {'id':2022,'name':'2022'}];
		return years;
	}
}
