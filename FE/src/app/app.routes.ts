import { Routes } from '@angular/router';
import { PlayerFormComponent } from './components/player-form/player-form.component';
import { MatchManagementComponent } from './components/match-management/match-management.component';
import { AppComponent } from './app.component';
import path from 'path';

export const routes: Routes = [
    { path: '', component: MatchManagementComponent },
    { path: 'match-management', component: MatchManagementComponent },
    { path: 'app-player-form', component: PlayerFormComponent },
];
