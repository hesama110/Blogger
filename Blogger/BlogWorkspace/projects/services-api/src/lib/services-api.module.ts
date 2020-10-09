import { NgModule, ModuleWithProviders } from '@angular/core';
import { WeatherForecastService } from './services.api.generated';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [],
  imports: [
    HttpClientModule
  ],
  exports: []
})
export class ServicesApiModule {

  static forRoot(environment: any): ModuleWithProviders<ServicesApiModule> {
    return {
      ngModule: ServicesApiModule,
      providers: [
        WeatherForecastService,
        { provide: 'env', useValue: environment }
      ]
    };
  }

  

  
}
