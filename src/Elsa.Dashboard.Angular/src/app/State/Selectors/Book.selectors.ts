import { createSelector, createFeatureSelector } from '@ngrx/store';
import { Book } from '../../BookList/Book.model';

export const selectBooks = createFeatureSelector<Book[]>('books');
