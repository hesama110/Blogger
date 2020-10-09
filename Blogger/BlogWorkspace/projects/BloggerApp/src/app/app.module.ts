import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import{ServicesApiModule} from 'services-api'
import { environment } from '../environments/environment';
import { LayoutComponent } from './layout/layout.component';

import { ViewsModule} from './views/views.module';

@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent
   
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ViewsModule,
    ServicesApiModule.forRoot(environment)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
