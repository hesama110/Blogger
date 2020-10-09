import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NavbarsComponent} from './navbars/navbars.component'
import {CategoriesComponent} from './categories/categories.component'
import {TagsComponent} from './tags/tags.component'


@NgModule({
  declarations: [ 
    NavbarsComponent,
    CategoriesComponent,
    TagsComponent],
  imports: [
    CommonModule
  ]
})
export class ViewsModule { }
