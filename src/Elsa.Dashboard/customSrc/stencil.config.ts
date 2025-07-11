import { Config } from '@stencil/core';
import nodePolyfills from 'rollup-plugin-node-polyfills';

export const config: Config = {
  namespace: 'custom-elsa',
  rollupPlugins: {
    after: [
      nodePolyfills(),
    ]
  },
  enableCache: true, //Added to try and improve performance
  srcDir: 'src',
  outputTargets: [
    {
      type: 'www',
      dir: '../www',
      serviceWorker: null, // disable service workers
    },
  ]
};
