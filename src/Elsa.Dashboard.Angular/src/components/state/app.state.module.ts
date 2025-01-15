import { NgModule } from "@angular/core";
import { StoreModule } from "@ngrx/store";
import { AppState, appStateReducer } from "./reducers/app.state.reducers";

@NgModule({
  imports: [
    StoreModule.forRoot({ appState: appStateReducer }
)
  ]
})
export class AppStateModule { }
