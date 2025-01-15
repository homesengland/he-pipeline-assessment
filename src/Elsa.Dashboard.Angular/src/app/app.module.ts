import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { booksReducer } from './State/Reducers/Book.reducers';

@NgModule({
  declarations: [
  ],
  imports: [
    BrowserModule, HttpClientModule,


    StoreModule.forRoot({ books: booksReducer })

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
