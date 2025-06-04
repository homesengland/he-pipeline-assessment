import { Inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { AppStateActionGroup } from '../actions/app.state.actions';
import { catchError, concatMap, map, mergeMap } from 'rxjs/operators';
import { from, of } from 'rxjs';
import { StoreStatus } from 'src/models/constants';
import { IntellisenseService } from '../../services/intellisense-service';

@Injectable()
export class AppEffects {
  constructor(
    @Inject(Actions) private actions$: Actions,
    private intellisenseService: IntellisenseService // Inject IntellisenseService here
  ) {}

  fetchJavaScriptTypeDefinitions$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AppStateActionGroup.fetchJavaScriptTypeDefinitions),
      mergeMap(() =>
        of(AppStateActionGroup.setJavaScriptTypeDefinitionsFetchStatus({ status: StoreStatus.Fetching })).pipe(
          mergeMap(() =>
            from(this.intellisenseService.getJavaScriptTypeDefinitions()).pipe(
              concatMap(javaScriptTypeDefinitions => [
                AppStateActionGroup.setJavaScriptTypeDefinitions({ javaScriptTypeDefinitions: javaScriptTypeDefinitions as string }),
                AppStateActionGroup.setJavaScriptTypeDefinitionsFetchStatus({ status: StoreStatus.Available })
              ]),
              catchError(() => of(AppStateActionGroup.setJavaScriptTypeDefinitionsFetchStatus({ status: StoreStatus.Error })))
            )
          )
        )
      )
    )
  );
}


