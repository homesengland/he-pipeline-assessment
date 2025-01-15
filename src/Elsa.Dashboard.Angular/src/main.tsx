import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from 'src/components/app.module';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
