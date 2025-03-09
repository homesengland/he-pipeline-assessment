module.exports = {
  preset: 'ts-jest',
  testEnvironment: 'jsdom',
  transform: {
    '^.+\\.ts$': 'ts-jest', // Only transform .ts files
  },
  transformIgnorePatterns: ['<rootDir>/node_modules/'],
};
