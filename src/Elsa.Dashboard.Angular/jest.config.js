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
  moduleNameMapper: {
    // Base src path
    "^src/(.*)$": "<rootDir>/src/$1",
    
    // Core modules
    "^@models/(.*)$": "<rootDir>/src/models/$1",
    "^@models$": "<rootDir>/src/models/index",
    
    // State
    "^@state/(.*)$": "<rootDir>/src/state/$1",
    "^@state$": "<rootDir>/src/state/index",
    
    // Services
    "^@services/(.*)$": "<rootDir>/src/services/$1",
    "^@services$": "<rootDir>/src/services/index",
    
    // Components
    "^@components/(.*)$": "<rootDir>/src/components/$1",
    "^@components$": "<rootDir>/src/components/index",

  },
};
