import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';


import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MenuComponent} from './menu/menu.component';
import {MatButtonModule, MatCardModule, MatToolbarModule} from '@angular/material';
import {DashboardComponent} from './dashboard/dashboard.component';
import {RouterModule, Routes} from "@angular/router";
import {WebSocketService} from "./web-socket.service";
import { LedcardComponent } from './dashboard/ledcard/ledcard.component';

const appRoutes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  { path: '**', component: DashboardComponent }
];


@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    DashboardComponent,
    LedcardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    RouterModule.forRoot(appRoutes),
    MatCardModule
  ],
  providers: [WebSocketService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
