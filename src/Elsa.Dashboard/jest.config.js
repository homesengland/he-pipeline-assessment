module.exports = {
    preset: 'ts-jest',
    testEnvironment: 'node',
    transform: {
      '^.+\\.ts?$': 'ts-jest',
      "^.+\\.(js|jsx)$": "babel-jest"
    },
    transformIgnorePatterns: ['<rootDir>/node_modules/'],
  };
