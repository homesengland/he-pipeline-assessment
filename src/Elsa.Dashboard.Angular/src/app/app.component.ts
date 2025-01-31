import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectBooks } from './State/Selectors/Book.selectors';
import { BooksService } from './BookList/Books.service';
import { BooksApiActions } from './State/Actions/Book.actions';
import { Book } from './BookList/Book.model';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [NgFor],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public bookList: Book[];

  constructor(private http: HttpClient, private booksService: BooksService, private store: Store) { }

  ngOnInit() {

    let bookArray  = this.booksService.getBooks();
    this.store.dispatch(BooksApiActions.retrievedBookList({ books: bookArray }));

    this.getBooksFromStore();
  }

  getBooksFromStore() {
    this.store.select(selectBooks).subscribe(data => {
      this.bookList = data
    })
  }

  title = 'Workflow';
}
