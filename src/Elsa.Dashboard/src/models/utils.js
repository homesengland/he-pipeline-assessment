export function ToLetter(num) {
    "use strict";
    var mod = num % 26, pow = num / 26 | 0, out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
    return pow ? ToLetter(pow) + out : out;
}
;
