import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Book } from './Book.model';

@Injectable({ providedIn: 'root' })
export class BooksService {


  getBooks(): Book[] {

    let Book1: Book = {
      id: "1",
        title: "Book 1 Title",
        author: "Author 1"
    };
    let Book2: Book = {
      id: "2",
        title: "Book 2 Title",
        author: "Author 2"
    };

    let BookArray = [];
    BookArray.push(Book1, Book2);

    return BookArray;

  }
}
