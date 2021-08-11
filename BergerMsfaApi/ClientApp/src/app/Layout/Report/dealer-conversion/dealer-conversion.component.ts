import { Component, OnInit } from '@angular/core';
import { NewDealerDevelopmentQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { colDef, IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NewDealerDevelopmentService } from 'src/app/Shared/Services/KPI/NewDealerDevelopmentService';

@Component({
  selector: 'app-dealer-conversion',
  templateUrl: './dealer-conversion.component.html',
  styleUrls: ['./dealer-conversion.component.css']
})
export class DealerConversionComponent implements OnInit {
	PAGE_SIZE: number;

  query: NewDealerDevelopmentQuery;
	searchOptionQuery: SearchOptionQuery;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination
	// renameKeys: any = {'billingAnalysisTypeText':'Billing Analysis Type'};
	allTotalKeysOfNumberType: boolean = true;
	// totalKeys: any[] = ['totalCall'];
	totalKeys: any[] = [];

	// ignoreKeys: any[] = ['billingAnalysisType','details','detailsBtnText'];

  constructor(private newDealerDevelopmentService:NewDealerDevelopmentService,
	private commonService: CommonService,
	) { }

  ngOnInit() {
    this.searchConfiguration();
  }

  
  public ptableSettings: IPTableSetting = {
		tableID: "collection-plans-table",
		tableClass: "table table-border ",
		tableName: 'Dealer Conversion from Competition',
		tableRowIDInternalName: "id",
		tableColDef: [
			// { headerName: 'User Full Name', width: '30%', internalName: 'userFullName', sort: true, type: "" },
			// { headerName: 'Month', width: '20%', internalName: 'monthName', sort: false, type: "" },
			// { headerName: 'Conversion Target', width: '20%', internalName: 'conversionTarget', sort: false, type: "" },
			// { headerName: 'Number of Converted from Competition', width: '10%', internalName: 'numberofConvertedfromCompetition', sort: true, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		pageSize: 12,
		enabledTotal: true,
		enabledDataLength: false,
	};

  searchConfiguration() {
		this.query = new NewDealerDevelopmentQuery({
		
			depot: '',
			territory: '',
			salesGroup: '',
			year: null
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}


	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequired:true}),
			// new SearchOptionDef({searchOption:EnumSearchOption.SalesGroup}),
			new SearchOptionDef({searchOption:EnumSearchOption.FiscalYear, isRequired:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		// console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		// this.query.salesGroups = queryObj.salesGroups;
		this.query.territory = queryObj.territories[0];
		// this.query.salesGroup =queryObj.salesGroups? queryObj.salesGroups[0]:null;
		this.query.year = queryObj.fiscalYear;

    this.loadData()
	}


  loadData() {
    this.newDealerDevelopmentService.GetDealerConversion(this.query).subscribe(
      (res:any) => {this.data = res.data;
	
		this.ptableColDefGenerate()
	}
      ,
      error => console.log(error),
      () => console.log('done')
    );
  }


  ptableColDefGenerate() {
	// this.data = this.data.map(obj => { return this.commonService.renameKeys(obj, this.renameKeys)});
	const obj = this.data[0] || {};
	console.log(obj);
	this.ptableSettings.tableColDef = Object.keys(obj).map((key) => {
		return { headerName: this.commonService.insertSpaces(key), internalName: key, 
			showTotal: (this.allTotalKeysOfNumberType ? (typeof obj[key] === 'number') : this.totalKeys.includes(key)) } as colDef;
	});
	
	// this.ptableSettings.tableColDef.push(
	// 	{ headerName: 'Details', width: '10%', internalName: 'detailsBtnText', sort: false, type: "button", 
	// 		onClick: 'true', innerBtnIcon: "" } as colDef);
}

}
