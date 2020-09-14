import { Component, OnInit } from '@angular/core';
import {NgbCarouselConfig} from '@ng-bootstrap/ng-bootstrap';
import  { Router } from '@angular/router'

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent implements OnInit {

  constructor(config: NgbCarouselConfig, public routerService: Router) { 
    config.showNavigationArrows = true;
    config.showNavigationIndicators = false;
    config.pauseOnHover = true;

  }

  ngOnInit() {
  }

  backToHome() {
    debugger;
    this.routerService.navigate(['/dashboard/common']);
  }

  heading = 'Carousels & Slideshows';
  subheading = 'Create easy and beautiful slideshows with these Vue components.';
  icon = 'pe-7s-album icon-gradient bg-sunny-morning';

  slideConfig = {
    slidesToShow: 1,
    dots: true,
  };

  slideConfig2 = {
    className: 'center',
    centerMode: true,
    infinite: true,
    centerPadding: '60px',
    slidesToShow: 3,
    speed: 500,
    dots: true,
  };

  slideConfig3 = {
    dots: true,
    infinite: false,
    speed: 500,
    slidesToShow: 4,
    slidesToScroll: 4,
    initialSlide: 0,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
          dots: true
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
          initialSlide: 2
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
    ]
  };

  slideConfig4 = {
    slidesToShow: 3,
    dots: true,
  };

  slideConfig5 = {
    className: 'slider variable-width',
    dots: true,
    infinite: true,
    centerMode: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    variableWidth: true
  };

  slideConfig6 = {
    className: 'center',
    infinite: true,
    slidesToShow: 1,
    speed: 500,
    adaptiveHeight: true,
    dots: true,
  };


}
