import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatchService, Sezona, Tym, ZapasDto, ZapasFilterDto, CreateZapasDto, UpdateZapasDto, Hrac } from '../../services/match.service';

interface HracSestava extends Hrac {
  hraje: boolean;
  kapitan: boolean;
  libero: boolean;
}

@Component({
  selector: 'app-match-management',
  templateUrl: './match-management.component.html',
  styleUrls: ['./match-management.component.scss'],
  imports: [CommonModule, FormsModule],
  standalone: true
})
export class MatchManagementComponent implements OnInit {
  // Dropdown data
  seasons: Sezona[] = [];
  teams: Tym[] = [];
  homeTeams: Tym[] = [];
  awayTeams: Tym[] = [];

  // Filter form
  selectedDate: string = '';
  selectedSeason?: number;
  selectedHomeTeam?: number;
  selectedAwayTeam?: number;

  // Search results
  searchPerformed: boolean = false;
  matches: ZapasDto[] = [];
  noMatchesFound: boolean = false;

  // Selected match details
  selectedMatch?: ZapasDto;
  editMode: boolean = false;
  
  // Match details form
  matchDetails: UpdateZapasDto | null = null;

  // Lineups
  homeLineup: HracSestava[] = [];
  awayLineup: HracSestava[] = [];

  constructor(private matchService: MatchService) {}

  ngOnInit() {
    this.loadSeasons();
    this.loadTeams();
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

  loadTeams() {
    this.matchService.getTeams().subscribe({
      next: (data) => {
        this.teams = data;
        this.updateTeamLists();
      },
      error: (err) => {
        console.error('Chyba při načítání týmů:', err);
        alert('Nepodařilo se načíst týmy.');
      }
    });
  }

  onSeasonChange() {
    if (this.selectedSeason) {
      this.matchService.getTeams(this.selectedSeason).subscribe({
        next: (data) => {
          this.teams = data;
          this.updateTeamLists();
        },
        error: (err) => {
          console.error('Chyba při načítání týmů pro sezónu:', err);
        }
      });
    } else {
      this.loadTeams();
    }
  }

  updateTeamLists() {
    this.homeTeams = this.teams.filter(t => t.idTym !== this.selectedAwayTeam);
    this.awayTeams = this.teams.filter(t => t.idTym !== this.selectedHomeTeam);
  }

  onHomeTeamChange() {
    this.updateTeamLists();
  }

  onAwayTeamChange() {
    this.updateTeamLists();
  }

  searchMatches() {
    const filter: ZapasFilterDto = {
      datum: this.selectedDate || undefined,
      idSezona: this.selectedSeason,
      idTym1: this.selectedHomeTeam,
      idTym2: this.selectedAwayTeam
    };

    this.matchService.findMatches(filter).subscribe({
      next: (data) => {
        this.matches = data;
        this.searchPerformed = true;
        this.noMatchesFound = data.length === 0;
        this.selectedMatch = undefined;
        this.editMode = false;
      },
      error: (err) => {
        console.error('Chyba při vyhledávání zápasů:', err);
        alert('Nepodařilo se vyhledat zápasy.');
      }
    });
  }

  createNewMatch() {
    if (!this.selectedDate || !this.selectedSeason || !this.selectedHomeTeam || !this.selectedAwayTeam) {
      alert('Vyplňte všechna pole pro vytvoření zápasu.');
      return;
    }

    const createDto: CreateZapasDto = {
      datum: this.selectedDate,
      idSezona: this.selectedSeason,
      idTym1: this.selectedHomeTeam,
      idTym2: this.selectedAwayTeam
    };

    this.matchService.createMatch(createDto).subscribe({
      next: (data) => {
        alert('Zápas byl úspěšně vytvořen.');
        this.selectMatch(data);
        this.matches = [data];
        this.noMatchesFound = false;
      },
      error: (err) => {
        console.error('Chyba při vytváření zápasu:', err);
        alert('Nepodařilo se vytvořit zápas.');
      }
    });
  }

  selectMatch(match: ZapasDto) {
    this.selectedMatch = match;
    this.editMode = false;
    
    this.matchDetails = {
      idZapas: match.idZapas,
      datum: match.datum,
      idSezona: match.idSezona,
      idTym1: match.idTym1,
      idTym2: match.idTym2,
      skoreTym1: match.skoreTym1,
      skoreTym2: match.skoreTym2,
      vitez: match.vitez
    };

    this.loadLineups();
  }

  loadLineups() {
    if (!this.selectedMatch) return;

    // Load saved lineups first
    this.matchService.getMatchLineups(this.selectedMatch.idZapas).subscribe({
      next: (savedLineups) => {
        // Create a map of saved lineup data by player ID
        const lineupMap = new Map(savedLineups.map(sl => [sl.idHrac, sl]));

        // Load home team players
        this.matchService.getPlayersByTeam(this.selectedMatch!.idTym1).subscribe({
          next: (players) => {
            this.homeLineup = players.map(p => {
              const saved = lineupMap.get(p.idHrac);
              return {
                ...p,
                hraje: saved?.hraje ?? false,
                kapitan: saved?.jeKapitan ?? false,
                libero: saved?.jeLibero ?? false
              };
            });
          },
          error: (err) => {
            console.error('Chyba při načítání hráčů domácího týmu:', err);
          }
        });

        // Load away team players
        this.matchService.getPlayersByTeam(this.selectedMatch!.idTym2).subscribe({
          next: (players) => {
            this.awayLineup = players.map(p => {
              const saved = lineupMap.get(p.idHrac);
              return {
                ...p,
                hraje: saved?.hraje ?? false,
                kapitan: saved?.jeKapitan ?? false,
                libero: saved?.jeLibero ?? false
              };
            });
          },
          error: (err) => {
            console.error('Chyba při načítání hráčů hostujícího týmu:', err);
          }
        });
      },
      error: (err) => {
        console.error('Chyba při načítání uložených sestav:', err);
        // If no saved lineups, load players with default values
        this.loadPlayersWithDefaults();
      }
    });
  }

  private loadPlayersWithDefaults() {
    if (!this.selectedMatch) return;

    // Load home team players
    this.matchService.getPlayersByTeam(this.selectedMatch.idTym1).subscribe({
      next: (players) => {
        this.homeLineup = players.map(p => ({
          ...p,
          hraje: false,
          kapitan: false,
          libero: false
        }));
      },
      error: (err) => {
        console.error('Chyba při načítání hráčů domácího týmu:', err);
      }
    });

    // Load away team players
    this.matchService.getPlayersByTeam(this.selectedMatch.idTym2).subscribe({
      next: (players) => {
        this.awayLineup = players.map(p => ({
          ...p,
          hraje: false,
          kapitan: false,
          libero: false
        }));
      },
      error: (err) => {
        console.error('Chyba při načítání hráčů hostujícího týmu:', err);
      }
    });
  }

  enableEditMode() {
    this.editMode = true;
  }

  saveMatchDetails() {
    if (!this.matchDetails) return;

    this.matchService.saveMatch(this.matchDetails).subscribe({
      next: () => {
        alert('Zápas byl úspěšně uložen.');
        this.editMode = false;
        // Refresh match details
        if (this.selectedMatch) {
          this.matchService.getMatchDetails(this.selectedMatch.idZapas).subscribe({
            next: (data) => {
              this.selectedMatch = data;
            }
          });
        }
      },
      error: (err) => {
        console.error('Chyba při ukládání zápasu:', err);
        alert('Nepodařilo se uložit zápas.');
      }
    });
  }

  saveLineups() {
    if (!this.selectedMatch) return;

    const sestava = [...this.homeLineup, ...this.awayLineup].map(hrac => ({
      IdHrac: hrac.idHrac,
      IdTym: hrac.idTym,
      JeKapitan: hrac.kapitan,
      JeLibero: hrac.libero,
      Hraje: hrac.hraje
    }));

    const request = {
      IdZapas: this.selectedMatch.idZapas,
      IdTymDomaci: this.selectedMatch.idTym1,
      IdTymHost: this.selectedMatch.idTym2,
      Sestava: sestava
    };

    this.matchService.saveMatchLineups(request).subscribe({
      next: () => {
        alert('Soupisky byly úspěšně uloženy.');
      },
      error: (err) => {
        console.error('Chyba při ukládání soupisk:', err);
        const errorMsg = err.error?.error || err.error?.message || 'Nepodařilo se uložit soupisky.';
        alert(errorMsg);
      }
    });
  }

  deleteMatch() {
    if (!this.selectedMatch) return;

    if (!confirm(`Opravdu chcete smazat zápas ${this.selectedMatch.nazevTym1} vs ${this.selectedMatch.nazevTym2}?`)) {
      return;
    }

    this.matchService.deleteMatch(this.selectedMatch.idZapas).subscribe({
      next: () => {
        alert('Zápas byl úspěšně smazán.');
        this.selectedMatch = undefined;
        this.matchDetails = null;
        this.searchMatches();
      },
      error: (err) => {
        console.error('Chyba při mazání zápasu:', err);
        alert('Nepodařilo se smazat zápas.');
      }
    });
  }
}
