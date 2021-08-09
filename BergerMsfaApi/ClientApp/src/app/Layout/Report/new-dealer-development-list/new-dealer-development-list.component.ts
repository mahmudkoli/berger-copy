import { Component, OnInit } from '@angular/core';
import { NewDealerDevelopmentQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { NewDealerDevelopmentService } from 'src/app/Shared/Services/KPI/NewDealerDevelopmentService';

@Component({
  selector: 'app-new-dealer-development-list',
  templateUrl: './new-dealer-development-list.component.html',
  styleUrls: ['./new-dealer-development-list.component.css']
})
export class NewDealerDevelopmentListComponent implements OnInit {
	PAGE_SIZE: number;

  query: NewDealerDevelopmentQuery;
	searchOptionQuery: SearchOptionQuery;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

  constructor(private newDealerDevelopmentService:NewDealerDevelopmentService) { }

  ngOnInit() {
    this.searchConfiguration();
  }

  
  public ptableSettings: IPTableSetting = {
		tableID: "collection-plans-table",
		tableClass: "table table-border ",
		tableName: 'Dealer Opening Status',
		tableRowIDInternalName: "id",
		tableColDef: [
			// { headerName: 'User Full Name', width: '30%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Month Name', width: '20%', internalName: 'monthName', sort: false, type: "" },
			{ headerName: 'Target', width: '20%', internalName: 'target', sort: false, type: "" },
			{ headerName: 'Actual', width: '10%', internalName: 'actual', sort: true, type: "" },
			{ headerName: 'TargetAch', width: '25%', internalName: 'targetAch', sort: true, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		
		enabledDataLength: true,
	};

  searchConfiguration() {
		this.query = new NewDealerDevelopmentQuery({
		
			depot: '',
			territory: '',
			year: null
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}


	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequired:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Year, isRequired:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		// console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		// this.query.salesGroups = queryObj.salesGroups;
		this.query.territory = queryObj.territories[0];
		this.query.year = queryObj.year;

    this.loadData()
	}


  loadData() {
    this.newDealerDevelopmentService.GetDealerOpeningStatus(this.query).subscribe(
      (res:any) =>{this.data = res.data; this.totalDataLength = res.length;
	console.log(res)
	}
	
      ,
      error => console.log(error),
      () => console.log('done')
    );
  }

}
