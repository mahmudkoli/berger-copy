import { Component, OnInit } from '@angular/core';
import { NewDealerDevelopmentQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { EnumMonthLabel } from 'src/app/Shared/Enums/employee-role';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { NewDealerDevelopmentService } from 'src/app/Shared/Services/KPI/NewDealerDevelopmentService';

@Component({
  selector: 'app-new-dealer-development',
  templateUrl: './new-dealer-development.component.html',
  styleUrls: ['./new-dealer-development.component.css']
})
export class NewDealerDevelopmentComponent implements OnInit {
  query: NewDealerDevelopmentQuery;
	searchOptionQuery: SearchOptionQuery;
	data: any[];
  month: MapObject[] = EnumMonthLabel.EnumMonth;

  constructor(
    private newDealerDevelopmentService:NewDealerDevelopmentService
  ) { }

  ngOnInit() {
		this.searchConfiguration();
    
  }

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
    this.data = [];
    this.newDealerDevelopmentService.getNewDealerDevelopment(this.query).subscribe(
      res => this.data = res.data,
      error => console.log(error),
      () => console.log('done')
    );
  }

  SaveOrUpdateData() {

this.newDealerDevelopmentService.SaveOrUpdateNewDealerDevelopment(this.data).subscribe(
      res => console.log(res),
      error => console.log(error),
      () => console.log('done')
    );
}



}
