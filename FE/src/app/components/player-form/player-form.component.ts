import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // Import FormsModule

@Component({
  selector: 'app-player-form',
  templateUrl: './player-form.component.html',
  styleUrls: ['./player-form.component.scss'],
  imports: [
    CommonModule,
    FormsModule
  ],
})
export class PlayerFormComponent implements OnInit {
  zapasy: any[] = [];
  tymy: any[] = [];
  selectedZapas: number = 0;

  domaciHraci: any[] = [];
  hostujiciHraci: any[] = [];
  selectedTymDomaci: any = null;
  selectedTymHost: any = null;

  constructor(private http: HttpClient,

    private commonModule: CommonModule,

  ) {}

  ngOnInit() {
    this.fetchZapasy();
  }

  fetchZapasy() {
    this.http.get<any[]>('/api/zapasy').subscribe((data) => {
      this.zapasy = data;
    });
  }

  onZapasChange() {
    const zapas = this.zapasy.find(z => z.idZapas == this.selectedZapas);
    if (!zapas) return;

    const idDomaci = zapas.domaciId || zapas.idTym1 || zapas.idTymDomaci;
    const idHost = zapas.hostId || zapas.idTym2 || zapas.idTymHost;

    this.selectedTymDomaci = zapas.domaciId || zapas.idTym1 || zapas.idTymDomaci;
    this.selectedTymHost = zapas.hostId || zapas.idTym2 || zapas.idTymHost;

    this.fetchHraci(idDomaci, 'domaci');
    this.fetchHraci(idHost, 'hostujici');
  }

  fetchHraci(idTym: number, typ: 'domaci' | 'hostujici') {
    this.http.get<any[]>(`/api/hraci/byTym/${idTym}`).subscribe((data) => {
      const hraci = data.map(h => ({
        ...h,
        idTym: idTym,
        hraje: false,
        kapitan: false,
        libero: false
      }));
      if (typ === 'domaci') {
        this.domaciHraci = hraci;
      } else {
        this.hostujiciHraci = hraci;
      }
    });
  }

  submitForm() {
    const sestava = [...this.domaciHraci, ...this.hostujiciHraci]
    .map((hrac: any) => ({
      IdHrac: Number(hrac.idHrac),
      IdTym: Number(hrac.idTym),
      JeKapitan: !!hrac.kapitan,
      JeLibero: !!hrac.libero,
      Hraje: !!hrac.hraje
    }))
    
  
    const requestBody = {
      IdZapas: Number(this.selectedZapas),
      IdTymDomaci: Number(this.selectedTymDomaci),
      IdTymHost: Number(this.selectedTymHost),
      Sestava: sestava
    };
  
    console.log('Odesílám sestavu:', requestBody);
  
    this.http.post('/api/transaction/vloz-sestavu', requestBody).subscribe({
      next: (response) => {
        console.log('Sestava uložena', response);
        alert('Sestava byla úspěšně uložena.');
      },
      error: (err) => {
        const errorMsg = err.error?.message || 'Nastala chyba při odesílání sestavy.';
        alert(errorMsg);
      }
    });
  }

  submitFormSQL() {
    const sestava = [...this.domaciHraci, ...this.hostujiciHraci]
    .map((hrac: any) => ({
      IdHrac: Number(hrac.idHrac),
      IdTym: Number(hrac.idTym),
      JeKapitan: !!hrac.kapitan,
      JeLibero: !!hrac.libero,
      Hraje: !!hrac.hraje
    }))
    
  
    const requestBody = {
      IdZapas: Number(this.selectedZapas),
      IdTymDomaci: Number(this.selectedTymDomaci),
      IdTymHost: Number(this.selectedTymHost),
      Sestava: sestava
    };
  
    console.log('Odesílám sestavu:', requestBody);
  
    this.http.post('/api/transaction/vloz-sestavu-tsql', requestBody).subscribe({
      next: (response) => {
        console.log('Sestava uložena', response);
        alert('Sestava byla úspěšně uložena.');
      },
      error: (err) => {
        const errorMsg = err.error?.message || 'Nastala chyba při odesílání sestavy.';
        alert(errorMsg);
      }
    });
  }
}
