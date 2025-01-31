import { createActionGroup, props } from '@ngrx/store';
import { Book } from '../../BookList/Book.model';

export const BooksApiActions = createActionGroup({
  source: 'book service',
  events: {
    'Retrieved Book List': props<{ books: Book[] }>(),
  },
});
