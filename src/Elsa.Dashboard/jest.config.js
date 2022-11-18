module.exports = {
    preset: 'ts-jest',
    testEnvironment: 'jsdom',
    transform: {
      '^.+\\.ts?$': 'ts-jest',
      "^.+\\.(js|jsx)$": "babel-jest"
    },
    transformIgnorePatterns: ['<rootDir>/node_modules/'],
    globals: {
      window: {}
    }
  };
