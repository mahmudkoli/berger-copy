import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.css']
})
export class AccessDeniedComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
  }

	goHome() {
		this.router.navigate([`/`], { relativeTo: this.activatedRoute });
	}

}
