import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: 'auth', loadChildren:() => import('./Outside-Layout/outside-layout.module').then(m => m.OutsideLayoutModule) },
  { path: '', loadChildren: () => import('./Layout/layout.module').then(m => m.LayoutModule) },   
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {
      scrollPositionRestoration: 'enabled',
      anchorScrolling: 'enabled',
    })],
  exports: [RouterModule]
})

export class AppRoutingModule {
}
