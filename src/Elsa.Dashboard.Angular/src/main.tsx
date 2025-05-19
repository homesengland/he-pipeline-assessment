import { bootstrapApplication }
  from "@angular/platform-browser";
import { WorkflowRoot } from './components/dashboard/pages/workflow-root/workflow-root';
import { provideRouter } from '@angular/router';
import { routes } from './app-routing';
import { HttpClient, provideHttpClient } from "@angular/common/http";
import { provideStore } from "@ngrx/store";
import { appStateReducer } from "./components/state/reducers/app.state.reducers";

bootstrapApplication(WorkflowRoot, {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    provideStore({ appState: appStateReducer }),

  ]
});

