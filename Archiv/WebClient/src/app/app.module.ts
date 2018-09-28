import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';

import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {
  MatSidenavModule,
  MatToolbarModule,
  MatButtonModule,
  MatIconModule,
  MatListModule,
  MatSelectModule,
  MatGridListModule,
  MatCardModule,
  MatMenuModule
} from '@angular/material';
import {LayoutModule} from '@angular/cdk/layout';
import {DashboardComponent} from './dashboard/dashboard.component';
import {RouterModule} from '@angular/router';
import {appRoutes} from './app-routes';
import {FlexLayoutModule} from '@angular/flex-layout';
import {ColorPickerModule} from 'ngx-color-picker';
import { LedBoardComponent } from './led-board/led-board.component';
import { LedCardComponent } from './led-board/led-card/led-card.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    LedBoardComponent,
    LedCardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatToolbarModule,
    LayoutModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    RouterModule.forRoot(appRoutes),
    FlexLayoutModule,
    ColorPickerModule,
    MatSelectModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
