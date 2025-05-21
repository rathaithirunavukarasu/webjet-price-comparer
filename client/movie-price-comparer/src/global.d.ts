import 'jest-fetch-mock';

declare global {
  const fetchMock: typeof import('jest-fetch-mock').default;
}

declare module 'react-router-dom';