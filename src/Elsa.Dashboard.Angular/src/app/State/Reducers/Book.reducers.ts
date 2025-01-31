import { createReducer, on } from '@ngrx/store';
import { BooksApiActions } from '../Actions/Book.actions'
import { Book } from '../../BookList/Book.model';

export const initialState: ReadonlyArray<Book> = [];

export const booksReducer = createReducer(
  initialState,
  on(BooksApiActions.retrievedBookList, (_state, { books }) => books)
);
