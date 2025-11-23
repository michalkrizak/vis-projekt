import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatchService, Sezona, Tym } from '../../services/match.service';
import { TeamDetailComponent } from '../team-detail/team-detail.component';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss'],
  imports: [CommonModule, FormsModule, TeamDetailComponent],
  standalone: true
})
export class TeamsComponent implements OnInit {
  seasons: Sezona[] = [];
  teams: Tym[] = [];
  selectedSeason?: number;
  selectedTeam?: Tym;
  showModal: boolean = false;
  
  constructor(private matchService: MatchService) {}

  ngOnInit() {
    this.loadSeasons();
  }

  loadSeasons() {
    this.matchService.getSeasons().subscribe({
      next: (data) => {
        this.seasons = data;
      },
      error: (err) => {
        console.error('Chyba při načítání sezón:', err);
        alert('Nepodařilo se načíst sezóny.');
      }
    });
  }

  onSeasonChange() {
    if (this.selectedSeason) {
      this.matchService.getTeams(this.selectedSeason).subscribe({
        next: (data) => {
          this.teams = data;
        },
        error: (err) => {
          console.error('Chyba při načítání týmů:', err);
          alert('Nepodařilo se načíst týmy.');
        }
      });
    } else {
      this.teams = [];
    }
  }

  openTeamDetail(team: Tym) {
    this.selectedTeam = team;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.selectedTeam = undefined;
  }
}
