import { setupZoneTestEnv } from 'jest-preset-angular/setup-env/zone';

setupZoneTestEnv();

jest.mock(
  'el-transition',
  () => ({
    enter: jest.fn().mockImplementation(() => Promise.resolve()),
    leave: jest.fn().mockImplementation(() => Promise.resolve()),
  }),
  { virtual: true },
);
