import { Routes } from '@angular/router';
import { PlayerFormComponent } from './components/player-form/player-form.component';
import { AppComponent } from './app.component';
import path from 'path';

export const routes: Routes = [
    {path: '', component: PlayerFormComponent },
    { path: 'app-player-form', component: PlayerFormComponent },
];
