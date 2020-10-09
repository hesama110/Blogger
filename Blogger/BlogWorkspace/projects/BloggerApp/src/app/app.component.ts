import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { WeatherForecastService } from 'services-api';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private router: Router, ww: WeatherForecastService) {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
    
    console.log(ww.weatherForecast());
    var val = ww.weatherForecast().subscribe(x => {
      console.log(x);

    })
  }
}
