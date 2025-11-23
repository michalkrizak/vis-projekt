import { Routes } from '@angular/router';
import { PlayerFormComponent } from './components/player-form/player-form.component';
import { MatchManagementComponent } from './components/match-management/match-management.component';
import { TeamsComponent } from './components/teams/teams.component';

export const routes: Routes = [
    { path: '', redirectTo: '/matches', pathMatch: 'full' },
    { path: 'matches', component: MatchManagementComponent },
    { path: 'teams', component: TeamsComponent },
    { path: 'match-management', component: MatchManagementComponent },
    { path: 'app-player-form', component: PlayerFormComponent },
];
