import { Routes } from '@angular/router';
import { PlayerFormComponent } from './components/player-form/player-form.component';
import { MatchManagementComponent } from './components/match-management/match-management.component';
import { TeamsComponent } from './components/teams/teams.component';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'matches', component: MatchManagementComponent },
    { path: 'teams', component: TeamsComponent },
    { path: 'match-management', component: MatchManagementComponent },
    { path: 'app-player-form', component: PlayerFormComponent },
];
