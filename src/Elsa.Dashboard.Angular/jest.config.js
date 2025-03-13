//// TEST 1 - MY NEW FILE
// module.exports = {
//   preset: 'ts-jest',
//   testEnvironment: 'jsdom',
//   transform: {
//     '^.+\\.ts$': 'ts-jest', // Only transform .ts files
//   },
//   transformIgnorePatterns: ['<rootDir>/node_modules/'],
// };

// // TEST 2 - THIS WORKED WITH NORMAL FUNCTION TEST but NOT with
// // the Mocking Test for HttpClient
// module.exports = {
//   roots: ['<rootDir>/src'],
//   preset: 'ts-jest',
//   testEnvironment: 'jsdom',
//   testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.tsx?$',
//   transform: {
//     '^.+\\.ts?$': 'ts-jest',
//     '^.+\\.(js|jsx)$': 'babel-jest',
//   },
//   transformIgnorePatterns: ['<rootDir>/node_modules/'],
//   globals: {
//     window: {},
//   },
//   moduleFileExtensions: ['ts', 'tsx', 'js'],
//   reporters: ['default', ['jest-junit', { suiteName: 'jest tests' }]],
// };

// // TEST 3 - DIDN'T WORK AT ALL, function tests prev. worked started failing,
// // mocks never worked. TypeError: configSet.processWithEsbuild is not a function
// module.exports = {
//   roots: ['<rootDir>/src'],
//   preset: 'jest-preset-angular',
//   setupFilesAfterEnv: ['<rootDir>/src/setup-jest.ts'], // init. jest-preset-angular
//   testEnvironment: 'jsdom',
//   testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.tsx?$',
//   transform: {
//     '^.+\\.(ts|js|html)$': 'ts-jest',
//   },
//   transformIgnorePatterns: ['node_modules/(?!.*\\.mjs$)'],
//   moduleFileExtensions: ['ts', 'tsx', 'js', 'html'],
//   reporters: ['default', ['jest-junit', { suiteName: 'jest tests' }]],
// };

// TEST 4 - WORKS, Mock and function tests worked using Jest Preset Angular Setup
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
