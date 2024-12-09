module.exports = {
    roots: [
      "<rootDir>/src"
    ],
    preset: 'ts-jest',
    testEnvironment: 'jsdom',
    testRegex: "(/__tests__/.*|(\\.|/)(test|spec))\\.tsx?$",
    transform: {
      '^.+\\.ts?$': 'ts-jest',
      "^.+\\.(js|jsx)$": "babel-jest"
    },
    transformIgnorePatterns: ['<rootDir>/node_modules/'],
    globals: {
      window: {}
    },
    moduleFileExtensions: [
      "ts",
      "tsx",
      "js"
    ],
    reporters: [
      "default",
    	[ "jest-junit", { suiteName: "jest tests" } ]
    ]
  };
