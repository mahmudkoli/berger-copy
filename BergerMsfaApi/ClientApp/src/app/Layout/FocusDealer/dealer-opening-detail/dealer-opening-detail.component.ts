import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { DealeropeningService } from 'src/app/Shared/Services/FocusDealer/dealeropening.service';


@Component({
  selector: 'app-dealer-opening-detail',
  templateUrl: './dealer-opening-detail.component.html',
  styleUrls: ['./dealer-opening-detail.component.css']
})
export class DealerOpeningDetailComponent implements OnInit {
  dealerOpen:any;
  public baseUrl: string;
  painter: any;
  constructor(

    private alertService: AlertService,
    private route: ActivatedRoute,
    private dealerOpeningSvc: DealeropeningService,
    private router: Router,
    @Inject('BASE_URL') baseUrl: string) { this.baseUrl = baseUrl; }

  ngOnInit() {
    if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
      console.log("id", this.route.snapshot.params.id);
      let id = this.route.snapshot.params.id;
      this.getDealerOpenDetailById(id);
    }
  }

  private getDealerOpenDetailById(id) {
    this.dealerOpeningSvc.GetDealerOpeningDetailById(id).subscribe(
      (result: any) => {
        // debugger;
            this.dealerOpen = result.data;
            console.log(this.dealerOpen);
      },
      (err: any) => console.log(err)
    );
  }

  back() {
    this.router.navigate(['dealer/openingList']);
  }
}
