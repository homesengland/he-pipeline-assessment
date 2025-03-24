module.exports = {
  roots: ['<rootDir>/src'],
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/src/setup-jest.ts'], // init. jest-preset-angular
  testEnvironment: 'jsdom',
  testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.tsx?$',
  transformIgnorePatterns: ['node_modules/(?!.*\\.mjs$)'],
  globals: {
    'ts-jest': {
      tsconfig: '<rootDir>/tsconfig.spec.json',
    },
    'window': {},
  },
  moduleFileExtensions: ['ts', 'tsx', 'js', 'html'],
  reporters: ['default', ['jest-junit', { suiteName: 'jest tests' }]],
};
