import { select } from '@angular-redux/store';
import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Menu } from 'src/app/Shared/Entity/Menu/menu.model';
import { MenuService } from 'src/app/Shared/Services/Menu-Details/menu.service';
import { environment } from 'src/environments/environment';
import { ThemeOptions } from '../../../../theme-options';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  public extraParameter: any;
  menuList: Menu[] = [];

  constructor(
    public globals: ThemeOptions,
    private activatedRoute: ActivatedRoute,
    private menuService: MenuService
  ) {}

  @select('config') public config$: Observable<any>;

  private newInnerWidth: number;
  private innerWidth: number;
  activeId = 'dashboardsMenu';
  prod: any;
  toggleSidebar() {
    this.globals.toggleSidebar = !this.globals.toggleSidebar;
  }

  sidebarHover() {
    this.globals.sidebarHover = !this.globals.sidebarHover;
  }

  ngOnInit() {
    this.prod = environment.production;
    setTimeout(() => {
      this.innerWidth = window.innerWidth;
      if (this.innerWidth < 1200) {
        this.globals.toggleSidebar = true;
      }
    });

    this.extraParameter =
      this.activatedRoute.snapshot.firstChild.data.extraParameter || '';

    this.getMenus();
  }

  getMenus() {
    let roleId = 1;
    if (localStorage.getItem('currentUser')) {
      const userInfo = JSON.parse(localStorage.getItem('currentUser'));
      roleId = userInfo.roleId;
    }
    this.menuService.getPermissionMenus(roleId).subscribe(
      (res: any) => {
        this.menuList = res.data;
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.newInnerWidth = event.target.innerWidth;

    if (this.newInnerWidth < 1200) {
      this.globals.toggleSidebar = true;
    } else {
      this.globals.toggleSidebar = false;
    }
  }
}
