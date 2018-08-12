import {Routes} from '@angular/router';
import {DashboardComponent} from './dashboard/dashboard.component';
import {LedBoardComponent} from './led-board/led-board.component';

/*export const appRoutes: Routes = [
  {path: 'led-board', component: LedBoardComponent},
  {
    path: '',
    redirectTo: '/led-board',
    pathMatch: 'full'
  },
  {path: '**', component: LedBoardComponent}
];
*/

export const appRoutes: Routes = [
  {path: '', pathMatch: 'full', component: LedBoardComponent}
];

