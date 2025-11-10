import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'volejbal-app';

  constructor(private router: Router) { }

  navigate(){
    this.router.navigate(['/app-player-form']);

  }

}
