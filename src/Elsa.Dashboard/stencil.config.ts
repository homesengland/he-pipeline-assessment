import { Config } from '@stencil/core';

export const config: Config = {
  namespace: 'custom-elsa',
  enableCache: true, //Added to try and improve performance
  srcDir: 'src',
  outputTargets: [
    {
      type: 'www',
      serviceWorker: null, // disable service workers
    },
  ]
};
