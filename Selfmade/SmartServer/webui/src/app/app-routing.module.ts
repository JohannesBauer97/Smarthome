import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TemperatureComponent} from './temperature/temperature.component';


const routes: Routes = [
  {path: 'home', component: TemperatureComponent},
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: '**', redirectTo: '/home'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
