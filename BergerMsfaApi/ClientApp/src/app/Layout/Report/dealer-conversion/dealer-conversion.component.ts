import { Component, OnInit } from '@angular/core';
import { NewDealerDevelopmentQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
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

  constructor(private newDealerDevelopmentService:NewDealerDevelopmentService) { }

  ngOnInit() {
    this.searchConfiguration();
  }

  
  public ptableSettings: IPTableSetting = {
		tableID: "collection-plans-table",
		tableClass: "table table-border ",
		tableName: 'Dealer Conversion from Competition:',
		tableRowIDInternalName: "id",
		tableColDef: [
			// { headerName: 'User Full Name', width: '30%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Month', width: '20%', internalName: 'monthName', sort: false, type: "" },
			{ headerName: 'Conversion Target', width: '20%', internalName: 'conversionTarget', sort: false, type: "" },
			{ headerName: 'Number of Converted from Competition', width: '10%', internalName: 'numberofConvertedfromCompetition', sort: true, type: "" },
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
    this.newDealerDevelopmentService.GetDealerConversion(this.query).subscribe(
      (res:any) => this.data = res.data
      ,
      error => console.log(error),
      () => console.log('done')
    );
  }

}
