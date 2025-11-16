import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Sezona {
  idSezona: number;
  rok: number;
  nazev: string;
}

export interface Tym {
  idTym: number;
  nazev: string;
  stadion?: string;
  trener?: string;
  datumZalozeni?: string;
}

export interface Hrac {
  idHrac: number;
  jmeno: string;
  prijmeni: string;
  pozice?: string;
  vek?: number;
  idTym: number;
}

export interface ZapasDto {
  idZapas: number;
  datum: string;
  idTym1: number;
  idTym2: number;
  idSezona: number;
  skoreTym1?: number;
  skoreTym2?: number;
  vitez?: number;
  nazevTym1?: string;
  nazevTym2?: string;
  nazevSezona?: string;
  nazevVitez?: string;
}

export interface ZapasFilterDto {
  datum?: string;
  idSezona?: number;
  idTym1?: number;
  idTym2?: number;
}

export interface CreateZapasDto {
  datum: string;
  idSezona: number;
  idTym1: number;
  idTym2: number;
}

export interface UpdateZapasDto {
  idZapas: number;
  datum: string;
  idSezona: number;
  idTym1: number;
  idTym2: number;
  skoreTym1?: number;
  skoreTym2?: number;
  vitez?: number;
}

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private apiUrl = '/api/zapas';

  constructor(private http: HttpClient) { }

  // F1: Get Seasons
  getSeasons(): Observable<Sezona[]> {
    return this.http.get<Sezona[]>(`${this.apiUrl}/seasons`);
  }

  // F2: Get Teams
  getTeams(idSezona?: number): Observable<Tym[]> {
    const url = idSezona 
      ? `${this.apiUrl}/teams?idSezona=${idSezona}`
      : `${this.apiUrl}/teams`;
    return this.http.get<Tym[]>(url);
  }

  // F3: Find Matches
  findMatches(filter: ZapasFilterDto): Observable<ZapasDto[]> {
    return this.http.post<ZapasDto[]>(`${this.apiUrl}/find`, filter);
  }

  // F4: Create Match
  createMatch(createDto: CreateZapasDto): Observable<ZapasDto> {
    return this.http.post<ZapasDto>(`${this.apiUrl}/create`, createDto);
  }

  // F6: Get Match Details
  getMatchDetails(idZapas: number): Observable<ZapasDto> {
    return this.http.get<ZapasDto>(`${this.apiUrl}/${idZapas}`);
  }

  // F7: Save Match
  saveMatch(updateDto: UpdateZapasDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/save`, updateDto);
  }

  // F8: Get Players By Team
  getPlayersByTeam(idTym: number): Observable<Hrac[]> {
    return this.http.get<Hrac[]>(`${this.apiUrl}/players/${idTym}`);
  }

  // F9: Save Match Lineups
  saveMatchLineups(request: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/lineups`, request);
  }

  // F10: Delete Match
  deleteMatch(idZapas: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${idZapas}`);
  }
}
