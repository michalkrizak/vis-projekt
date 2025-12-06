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
  selectedMatch: ZapasDto | null = null;
  selectedPlayer: Hrac | null = null;
  playerMatches: ZapasDto[] = [];
  loadingPlayerMatches: boolean = false;

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

  openMatchDetail(match: ZapasDto) {
    this.selectedMatch = match;
  }

  closeMatchDetail() {
    this.selectedMatch = null;
  }

  openPlayerDetail(player: Hrac) {
    console.log('Otevírám detail hráče:', player);
    this.selectedPlayer = player;
    this.loadPlayerMatches(player.idHrac);
  }

  closePlayerDetail() {
    this.selectedPlayer = null;
    this.playerMatches = [];
  }

  loadPlayerMatches(playerId: number) {
    this.loadingPlayerMatches = true;
    // Načteme všechny zápasy týmu
    this.matchService.getTeamMatches(this.team.idTym).subscribe({
      next: (teamMatches) => {
        // Pro každý zápas zkontrolujeme, zda v něm hráč hrál
        const matchCheckPromises = teamMatches.map(match => 
          this.matchService.getMatchLineups(match.idZapas).toPromise().then(lineups => {
            const playerInMatch = lineups?.some(lineup => lineup.idHrac === playerId);
            return playerInMatch ? match : null;
          }).catch(() => null)
        );
        
        Promise.all(matchCheckPromises).then(results => {
          this.playerMatches = results.filter(m => m != null) as ZapasDto[];
          this.loadingPlayerMatches = false;
        }).catch(err => {
          console.error('Chyba při načítání zápasů hráče:', err);
          this.loadingPlayerMatches = false;
        });
      },
      error: (err) => {
        console.error('Chyba při načítání zápasů:', err);
        this.loadingPlayerMatches = false;
      }
    });
  }

  openMatchDetailFromPlayer(match: ZapasDto) {
    this.closePlayerDetail();
    this.selectedMatch = match;
  }

  onClose() {
    this.close.emit();
  }
}
