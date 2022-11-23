export type Map<T> = {
  [key: string]: T
};

export function ToLetter(num: number) {
  "use strict";
  var mod = num % 26,
    pow = num / 26 | 0,
    out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
  return pow ? ToLetter(pow) + out : out;
};
