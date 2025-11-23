import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatchService, Tym, Hrac, ZapasDto } from '../../services/match.service';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.scss'],
  imports: [CommonModule],
  standalone: true
})
export class TeamDetailComponent implements OnInit {
  @Input() team!: Tym;
  @Output() close = new EventEmitter<void>();

  players: Hrac[] = [];
  matches: ZapasDto[] = [];
  activeTab: 'players' | 'matches' = 'players';
  loading: boolean = false;

  constructor(private matchService: MatchService) {}

  ngOnInit() {
    this.loadPlayers();
    this.loadMatches();
  }

  loadPlayers() {
    this.loading = true;
    this.matchService.getPlayersByTeam(this.team.idTym).subscribe({
      next: (data) => {
        this.players = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Chyba při načítání hráčů:', err);
        this.loading = false;
      }
    });
  }

  loadMatches() {
    this.matchService.getTeamMatches(this.team.idTym).subscribe({
      next: (data) => {
        this.matches = data;
      },
      error: (err) => {
        console.error('Chyba při načítání zápasů:', err);
      }
    });
  }

  setActiveTab(tab: 'players' | 'matches') {
    this.activeTab = tab;
  }

  onClose() {
    this.close.emit();
  }
}
