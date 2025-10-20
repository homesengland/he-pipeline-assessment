import { Config } from '@stencil/core';

export const config: Config = {
  namespace: 'elsa-workflows-studio',

  outputTargets: [
    {
      type: 'www',
      buildDir: '../wwwroot/elsa-workflows-studio',
      copy: [
        {src: 'assets', dest: 'build/assets'},
      ]
    },
  ],
  globalStyle: 'src/globals/tailwind.css',
  plugins: []
};
