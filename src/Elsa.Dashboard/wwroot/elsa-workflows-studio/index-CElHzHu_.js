import './_commonjsHelpers-Cf5sKic0.js';
import { O as require_baseAssignValue, S as requireKeys, P as requireIsObject, T as require_isPrototype, U as require_arrayLikeKeys, I as requireIsArrayLike, V as require_root, W as require_getSymbols, X as require_overArg, d as require_arrayPush, Y as requireStubArray, Z as require_baseGetAllKeys, _ as require_Uint8Array, $ as require_Symbol, a0 as require_getTag, J as requireIsObjectLike, j as require_baseUnary, a1 as require_nodeUtil, a2 as require_Stack, a3 as require_arrayEach, f as require_copyArray, a4 as require_getAllKeys, g as requireIsArray, a5 as requireIsBuffer, a6 as require_hasPath, a7 as require_baseKeys, a8 as requireIsArguments, a9 as requireIsTypedArray, aa as require_baseForOwn, m as require_baseIteratee, Q as requireIsFunction, ab as requireValues, ac as requireSize, ad as requireReduce, ae as requireMap, af as requireFilter, R as requireEach, ag as requireConstant, l as require_baseRest, B as requireEq, a as require_isIterateeCall, ah as require_baseFor, ai as require_castFunction, z as requireIsSymbol, A as requireIdentity, aj as require_baseGetTag, ak as require_baseGet, v as require_castPath, al as requireHasIn, am as requireToFinite, an as requireToString, ao as requireSortBy, n as requireLast, ap as requireForEach, aq as requireFind } from './collection-B4sYCr2r.js';
import { j as require_assignValue, h as requireUnion, b as requireIsArrayLikeObject, e as require_baseSet, c as require_flatRest, g as requireZipObject, i as requireFlatten } from './_baseSet-SXJectIy.js';

// Unique ID creation requires a high quality random # generator. In the browser we therefore
// require the crypto API and do not support built-in fallback to lower quality random number
// generators (like Math.random()).
var getRandomValues;
var rnds8 = new Uint8Array(16);
function rng() {
  // lazy load so that environments that need to polyfill have a chance to do so
  if (!getRandomValues) {
    // getRandomValues needs to be invoked in a context where "this" is a Crypto implementation. Also,
    // find the complete implementation of crypto (msCrypto) on IE11.
    getRandomValues = typeof crypto !== 'undefined' && crypto.getRandomValues && crypto.getRandomValues.bind(crypto) || typeof msCrypto !== 'undefined' && typeof msCrypto.getRandomValues === 'function' && msCrypto.getRandomValues.bind(msCrypto);

    if (!getRandomValues) {
      throw new Error('crypto.getRandomValues() not supported. See https://github.com/uuidjs/uuid#getrandomvalues-not-supported');
    }
  }

  return getRandomValues(rnds8);
}

var REGEX = /^(?:[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}|00000000-0000-0000-0000-000000000000)$/i;

function validate(uuid) {
  return typeof uuid === 'string' && REGEX.test(uuid);
}

/**
 * Convert array of 16 byte values to UUID string format of the form:
 * XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX
 */

var byteToHex = [];

for (var i = 0; i < 256; ++i) {
  byteToHex.push((i + 0x100).toString(16).substr(1));
}

function stringify(arr) {
  var offset = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 0;
  // Note: Be careful editing this code!  It's been tuned for performance
  // and works in ways you may not expect. See https://github.com/uuidjs/uuid/pull/434
  var uuid = (byteToHex[arr[offset + 0]] + byteToHex[arr[offset + 1]] + byteToHex[arr[offset + 2]] + byteToHex[arr[offset + 3]] + '-' + byteToHex[arr[offset + 4]] + byteToHex[arr[offset + 5]] + '-' + byteToHex[arr[offset + 6]] + byteToHex[arr[offset + 7]] + '-' + byteToHex[arr[offset + 8]] + byteToHex[arr[offset + 9]] + '-' + byteToHex[arr[offset + 10]] + byteToHex[arr[offset + 11]] + byteToHex[arr[offset + 12]] + byteToHex[arr[offset + 13]] + byteToHex[arr[offset + 14]] + byteToHex[arr[offset + 15]]).toLowerCase(); // Consistency check for valid UUID.  If this throws, it's likely due to one
  // of the following:
  // - One or more input array values don't map to a hex octet (leading to
  // "undefined" in the uuid)
  // - Invalid input values for the RFC `version` or `variant` fields

  if (!validate(uuid)) {
    throw TypeError('Stringified UUID is invalid');
  }

  return uuid;
}

//
// Inspired by https://github.com/LiosK/UUID.js
// and http://docs.python.org/library/uuid.html

var _nodeId;

var _clockseq; // Previous uuid creation time


var _lastMSecs = 0;
var _lastNSecs = 0; // See https://github.com/uuidjs/uuid for API details

function v1(options, buf, offset) {
  var i = buf && offset || 0;
  var b = buf || new Array(16);
  options = options || {};
  var node = options.node || _nodeId;
  var clockseq = options.clockseq !== undefined ? options.clockseq : _clockseq; // node and clockseq need to be initialized to random values if they're not
  // specified.  We do this lazily to minimize issues related to insufficient
  // system entropy.  See #189

  if (node == null || clockseq == null) {
    var seedBytes = options.random || (options.rng || rng)();

    if (node == null) {
      // Per 4.5, create and 48-bit node id, (47 random bits + multicast bit = 1)
      node = _nodeId = [seedBytes[0] | 0x01, seedBytes[1], seedBytes[2], seedBytes[3], seedBytes[4], seedBytes[5]];
    }

    if (clockseq == null) {
      // Per 4.2.2, randomize (14 bit) clockseq
      clockseq = _clockseq = (seedBytes[6] << 8 | seedBytes[7]) & 0x3fff;
    }
  } // UUID timestamps are 100 nano-second units since the Gregorian epoch,
  // (1582-10-15 00:00).  JSNumbers aren't precise enough for this, so
  // time is handled internally as 'msecs' (integer milliseconds) and 'nsecs'
  // (100-nanoseconds offset from msecs) since unix epoch, 1970-01-01 00:00.


  var msecs = options.msecs !== undefined ? options.msecs : Date.now(); // Per 4.2.1.2, use count of uuid's generated during the current clock
  // cycle to simulate higher resolution clock

  var nsecs = options.nsecs !== undefined ? options.nsecs : _lastNSecs + 1; // Time since last uuid creation (in msecs)

  var dt = msecs - _lastMSecs + (nsecs - _lastNSecs) / 10000; // Per 4.2.1.2, Bump clockseq on clock regression

  if (dt < 0 && options.clockseq === undefined) {
    clockseq = clockseq + 1 & 0x3fff;
  } // Reset nsecs if clock regresses (new clockseq) or we've moved onto a new
  // time interval


  if ((dt < 0 || msecs > _lastMSecs) && options.nsecs === undefined) {
    nsecs = 0;
  } // Per 4.2.1.2 Throw error if too many uuids are requested


  if (nsecs >= 10000) {
    throw new Error("uuid.v1(): Can't create more than 10M uuids/sec");
  }

  _lastMSecs = msecs;
  _lastNSecs = nsecs;
  _clockseq = clockseq; // Per 4.1.4 - Convert from unix epoch to Gregorian epoch

  msecs += 12219292800000; // `time_low`

  var tl = ((msecs & 0xfffffff) * 10000 + nsecs) % 0x100000000;
  b[i++] = tl >>> 24 & 0xff;
  b[i++] = tl >>> 16 & 0xff;
  b[i++] = tl >>> 8 & 0xff;
  b[i++] = tl & 0xff; // `time_mid`

  var tmh = msecs / 0x100000000 * 10000 & 0xfffffff;
  b[i++] = tmh >>> 8 & 0xff;
  b[i++] = tmh & 0xff; // `time_high_and_version`

  b[i++] = tmh >>> 24 & 0xf | 0x10; // include version

  b[i++] = tmh >>> 16 & 0xff; // `clock_seq_hi_and_reserved` (Per 4.2.2 - include variant)

  b[i++] = clockseq >>> 8 | 0x80; // `clock_seq_low`

  b[i++] = clockseq & 0xff; // `node`

  for (var n = 0; n < 6; ++n) {
    b[i + n] = node[n];
  }

  return buf || stringify(b);
}

function parse(uuid) {
  if (!validate(uuid)) {
    throw TypeError('Invalid UUID');
  }

  var v;
  var arr = new Uint8Array(16); // Parse ########-....-....-....-............

  arr[0] = (v = parseInt(uuid.slice(0, 8), 16)) >>> 24;
  arr[1] = v >>> 16 & 0xff;
  arr[2] = v >>> 8 & 0xff;
  arr[3] = v & 0xff; // Parse ........-####-....-....-............

  arr[4] = (v = parseInt(uuid.slice(9, 13), 16)) >>> 8;
  arr[5] = v & 0xff; // Parse ........-....-####-....-............

  arr[6] = (v = parseInt(uuid.slice(14, 18), 16)) >>> 8;
  arr[7] = v & 0xff; // Parse ........-....-....-####-............

  arr[8] = (v = parseInt(uuid.slice(19, 23), 16)) >>> 8;
  arr[9] = v & 0xff; // Parse ........-....-....-....-############
  // (Use "/" to avoid 32-bit truncation when bit-shifting high-order bytes)

  arr[10] = (v = parseInt(uuid.slice(24, 36), 16)) / 0x10000000000 & 0xff;
  arr[11] = v / 0x100000000 & 0xff;
  arr[12] = v >>> 24 & 0xff;
  arr[13] = v >>> 16 & 0xff;
  arr[14] = v >>> 8 & 0xff;
  arr[15] = v & 0xff;
  return arr;
}

function stringToBytes(str) {
  str = unescape(encodeURIComponent(str)); // UTF8 escape

  var bytes = [];

  for (var i = 0; i < str.length; ++i) {
    bytes.push(str.charCodeAt(i));
  }

  return bytes;
}

var DNS = '6ba7b810-9dad-11d1-80b4-00c04fd430c8';
var URL = '6ba7b811-9dad-11d1-80b4-00c04fd430c8';
function v35 (name, version, hashfunc) {
  function generateUUID(value, namespace, buf, offset) {
    if (typeof value === 'string') {
      value = stringToBytes(value);
    }

    if (typeof namespace === 'string') {
      namespace = parse(namespace);
    }

    if (namespace.length !== 16) {
      throw TypeError('Namespace must be array-like (16 iterable integer values, 0-255)');
    } // Compute hash of namespace and value, Per 4.3
    // Future: Use spread syntax when supported on all platforms, e.g. `bytes =
    // hashfunc([...namespace, ... value])`


    var bytes = new Uint8Array(16 + value.length);
    bytes.set(namespace);
    bytes.set(value, namespace.length);
    bytes = hashfunc(bytes);
    bytes[6] = bytes[6] & 0x0f | version;
    bytes[8] = bytes[8] & 0x3f | 0x80;

    if (buf) {
      offset = offset || 0;

      for (var i = 0; i < 16; ++i) {
        buf[offset + i] = bytes[i];
      }

      return buf;
    }

    return stringify(bytes);
  } // Function#name is not settable on some platforms (#270)


  try {
    generateUUID.name = name; // eslint-disable-next-line no-empty
  } catch (err) {} // For CommonJS default export support


  generateUUID.DNS = DNS;
  generateUUID.URL = URL;
  return generateUUID;
}

/*
 * Browser-compatible JavaScript MD5
 *
 * Modification of JavaScript MD5
 * https://github.com/blueimp/JavaScript-MD5
 *
 * Copyright 2011, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * https://opensource.org/licenses/MIT
 *
 * Based on
 * A JavaScript implementation of the RSA Data Security, Inc. MD5 Message
 * Digest Algorithm, as defined in RFC 1321.
 * Version 2.2 Copyright (C) Paul Johnston 1999 - 2009
 * Other contributors: Greg Holt, Andrew Kepert, Ydnar, Lostinet
 * Distributed under the BSD License
 * See http://pajhome.org.uk/crypt/md5 for more info.
 */
function md5(bytes) {
  if (typeof bytes === 'string') {
    var msg = unescape(encodeURIComponent(bytes)); // UTF8 escape

    bytes = new Uint8Array(msg.length);

    for (var i = 0; i < msg.length; ++i) {
      bytes[i] = msg.charCodeAt(i);
    }
  }

  return md5ToHexEncodedArray(wordsToMd5(bytesToWords(bytes), bytes.length * 8));
}
/*
 * Convert an array of little-endian words to an array of bytes
 */


function md5ToHexEncodedArray(input) {
  var output = [];
  var length32 = input.length * 32;
  var hexTab = '0123456789abcdef';

  for (var i = 0; i < length32; i += 8) {
    var x = input[i >> 5] >>> i % 32 & 0xff;
    var hex = parseInt(hexTab.charAt(x >>> 4 & 0x0f) + hexTab.charAt(x & 0x0f), 16);
    output.push(hex);
  }

  return output;
}
/**
 * Calculate output length with padding and bit length
 */


function getOutputLength(inputLength8) {
  return (inputLength8 + 64 >>> 9 << 4) + 14 + 1;
}
/*
 * Calculate the MD5 of an array of little-endian words, and a bit length.
 */


function wordsToMd5(x, len) {
  /* append padding */
  x[len >> 5] |= 0x80 << len % 32;
  x[getOutputLength(len) - 1] = len;
  var a = 1732584193;
  var b = -271733879;
  var c = -1732584194;
  var d = 271733878;

  for (var i = 0; i < x.length; i += 16) {
    var olda = a;
    var oldb = b;
    var oldc = c;
    var oldd = d;
    a = md5ff(a, b, c, d, x[i], 7, -680876936);
    d = md5ff(d, a, b, c, x[i + 1], 12, -389564586);
    c = md5ff(c, d, a, b, x[i + 2], 17, 606105819);
    b = md5ff(b, c, d, a, x[i + 3], 22, -1044525330);
    a = md5ff(a, b, c, d, x[i + 4], 7, -176418897);
    d = md5ff(d, a, b, c, x[i + 5], 12, 1200080426);
    c = md5ff(c, d, a, b, x[i + 6], 17, -1473231341);
    b = md5ff(b, c, d, a, x[i + 7], 22, -45705983);
    a = md5ff(a, b, c, d, x[i + 8], 7, 1770035416);
    d = md5ff(d, a, b, c, x[i + 9], 12, -1958414417);
    c = md5ff(c, d, a, b, x[i + 10], 17, -42063);
    b = md5ff(b, c, d, a, x[i + 11], 22, -1990404162);
    a = md5ff(a, b, c, d, x[i + 12], 7, 1804603682);
    d = md5ff(d, a, b, c, x[i + 13], 12, -40341101);
    c = md5ff(c, d, a, b, x[i + 14], 17, -1502002290);
    b = md5ff(b, c, d, a, x[i + 15], 22, 1236535329);
    a = md5gg(a, b, c, d, x[i + 1], 5, -165796510);
    d = md5gg(d, a, b, c, x[i + 6], 9, -1069501632);
    c = md5gg(c, d, a, b, x[i + 11], 14, 643717713);
    b = md5gg(b, c, d, a, x[i], 20, -373897302);
    a = md5gg(a, b, c, d, x[i + 5], 5, -701558691);
    d = md5gg(d, a, b, c, x[i + 10], 9, 38016083);
    c = md5gg(c, d, a, b, x[i + 15], 14, -660478335);
    b = md5gg(b, c, d, a, x[i + 4], 20, -405537848);
    a = md5gg(a, b, c, d, x[i + 9], 5, 568446438);
    d = md5gg(d, a, b, c, x[i + 14], 9, -1019803690);
    c = md5gg(c, d, a, b, x[i + 3], 14, -187363961);
    b = md5gg(b, c, d, a, x[i + 8], 20, 1163531501);
    a = md5gg(a, b, c, d, x[i + 13], 5, -1444681467);
    d = md5gg(d, a, b, c, x[i + 2], 9, -51403784);
    c = md5gg(c, d, a, b, x[i + 7], 14, 1735328473);
    b = md5gg(b, c, d, a, x[i + 12], 20, -1926607734);
    a = md5hh(a, b, c, d, x[i + 5], 4, -378558);
    d = md5hh(d, a, b, c, x[i + 8], 11, -2022574463);
    c = md5hh(c, d, a, b, x[i + 11], 16, 1839030562);
    b = md5hh(b, c, d, a, x[i + 14], 23, -35309556);
    a = md5hh(a, b, c, d, x[i + 1], 4, -1530992060);
    d = md5hh(d, a, b, c, x[i + 4], 11, 1272893353);
    c = md5hh(c, d, a, b, x[i + 7], 16, -155497632);
    b = md5hh(b, c, d, a, x[i + 10], 23, -1094730640);
    a = md5hh(a, b, c, d, x[i + 13], 4, 681279174);
    d = md5hh(d, a, b, c, x[i], 11, -358537222);
    c = md5hh(c, d, a, b, x[i + 3], 16, -722521979);
    b = md5hh(b, c, d, a, x[i + 6], 23, 76029189);
    a = md5hh(a, b, c, d, x[i + 9], 4, -640364487);
    d = md5hh(d, a, b, c, x[i + 12], 11, -421815835);
    c = md5hh(c, d, a, b, x[i + 15], 16, 530742520);
    b = md5hh(b, c, d, a, x[i + 2], 23, -995338651);
    a = md5ii(a, b, c, d, x[i], 6, -198630844);
    d = md5ii(d, a, b, c, x[i + 7], 10, 1126891415);
    c = md5ii(c, d, a, b, x[i + 14], 15, -1416354905);
    b = md5ii(b, c, d, a, x[i + 5], 21, -57434055);
    a = md5ii(a, b, c, d, x[i + 12], 6, 1700485571);
    d = md5ii(d, a, b, c, x[i + 3], 10, -1894986606);
    c = md5ii(c, d, a, b, x[i + 10], 15, -1051523);
    b = md5ii(b, c, d, a, x[i + 1], 21, -2054922799);
    a = md5ii(a, b, c, d, x[i + 8], 6, 1873313359);
    d = md5ii(d, a, b, c, x[i + 15], 10, -30611744);
    c = md5ii(c, d, a, b, x[i + 6], 15, -1560198380);
    b = md5ii(b, c, d, a, x[i + 13], 21, 1309151649);
    a = md5ii(a, b, c, d, x[i + 4], 6, -145523070);
    d = md5ii(d, a, b, c, x[i + 11], 10, -1120210379);
    c = md5ii(c, d, a, b, x[i + 2], 15, 718787259);
    b = md5ii(b, c, d, a, x[i + 9], 21, -343485551);
    a = safeAdd(a, olda);
    b = safeAdd(b, oldb);
    c = safeAdd(c, oldc);
    d = safeAdd(d, oldd);
  }

  return [a, b, c, d];
}
/*
 * Convert an array bytes to an array of little-endian words
 * Characters >255 have their high-byte silently ignored.
 */


function bytesToWords(input) {
  if (input.length === 0) {
    return [];
  }

  var length8 = input.length * 8;
  var output = new Uint32Array(getOutputLength(length8));

  for (var i = 0; i < length8; i += 8) {
    output[i >> 5] |= (input[i / 8] & 0xff) << i % 32;
  }

  return output;
}
/*
 * Add integers, wrapping at 2^32. This uses 16-bit operations internally
 * to work around bugs in some JS interpreters.
 */


function safeAdd(x, y) {
  var lsw = (x & 0xffff) + (y & 0xffff);
  var msw = (x >> 16) + (y >> 16) + (lsw >> 16);
  return msw << 16 | lsw & 0xffff;
}
/*
 * Bitwise rotate a 32-bit number to the left.
 */


function bitRotateLeft(num, cnt) {
  return num << cnt | num >>> 32 - cnt;
}
/*
 * These functions implement the four basic operations the algorithm uses.
 */


function md5cmn(q, a, b, x, s, t) {
  return safeAdd(bitRotateLeft(safeAdd(safeAdd(a, q), safeAdd(x, t)), s), b);
}

function md5ff(a, b, c, d, x, s, t) {
  return md5cmn(b & c | ~b & d, a, b, x, s, t);
}

function md5gg(a, b, c, d, x, s, t) {
  return md5cmn(b & d | c & ~d, a, b, x, s, t);
}

function md5hh(a, b, c, d, x, s, t) {
  return md5cmn(b ^ c ^ d, a, b, x, s, t);
}

function md5ii(a, b, c, d, x, s, t) {
  return md5cmn(c ^ (b | ~d), a, b, x, s, t);
}

var v3 = v35('v3', 0x30, md5);

function v4(options, buf, offset) {
  options = options || {};
  var rnds = options.random || (options.rng || rng)(); // Per 4.4, set bits for version and `clock_seq_hi_and_reserved`

  rnds[6] = rnds[6] & 0x0f | 0x40;
  rnds[8] = rnds[8] & 0x3f | 0x80; // Copy bytes to buffer, if provided

  if (buf) {
    offset = offset || 0;

    for (var i = 0; i < 16; ++i) {
      buf[offset + i] = rnds[i];
    }

    return buf;
  }

  return stringify(rnds);
}

// Adapted from Chris Veness' SHA1 code at
// http://www.movable-type.co.uk/scripts/sha1.html
function f(s, x, y, z) {
  switch (s) {
    case 0:
      return x & y ^ ~x & z;

    case 1:
      return x ^ y ^ z;

    case 2:
      return x & y ^ x & z ^ y & z;

    case 3:
      return x ^ y ^ z;
  }
}

function ROTL(x, n) {
  return x << n | x >>> 32 - n;
}

function sha1(bytes) {
  var K = [0x5a827999, 0x6ed9eba1, 0x8f1bbcdc, 0xca62c1d6];
  var H = [0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476, 0xc3d2e1f0];

  if (typeof bytes === 'string') {
    var msg = unescape(encodeURIComponent(bytes)); // UTF8 escape

    bytes = [];

    for (var i = 0; i < msg.length; ++i) {
      bytes.push(msg.charCodeAt(i));
    }
  } else if (!Array.isArray(bytes)) {
    // Convert Array-like to Array
    bytes = Array.prototype.slice.call(bytes);
  }

  bytes.push(0x80);
  var l = bytes.length / 4 + 2;
  var N = Math.ceil(l / 16);
  var M = new Array(N);

  for (var _i = 0; _i < N; ++_i) {
    var arr = new Uint32Array(16);

    for (var j = 0; j < 16; ++j) {
      arr[j] = bytes[_i * 64 + j * 4] << 24 | bytes[_i * 64 + j * 4 + 1] << 16 | bytes[_i * 64 + j * 4 + 2] << 8 | bytes[_i * 64 + j * 4 + 3];
    }

    M[_i] = arr;
  }

  M[N - 1][14] = (bytes.length - 1) * 8 / Math.pow(2, 32);
  M[N - 1][14] = Math.floor(M[N - 1][14]);
  M[N - 1][15] = (bytes.length - 1) * 8 & 0xffffffff;

  for (var _i2 = 0; _i2 < N; ++_i2) {
    var W = new Uint32Array(80);

    for (var t = 0; t < 16; ++t) {
      W[t] = M[_i2][t];
    }

    for (var _t = 16; _t < 80; ++_t) {
      W[_t] = ROTL(W[_t - 3] ^ W[_t - 8] ^ W[_t - 14] ^ W[_t - 16], 1);
    }

    var a = H[0];
    var b = H[1];
    var c = H[2];
    var d = H[3];
    var e = H[4];

    for (var _t2 = 0; _t2 < 80; ++_t2) {
      var s = Math.floor(_t2 / 20);
      var T = ROTL(a, 5) + f(s, b, c, d) + e + K[s] + W[_t2] >>> 0;
      e = d;
      d = c;
      c = ROTL(b, 30) >>> 0;
      b = a;
      a = T;
    }

    H[0] = H[0] + a >>> 0;
    H[1] = H[1] + b >>> 0;
    H[2] = H[2] + c >>> 0;
    H[3] = H[3] + d >>> 0;
    H[4] = H[4] + e >>> 0;
  }

  return [H[0] >> 24 & 0xff, H[0] >> 16 & 0xff, H[0] >> 8 & 0xff, H[0] & 0xff, H[1] >> 24 & 0xff, H[1] >> 16 & 0xff, H[1] >> 8 & 0xff, H[1] & 0xff, H[2] >> 24 & 0xff, H[2] >> 16 & 0xff, H[2] >> 8 & 0xff, H[2] & 0xff, H[3] >> 24 & 0xff, H[3] >> 16 & 0xff, H[3] >> 8 & 0xff, H[3] & 0xff, H[4] >> 24 & 0xff, H[4] >> 16 & 0xff, H[4] >> 8 & 0xff, H[4] & 0xff];
}

var v5 = v35('v5', 0x50, sha1);

var nil = '00000000-0000-0000-0000-000000000000';

function version$2(uuid) {
  if (!validate(uuid)) {
    throw TypeError('Invalid UUID');
  }

  return parseInt(uuid.substr(14, 1), 16);
}

function commonjsRequire(path) {
	throw new Error('Could not dynamically require "' + path + '". Please configure the dynamicRequireTargets or/and ignoreDynamicRequires option of @rollup/plugin-commonjs appropriately for this require call to work.');
}

var _copyObject;
var hasRequired_copyObject;

function require_copyObject () {
	if (hasRequired_copyObject) return _copyObject;
	hasRequired_copyObject = 1;
	var assignValue = require_assignValue(),
	    baseAssignValue = require_baseAssignValue();

	/**
	 * Copies properties of `source` to `object`.
	 *
	 * @private
	 * @param {Object} source The object to copy properties from.
	 * @param {Array} props The property identifiers to copy.
	 * @param {Object} [object={}] The object to copy properties to.
	 * @param {Function} [customizer] The function to customize copied values.
	 * @returns {Object} Returns `object`.
	 */
	function copyObject(source, props, object, customizer) {
	  var isNew = !object;
	  object || (object = {});

	  var index = -1,
	      length = props.length;

	  while (++index < length) {
	    var key = props[index];

	    var newValue = customizer
	      ? customizer(object[key], source[key], key, object, source)
	      : undefined;

	    if (newValue === undefined) {
	      newValue = source[key];
	    }
	    if (isNew) {
	      baseAssignValue(object, key, newValue);
	    } else {
	      assignValue(object, key, newValue);
	    }
	  }
	  return object;
	}

	_copyObject = copyObject;
	return _copyObject;
}

var _baseAssign;
var hasRequired_baseAssign;

function require_baseAssign () {
	if (hasRequired_baseAssign) return _baseAssign;
	hasRequired_baseAssign = 1;
	var copyObject = require_copyObject(),
	    keys = requireKeys();

	/**
	 * The base implementation of `_.assign` without support for multiple sources
	 * or `customizer` functions.
	 *
	 * @private
	 * @param {Object} object The destination object.
	 * @param {Object} source The source object.
	 * @returns {Object} Returns `object`.
	 */
	function baseAssign(object, source) {
	  return object && copyObject(source, keys(source), object);
	}

	_baseAssign = baseAssign;
	return _baseAssign;
}

/**
 * This function is like
 * [`Object.keys`](http://ecma-international.org/ecma-262/7.0/#sec-object.keys)
 * except that it includes inherited enumerable properties.
 *
 * @private
 * @param {Object} object The object to query.
 * @returns {Array} Returns the array of property names.
 */

var _nativeKeysIn;
var hasRequired_nativeKeysIn;

function require_nativeKeysIn () {
	if (hasRequired_nativeKeysIn) return _nativeKeysIn;
	hasRequired_nativeKeysIn = 1;
	function nativeKeysIn(object) {
	  var result = [];
	  if (object != null) {
	    for (var key in Object(object)) {
	      result.push(key);
	    }
	  }
	  return result;
	}

	_nativeKeysIn = nativeKeysIn;
	return _nativeKeysIn;
}

var _baseKeysIn;
var hasRequired_baseKeysIn;

function require_baseKeysIn () {
	if (hasRequired_baseKeysIn) return _baseKeysIn;
	hasRequired_baseKeysIn = 1;
	var isObject = requireIsObject(),
	    isPrototype = require_isPrototype(),
	    nativeKeysIn = require_nativeKeysIn();

	/** Used for built-in method references. */
	var objectProto = Object.prototype;

	/** Used to check objects for own properties. */
	var hasOwnProperty = objectProto.hasOwnProperty;

	/**
	 * The base implementation of `_.keysIn` which doesn't treat sparse arrays as dense.
	 *
	 * @private
	 * @param {Object} object The object to query.
	 * @returns {Array} Returns the array of property names.
	 */
	function baseKeysIn(object) {
	  if (!isObject(object)) {
	    return nativeKeysIn(object);
	  }
	  var isProto = isPrototype(object),
	      result = [];

	  for (var key in object) {
	    if (!(key == 'constructor' && (isProto || !hasOwnProperty.call(object, key)))) {
	      result.push(key);
	    }
	  }
	  return result;
	}

	_baseKeysIn = baseKeysIn;
	return _baseKeysIn;
}

var keysIn_1;
var hasRequiredKeysIn;

function requireKeysIn () {
	if (hasRequiredKeysIn) return keysIn_1;
	hasRequiredKeysIn = 1;
	var arrayLikeKeys = require_arrayLikeKeys(),
	    baseKeysIn = require_baseKeysIn(),
	    isArrayLike = requireIsArrayLike();

	/**
	 * Creates an array of the own and inherited enumerable property names of `object`.
	 *
	 * **Note:** Non-object values are coerced to objects.
	 *
	 * @static
	 * @memberOf _
	 * @since 3.0.0
	 * @category Object
	 * @param {Object} object The object to query.
	 * @returns {Array} Returns the array of property names.
	 * @example
	 *
	 * function Foo() {
	 *   this.a = 1;
	 *   this.b = 2;
	 * }
	 *
	 * Foo.prototype.c = 3;
	 *
	 * _.keysIn(new Foo);
	 * // => ['a', 'b', 'c'] (iteration order is not guaranteed)
	 */
	function keysIn(object) {
	  return isArrayLike(object) ? arrayLikeKeys(object, true) : baseKeysIn(object);
	}

	keysIn_1 = keysIn;
	return keysIn_1;
}

var _baseAssignIn;
var hasRequired_baseAssignIn;

function require_baseAssignIn () {
	if (hasRequired_baseAssignIn) return _baseAssignIn;
	hasRequired_baseAssignIn = 1;
	var copyObject = require_copyObject(),
	    keysIn = requireKeysIn();

	/**
	 * The base implementation of `_.assignIn` without support for multiple sources
	 * or `customizer` functions.
	 *
	 * @private
	 * @param {Object} object The destination object.
	 * @param {Object} source The source object.
	 * @returns {Object} Returns `object`.
	 */
	function baseAssignIn(object, source) {
	  return object && copyObject(source, keysIn(source), object);
	}

	_baseAssignIn = baseAssignIn;
	return _baseAssignIn;
}

var _cloneBuffer$1 = {exports: {}};

var _cloneBuffer = _cloneBuffer$1.exports;

var hasRequired_cloneBuffer;

function require_cloneBuffer () {
	if (hasRequired_cloneBuffer) return _cloneBuffer$1.exports;
	hasRequired_cloneBuffer = 1;
	(function (module, exports) {
		var root = require_root();

		/** Detect free variable `exports`. */
		var freeExports = 'object' == 'object' && exports && !exports.nodeType && exports;

		/** Detect free variable `module`. */
		var freeModule = freeExports && 'object' == 'object' && module && !module.nodeType && module;

		/** Detect the popular CommonJS extension `module.exports`. */
		var moduleExports = freeModule && freeModule.exports === freeExports;

		/** Built-in value references. */
		var Buffer = moduleExports ? root.Buffer : undefined,
		    allocUnsafe = Buffer ? Buffer.allocUnsafe : undefined;

		/**
		 * Creates a clone of  `buffer`.
		 *
		 * @private
		 * @param {Buffer} buffer The buffer to clone.
		 * @param {boolean} [isDeep] Specify a deep clone.
		 * @returns {Buffer} Returns the cloned buffer.
		 */
		function cloneBuffer(buffer, isDeep) {
		  if (isDeep) {
		    return buffer.slice();
		  }
		  var length = buffer.length,
		      result = allocUnsafe ? allocUnsafe(length) : new buffer.constructor(length);

		  buffer.copy(result);
		  return result;
		}

		module.exports = cloneBuffer; 
	} (_cloneBuffer$1, _cloneBuffer$1.exports));
	return _cloneBuffer$1.exports;
}

var _copySymbols;
var hasRequired_copySymbols;

function require_copySymbols () {
	if (hasRequired_copySymbols) return _copySymbols;
	hasRequired_copySymbols = 1;
	var copyObject = require_copyObject(),
	    getSymbols = require_getSymbols();

	/**
	 * Copies own symbols of `source` to `object`.
	 *
	 * @private
	 * @param {Object} source The object to copy symbols from.
	 * @param {Object} [object={}] The object to copy symbols to.
	 * @returns {Object} Returns `object`.
	 */
	function copySymbols(source, object) {
	  return copyObject(source, getSymbols(source), object);
	}

	_copySymbols = copySymbols;
	return _copySymbols;
}

var _getPrototype;
var hasRequired_getPrototype;

function require_getPrototype () {
	if (hasRequired_getPrototype) return _getPrototype;
	hasRequired_getPrototype = 1;
	var overArg = require_overArg();

	/** Built-in value references. */
	var getPrototype = overArg(Object.getPrototypeOf, Object);

	_getPrototype = getPrototype;
	return _getPrototype;
}

var _getSymbolsIn;
var hasRequired_getSymbolsIn;

function require_getSymbolsIn () {
	if (hasRequired_getSymbolsIn) return _getSymbolsIn;
	hasRequired_getSymbolsIn = 1;
	var arrayPush = require_arrayPush(),
	    getPrototype = require_getPrototype(),
	    getSymbols = require_getSymbols(),
	    stubArray = requireStubArray();

	/* Built-in method references for those with the same name as other `lodash` methods. */
	var nativeGetSymbols = Object.getOwnPropertySymbols;

	/**
	 * Creates an array of the own and inherited enumerable symbols of `object`.
	 *
	 * @private
	 * @param {Object} object The object to query.
	 * @returns {Array} Returns the array of symbols.
	 */
	var getSymbolsIn = !nativeGetSymbols ? stubArray : function(object) {
	  var result = [];
	  while (object) {
	    arrayPush(result, getSymbols(object));
	    object = getPrototype(object);
	  }
	  return result;
	};

	_getSymbolsIn = getSymbolsIn;
	return _getSymbolsIn;
}

var _copySymbolsIn;
var hasRequired_copySymbolsIn;

function require_copySymbolsIn () {
	if (hasRequired_copySymbolsIn) return _copySymbolsIn;
	hasRequired_copySymbolsIn = 1;
	var copyObject = require_copyObject(),
	    getSymbolsIn = require_getSymbolsIn();

	/**
	 * Copies own and inherited symbols of `source` to `object`.
	 *
	 * @private
	 * @param {Object} source The object to copy symbols from.
	 * @param {Object} [object={}] The object to copy symbols to.
	 * @returns {Object} Returns `object`.
	 */
	function copySymbolsIn(source, object) {
	  return copyObject(source, getSymbolsIn(source), object);
	}

	_copySymbolsIn = copySymbolsIn;
	return _copySymbolsIn;
}

var _getAllKeysIn;
var hasRequired_getAllKeysIn;

function require_getAllKeysIn () {
	if (hasRequired_getAllKeysIn) return _getAllKeysIn;
	hasRequired_getAllKeysIn = 1;
	var baseGetAllKeys = require_baseGetAllKeys(),
	    getSymbolsIn = require_getSymbolsIn(),
	    keysIn = requireKeysIn();

	/**
	 * Creates an array of own and inherited enumerable property names and
	 * symbols of `object`.
	 *
	 * @private
	 * @param {Object} object The object to query.
	 * @returns {Array} Returns the array of property names and symbols.
	 */
	function getAllKeysIn(object) {
	  return baseGetAllKeys(object, keysIn, getSymbolsIn);
	}

	_getAllKeysIn = getAllKeysIn;
	return _getAllKeysIn;
}

/** Used for built-in method references. */

var _initCloneArray;
var hasRequired_initCloneArray;

function require_initCloneArray () {
	if (hasRequired_initCloneArray) return _initCloneArray;
	hasRequired_initCloneArray = 1;
	var objectProto = Object.prototype;

	/** Used to check objects for own properties. */
	var hasOwnProperty = objectProto.hasOwnProperty;

	/**
	 * Initializes an array clone.
	 *
	 * @private
	 * @param {Array} array The array to clone.
	 * @returns {Array} Returns the initialized clone.
	 */
	function initCloneArray(array) {
	  var length = array.length,
	      result = new array.constructor(length);

	  // Add properties assigned by `RegExp#exec`.
	  if (length && typeof array[0] == 'string' && hasOwnProperty.call(array, 'index')) {
	    result.index = array.index;
	    result.input = array.input;
	  }
	  return result;
	}

	_initCloneArray = initCloneArray;
	return _initCloneArray;
}

var _cloneArrayBuffer;
var hasRequired_cloneArrayBuffer;

function require_cloneArrayBuffer () {
	if (hasRequired_cloneArrayBuffer) return _cloneArrayBuffer;
	hasRequired_cloneArrayBuffer = 1;
	var Uint8Array = require_Uint8Array();

	/**
	 * Creates a clone of `arrayBuffer`.
	 *
	 * @private
	 * @param {ArrayBuffer} arrayBuffer The array buffer to clone.
	 * @returns {ArrayBuffer} Returns the cloned array buffer.
	 */
	function cloneArrayBuffer(arrayBuffer) {
	  var result = new arrayBuffer.constructor(arrayBuffer.byteLength);
	  new Uint8Array(result).set(new Uint8Array(arrayBuffer));
	  return result;
	}

	_cloneArrayBuffer = cloneArrayBuffer;
	return _cloneArrayBuffer;
}

var _cloneDataView;
var hasRequired_cloneDataView;

function require_cloneDataView () {
	if (hasRequired_cloneDataView) return _cloneDataView;
	hasRequired_cloneDataView = 1;
	var cloneArrayBuffer = require_cloneArrayBuffer();

	/**
	 * Creates a clone of `dataView`.
	 *
	 * @private
	 * @param {Object} dataView The data view to clone.
	 * @param {boolean} [isDeep] Specify a deep clone.
	 * @returns {Object} Returns the cloned data view.
	 */
	function cloneDataView(dataView, isDeep) {
	  var buffer = isDeep ? cloneArrayBuffer(dataView.buffer) : dataView.buffer;
	  return new dataView.constructor(buffer, dataView.byteOffset, dataView.byteLength);
	}

	_cloneDataView = cloneDataView;
	return _cloneDataView;
}

/** Used to match `RegExp` flags from their coerced string values. */

var _cloneRegExp;
var hasRequired_cloneRegExp;

function require_cloneRegExp () {
	if (hasRequired_cloneRegExp) return _cloneRegExp;
	hasRequired_cloneRegExp = 1;
	var reFlags = /\w*$/;

	/**
	 * Creates a clone of `regexp`.
	 *
	 * @private
	 * @param {Object} regexp The regexp to clone.
	 * @returns {Object} Returns the cloned regexp.
	 */
	function cloneRegExp(regexp) {
	  var result = new regexp.constructor(regexp.source, reFlags.exec(regexp));
	  result.lastIndex = regexp.lastIndex;
	  return result;
	}

	_cloneRegExp = cloneRegExp;
	return _cloneRegExp;
}

var _cloneSymbol;
var hasRequired_cloneSymbol;

function require_cloneSymbol () {
	if (hasRequired_cloneSymbol) return _cloneSymbol;
	hasRequired_cloneSymbol = 1;
	var Symbol = require_Symbol();

	/** Used to convert symbols to primitives and strings. */
	var symbolProto = Symbol ? Symbol.prototype : undefined,
	    symbolValueOf = symbolProto ? symbolProto.valueOf : undefined;

	/**
	 * Creates a clone of the `symbol` object.
	 *
	 * @private
	 * @param {Object} symbol The symbol object to clone.
	 * @returns {Object} Returns the cloned symbol object.
	 */
	function cloneSymbol(symbol) {
	  return symbolValueOf ? Object(symbolValueOf.call(symbol)) : {};
	}

	_cloneSymbol = cloneSymbol;
	return _cloneSymbol;
}

var _cloneTypedArray;
var hasRequired_cloneTypedArray;

function require_cloneTypedArray () {
	if (hasRequired_cloneTypedArray) return _cloneTypedArray;
	hasRequired_cloneTypedArray = 1;
	var cloneArrayBuffer = require_cloneArrayBuffer();

	/**
	 * Creates a clone of `typedArray`.
	 *
	 * @private
	 * @param {Object} typedArray The typed array to clone.
	 * @param {boolean} [isDeep] Specify a deep clone.
	 * @returns {Object} Returns the cloned typed array.
	 */
	function cloneTypedArray(typedArray, isDeep) {
	  var buffer = isDeep ? cloneArrayBuffer(typedArray.buffer) : typedArray.buffer;
	  return new typedArray.constructor(buffer, typedArray.byteOffset, typedArray.length);
	}

	_cloneTypedArray = cloneTypedArray;
	return _cloneTypedArray;
}

var _initCloneByTag;
var hasRequired_initCloneByTag;

function require_initCloneByTag () {
	if (hasRequired_initCloneByTag) return _initCloneByTag;
	hasRequired_initCloneByTag = 1;
	var cloneArrayBuffer = require_cloneArrayBuffer(),
	    cloneDataView = require_cloneDataView(),
	    cloneRegExp = require_cloneRegExp(),
	    cloneSymbol = require_cloneSymbol(),
	    cloneTypedArray = require_cloneTypedArray();

	/** `Object#toString` result references. */
	var boolTag = '[object Boolean]',
	    dateTag = '[object Date]',
	    mapTag = '[object Map]',
	    numberTag = '[object Number]',
	    regexpTag = '[object RegExp]',
	    setTag = '[object Set]',
	    stringTag = '[object String]',
	    symbolTag = '[object Symbol]';

	var arrayBufferTag = '[object ArrayBuffer]',
	    dataViewTag = '[object DataView]',
	    float32Tag = '[object Float32Array]',
	    float64Tag = '[object Float64Array]',
	    int8Tag = '[object Int8Array]',
	    int16Tag = '[object Int16Array]',
	    int32Tag = '[object Int32Array]',
	    uint8Tag = '[object Uint8Array]',
	    uint8ClampedTag = '[object Uint8ClampedArray]',
	    uint16Tag = '[object Uint16Array]',
	    uint32Tag = '[object Uint32Array]';

	/**
	 * Initializes an object clone based on its `toStringTag`.
	 *
	 * **Note:** This function only supports cloning values with tags of
	 * `Boolean`, `Date`, `Error`, `Map`, `Number`, `RegExp`, `Set`, or `String`.
	 *
	 * @private
	 * @param {Object} object The object to clone.
	 * @param {string} tag The `toStringTag` of the object to clone.
	 * @param {boolean} [isDeep] Specify a deep clone.
	 * @returns {Object} Returns the initialized clone.
	 */
	function initCloneByTag(object, tag, isDeep) {
	  var Ctor = object.constructor;
	  switch (tag) {
	    case arrayBufferTag:
	      return cloneArrayBuffer(object);

	    case boolTag:
	    case dateTag:
	      return new Ctor(+object);

	    case dataViewTag:
	      return cloneDataView(object, isDeep);

	    case float32Tag: case float64Tag:
	    case int8Tag: case int16Tag: case int32Tag:
	    case uint8Tag: case uint8ClampedTag: case uint16Tag: case uint32Tag:
	      return cloneTypedArray(object, isDeep);

	    case mapTag:
	      return new Ctor;

	    case numberTag:
	    case stringTag:
	      return new Ctor(object);

	    case regexpTag:
	      return cloneRegExp(object);

	    case setTag:
	      return new Ctor;

	    case symbolTag:
	      return cloneSymbol(object);
	  }
	}

	_initCloneByTag = initCloneByTag;
	return _initCloneByTag;
}

var _baseCreate;
var hasRequired_baseCreate;

function require_baseCreate () {
	if (hasRequired_baseCreate) return _baseCreate;
	hasRequired_baseCreate = 1;
	var isObject = requireIsObject();

	/** Built-in value references. */
	var objectCreate = Object.create;

	/**
	 * The base implementation of `_.create` without support for assigning
	 * properties to the created object.
	 *
	 * @private
	 * @param {Object} proto The object to inherit from.
	 * @returns {Object} Returns the new object.
	 */
	var baseCreate = (function() {
	  function object() {}
	  return function(proto) {
	    if (!isObject(proto)) {
	      return {};
	    }
	    if (objectCreate) {
	      return objectCreate(proto);
	    }
	    object.prototype = proto;
	    var result = new object;
	    object.prototype = undefined;
	    return result;
	  };
	}());

	_baseCreate = baseCreate;
	return _baseCreate;
}

var _initCloneObject;
var hasRequired_initCloneObject;

function require_initCloneObject () {
	if (hasRequired_initCloneObject) return _initCloneObject;
	hasRequired_initCloneObject = 1;
	var baseCreate = require_baseCreate(),
	    getPrototype = require_getPrototype(),
	    isPrototype = require_isPrototype();

	/**
	 * Initializes an object clone.
	 *
	 * @private
	 * @param {Object} object The object to clone.
	 * @returns {Object} Returns the initialized clone.
	 */
	function initCloneObject(object) {
	  return (typeof object.constructor == 'function' && !isPrototype(object))
	    ? baseCreate(getPrototype(object))
	    : {};
	}

	_initCloneObject = initCloneObject;
	return _initCloneObject;
}

var _baseIsMap;
var hasRequired_baseIsMap;

function require_baseIsMap () {
	if (hasRequired_baseIsMap) return _baseIsMap;
	hasRequired_baseIsMap = 1;
	var getTag = require_getTag(),
	    isObjectLike = requireIsObjectLike();

	/** `Object#toString` result references. */
	var mapTag = '[object Map]';

	/**
	 * The base implementation of `_.isMap` without Node.js optimizations.
	 *
	 * @private
	 * @param {*} value The value to check.
	 * @returns {boolean} Returns `true` if `value` is a map, else `false`.
	 */
	function baseIsMap(value) {
	  return isObjectLike(value) && getTag(value) == mapTag;
	}

	_baseIsMap = baseIsMap;
	return _baseIsMap;
}

var isMap_1;
var hasRequiredIsMap;

function requireIsMap () {
	if (hasRequiredIsMap) return isMap_1;
	hasRequiredIsMap = 1;
	var baseIsMap = require_baseIsMap(),
	    baseUnary = require_baseUnary(),
	    nodeUtil = require_nodeUtil();

	/* Node.js helper references. */
	var nodeIsMap = nodeUtil && nodeUtil.isMap;

	/**
	 * Checks if `value` is classified as a `Map` object.
	 *
	 * @static
	 * @memberOf _
	 * @since 4.3.0
	 * @category Lang
	 * @param {*} value The value to check.
	 * @returns {boolean} Returns `true` if `value` is a map, else `false`.
	 * @example
	 *
	 * _.isMap(new Map);
	 * // => true
	 *
	 * _.isMap(new WeakMap);
	 * // => false
	 */
	var isMap = nodeIsMap ? baseUnary(nodeIsMap) : baseIsMap;

	isMap_1 = isMap;
	return isMap_1;
}

var _baseIsSet;
var hasRequired_baseIsSet;

function require_baseIsSet () {
	if (hasRequired_baseIsSet) return _baseIsSet;
	hasRequired_baseIsSet = 1;
	var getTag = require_getTag(),
	    isObjectLike = requireIsObjectLike();

	/** `Object#toString` result references. */
	var setTag = '[object Set]';

	/**
	 * The base implementation of `_.isSet` without Node.js optimizations.
	 *
	 * @private
	 * @param {*} value The value to check.
	 * @returns {boolean} Returns `true` if `value` is a set, else `false`.
	 */
	function baseIsSet(value) {
	  return isObjectLike(value) && getTag(value) == setTag;
	}

	_baseIsSet = baseIsSet;
	return _baseIsSet;
}

var isSet_1;
var hasRequiredIsSet;

function requireIsSet () {
	if (hasRequiredIsSet) return isSet_1;
	hasRequiredIsSet = 1;
	var baseIsSet = require_baseIsSet(),
	    baseUnary = require_baseUnary(),
	    nodeUtil = require_nodeUtil();

	/* Node.js helper references. */
	var nodeIsSet = nodeUtil && nodeUtil.isSet;

	/**
	 * Checks if `value` is classified as a `Set` object.
	 *
	 * @static
	 * @memberOf _
	 * @since 4.3.0
	 * @category Lang
	 * @param {*} value The value to check.
	 * @returns {boolean} Returns `true` if `value` is a set, else `false`.
	 * @example
	 *
	 * _.isSet(new Set);
	 * // => true
	 *
	 * _.isSet(new WeakSet);
	 * // => false
	 */
	var isSet = nodeIsSet ? baseUnary(nodeIsSet) : baseIsSet;

	isSet_1 = isSet;
	return isSet_1;
}

var _baseClone;
var hasRequired_baseClone;

function require_baseClone () {
	if (hasRequired_baseClone) return _baseClone;
	hasRequired_baseClone = 1;
	var Stack = require_Stack(),
	    arrayEach = require_arrayEach(),
	    assignValue = require_assignValue(),
	    baseAssign = require_baseAssign(),
	    baseAssignIn = require_baseAssignIn(),
	    cloneBuffer = require_cloneBuffer(),
	    copyArray = require_copyArray(),
	    copySymbols = require_copySymbols(),
	    copySymbolsIn = require_copySymbolsIn(),
	    getAllKeys = require_getAllKeys(),
	    getAllKeysIn = require_getAllKeysIn(),
	    getTag = require_getTag(),
	    initCloneArray = require_initCloneArray(),
	    initCloneByTag = require_initCloneByTag(),
	    initCloneObject = require_initCloneObject(),
	    isArray = requireIsArray(),
	    isBuffer = requireIsBuffer(),
	    isMap = requireIsMap(),
	    isObject = requireIsObject(),
	    isSet = requireIsSet(),
	    keys = requireKeys(),
	    keysIn = requireKeysIn();

	/** Used to compose bitmasks for cloning. */
	var CLONE_DEEP_FLAG = 1,
	    CLONE_FLAT_FLAG = 2,
	    CLONE_SYMBOLS_FLAG = 4;

	/** `Object#toString` result references. */
	var argsTag = '[object Arguments]',
	    arrayTag = '[object Array]',
	    boolTag = '[object Boolean]',
	    dateTag = '[object Date]',
	    errorTag = '[object Error]',
	    funcTag = '[object Function]',
	    genTag = '[object GeneratorFunction]',
	    mapTag = '[object Map]',
	    numberTag = '[object Number]',
	    objectTag = '[object Object]',
	    regexpTag = '[object RegExp]',
	    setTag = '[object Set]',
	    stringTag = '[object String]',
	    symbolTag = '[object Symbol]',
	    weakMapTag = '[object WeakMap]';

	var arrayBufferTag = '[object ArrayBuffer]',
	    dataViewTag = '[object DataView]',
	    float32Tag = '[object Float32Array]',
	    float64Tag = '[object Float64Array]',
	    int8Tag = '[object Int8Array]',
	    int16Tag = '[object Int16Array]',
	    int32Tag = '[object Int32Array]',
	    uint8Tag = '[object Uint8Array]',
	    uint8ClampedTag = '[object Uint8ClampedArray]',
	    uint16Tag = '[object Uint16Array]',
	    uint32Tag = '[object Uint32Array]';

	/** Used to identify `toStringTag` values supported by `_.clone`. */
	var cloneableTags = {};
	cloneableTags[argsTag] = cloneableTags[arrayTag] =
	cloneableTags[arrayBufferTag] = cloneableTags[dataViewTag] =
	cloneableTags[boolTag] = cloneableTags[dateTag] =
	cloneableTags[float32Tag] = cloneableTags[float64Tag] =
	cloneableTags[int8Tag] = cloneableTags[int16Tag] =
	cloneableTags[int32Tag] = cloneableTags[mapTag] =
	cloneableTags[numberTag] = cloneableTags[objectTag] =
	cloneableTags[regexpTag] = cloneableTags[setTag] =
	cloneableTags[stringTag] = cloneableTags[symbolTag] =
	cloneableTags[uint8Tag] = cloneableTags[uint8ClampedTag] =
	cloneableTags[uint16Tag] = cloneableTags[uint32Tag] = true;
	cloneableTags[errorTag] = cloneableTags[funcTag] =
	cloneableTags[weakMapTag] = false;

	/**
	 * The base implementation of `_.clone` and `_.cloneDeep` which tracks
	 * traversed objects.
	 *
	 * @private
	 * @param {*} value The value to clone.
	 * @param {boolean} bitmask The bitmask flags.
	 *  1 - Deep clone
	 *  2 - Flatten inherited properties
	 *  4 - Clone symbols
	 * @param {Function} [customizer] The function to customize cloning.
	 * @param {string} [key] The key of `value`.
	 * @param {Object} [object] The parent object of `value`.
	 * @param {Object} [stack] Tracks traversed objects and their clone counterparts.
	 * @returns {*} Returns the cloned value.
	 */
	function baseClone(value, bitmask, customizer, key, object, stack) {
	  var result,
	      isDeep = bitmask & CLONE_DEEP_FLAG,
	      isFlat = bitmask & CLONE_FLAT_FLAG,
	      isFull = bitmask & CLONE_SYMBOLS_FLAG;

	  if (customizer) {
	    result = object ? customizer(value, key, object, stack) : customizer(value);
	  }
	  if (result !== undefined) {
	    return result;
	  }
	  if (!isObject(value)) {
	    return value;
	  }
	  var isArr = isArray(value);
	  if (isArr) {
	    result = initCloneArray(value);
	    if (!isDeep) {
	      return copyArray(value, result);
	    }
	  } else {
	    var tag = getTag(value),
	        isFunc = tag == funcTag || tag == genTag;

	    if (isBuffer(value)) {
	      return cloneBuffer(value, isDeep);
	    }
	    if (tag == objectTag || tag == argsTag || (isFunc && !object)) {
	      result = (isFlat || isFunc) ? {} : initCloneObject(value);
	      if (!isDeep) {
	        return isFlat
	          ? copySymbolsIn(value, baseAssignIn(result, value))
	          : copySymbols(value, baseAssign(result, value));
	      }
	    } else {
	      if (!cloneableTags[tag]) {
	        return object ? value : {};
	      }
	      result = initCloneByTag(value, tag, isDeep);
	    }
	  }
	  // Check for circular references and return its corresponding clone.
	  stack || (stack = new Stack);
	  var stacked = stack.get(value);
	  if (stacked) {
	    return stacked;
	  }
	  stack.set(value, result);

	  if (isSet(value)) {
	    value.forEach(function(subValue) {
	      result.add(baseClone(subValue, bitmask, customizer, subValue, value, stack));
	    });
	  } else if (isMap(value)) {
	    value.forEach(function(subValue, key) {
	      result.set(key, baseClone(subValue, bitmask, customizer, key, value, stack));
	    });
	  }

	  var keysFunc = isFull
	    ? (isFlat ? getAllKeysIn : getAllKeys)
	    : (isFlat ? keysIn : keys);

	  var props = isArr ? undefined : keysFunc(value);
	  arrayEach(props || value, function(subValue, key) {
	    if (props) {
	      key = subValue;
	      subValue = value[key];
	    }
	    // Recursively populate clone (susceptible to call stack limits).
	    assignValue(result, key, baseClone(subValue, bitmask, customizer, key, value, stack));
	  });
	  return result;
	}

	_baseClone = baseClone;
	return _baseClone;
}

var clone_1;
var hasRequiredClone;

function requireClone () {
	if (hasRequiredClone) return clone_1;
	hasRequiredClone = 1;
	var baseClone = require_baseClone();

	/** Used to compose bitmasks for cloning. */
	var CLONE_SYMBOLS_FLAG = 4;

	/**
	 * Creates a shallow clone of `value`.
	 *
	 * **Note:** This method is loosely based on the
	 * [structured clone algorithm](https://mdn.io/Structured_clone_algorithm)
	 * and supports cloning arrays, array buffers, booleans, date objects, maps,
	 * numbers, `Object` objects, regexes, sets, strings, symbols, and typed
	 * arrays. The own enumerable properties of `arguments` objects are cloned
	 * as plain objects. An empty object is returned for uncloneable values such
	 * as error objects, functions, DOM nodes, and WeakMaps.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.1.0
	 * @category Lang
	 * @param {*} value The value to clone.
	 * @returns {*} Returns the cloned value.
	 * @see _.cloneDeep
	 * @example
	 *
	 * var objects = [{ 'a': 1 }, { 'b': 2 }];
	 *
	 * var shallow = _.clone(objects);
	 * console.log(shallow[0] === objects[0]);
	 * // => true
	 */
	function clone(value) {
	  return baseClone(value, CLONE_SYMBOLS_FLAG);
	}

	clone_1 = clone;
	return clone_1;
}

/** Used for built-in method references. */

var _baseHas;
var hasRequired_baseHas;

function require_baseHas () {
	if (hasRequired_baseHas) return _baseHas;
	hasRequired_baseHas = 1;
	var objectProto = Object.prototype;

	/** Used to check objects for own properties. */
	var hasOwnProperty = objectProto.hasOwnProperty;

	/**
	 * The base implementation of `_.has` without support for deep paths.
	 *
	 * @private
	 * @param {Object} [object] The object to query.
	 * @param {Array|string} key The key to check.
	 * @returns {boolean} Returns `true` if `key` exists, else `false`.
	 */
	function baseHas(object, key) {
	  return object != null && hasOwnProperty.call(object, key);
	}

	_baseHas = baseHas;
	return _baseHas;
}

var has_1;
var hasRequiredHas;

function requireHas () {
	if (hasRequiredHas) return has_1;
	hasRequiredHas = 1;
	var baseHas = require_baseHas(),
	    hasPath = require_hasPath();

	/**
	 * Checks if `path` is a direct property of `object`.
	 *
	 * @static
	 * @since 0.1.0
	 * @memberOf _
	 * @category Object
	 * @param {Object} object The object to query.
	 * @param {Array|string} path The path to check.
	 * @returns {boolean} Returns `true` if `path` exists, else `false`.
	 * @example
	 *
	 * var object = { 'a': { 'b': 2 } };
	 * var other = _.create({ 'a': _.create({ 'b': 2 }) });
	 *
	 * _.has(object, 'a');
	 * // => true
	 *
	 * _.has(object, 'a.b');
	 * // => true
	 *
	 * _.has(object, ['a', 'b']);
	 * // => true
	 *
	 * _.has(other, 'a');
	 * // => false
	 */
	function has(object, path) {
	  return object != null && hasPath(object, path, baseHas);
	}

	has_1 = has;
	return has_1;
}

var isEmpty_1;
var hasRequiredIsEmpty;

function requireIsEmpty () {
	if (hasRequiredIsEmpty) return isEmpty_1;
	hasRequiredIsEmpty = 1;
	var baseKeys = require_baseKeys(),
	    getTag = require_getTag(),
	    isArguments = requireIsArguments(),
	    isArray = requireIsArray(),
	    isArrayLike = requireIsArrayLike(),
	    isBuffer = requireIsBuffer(),
	    isPrototype = require_isPrototype(),
	    isTypedArray = requireIsTypedArray();

	/** `Object#toString` result references. */
	var mapTag = '[object Map]',
	    setTag = '[object Set]';

	/** Used for built-in method references. */
	var objectProto = Object.prototype;

	/** Used to check objects for own properties. */
	var hasOwnProperty = objectProto.hasOwnProperty;

	/**
	 * Checks if `value` is an empty object, collection, map, or set.
	 *
	 * Objects are considered empty if they have no own enumerable string keyed
	 * properties.
	 *
	 * Array-like values such as `arguments` objects, arrays, buffers, strings, or
	 * jQuery-like collections are considered empty if they have a `length` of `0`.
	 * Similarly, maps and sets are considered empty if they have a `size` of `0`.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.1.0
	 * @category Lang
	 * @param {*} value The value to check.
	 * @returns {boolean} Returns `true` if `value` is empty, else `false`.
	 * @example
	 *
	 * _.isEmpty(null);
	 * // => true
	 *
	 * _.isEmpty(true);
	 * // => true
	 *
	 * _.isEmpty(1);
	 * // => true
	 *
	 * _.isEmpty([1, 2, 3]);
	 * // => false
	 *
	 * _.isEmpty({ 'a': 1 });
	 * // => false
	 */
	function isEmpty(value) {
	  if (value == null) {
	    return true;
	  }
	  if (isArrayLike(value) &&
	      (isArray(value) || typeof value == 'string' || typeof value.splice == 'function' ||
	        isBuffer(value) || isTypedArray(value) || isArguments(value))) {
	    return !value.length;
	  }
	  var tag = getTag(value);
	  if (tag == mapTag || tag == setTag) {
	    return !value.size;
	  }
	  if (isPrototype(value)) {
	    return !baseKeys(value).length;
	  }
	  for (var key in value) {
	    if (hasOwnProperty.call(value, key)) {
	      return false;
	    }
	  }
	  return true;
	}

	isEmpty_1 = isEmpty;
	return isEmpty_1;
}

/**
 * Checks if `value` is `undefined`.
 *
 * @static
 * @since 0.1.0
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is `undefined`, else `false`.
 * @example
 *
 * _.isUndefined(void 0);
 * // => true
 *
 * _.isUndefined(null);
 * // => false
 */

var isUndefined_1;
var hasRequiredIsUndefined;

function requireIsUndefined () {
	if (hasRequiredIsUndefined) return isUndefined_1;
	hasRequiredIsUndefined = 1;
	function isUndefined(value) {
	  return value === undefined;
	}

	isUndefined_1 = isUndefined;
	return isUndefined_1;
}

var transform_1;
var hasRequiredTransform;

function requireTransform () {
	if (hasRequiredTransform) return transform_1;
	hasRequiredTransform = 1;
	var arrayEach = require_arrayEach(),
	    baseCreate = require_baseCreate(),
	    baseForOwn = require_baseForOwn(),
	    baseIteratee = require_baseIteratee(),
	    getPrototype = require_getPrototype(),
	    isArray = requireIsArray(),
	    isBuffer = requireIsBuffer(),
	    isFunction = requireIsFunction(),
	    isObject = requireIsObject(),
	    isTypedArray = requireIsTypedArray();

	/**
	 * An alternative to `_.reduce`; this method transforms `object` to a new
	 * `accumulator` object which is the result of running each of its own
	 * enumerable string keyed properties thru `iteratee`, with each invocation
	 * potentially mutating the `accumulator` object. If `accumulator` is not
	 * provided, a new object with the same `[[Prototype]]` will be used. The
	 * iteratee is invoked with four arguments: (accumulator, value, key, object).
	 * Iteratee functions may exit iteration early by explicitly returning `false`.
	 *
	 * @static
	 * @memberOf _
	 * @since 1.3.0
	 * @category Object
	 * @param {Object} object The object to iterate over.
	 * @param {Function} [iteratee=_.identity] The function invoked per iteration.
	 * @param {*} [accumulator] The custom accumulator value.
	 * @returns {*} Returns the accumulated value.
	 * @example
	 *
	 * _.transform([2, 3, 4], function(result, n) {
	 *   result.push(n *= n);
	 *   return n % 2 == 0;
	 * }, []);
	 * // => [4, 9]
	 *
	 * _.transform({ 'a': 1, 'b': 2, 'c': 1 }, function(result, value, key) {
	 *   (result[value] || (result[value] = [])).push(key);
	 * }, {});
	 * // => { '1': ['a', 'c'], '2': ['b'] }
	 */
	function transform(object, iteratee, accumulator) {
	  var isArr = isArray(object),
	      isArrLike = isArr || isBuffer(object) || isTypedArray(object);

	  iteratee = baseIteratee(iteratee, 4);
	  if (accumulator == null) {
	    var Ctor = object && object.constructor;
	    if (isArrLike) {
	      accumulator = isArr ? new Ctor : [];
	    }
	    else if (isObject(object)) {
	      accumulator = isFunction(Ctor) ? baseCreate(getPrototype(object)) : {};
	    }
	    else {
	      accumulator = {};
	    }
	  }
	  (isArrLike ? arrayEach : baseForOwn)(object, function(value, index, object) {
	    return iteratee(accumulator, value, index, object);
	  });
	  return accumulator;
	}

	transform_1 = transform;
	return transform_1;
}

/* global window */

var lodash_1$1;
var hasRequiredLodash$1;

function requireLodash$1 () {
	if (hasRequiredLodash$1) return lodash_1$1;
	hasRequiredLodash$1 = 1;
	var lodash;

	if (typeof commonjsRequire === "function") {
	  try {
	    lodash = {
	      clone: requireClone(),
	      constant: requireConstant(),
	      each: requireEach(),
	      filter: requireFilter(),
	      has:  requireHas(),
	      isArray: requireIsArray(),
	      isEmpty: requireIsEmpty(),
	      isFunction: requireIsFunction(),
	      isUndefined: requireIsUndefined(),
	      keys: requireKeys(),
	      map: requireMap(),
	      reduce: requireReduce(),
	      size: requireSize(),
	      transform: requireTransform(),
	      union: requireUnion(),
	      values: requireValues()
	    };
	  } catch (e) {
	    // continue regardless of error
	  }
	}

	if (!lodash) {
	  lodash = window._;
	}

	lodash_1$1 = lodash;
	return lodash_1$1;
}

var graph;
var hasRequiredGraph;

function requireGraph () {
	if (hasRequiredGraph) return graph;
	hasRequiredGraph = 1;
	"use strict";

	var _ = requireLodash$1();

	graph = Graph;

	var DEFAULT_EDGE_NAME = "\x00";
	var GRAPH_NODE = "\x00";
	var EDGE_KEY_DELIM = "\x01";

	// Implementation notes:
	//
	//  * Node id query functions should return string ids for the nodes
	//  * Edge id query functions should return an "edgeObj", edge object, that is
	//    composed of enough information to uniquely identify an edge: {v, w, name}.
	//  * Internally we use an "edgeId", a stringified form of the edgeObj, to
	//    reference edges. This is because we need a performant way to look these
	//    edges up and, object properties, which have string keys, are the closest
	//    we're going to get to a performant hashtable in JavaScript.

	function Graph(opts) {
	  this._isDirected = _.has(opts, "directed") ? opts.directed : true;
	  this._isMultigraph = _.has(opts, "multigraph") ? opts.multigraph : false;
	  this._isCompound = _.has(opts, "compound") ? opts.compound : false;

	  // Label for the graph itself
	  this._label = undefined;

	  // Defaults to be set when creating a new node
	  this._defaultNodeLabelFn = _.constant(undefined);

	  // Defaults to be set when creating a new edge
	  this._defaultEdgeLabelFn = _.constant(undefined);

	  // v -> label
	  this._nodes = {};

	  if (this._isCompound) {
	    // v -> parent
	    this._parent = {};

	    // v -> children
	    this._children = {};
	    this._children[GRAPH_NODE] = {};
	  }

	  // v -> edgeObj
	  this._in = {};

	  // u -> v -> Number
	  this._preds = {};

	  // v -> edgeObj
	  this._out = {};

	  // v -> w -> Number
	  this._sucs = {};

	  // e -> edgeObj
	  this._edgeObjs = {};

	  // e -> label
	  this._edgeLabels = {};
	}

	/* Number of nodes in the graph. Should only be changed by the implementation. */
	Graph.prototype._nodeCount = 0;

	/* Number of edges in the graph. Should only be changed by the implementation. */
	Graph.prototype._edgeCount = 0;


	/* === Graph functions ========= */

	Graph.prototype.isDirected = function() {
	  return this._isDirected;
	};

	Graph.prototype.isMultigraph = function() {
	  return this._isMultigraph;
	};

	Graph.prototype.isCompound = function() {
	  return this._isCompound;
	};

	Graph.prototype.setGraph = function(label) {
	  this._label = label;
	  return this;
	};

	Graph.prototype.graph = function() {
	  return this._label;
	};


	/* === Node functions ========== */

	Graph.prototype.setDefaultNodeLabel = function(newDefault) {
	  if (!_.isFunction(newDefault)) {
	    newDefault = _.constant(newDefault);
	  }
	  this._defaultNodeLabelFn = newDefault;
	  return this;
	};

	Graph.prototype.nodeCount = function() {
	  return this._nodeCount;
	};

	Graph.prototype.nodes = function() {
	  return _.keys(this._nodes);
	};

	Graph.prototype.sources = function() {
	  var self = this;
	  return _.filter(this.nodes(), function(v) {
	    return _.isEmpty(self._in[v]);
	  });
	};

	Graph.prototype.sinks = function() {
	  var self = this;
	  return _.filter(this.nodes(), function(v) {
	    return _.isEmpty(self._out[v]);
	  });
	};

	Graph.prototype.setNodes = function(vs, value) {
	  var args = arguments;
	  var self = this;
	  _.each(vs, function(v) {
	    if (args.length > 1) {
	      self.setNode(v, value);
	    } else {
	      self.setNode(v);
	    }
	  });
	  return this;
	};

	Graph.prototype.setNode = function(v, value) {
	  if (_.has(this._nodes, v)) {
	    if (arguments.length > 1) {
	      this._nodes[v] = value;
	    }
	    return this;
	  }

	  this._nodes[v] = arguments.length > 1 ? value : this._defaultNodeLabelFn(v);
	  if (this._isCompound) {
	    this._parent[v] = GRAPH_NODE;
	    this._children[v] = {};
	    this._children[GRAPH_NODE][v] = true;
	  }
	  this._in[v] = {};
	  this._preds[v] = {};
	  this._out[v] = {};
	  this._sucs[v] = {};
	  ++this._nodeCount;
	  return this;
	};

	Graph.prototype.node = function(v) {
	  return this._nodes[v];
	};

	Graph.prototype.hasNode = function(v) {
	  return _.has(this._nodes, v);
	};

	Graph.prototype.removeNode =  function(v) {
	  var self = this;
	  if (_.has(this._nodes, v)) {
	    var removeEdge = function(e) { self.removeEdge(self._edgeObjs[e]); };
	    delete this._nodes[v];
	    if (this._isCompound) {
	      this._removeFromParentsChildList(v);
	      delete this._parent[v];
	      _.each(this.children(v), function(child) {
	        self.setParent(child);
	      });
	      delete this._children[v];
	    }
	    _.each(_.keys(this._in[v]), removeEdge);
	    delete this._in[v];
	    delete this._preds[v];
	    _.each(_.keys(this._out[v]), removeEdge);
	    delete this._out[v];
	    delete this._sucs[v];
	    --this._nodeCount;
	  }
	  return this;
	};

	Graph.prototype.setParent = function(v, parent) {
	  if (!this._isCompound) {
	    throw new Error("Cannot set parent in a non-compound graph");
	  }

	  if (_.isUndefined(parent)) {
	    parent = GRAPH_NODE;
	  } else {
	    // Coerce parent to string
	    parent += "";
	    for (var ancestor = parent;
	      !_.isUndefined(ancestor);
	      ancestor = this.parent(ancestor)) {
	      if (ancestor === v) {
	        throw new Error("Setting " + parent+ " as parent of " + v +
	                        " would create a cycle");
	      }
	    }

	    this.setNode(parent);
	  }

	  this.setNode(v);
	  this._removeFromParentsChildList(v);
	  this._parent[v] = parent;
	  this._children[parent][v] = true;
	  return this;
	};

	Graph.prototype._removeFromParentsChildList = function(v) {
	  delete this._children[this._parent[v]][v];
	};

	Graph.prototype.parent = function(v) {
	  if (this._isCompound) {
	    var parent = this._parent[v];
	    if (parent !== GRAPH_NODE) {
	      return parent;
	    }
	  }
	};

	Graph.prototype.children = function(v) {
	  if (_.isUndefined(v)) {
	    v = GRAPH_NODE;
	  }

	  if (this._isCompound) {
	    var children = this._children[v];
	    if (children) {
	      return _.keys(children);
	    }
	  } else if (v === GRAPH_NODE) {
	    return this.nodes();
	  } else if (this.hasNode(v)) {
	    return [];
	  }
	};

	Graph.prototype.predecessors = function(v) {
	  var predsV = this._preds[v];
	  if (predsV) {
	    return _.keys(predsV);
	  }
	};

	Graph.prototype.successors = function(v) {
	  var sucsV = this._sucs[v];
	  if (sucsV) {
	    return _.keys(sucsV);
	  }
	};

	Graph.prototype.neighbors = function(v) {
	  var preds = this.predecessors(v);
	  if (preds) {
	    return _.union(preds, this.successors(v));
	  }
	};

	Graph.prototype.isLeaf = function (v) {
	  var neighbors;
	  if (this.isDirected()) {
	    neighbors = this.successors(v);
	  } else {
	    neighbors = this.neighbors(v);
	  }
	  return neighbors.length === 0;
	};

	Graph.prototype.filterNodes = function(filter) {
	  var copy = new this.constructor({
	    directed: this._isDirected,
	    multigraph: this._isMultigraph,
	    compound: this._isCompound
	  });

	  copy.setGraph(this.graph());

	  var self = this;
	  _.each(this._nodes, function(value, v) {
	    if (filter(v)) {
	      copy.setNode(v, value);
	    }
	  });

	  _.each(this._edgeObjs, function(e) {
	    if (copy.hasNode(e.v) && copy.hasNode(e.w)) {
	      copy.setEdge(e, self.edge(e));
	    }
	  });

	  var parents = {};
	  function findParent(v) {
	    var parent = self.parent(v);
	    if (parent === undefined || copy.hasNode(parent)) {
	      parents[v] = parent;
	      return parent;
	    } else if (parent in parents) {
	      return parents[parent];
	    } else {
	      return findParent(parent);
	    }
	  }

	  if (this._isCompound) {
	    _.each(copy.nodes(), function(v) {
	      copy.setParent(v, findParent(v));
	    });
	  }

	  return copy;
	};

	/* === Edge functions ========== */

	Graph.prototype.setDefaultEdgeLabel = function(newDefault) {
	  if (!_.isFunction(newDefault)) {
	    newDefault = _.constant(newDefault);
	  }
	  this._defaultEdgeLabelFn = newDefault;
	  return this;
	};

	Graph.prototype.edgeCount = function() {
	  return this._edgeCount;
	};

	Graph.prototype.edges = function() {
	  return _.values(this._edgeObjs);
	};

	Graph.prototype.setPath = function(vs, value) {
	  var self = this;
	  var args = arguments;
	  _.reduce(vs, function(v, w) {
	    if (args.length > 1) {
	      self.setEdge(v, w, value);
	    } else {
	      self.setEdge(v, w);
	    }
	    return w;
	  });
	  return this;
	};

	/*
	 * setEdge(v, w, [value, [name]])
	 * setEdge({ v, w, [name] }, [value])
	 */
	Graph.prototype.setEdge = function() {
	  var v, w, name, value;
	  var valueSpecified = false;
	  var arg0 = arguments[0];

	  if (typeof arg0 === "object" && arg0 !== null && "v" in arg0) {
	    v = arg0.v;
	    w = arg0.w;
	    name = arg0.name;
	    if (arguments.length === 2) {
	      value = arguments[1];
	      valueSpecified = true;
	    }
	  } else {
	    v = arg0;
	    w = arguments[1];
	    name = arguments[3];
	    if (arguments.length > 2) {
	      value = arguments[2];
	      valueSpecified = true;
	    }
	  }

	  v = "" + v;
	  w = "" + w;
	  if (!_.isUndefined(name)) {
	    name = "" + name;
	  }

	  var e = edgeArgsToId(this._isDirected, v, w, name);
	  if (_.has(this._edgeLabels, e)) {
	    if (valueSpecified) {
	      this._edgeLabels[e] = value;
	    }
	    return this;
	  }

	  if (!_.isUndefined(name) && !this._isMultigraph) {
	    throw new Error("Cannot set a named edge when isMultigraph = false");
	  }

	  // It didn't exist, so we need to create it.
	  // First ensure the nodes exist.
	  this.setNode(v);
	  this.setNode(w);

	  this._edgeLabels[e] = valueSpecified ? value : this._defaultEdgeLabelFn(v, w, name);

	  var edgeObj = edgeArgsToObj(this._isDirected, v, w, name);
	  // Ensure we add undirected edges in a consistent way.
	  v = edgeObj.v;
	  w = edgeObj.w;

	  Object.freeze(edgeObj);
	  this._edgeObjs[e] = edgeObj;
	  incrementOrInitEntry(this._preds[w], v);
	  incrementOrInitEntry(this._sucs[v], w);
	  this._in[w][e] = edgeObj;
	  this._out[v][e] = edgeObj;
	  this._edgeCount++;
	  return this;
	};

	Graph.prototype.edge = function(v, w, name) {
	  var e = (arguments.length === 1
	    ? edgeObjToId(this._isDirected, arguments[0])
	    : edgeArgsToId(this._isDirected, v, w, name));
	  return this._edgeLabels[e];
	};

	Graph.prototype.hasEdge = function(v, w, name) {
	  var e = (arguments.length === 1
	    ? edgeObjToId(this._isDirected, arguments[0])
	    : edgeArgsToId(this._isDirected, v, w, name));
	  return _.has(this._edgeLabels, e);
	};

	Graph.prototype.removeEdge = function(v, w, name) {
	  var e = (arguments.length === 1
	    ? edgeObjToId(this._isDirected, arguments[0])
	    : edgeArgsToId(this._isDirected, v, w, name));
	  var edge = this._edgeObjs[e];
	  if (edge) {
	    v = edge.v;
	    w = edge.w;
	    delete this._edgeLabels[e];
	    delete this._edgeObjs[e];
	    decrementOrRemoveEntry(this._preds[w], v);
	    decrementOrRemoveEntry(this._sucs[v], w);
	    delete this._in[w][e];
	    delete this._out[v][e];
	    this._edgeCount--;
	  }
	  return this;
	};

	Graph.prototype.inEdges = function(v, u) {
	  var inV = this._in[v];
	  if (inV) {
	    var edges = _.values(inV);
	    if (!u) {
	      return edges;
	    }
	    return _.filter(edges, function(edge) { return edge.v === u; });
	  }
	};

	Graph.prototype.outEdges = function(v, w) {
	  var outV = this._out[v];
	  if (outV) {
	    var edges = _.values(outV);
	    if (!w) {
	      return edges;
	    }
	    return _.filter(edges, function(edge) { return edge.w === w; });
	  }
	};

	Graph.prototype.nodeEdges = function(v, w) {
	  var inEdges = this.inEdges(v, w);
	  if (inEdges) {
	    return inEdges.concat(this.outEdges(v, w));
	  }
	};

	function incrementOrInitEntry(map, k) {
	  if (map[k]) {
	    map[k]++;
	  } else {
	    map[k] = 1;
	  }
	}

	function decrementOrRemoveEntry(map, k) {
	  if (!--map[k]) { delete map[k]; }
	}

	function edgeArgsToId(isDirected, v_, w_, name) {
	  var v = "" + v_;
	  var w = "" + w_;
	  if (!isDirected && v > w) {
	    var tmp = v;
	    v = w;
	    w = tmp;
	  }
	  return v + EDGE_KEY_DELIM + w + EDGE_KEY_DELIM +
	             (_.isUndefined(name) ? DEFAULT_EDGE_NAME : name);
	}

	function edgeArgsToObj(isDirected, v_, w_, name) {
	  var v = "" + v_;
	  var w = "" + w_;
	  if (!isDirected && v > w) {
	    var tmp = v;
	    v = w;
	    w = tmp;
	  }
	  var edgeObj =  { v: v, w: w };
	  if (name) {
	    edgeObj.name = name;
	  }
	  return edgeObj;
	}

	function edgeObjToId(isDirected, edgeObj) {
	  return edgeArgsToId(isDirected, edgeObj.v, edgeObj.w, edgeObj.name);
	}
	return graph;
}

var version$1;
var hasRequiredVersion$1;

function requireVersion$1 () {
	if (hasRequiredVersion$1) return version$1;
	hasRequiredVersion$1 = 1;
	version$1 = '2.1.8';
	return version$1;
}

var lib;
var hasRequiredLib;

function requireLib () {
	if (hasRequiredLib) return lib;
	hasRequiredLib = 1;
	// Includes only the "core" of graphlib
	lib = {
	  Graph: requireGraph(),
	  version: requireVersion$1()
	};
	return lib;
}

var json;
var hasRequiredJson;

function requireJson () {
	if (hasRequiredJson) return json;
	hasRequiredJson = 1;
	var _ = requireLodash$1();
	var Graph = requireGraph();

	json = {
	  write: write,
	  read: read
	};

	function write(g) {
	  var json = {
	    options: {
	      directed: g.isDirected(),
	      multigraph: g.isMultigraph(),
	      compound: g.isCompound()
	    },
	    nodes: writeNodes(g),
	    edges: writeEdges(g)
	  };
	  if (!_.isUndefined(g.graph())) {
	    json.value = _.clone(g.graph());
	  }
	  return json;
	}

	function writeNodes(g) {
	  return _.map(g.nodes(), function(v) {
	    var nodeValue = g.node(v);
	    var parent = g.parent(v);
	    var node = { v: v };
	    if (!_.isUndefined(nodeValue)) {
	      node.value = nodeValue;
	    }
	    if (!_.isUndefined(parent)) {
	      node.parent = parent;
	    }
	    return node;
	  });
	}

	function writeEdges(g) {
	  return _.map(g.edges(), function(e) {
	    var edgeValue = g.edge(e);
	    var edge = { v: e.v, w: e.w };
	    if (!_.isUndefined(e.name)) {
	      edge.name = e.name;
	    }
	    if (!_.isUndefined(edgeValue)) {
	      edge.value = edgeValue;
	    }
	    return edge;
	  });
	}

	function read(json) {
	  var g = new Graph(json.options).setGraph(json.value);
	  _.each(json.nodes, function(entry) {
	    g.setNode(entry.v, entry.value);
	    if (entry.parent) {
	      g.setParent(entry.v, entry.parent);
	    }
	  });
	  _.each(json.edges, function(entry) {
	    g.setEdge({ v: entry.v, w: entry.w, name: entry.name }, entry.value);
	  });
	  return g;
	}
	return json;
}

var components_1;
var hasRequiredComponents;

function requireComponents () {
	if (hasRequiredComponents) return components_1;
	hasRequiredComponents = 1;
	var _ = requireLodash$1();

	components_1 = components;

	function components(g) {
	  var visited = {};
	  var cmpts = [];
	  var cmpt;

	  function dfs(v) {
	    if (_.has(visited, v)) return;
	    visited[v] = true;
	    cmpt.push(v);
	    _.each(g.successors(v), dfs);
	    _.each(g.predecessors(v), dfs);
	  }

	  _.each(g.nodes(), function(v) {
	    cmpt = [];
	    dfs(v);
	    if (cmpt.length) {
	      cmpts.push(cmpt);
	    }
	  });

	  return cmpts;
	}
	return components_1;
}

var priorityQueue;
var hasRequiredPriorityQueue;

function requirePriorityQueue () {
	if (hasRequiredPriorityQueue) return priorityQueue;
	hasRequiredPriorityQueue = 1;
	var _ = requireLodash$1();

	priorityQueue = PriorityQueue;

	/**
	 * A min-priority queue data structure. This algorithm is derived from Cormen,
	 * et al., "Introduction to Algorithms". The basic idea of a min-priority
	 * queue is that you can efficiently (in O(1) time) get the smallest key in
	 * the queue. Adding and removing elements takes O(log n) time. A key can
	 * have its priority decreased in O(log n) time.
	 */
	function PriorityQueue() {
	  this._arr = [];
	  this._keyIndices = {};
	}

	/**
	 * Returns the number of elements in the queue. Takes `O(1)` time.
	 */
	PriorityQueue.prototype.size = function() {
	  return this._arr.length;
	};

	/**
	 * Returns the keys that are in the queue. Takes `O(n)` time.
	 */
	PriorityQueue.prototype.keys = function() {
	  return this._arr.map(function(x) { return x.key; });
	};

	/**
	 * Returns `true` if **key** is in the queue and `false` if not.
	 */
	PriorityQueue.prototype.has = function(key) {
	  return _.has(this._keyIndices, key);
	};

	/**
	 * Returns the priority for **key**. If **key** is not present in the queue
	 * then this function returns `undefined`. Takes `O(1)` time.
	 *
	 * @param {Object} key
	 */
	PriorityQueue.prototype.priority = function(key) {
	  var index = this._keyIndices[key];
	  if (index !== undefined) {
	    return this._arr[index].priority;
	  }
	};

	/**
	 * Returns the key for the minimum element in this queue. If the queue is
	 * empty this function throws an Error. Takes `O(1)` time.
	 */
	PriorityQueue.prototype.min = function() {
	  if (this.size() === 0) {
	    throw new Error("Queue underflow");
	  }
	  return this._arr[0].key;
	};

	/**
	 * Inserts a new key into the priority queue. If the key already exists in
	 * the queue this function returns `false`; otherwise it will return `true`.
	 * Takes `O(n)` time.
	 *
	 * @param {Object} key the key to add
	 * @param {Number} priority the initial priority for the key
	 */
	PriorityQueue.prototype.add = function(key, priority) {
	  var keyIndices = this._keyIndices;
	  key = String(key);
	  if (!_.has(keyIndices, key)) {
	    var arr = this._arr;
	    var index = arr.length;
	    keyIndices[key] = index;
	    arr.push({key: key, priority: priority});
	    this._decrease(index);
	    return true;
	  }
	  return false;
	};

	/**
	 * Removes and returns the smallest key in the queue. Takes `O(log n)` time.
	 */
	PriorityQueue.prototype.removeMin = function() {
	  this._swap(0, this._arr.length - 1);
	  var min = this._arr.pop();
	  delete this._keyIndices[min.key];
	  this._heapify(0);
	  return min.key;
	};

	/**
	 * Decreases the priority for **key** to **priority**. If the new priority is
	 * greater than the previous priority, this function will throw an Error.
	 *
	 * @param {Object} key the key for which to raise priority
	 * @param {Number} priority the new priority for the key
	 */
	PriorityQueue.prototype.decrease = function(key, priority) {
	  var index = this._keyIndices[key];
	  if (priority > this._arr[index].priority) {
	    throw new Error("New priority is greater than current priority. " +
	        "Key: " + key + " Old: " + this._arr[index].priority + " New: " + priority);
	  }
	  this._arr[index].priority = priority;
	  this._decrease(index);
	};

	PriorityQueue.prototype._heapify = function(i) {
	  var arr = this._arr;
	  var l = 2 * i;
	  var r = l + 1;
	  var largest = i;
	  if (l < arr.length) {
	    largest = arr[l].priority < arr[largest].priority ? l : largest;
	    if (r < arr.length) {
	      largest = arr[r].priority < arr[largest].priority ? r : largest;
	    }
	    if (largest !== i) {
	      this._swap(i, largest);
	      this._heapify(largest);
	    }
	  }
	};

	PriorityQueue.prototype._decrease = function(index) {
	  var arr = this._arr;
	  var priority = arr[index].priority;
	  var parent;
	  while (index !== 0) {
	    parent = index >> 1;
	    if (arr[parent].priority < priority) {
	      break;
	    }
	    this._swap(index, parent);
	    index = parent;
	  }
	};

	PriorityQueue.prototype._swap = function(i, j) {
	  var arr = this._arr;
	  var keyIndices = this._keyIndices;
	  var origArrI = arr[i];
	  var origArrJ = arr[j];
	  arr[i] = origArrJ;
	  arr[j] = origArrI;
	  keyIndices[origArrJ.key] = i;
	  keyIndices[origArrI.key] = j;
	};
	return priorityQueue;
}

var dijkstra_1;
var hasRequiredDijkstra;

function requireDijkstra () {
	if (hasRequiredDijkstra) return dijkstra_1;
	hasRequiredDijkstra = 1;
	var _ = requireLodash$1();
	var PriorityQueue = requirePriorityQueue();

	dijkstra_1 = dijkstra;

	var DEFAULT_WEIGHT_FUNC = _.constant(1);

	function dijkstra(g, source, weightFn, edgeFn) {
	  return runDijkstra(g, String(source),
	    weightFn || DEFAULT_WEIGHT_FUNC,
	    edgeFn || function(v) { return g.outEdges(v); });
	}

	function runDijkstra(g, source, weightFn, edgeFn) {
	  var results = {};
	  var pq = new PriorityQueue();
	  var v, vEntry;

	  var updateNeighbors = function(edge) {
	    var w = edge.v !== v ? edge.v : edge.w;
	    var wEntry = results[w];
	    var weight = weightFn(edge);
	    var distance = vEntry.distance + weight;

	    if (weight < 0) {
	      throw new Error("dijkstra does not allow negative edge weights. " +
	                      "Bad edge: " + edge + " Weight: " + weight);
	    }

	    if (distance < wEntry.distance) {
	      wEntry.distance = distance;
	      wEntry.predecessor = v;
	      pq.decrease(w, distance);
	    }
	  };

	  g.nodes().forEach(function(v) {
	    var distance = v === source ? 0 : Number.POSITIVE_INFINITY;
	    results[v] = { distance: distance };
	    pq.add(v, distance);
	  });

	  while (pq.size() > 0) {
	    v = pq.removeMin();
	    vEntry = results[v];
	    if (vEntry.distance === Number.POSITIVE_INFINITY) {
	      break;
	    }

	    edgeFn(v).forEach(updateNeighbors);
	  }

	  return results;
	}
	return dijkstra_1;
}

var dijkstraAll_1;
var hasRequiredDijkstraAll;

function requireDijkstraAll () {
	if (hasRequiredDijkstraAll) return dijkstraAll_1;
	hasRequiredDijkstraAll = 1;
	var dijkstra = requireDijkstra();
	var _ = requireLodash$1();

	dijkstraAll_1 = dijkstraAll;

	function dijkstraAll(g, weightFunc, edgeFunc) {
	  return _.transform(g.nodes(), function(acc, v) {
	    acc[v] = dijkstra(g, v, weightFunc, edgeFunc);
	  }, {});
	}
	return dijkstraAll_1;
}

var tarjan_1;
var hasRequiredTarjan;

function requireTarjan () {
	if (hasRequiredTarjan) return tarjan_1;
	hasRequiredTarjan = 1;
	var _ = requireLodash$1();

	tarjan_1 = tarjan;

	function tarjan(g) {
	  var index = 0;
	  var stack = [];
	  var visited = {}; // node id -> { onStack, lowlink, index }
	  var results = [];

	  function dfs(v) {
	    var entry = visited[v] = {
	      onStack: true,
	      lowlink: index,
	      index: index++
	    };
	    stack.push(v);

	    g.successors(v).forEach(function(w) {
	      if (!_.has(visited, w)) {
	        dfs(w);
	        entry.lowlink = Math.min(entry.lowlink, visited[w].lowlink);
	      } else if (visited[w].onStack) {
	        entry.lowlink = Math.min(entry.lowlink, visited[w].index);
	      }
	    });

	    if (entry.lowlink === entry.index) {
	      var cmpt = [];
	      var w;
	      do {
	        w = stack.pop();
	        visited[w].onStack = false;
	        cmpt.push(w);
	      } while (v !== w);
	      results.push(cmpt);
	    }
	  }

	  g.nodes().forEach(function(v) {
	    if (!_.has(visited, v)) {
	      dfs(v);
	    }
	  });

	  return results;
	}
	return tarjan_1;
}

var findCycles_1;
var hasRequiredFindCycles;

function requireFindCycles () {
	if (hasRequiredFindCycles) return findCycles_1;
	hasRequiredFindCycles = 1;
	var _ = requireLodash$1();
	var tarjan = requireTarjan();

	findCycles_1 = findCycles;

	function findCycles(g) {
	  return _.filter(tarjan(g), function(cmpt) {
	    return cmpt.length > 1 || (cmpt.length === 1 && g.hasEdge(cmpt[0], cmpt[0]));
	  });
	}
	return findCycles_1;
}

var floydWarshall_1;
var hasRequiredFloydWarshall;

function requireFloydWarshall () {
	if (hasRequiredFloydWarshall) return floydWarshall_1;
	hasRequiredFloydWarshall = 1;
	var _ = requireLodash$1();

	floydWarshall_1 = floydWarshall;

	var DEFAULT_WEIGHT_FUNC = _.constant(1);

	function floydWarshall(g, weightFn, edgeFn) {
	  return runFloydWarshall(g,
	    weightFn || DEFAULT_WEIGHT_FUNC,
	    edgeFn || function(v) { return g.outEdges(v); });
	}

	function runFloydWarshall(g, weightFn, edgeFn) {
	  var results = {};
	  var nodes = g.nodes();

	  nodes.forEach(function(v) {
	    results[v] = {};
	    results[v][v] = { distance: 0 };
	    nodes.forEach(function(w) {
	      if (v !== w) {
	        results[v][w] = { distance: Number.POSITIVE_INFINITY };
	      }
	    });
	    edgeFn(v).forEach(function(edge) {
	      var w = edge.v === v ? edge.w : edge.v;
	      var d = weightFn(edge);
	      results[v][w] = { distance: d, predecessor: v };
	    });
	  });

	  nodes.forEach(function(k) {
	    var rowK = results[k];
	    nodes.forEach(function(i) {
	      var rowI = results[i];
	      nodes.forEach(function(j) {
	        var ik = rowI[k];
	        var kj = rowK[j];
	        var ij = rowI[j];
	        var altDistance = ik.distance + kj.distance;
	        if (altDistance < ij.distance) {
	          ij.distance = altDistance;
	          ij.predecessor = kj.predecessor;
	        }
	      });
	    });
	  });

	  return results;
	}
	return floydWarshall_1;
}

var topsort_1;
var hasRequiredTopsort;

function requireTopsort () {
	if (hasRequiredTopsort) return topsort_1;
	hasRequiredTopsort = 1;
	var _ = requireLodash$1();

	topsort_1 = topsort;
	topsort.CycleException = CycleException;

	function topsort(g) {
	  var visited = {};
	  var stack = {};
	  var results = [];

	  function visit(node) {
	    if (_.has(stack, node)) {
	      throw new CycleException();
	    }

	    if (!_.has(visited, node)) {
	      stack[node] = true;
	      visited[node] = true;
	      _.each(g.predecessors(node), visit);
	      delete stack[node];
	      results.push(node);
	    }
	  }

	  _.each(g.sinks(), visit);

	  if (_.size(visited) !== g.nodeCount()) {
	    throw new CycleException();
	  }

	  return results;
	}

	function CycleException() {}
	CycleException.prototype = new Error(); // must be an instance of Error to pass testing
	return topsort_1;
}

var isAcyclic_1;
var hasRequiredIsAcyclic;

function requireIsAcyclic () {
	if (hasRequiredIsAcyclic) return isAcyclic_1;
	hasRequiredIsAcyclic = 1;
	var topsort = requireTopsort();

	isAcyclic_1 = isAcyclic;

	function isAcyclic(g) {
	  try {
	    topsort(g);
	  } catch (e) {
	    if (e instanceof topsort.CycleException) {
	      return false;
	    }
	    throw e;
	  }
	  return true;
	}
	return isAcyclic_1;
}

var dfs_1;
var hasRequiredDfs;

function requireDfs () {
	if (hasRequiredDfs) return dfs_1;
	hasRequiredDfs = 1;
	var _ = requireLodash$1();

	dfs_1 = dfs;

	/*
	 * A helper that preforms a pre- or post-order traversal on the input graph
	 * and returns the nodes in the order they were visited. If the graph is
	 * undirected then this algorithm will navigate using neighbors. If the graph
	 * is directed then this algorithm will navigate using successors.
	 *
	 * Order must be one of "pre" or "post".
	 */
	function dfs(g, vs, order) {
	  if (!_.isArray(vs)) {
	    vs = [vs];
	  }

	  var navigation = (g.isDirected() ? g.successors : g.neighbors).bind(g);

	  var acc = [];
	  var visited = {};
	  _.each(vs, function(v) {
	    if (!g.hasNode(v)) {
	      throw new Error("Graph does not have node: " + v);
	    }

	    doDfs(g, v, order === "post", visited, navigation, acc);
	  });
	  return acc;
	}

	function doDfs(g, v, postorder, visited, navigation, acc) {
	  if (!_.has(visited, v)) {
	    visited[v] = true;

	    if (!postorder) { acc.push(v); }
	    _.each(navigation(v), function(w) {
	      doDfs(g, w, postorder, visited, navigation, acc);
	    });
	    if (postorder) { acc.push(v); }
	  }
	}
	return dfs_1;
}

var postorder_1;
var hasRequiredPostorder;

function requirePostorder () {
	if (hasRequiredPostorder) return postorder_1;
	hasRequiredPostorder = 1;
	var dfs = requireDfs();

	postorder_1 = postorder;

	function postorder(g, vs) {
	  return dfs(g, vs, "post");
	}
	return postorder_1;
}

var preorder_1;
var hasRequiredPreorder;

function requirePreorder () {
	if (hasRequiredPreorder) return preorder_1;
	hasRequiredPreorder = 1;
	var dfs = requireDfs();

	preorder_1 = preorder;

	function preorder(g, vs) {
	  return dfs(g, vs, "pre");
	}
	return preorder_1;
}

var prim_1;
var hasRequiredPrim;

function requirePrim () {
	if (hasRequiredPrim) return prim_1;
	hasRequiredPrim = 1;
	var _ = requireLodash$1();
	var Graph = requireGraph();
	var PriorityQueue = requirePriorityQueue();

	prim_1 = prim;

	function prim(g, weightFunc) {
	  var result = new Graph();
	  var parents = {};
	  var pq = new PriorityQueue();
	  var v;

	  function updateNeighbors(edge) {
	    var w = edge.v === v ? edge.w : edge.v;
	    var pri = pq.priority(w);
	    if (pri !== undefined) {
	      var edgeWeight = weightFunc(edge);
	      if (edgeWeight < pri) {
	        parents[w] = v;
	        pq.decrease(w, edgeWeight);
	      }
	    }
	  }

	  if (g.nodeCount() === 0) {
	    return result;
	  }

	  _.each(g.nodes(), function(v) {
	    pq.add(v, Number.POSITIVE_INFINITY);
	    result.setNode(v);
	  });

	  // Start from an arbitrary node
	  pq.decrease(g.nodes()[0], 0);

	  var init = false;
	  while (pq.size() > 0) {
	    v = pq.removeMin();
	    if (_.has(parents, v)) {
	      result.setEdge(v, parents[v]);
	    } else if (init) {
	      throw new Error("Input graph is not connected: " + g);
	    } else {
	      init = true;
	    }

	    g.nodeEdges(v).forEach(updateNeighbors);
	  }

	  return result;
	}
	return prim_1;
}

var alg;
var hasRequiredAlg;

function requireAlg () {
	if (hasRequiredAlg) return alg;
	hasRequiredAlg = 1;
	alg = {
	  components: requireComponents(),
	  dijkstra: requireDijkstra(),
	  dijkstraAll: requireDijkstraAll(),
	  findCycles: requireFindCycles(),
	  floydWarshall: requireFloydWarshall(),
	  isAcyclic: requireIsAcyclic(),
	  postorder: requirePostorder(),
	  preorder: requirePreorder(),
	  prim: requirePrim(),
	  tarjan: requireTarjan(),
	  topsort: requireTopsort()
	};
	return alg;
}

/**
 * Copyright (c) 2014, Chris Pettitt
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors
 * may be used to endorse or promote products derived from this software without
 * specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

var graphlib;
var hasRequiredGraphlib$1;

function requireGraphlib$1 () {
	if (hasRequiredGraphlib$1) return graphlib;
	hasRequiredGraphlib$1 = 1;
	var lib = requireLib();

	graphlib = {
	  Graph: lib.Graph,
	  json: requireJson(),
	  alg: requireAlg(),
	  version: lib.version
	};
	return graphlib;
}

/* global window */

var graphlib_1;
var hasRequiredGraphlib;

function requireGraphlib () {
	if (hasRequiredGraphlib) return graphlib_1;
	hasRequiredGraphlib = 1;
	var graphlib;

	if (typeof commonjsRequire === "function") {
	  try {
	    graphlib = requireGraphlib$1();
	  } catch (e) {
	    // continue regardless of error
	  }
	}

	if (!graphlib) {
	  graphlib = window.graphlib;
	}

	graphlib_1 = graphlib;
	return graphlib_1;
}

var cloneDeep_1;
var hasRequiredCloneDeep;

function requireCloneDeep () {
	if (hasRequiredCloneDeep) return cloneDeep_1;
	hasRequiredCloneDeep = 1;
	var baseClone = require_baseClone();

	/** Used to compose bitmasks for cloning. */
	var CLONE_DEEP_FLAG = 1,
	    CLONE_SYMBOLS_FLAG = 4;

	/**
	 * This method is like `_.clone` except that it recursively clones `value`.
	 *
	 * @static
	 * @memberOf _
	 * @since 1.0.0
	 * @category Lang
	 * @param {*} value The value to recursively clone.
	 * @returns {*} Returns the deep cloned value.
	 * @see _.clone
	 * @example
	 *
	 * var objects = [{ 'a': 1 }, { 'b': 2 }];
	 *
	 * var deep = _.cloneDeep(objects);
	 * console.log(deep[0] === objects[0]);
	 * // => false
	 */
	function cloneDeep(value) {
	  return baseClone(value, CLONE_DEEP_FLAG | CLONE_SYMBOLS_FLAG);
	}

	cloneDeep_1 = cloneDeep;
	return cloneDeep_1;
}

var defaults_1;
var hasRequiredDefaults;

function requireDefaults () {
	if (hasRequiredDefaults) return defaults_1;
	hasRequiredDefaults = 1;
	var baseRest = require_baseRest(),
	    eq = requireEq(),
	    isIterateeCall = require_isIterateeCall(),
	    keysIn = requireKeysIn();

	/** Used for built-in method references. */
	var objectProto = Object.prototype;

	/** Used to check objects for own properties. */
	var hasOwnProperty = objectProto.hasOwnProperty;

	/**
	 * Assigns own and inherited enumerable string keyed properties of source
	 * objects to the destination object for all destination properties that
	 * resolve to `undefined`. Source objects are applied from left to right.
	 * Once a property is set, additional values of the same property are ignored.
	 *
	 * **Note:** This method mutates `object`.
	 *
	 * @static
	 * @since 0.1.0
	 * @memberOf _
	 * @category Object
	 * @param {Object} object The destination object.
	 * @param {...Object} [sources] The source objects.
	 * @returns {Object} Returns `object`.
	 * @see _.defaultsDeep
	 * @example
	 *
	 * _.defaults({ 'a': 1 }, { 'b': 2 }, { 'a': 3 });
	 * // => { 'a': 1, 'b': 2 }
	 */
	var defaults = baseRest(function(object, sources) {
	  object = Object(object);

	  var index = -1;
	  var length = sources.length;
	  var guard = length > 2 ? sources[2] : undefined;

	  if (guard && isIterateeCall(sources[0], sources[1], guard)) {
	    length = 1;
	  }

	  while (++index < length) {
	    var source = sources[index];
	    var props = keysIn(source);
	    var propsIndex = -1;
	    var propsLength = props.length;

	    while (++propsIndex < propsLength) {
	      var key = props[propsIndex];
	      var value = object[key];

	      if (value === undefined ||
	          (eq(value, objectProto[key]) && !hasOwnProperty.call(object, key))) {
	        object[key] = source[key];
	      }
	    }
	  }

	  return object;
	});

	defaults_1 = defaults;
	return defaults_1;
}

var forIn_1;
var hasRequiredForIn;

function requireForIn () {
	if (hasRequiredForIn) return forIn_1;
	hasRequiredForIn = 1;
	var baseFor = require_baseFor(),
	    castFunction = require_castFunction(),
	    keysIn = requireKeysIn();

	/**
	 * Iterates over own and inherited enumerable string keyed properties of an
	 * object and invokes `iteratee` for each property. The iteratee is invoked
	 * with three arguments: (value, key, object). Iteratee functions may exit
	 * iteration early by explicitly returning `false`.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.3.0
	 * @category Object
	 * @param {Object} object The object to iterate over.
	 * @param {Function} [iteratee=_.identity] The function invoked per iteration.
	 * @returns {Object} Returns `object`.
	 * @see _.forInRight
	 * @example
	 *
	 * function Foo() {
	 *   this.a = 1;
	 *   this.b = 2;
	 * }
	 *
	 * Foo.prototype.c = 3;
	 *
	 * _.forIn(new Foo, function(value, key) {
	 *   console.log(key);
	 * });
	 * // => Logs 'a', 'b', then 'c' (iteration order is not guaranteed).
	 */
	function forIn(object, iteratee) {
	  return object == null
	    ? object
	    : baseFor(object, castFunction(iteratee), keysIn);
	}

	forIn_1 = forIn;
	return forIn_1;
}

var mapValues_1;
var hasRequiredMapValues;

function requireMapValues () {
	if (hasRequiredMapValues) return mapValues_1;
	hasRequiredMapValues = 1;
	var baseAssignValue = require_baseAssignValue(),
	    baseForOwn = require_baseForOwn(),
	    baseIteratee = require_baseIteratee();

	/**
	 * Creates an object with the same keys as `object` and values generated
	 * by running each own enumerable string keyed property of `object` thru
	 * `iteratee`. The iteratee is invoked with three arguments:
	 * (value, key, object).
	 *
	 * @static
	 * @memberOf _
	 * @since 2.4.0
	 * @category Object
	 * @param {Object} object The object to iterate over.
	 * @param {Function} [iteratee=_.identity] The function invoked per iteration.
	 * @returns {Object} Returns the new mapped object.
	 * @see _.mapKeys
	 * @example
	 *
	 * var users = {
	 *   'fred':    { 'user': 'fred',    'age': 40 },
	 *   'pebbles': { 'user': 'pebbles', 'age': 1 }
	 * };
	 *
	 * _.mapValues(users, function(o) { return o.age; });
	 * // => { 'fred': 40, 'pebbles': 1 } (iteration order is not guaranteed)
	 *
	 * // The `_.property` iteratee shorthand.
	 * _.mapValues(users, 'age');
	 * // => { 'fred': 40, 'pebbles': 1 } (iteration order is not guaranteed)
	 */
	function mapValues(object, iteratee) {
	  var result = {};
	  iteratee = baseIteratee(iteratee, 3);

	  baseForOwn(object, function(value, key, object) {
	    baseAssignValue(result, key, iteratee(value, key, object));
	  });
	  return result;
	}

	mapValues_1 = mapValues;
	return mapValues_1;
}

var _baseExtremum;
var hasRequired_baseExtremum;

function require_baseExtremum () {
	if (hasRequired_baseExtremum) return _baseExtremum;
	hasRequired_baseExtremum = 1;
	var isSymbol = requireIsSymbol();

	/**
	 * The base implementation of methods like `_.max` and `_.min` which accepts a
	 * `comparator` to determine the extremum value.
	 *
	 * @private
	 * @param {Array} array The array to iterate over.
	 * @param {Function} iteratee The iteratee invoked per iteration.
	 * @param {Function} comparator The comparator used to compare values.
	 * @returns {*} Returns the extremum value.
	 */
	function baseExtremum(array, iteratee, comparator) {
	  var index = -1,
	      length = array.length;

	  while (++index < length) {
	    var value = array[index],
	        current = iteratee(value);

	    if (current != null && (computed === undefined
	          ? (current === current && !isSymbol(current))
	          : comparator(current, computed)
	        )) {
	      var computed = current,
	          result = value;
	    }
	  }
	  return result;
	}

	_baseExtremum = baseExtremum;
	return _baseExtremum;
}

/**
 * The base implementation of `_.gt` which doesn't coerce arguments.
 *
 * @private
 * @param {*} value The value to compare.
 * @param {*} other The other value to compare.
 * @returns {boolean} Returns `true` if `value` is greater than `other`,
 *  else `false`.
 */

var _baseGt;
var hasRequired_baseGt;

function require_baseGt () {
	if (hasRequired_baseGt) return _baseGt;
	hasRequired_baseGt = 1;
	function baseGt(value, other) {
	  return value > other;
	}

	_baseGt = baseGt;
	return _baseGt;
}

var max_1;
var hasRequiredMax;

function requireMax () {
	if (hasRequiredMax) return max_1;
	hasRequiredMax = 1;
	var baseExtremum = require_baseExtremum(),
	    baseGt = require_baseGt(),
	    identity = requireIdentity();

	/**
	 * Computes the maximum value of `array`. If `array` is empty or falsey,
	 * `undefined` is returned.
	 *
	 * @static
	 * @since 0.1.0
	 * @memberOf _
	 * @category Math
	 * @param {Array} array The array to iterate over.
	 * @returns {*} Returns the maximum value.
	 * @example
	 *
	 * _.max([4, 2, 8, 6]);
	 * // => 8
	 *
	 * _.max([]);
	 * // => undefined
	 */
	function max(array) {
	  return (array && array.length)
	    ? baseExtremum(array, identity, baseGt)
	    : undefined;
	}

	max_1 = max;
	return max_1;
}

var _assignMergeValue;
var hasRequired_assignMergeValue;

function require_assignMergeValue () {
	if (hasRequired_assignMergeValue) return _assignMergeValue;
	hasRequired_assignMergeValue = 1;
	var baseAssignValue = require_baseAssignValue(),
	    eq = requireEq();

	/**
	 * This function is like `assignValue` except that it doesn't assign
	 * `undefined` values.
	 *
	 * @private
	 * @param {Object} object The object to modify.
	 * @param {string} key The key of the property to assign.
	 * @param {*} value The value to assign.
	 */
	function assignMergeValue(object, key, value) {
	  if ((value !== undefined && !eq(object[key], value)) ||
	      (value === undefined && !(key in object))) {
	    baseAssignValue(object, key, value);
	  }
	}

	_assignMergeValue = assignMergeValue;
	return _assignMergeValue;
}

var isPlainObject_1;
var hasRequiredIsPlainObject;

function requireIsPlainObject () {
	if (hasRequiredIsPlainObject) return isPlainObject_1;
	hasRequiredIsPlainObject = 1;
	var baseGetTag = require_baseGetTag(),
	    getPrototype = require_getPrototype(),
	    isObjectLike = requireIsObjectLike();

	/** `Object#toString` result references. */
	var objectTag = '[object Object]';

	/** Used for built-in method references. */
	var funcProto = Function.prototype,
	    objectProto = Object.prototype;

	/** Used to resolve the decompiled source of functions. */
	var funcToString = funcProto.toString;

	/** Used to check objects for own properties. */
	var hasOwnProperty = objectProto.hasOwnProperty;

	/** Used to infer the `Object` constructor. */
	var objectCtorString = funcToString.call(Object);

	/**
	 * Checks if `value` is a plain object, that is, an object created by the
	 * `Object` constructor or one with a `[[Prototype]]` of `null`.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.8.0
	 * @category Lang
	 * @param {*} value The value to check.
	 * @returns {boolean} Returns `true` if `value` is a plain object, else `false`.
	 * @example
	 *
	 * function Foo() {
	 *   this.a = 1;
	 * }
	 *
	 * _.isPlainObject(new Foo);
	 * // => false
	 *
	 * _.isPlainObject([1, 2, 3]);
	 * // => false
	 *
	 * _.isPlainObject({ 'x': 0, 'y': 0 });
	 * // => true
	 *
	 * _.isPlainObject(Object.create(null));
	 * // => true
	 */
	function isPlainObject(value) {
	  if (!isObjectLike(value) || baseGetTag(value) != objectTag) {
	    return false;
	  }
	  var proto = getPrototype(value);
	  if (proto === null) {
	    return true;
	  }
	  var Ctor = hasOwnProperty.call(proto, 'constructor') && proto.constructor;
	  return typeof Ctor == 'function' && Ctor instanceof Ctor &&
	    funcToString.call(Ctor) == objectCtorString;
	}

	isPlainObject_1 = isPlainObject;
	return isPlainObject_1;
}

/**
 * Gets the value at `key`, unless `key` is "__proto__" or "constructor".
 *
 * @private
 * @param {Object} object The object to query.
 * @param {string} key The key of the property to get.
 * @returns {*} Returns the property value.
 */

var _safeGet;
var hasRequired_safeGet;

function require_safeGet () {
	if (hasRequired_safeGet) return _safeGet;
	hasRequired_safeGet = 1;
	function safeGet(object, key) {
	  if (key === 'constructor' && typeof object[key] === 'function') {
	    return;
	  }

	  if (key == '__proto__') {
	    return;
	  }

	  return object[key];
	}

	_safeGet = safeGet;
	return _safeGet;
}

var toPlainObject_1;
var hasRequiredToPlainObject;

function requireToPlainObject () {
	if (hasRequiredToPlainObject) return toPlainObject_1;
	hasRequiredToPlainObject = 1;
	var copyObject = require_copyObject(),
	    keysIn = requireKeysIn();

	/**
	 * Converts `value` to a plain object flattening inherited enumerable string
	 * keyed properties of `value` to own properties of the plain object.
	 *
	 * @static
	 * @memberOf _
	 * @since 3.0.0
	 * @category Lang
	 * @param {*} value The value to convert.
	 * @returns {Object} Returns the converted plain object.
	 * @example
	 *
	 * function Foo() {
	 *   this.b = 2;
	 * }
	 *
	 * Foo.prototype.c = 3;
	 *
	 * _.assign({ 'a': 1 }, new Foo);
	 * // => { 'a': 1, 'b': 2 }
	 *
	 * _.assign({ 'a': 1 }, _.toPlainObject(new Foo));
	 * // => { 'a': 1, 'b': 2, 'c': 3 }
	 */
	function toPlainObject(value) {
	  return copyObject(value, keysIn(value));
	}

	toPlainObject_1 = toPlainObject;
	return toPlainObject_1;
}

var _baseMergeDeep;
var hasRequired_baseMergeDeep;

function require_baseMergeDeep () {
	if (hasRequired_baseMergeDeep) return _baseMergeDeep;
	hasRequired_baseMergeDeep = 1;
	var assignMergeValue = require_assignMergeValue(),
	    cloneBuffer = require_cloneBuffer(),
	    cloneTypedArray = require_cloneTypedArray(),
	    copyArray = require_copyArray(),
	    initCloneObject = require_initCloneObject(),
	    isArguments = requireIsArguments(),
	    isArray = requireIsArray(),
	    isArrayLikeObject = requireIsArrayLikeObject(),
	    isBuffer = requireIsBuffer(),
	    isFunction = requireIsFunction(),
	    isObject = requireIsObject(),
	    isPlainObject = requireIsPlainObject(),
	    isTypedArray = requireIsTypedArray(),
	    safeGet = require_safeGet(),
	    toPlainObject = requireToPlainObject();

	/**
	 * A specialized version of `baseMerge` for arrays and objects which performs
	 * deep merges and tracks traversed objects enabling objects with circular
	 * references to be merged.
	 *
	 * @private
	 * @param {Object} object The destination object.
	 * @param {Object} source The source object.
	 * @param {string} key The key of the value to merge.
	 * @param {number} srcIndex The index of `source`.
	 * @param {Function} mergeFunc The function to merge values.
	 * @param {Function} [customizer] The function to customize assigned values.
	 * @param {Object} [stack] Tracks traversed source values and their merged
	 *  counterparts.
	 */
	function baseMergeDeep(object, source, key, srcIndex, mergeFunc, customizer, stack) {
	  var objValue = safeGet(object, key),
	      srcValue = safeGet(source, key),
	      stacked = stack.get(srcValue);

	  if (stacked) {
	    assignMergeValue(object, key, stacked);
	    return;
	  }
	  var newValue = customizer
	    ? customizer(objValue, srcValue, (key + ''), object, source, stack)
	    : undefined;

	  var isCommon = newValue === undefined;

	  if (isCommon) {
	    var isArr = isArray(srcValue),
	        isBuff = !isArr && isBuffer(srcValue),
	        isTyped = !isArr && !isBuff && isTypedArray(srcValue);

	    newValue = srcValue;
	    if (isArr || isBuff || isTyped) {
	      if (isArray(objValue)) {
	        newValue = objValue;
	      }
	      else if (isArrayLikeObject(objValue)) {
	        newValue = copyArray(objValue);
	      }
	      else if (isBuff) {
	        isCommon = false;
	        newValue = cloneBuffer(srcValue, true);
	      }
	      else if (isTyped) {
	        isCommon = false;
	        newValue = cloneTypedArray(srcValue, true);
	      }
	      else {
	        newValue = [];
	      }
	    }
	    else if (isPlainObject(srcValue) || isArguments(srcValue)) {
	      newValue = objValue;
	      if (isArguments(objValue)) {
	        newValue = toPlainObject(objValue);
	      }
	      else if (!isObject(objValue) || isFunction(objValue)) {
	        newValue = initCloneObject(srcValue);
	      }
	    }
	    else {
	      isCommon = false;
	    }
	  }
	  if (isCommon) {
	    // Recursively merge objects and arrays (susceptible to call stack limits).
	    stack.set(srcValue, newValue);
	    mergeFunc(newValue, srcValue, srcIndex, customizer, stack);
	    stack['delete'](srcValue);
	  }
	  assignMergeValue(object, key, newValue);
	}

	_baseMergeDeep = baseMergeDeep;
	return _baseMergeDeep;
}

var _baseMerge;
var hasRequired_baseMerge;

function require_baseMerge () {
	if (hasRequired_baseMerge) return _baseMerge;
	hasRequired_baseMerge = 1;
	var Stack = require_Stack(),
	    assignMergeValue = require_assignMergeValue(),
	    baseFor = require_baseFor(),
	    baseMergeDeep = require_baseMergeDeep(),
	    isObject = requireIsObject(),
	    keysIn = requireKeysIn(),
	    safeGet = require_safeGet();

	/**
	 * The base implementation of `_.merge` without support for multiple sources.
	 *
	 * @private
	 * @param {Object} object The destination object.
	 * @param {Object} source The source object.
	 * @param {number} srcIndex The index of `source`.
	 * @param {Function} [customizer] The function to customize merged values.
	 * @param {Object} [stack] Tracks traversed source values and their merged
	 *  counterparts.
	 */
	function baseMerge(object, source, srcIndex, customizer, stack) {
	  if (object === source) {
	    return;
	  }
	  baseFor(source, function(srcValue, key) {
	    stack || (stack = new Stack);
	    if (isObject(srcValue)) {
	      baseMergeDeep(object, source, key, srcIndex, baseMerge, customizer, stack);
	    }
	    else {
	      var newValue = customizer
	        ? customizer(safeGet(object, key), srcValue, (key + ''), object, source, stack)
	        : undefined;

	      if (newValue === undefined) {
	        newValue = srcValue;
	      }
	      assignMergeValue(object, key, newValue);
	    }
	  }, keysIn);
	}

	_baseMerge = baseMerge;
	return _baseMerge;
}

var _createAssigner;
var hasRequired_createAssigner;

function require_createAssigner () {
	if (hasRequired_createAssigner) return _createAssigner;
	hasRequired_createAssigner = 1;
	var baseRest = require_baseRest(),
	    isIterateeCall = require_isIterateeCall();

	/**
	 * Creates a function like `_.assign`.
	 *
	 * @private
	 * @param {Function} assigner The function to assign values.
	 * @returns {Function} Returns the new assigner function.
	 */
	function createAssigner(assigner) {
	  return baseRest(function(object, sources) {
	    var index = -1,
	        length = sources.length,
	        customizer = length > 1 ? sources[length - 1] : undefined,
	        guard = length > 2 ? sources[2] : undefined;

	    customizer = (assigner.length > 3 && typeof customizer == 'function')
	      ? (length--, customizer)
	      : undefined;

	    if (guard && isIterateeCall(sources[0], sources[1], guard)) {
	      customizer = length < 3 ? undefined : customizer;
	      length = 1;
	    }
	    object = Object(object);
	    while (++index < length) {
	      var source = sources[index];
	      if (source) {
	        assigner(object, source, index, customizer);
	      }
	    }
	    return object;
	  });
	}

	_createAssigner = createAssigner;
	return _createAssigner;
}

var merge_1;
var hasRequiredMerge;

function requireMerge () {
	if (hasRequiredMerge) return merge_1;
	hasRequiredMerge = 1;
	var baseMerge = require_baseMerge(),
	    createAssigner = require_createAssigner();

	/**
	 * This method is like `_.assign` except that it recursively merges own and
	 * inherited enumerable string keyed properties of source objects into the
	 * destination object. Source properties that resolve to `undefined` are
	 * skipped if a destination value exists. Array and plain object properties
	 * are merged recursively. Other objects and value types are overridden by
	 * assignment. Source objects are applied from left to right. Subsequent
	 * sources overwrite property assignments of previous sources.
	 *
	 * **Note:** This method mutates `object`.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.5.0
	 * @category Object
	 * @param {Object} object The destination object.
	 * @param {...Object} [sources] The source objects.
	 * @returns {Object} Returns `object`.
	 * @example
	 *
	 * var object = {
	 *   'a': [{ 'b': 2 }, { 'd': 4 }]
	 * };
	 *
	 * var other = {
	 *   'a': [{ 'c': 3 }, { 'e': 5 }]
	 * };
	 *
	 * _.merge(object, other);
	 * // => { 'a': [{ 'b': 2, 'c': 3 }, { 'd': 4, 'e': 5 }] }
	 */
	var merge = createAssigner(function(object, source, srcIndex) {
	  baseMerge(object, source, srcIndex);
	});

	merge_1 = merge;
	return merge_1;
}

/**
 * The base implementation of `_.lt` which doesn't coerce arguments.
 *
 * @private
 * @param {*} value The value to compare.
 * @param {*} other The other value to compare.
 * @returns {boolean} Returns `true` if `value` is less than `other`,
 *  else `false`.
 */

var _baseLt;
var hasRequired_baseLt;

function require_baseLt () {
	if (hasRequired_baseLt) return _baseLt;
	hasRequired_baseLt = 1;
	function baseLt(value, other) {
	  return value < other;
	}

	_baseLt = baseLt;
	return _baseLt;
}

var min_1;
var hasRequiredMin;

function requireMin () {
	if (hasRequiredMin) return min_1;
	hasRequiredMin = 1;
	var baseExtremum = require_baseExtremum(),
	    baseLt = require_baseLt(),
	    identity = requireIdentity();

	/**
	 * Computes the minimum value of `array`. If `array` is empty or falsey,
	 * `undefined` is returned.
	 *
	 * @static
	 * @since 0.1.0
	 * @memberOf _
	 * @category Math
	 * @param {Array} array The array to iterate over.
	 * @returns {*} Returns the minimum value.
	 * @example
	 *
	 * _.min([4, 2, 8, 6]);
	 * // => 2
	 *
	 * _.min([]);
	 * // => undefined
	 */
	function min(array) {
	  return (array && array.length)
	    ? baseExtremum(array, identity, baseLt)
	    : undefined;
	}

	min_1 = min;
	return min_1;
}

var minBy_1;
var hasRequiredMinBy;

function requireMinBy () {
	if (hasRequiredMinBy) return minBy_1;
	hasRequiredMinBy = 1;
	var baseExtremum = require_baseExtremum(),
	    baseIteratee = require_baseIteratee(),
	    baseLt = require_baseLt();

	/**
	 * This method is like `_.min` except that it accepts `iteratee` which is
	 * invoked for each element in `array` to generate the criterion by which
	 * the value is ranked. The iteratee is invoked with one argument: (value).
	 *
	 * @static
	 * @memberOf _
	 * @since 4.0.0
	 * @category Math
	 * @param {Array} array The array to iterate over.
	 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
	 * @returns {*} Returns the minimum value.
	 * @example
	 *
	 * var objects = [{ 'n': 1 }, { 'n': 2 }];
	 *
	 * _.minBy(objects, function(o) { return o.n; });
	 * // => { 'n': 1 }
	 *
	 * // The `_.property` iteratee shorthand.
	 * _.minBy(objects, 'n');
	 * // => { 'n': 1 }
	 */
	function minBy(array, iteratee) {
	  return (array && array.length)
	    ? baseExtremum(array, baseIteratee(iteratee, 2), baseLt)
	    : undefined;
	}

	minBy_1 = minBy;
	return minBy_1;
}

var now_1;
var hasRequiredNow;

function requireNow () {
	if (hasRequiredNow) return now_1;
	hasRequiredNow = 1;
	var root = require_root();

	/**
	 * Gets the timestamp of the number of milliseconds that have elapsed since
	 * the Unix epoch (1 January 1970 00:00:00 UTC).
	 *
	 * @static
	 * @memberOf _
	 * @since 2.4.0
	 * @category Date
	 * @returns {number} Returns the timestamp.
	 * @example
	 *
	 * _.defer(function(stamp) {
	 *   console.log(_.now() - stamp);
	 * }, _.now());
	 * // => Logs the number of milliseconds it took for the deferred invocation.
	 */
	var now = function() {
	  return root.Date.now();
	};

	now_1 = now;
	return now_1;
}

var _basePickBy;
var hasRequired_basePickBy;

function require_basePickBy () {
	if (hasRequired_basePickBy) return _basePickBy;
	hasRequired_basePickBy = 1;
	var baseGet = require_baseGet(),
	    baseSet = require_baseSet(),
	    castPath = require_castPath();

	/**
	 * The base implementation of  `_.pickBy` without support for iteratee shorthands.
	 *
	 * @private
	 * @param {Object} object The source object.
	 * @param {string[]} paths The property paths to pick.
	 * @param {Function} predicate The function invoked per property.
	 * @returns {Object} Returns the new object.
	 */
	function basePickBy(object, paths, predicate) {
	  var index = -1,
	      length = paths.length,
	      result = {};

	  while (++index < length) {
	    var path = paths[index],
	        value = baseGet(object, path);

	    if (predicate(value, path)) {
	      baseSet(result, castPath(path, object), value);
	    }
	  }
	  return result;
	}

	_basePickBy = basePickBy;
	return _basePickBy;
}

var _basePick;
var hasRequired_basePick;

function require_basePick () {
	if (hasRequired_basePick) return _basePick;
	hasRequired_basePick = 1;
	var basePickBy = require_basePickBy(),
	    hasIn = requireHasIn();

	/**
	 * The base implementation of `_.pick` without support for individual
	 * property identifiers.
	 *
	 * @private
	 * @param {Object} object The source object.
	 * @param {string[]} paths The property paths to pick.
	 * @returns {Object} Returns the new object.
	 */
	function basePick(object, paths) {
	  return basePickBy(object, paths, function(value, path) {
	    return hasIn(object, path);
	  });
	}

	_basePick = basePick;
	return _basePick;
}

var pick_1;
var hasRequiredPick;

function requirePick () {
	if (hasRequiredPick) return pick_1;
	hasRequiredPick = 1;
	var basePick = require_basePick(),
	    flatRest = require_flatRest();

	/**
	 * Creates an object composed of the picked `object` properties.
	 *
	 * @static
	 * @since 0.1.0
	 * @memberOf _
	 * @category Object
	 * @param {Object} object The source object.
	 * @param {...(string|string[])} [paths] The property paths to pick.
	 * @returns {Object} Returns the new object.
	 * @example
	 *
	 * var object = { 'a': 1, 'b': '2', 'c': 3 };
	 *
	 * _.pick(object, ['a', 'c']);
	 * // => { 'a': 1, 'c': 3 }
	 */
	var pick = flatRest(function(object, paths) {
	  return object == null ? {} : basePick(object, paths);
	});

	pick_1 = pick;
	return pick_1;
}

/* Built-in method references for those with the same name as other `lodash` methods. */

var _baseRange;
var hasRequired_baseRange;

function require_baseRange () {
	if (hasRequired_baseRange) return _baseRange;
	hasRequired_baseRange = 1;
	var nativeCeil = Math.ceil,
	    nativeMax = Math.max;

	/**
	 * The base implementation of `_.range` and `_.rangeRight` which doesn't
	 * coerce arguments.
	 *
	 * @private
	 * @param {number} start The start of the range.
	 * @param {number} end The end of the range.
	 * @param {number} step The value to increment or decrement by.
	 * @param {boolean} [fromRight] Specify iterating from right to left.
	 * @returns {Array} Returns the range of numbers.
	 */
	function baseRange(start, end, step, fromRight) {
	  var index = -1,
	      length = nativeMax(nativeCeil((end - start) / (step || 1)), 0),
	      result = Array(length);

	  while (length--) {
	    result[fromRight ? length : ++index] = start;
	    start += step;
	  }
	  return result;
	}

	_baseRange = baseRange;
	return _baseRange;
}

var _createRange;
var hasRequired_createRange;

function require_createRange () {
	if (hasRequired_createRange) return _createRange;
	hasRequired_createRange = 1;
	var baseRange = require_baseRange(),
	    isIterateeCall = require_isIterateeCall(),
	    toFinite = requireToFinite();

	/**
	 * Creates a `_.range` or `_.rangeRight` function.
	 *
	 * @private
	 * @param {boolean} [fromRight] Specify iterating from right to left.
	 * @returns {Function} Returns the new range function.
	 */
	function createRange(fromRight) {
	  return function(start, end, step) {
	    if (step && typeof step != 'number' && isIterateeCall(start, end, step)) {
	      end = step = undefined;
	    }
	    // Ensure the sign of `-0` is preserved.
	    start = toFinite(start);
	    if (end === undefined) {
	      end = start;
	      start = 0;
	    } else {
	      end = toFinite(end);
	    }
	    step = step === undefined ? (start < end ? 1 : -1) : toFinite(step);
	    return baseRange(start, end, step, fromRight);
	  };
	}

	_createRange = createRange;
	return _createRange;
}

var range_1;
var hasRequiredRange;

function requireRange () {
	if (hasRequiredRange) return range_1;
	hasRequiredRange = 1;
	var createRange = require_createRange();

	/**
	 * Creates an array of numbers (positive and/or negative) progressing from
	 * `start` up to, but not including, `end`. A step of `-1` is used if a negative
	 * `start` is specified without an `end` or `step`. If `end` is not specified,
	 * it's set to `start` with `start` then set to `0`.
	 *
	 * **Note:** JavaScript follows the IEEE-754 standard for resolving
	 * floating-point values which can produce unexpected results.
	 *
	 * @static
	 * @since 0.1.0
	 * @memberOf _
	 * @category Util
	 * @param {number} [start=0] The start of the range.
	 * @param {number} end The end of the range.
	 * @param {number} [step=1] The value to increment or decrement by.
	 * @returns {Array} Returns the range of numbers.
	 * @see _.inRange, _.rangeRight
	 * @example
	 *
	 * _.range(4);
	 * // => [0, 1, 2, 3]
	 *
	 * _.range(-4);
	 * // => [0, -1, -2, -3]
	 *
	 * _.range(1, 5);
	 * // => [1, 2, 3, 4]
	 *
	 * _.range(0, 20, 5);
	 * // => [0, 5, 10, 15]
	 *
	 * _.range(0, -4, -1);
	 * // => [0, -1, -2, -3]
	 *
	 * _.range(1, 4, 0);
	 * // => [1, 1, 1]
	 *
	 * _.range(0);
	 * // => []
	 */
	var range = createRange();

	range_1 = range;
	return range_1;
}

var uniqueId_1;
var hasRequiredUniqueId;

function requireUniqueId () {
	if (hasRequiredUniqueId) return uniqueId_1;
	hasRequiredUniqueId = 1;
	var toString = requireToString();

	/** Used to generate unique IDs. */
	var idCounter = 0;

	/**
	 * Generates a unique ID. If `prefix` is given, the ID is appended to it.
	 *
	 * @static
	 * @since 0.1.0
	 * @memberOf _
	 * @category Util
	 * @param {string} [prefix=''] The value to prefix the ID with.
	 * @returns {string} Returns the unique ID.
	 * @example
	 *
	 * _.uniqueId('contact_');
	 * // => 'contact_104'
	 *
	 * _.uniqueId();
	 * // => '105'
	 */
	function uniqueId(prefix) {
	  var id = ++idCounter;
	  return toString(prefix) + id;
	}

	uniqueId_1 = uniqueId;
	return uniqueId_1;
}

/* global window */

var lodash_1;
var hasRequiredLodash;

function requireLodash () {
	if (hasRequiredLodash) return lodash_1;
	hasRequiredLodash = 1;
	var lodash;

	if (typeof commonjsRequire === "function") {
	  try {
	    lodash = {
	      cloneDeep: requireCloneDeep(),
	      constant: requireConstant(),
	      defaults: requireDefaults(),
	      each: requireEach(),
	      filter: requireFilter(),
	      find: requireFind(),
	      flatten: requireFlatten(),
	      forEach: requireForEach(),
	      forIn: requireForIn(),
	      has:  requireHas(),
	      isUndefined: requireIsUndefined(),
	      last: requireLast(),
	      map: requireMap(),
	      mapValues: requireMapValues(),
	      max: requireMax(),
	      merge: requireMerge(),
	      min: requireMin(),
	      minBy: requireMinBy(),
	      now: requireNow(),
	      pick: requirePick(),
	      range: requireRange(),
	      reduce: requireReduce(),
	      sortBy: requireSortBy(),
	      uniqueId: requireUniqueId(),
	      values: requireValues(),
	      zipObject: requireZipObject(),
	    };
	  } catch (e) {
	    // continue regardless of error
	  }
	}

	if (!lodash) {
	  lodash = window._;
	}

	lodash_1 = lodash;
	return lodash_1;
}

/*
 * Simple doubly linked list implementation derived from Cormen, et al.,
 * "Introduction to Algorithms".
 */

var list;
var hasRequiredList;

function requireList () {
	if (hasRequiredList) return list;
	hasRequiredList = 1;
	list = List;

	function List() {
	  var sentinel = {};
	  sentinel._next = sentinel._prev = sentinel;
	  this._sentinel = sentinel;
	}

	List.prototype.dequeue = function() {
	  var sentinel = this._sentinel;
	  var entry = sentinel._prev;
	  if (entry !== sentinel) {
	    unlink(entry);
	    return entry;
	  }
	};

	List.prototype.enqueue = function(entry) {
	  var sentinel = this._sentinel;
	  if (entry._prev && entry._next) {
	    unlink(entry);
	  }
	  entry._next = sentinel._next;
	  sentinel._next._prev = entry;
	  sentinel._next = entry;
	  entry._prev = sentinel;
	};

	List.prototype.toString = function() {
	  var strs = [];
	  var sentinel = this._sentinel;
	  var curr = sentinel._prev;
	  while (curr !== sentinel) {
	    strs.push(JSON.stringify(curr, filterOutLinks));
	    curr = curr._prev;
	  }
	  return "[" + strs.join(", ") + "]";
	};

	function unlink(entry) {
	  entry._prev._next = entry._next;
	  entry._next._prev = entry._prev;
	  delete entry._next;
	  delete entry._prev;
	}

	function filterOutLinks(k, v) {
	  if (k !== "_next" && k !== "_prev") {
	    return v;
	  }
	}
	return list;
}

var greedyFas;
var hasRequiredGreedyFas;

function requireGreedyFas () {
	if (hasRequiredGreedyFas) return greedyFas;
	hasRequiredGreedyFas = 1;
	var _ = requireLodash();
	var Graph = requireGraphlib().Graph;
	var List = requireList();

	/*
	 * A greedy heuristic for finding a feedback arc set for a graph. A feedback
	 * arc set is a set of edges that can be removed to make a graph acyclic.
	 * The algorithm comes from: P. Eades, X. Lin, and W. F. Smyth, "A fast and
	 * effective heuristic for the feedback arc set problem." This implementation
	 * adjusts that from the paper to allow for weighted edges.
	 */
	greedyFas = greedyFAS;

	var DEFAULT_WEIGHT_FN = _.constant(1);

	function greedyFAS(g, weightFn) {
	  if (g.nodeCount() <= 1) {
	    return [];
	  }
	  var state = buildState(g, weightFn || DEFAULT_WEIGHT_FN);
	  var results = doGreedyFAS(state.graph, state.buckets, state.zeroIdx);

	  // Expand multi-edges
	  return _.flatten(_.map(results, function(e) {
	    return g.outEdges(e.v, e.w);
	  }), true);
	}

	function doGreedyFAS(g, buckets, zeroIdx) {
	  var results = [];
	  var sources = buckets[buckets.length - 1];
	  var sinks = buckets[0];

	  var entry;
	  while (g.nodeCount()) {
	    while ((entry = sinks.dequeue()))   { removeNode(g, buckets, zeroIdx, entry); }
	    while ((entry = sources.dequeue())) { removeNode(g, buckets, zeroIdx, entry); }
	    if (g.nodeCount()) {
	      for (var i = buckets.length - 2; i > 0; --i) {
	        entry = buckets[i].dequeue();
	        if (entry) {
	          results = results.concat(removeNode(g, buckets, zeroIdx, entry, true));
	          break;
	        }
	      }
	    }
	  }

	  return results;
	}

	function removeNode(g, buckets, zeroIdx, entry, collectPredecessors) {
	  var results = collectPredecessors ? [] : undefined;

	  _.forEach(g.inEdges(entry.v), function(edge) {
	    var weight = g.edge(edge);
	    var uEntry = g.node(edge.v);

	    if (collectPredecessors) {
	      results.push({ v: edge.v, w: edge.w });
	    }

	    uEntry.out -= weight;
	    assignBucket(buckets, zeroIdx, uEntry);
	  });

	  _.forEach(g.outEdges(entry.v), function(edge) {
	    var weight = g.edge(edge);
	    var w = edge.w;
	    var wEntry = g.node(w);
	    wEntry["in"] -= weight;
	    assignBucket(buckets, zeroIdx, wEntry);
	  });

	  g.removeNode(entry.v);

	  return results;
	}

	function buildState(g, weightFn) {
	  var fasGraph = new Graph();
	  var maxIn = 0;
	  var maxOut = 0;

	  _.forEach(g.nodes(), function(v) {
	    fasGraph.setNode(v, { v: v, "in": 0, out: 0 });
	  });

	  // Aggregate weights on nodes, but also sum the weights across multi-edges
	  // into a single edge for the fasGraph.
	  _.forEach(g.edges(), function(e) {
	    var prevWeight = fasGraph.edge(e.v, e.w) || 0;
	    var weight = weightFn(e);
	    var edgeWeight = prevWeight + weight;
	    fasGraph.setEdge(e.v, e.w, edgeWeight);
	    maxOut = Math.max(maxOut, fasGraph.node(e.v).out += weight);
	    maxIn  = Math.max(maxIn,  fasGraph.node(e.w)["in"]  += weight);
	  });

	  var buckets = _.range(maxOut + maxIn + 3).map(function() { return new List(); });
	  var zeroIdx = maxIn + 1;

	  _.forEach(fasGraph.nodes(), function(v) {
	    assignBucket(buckets, zeroIdx, fasGraph.node(v));
	  });

	  return { graph: fasGraph, buckets: buckets, zeroIdx: zeroIdx };
	}

	function assignBucket(buckets, zeroIdx, entry) {
	  if (!entry.out) {
	    buckets[0].enqueue(entry);
	  } else if (!entry["in"]) {
	    buckets[buckets.length - 1].enqueue(entry);
	  } else {
	    buckets[entry.out - entry["in"] + zeroIdx].enqueue(entry);
	  }
	}
	return greedyFas;
}

var acyclic;
var hasRequiredAcyclic;

function requireAcyclic () {
	if (hasRequiredAcyclic) return acyclic;
	hasRequiredAcyclic = 1;
	"use strict";

	var _ = requireLodash();
	var greedyFAS = requireGreedyFas();

	acyclic = {
	  run: run,
	  undo: undo
	};

	function run(g) {
	  var fas = (g.graph().acyclicer === "greedy"
	    ? greedyFAS(g, weightFn(g))
	    : dfsFAS(g));
	  _.forEach(fas, function(e) {
	    var label = g.edge(e);
	    g.removeEdge(e);
	    label.forwardName = e.name;
	    label.reversed = true;
	    g.setEdge(e.w, e.v, label, _.uniqueId("rev"));
	  });

	  function weightFn(g) {
	    return function(e) {
	      return g.edge(e).weight;
	    };
	  }
	}

	function dfsFAS(g) {
	  var fas = [];
	  var stack = {};
	  var visited = {};

	  function dfs(v) {
	    if (_.has(visited, v)) {
	      return;
	    }
	    visited[v] = true;
	    stack[v] = true;
	    _.forEach(g.outEdges(v), function(e) {
	      if (_.has(stack, e.w)) {
	        fas.push(e);
	      } else {
	        dfs(e.w);
	      }
	    });
	    delete stack[v];
	  }

	  _.forEach(g.nodes(), dfs);
	  return fas;
	}

	function undo(g) {
	  _.forEach(g.edges(), function(e) {
	    var label = g.edge(e);
	    if (label.reversed) {
	      g.removeEdge(e);

	      var forwardName = label.forwardName;
	      delete label.reversed;
	      delete label.forwardName;
	      g.setEdge(e.w, e.v, label, forwardName);
	    }
	  });
	}
	return acyclic;
}

/* eslint "no-console": off */

var util$1;
var hasRequiredUtil$1;

function requireUtil$1 () {
	if (hasRequiredUtil$1) return util$1;
	hasRequiredUtil$1 = 1;
	"use strict";

	var _ = requireLodash();
	var Graph = requireGraphlib().Graph;

	util$1 = {
	  addDummyNode: addDummyNode,
	  simplify: simplify,
	  asNonCompoundGraph: asNonCompoundGraph,
	  successorWeights: successorWeights,
	  predecessorWeights: predecessorWeights,
	  intersectRect: intersectRect,
	  buildLayerMatrix: buildLayerMatrix,
	  normalizeRanks: normalizeRanks,
	  removeEmptyRanks: removeEmptyRanks,
	  addBorderNode: addBorderNode,
	  maxRank: maxRank,
	  partition: partition,
	  time: time,
	  notime: notime
	};

	/*
	 * Adds a dummy node to the graph and return v.
	 */
	function addDummyNode(g, type, attrs, name) {
	  var v;
	  do {
	    v = _.uniqueId(name);
	  } while (g.hasNode(v));

	  attrs.dummy = type;
	  g.setNode(v, attrs);
	  return v;
	}

	/*
	 * Returns a new graph with only simple edges. Handles aggregation of data
	 * associated with multi-edges.
	 */
	function simplify(g) {
	  var simplified = new Graph().setGraph(g.graph());
	  _.forEach(g.nodes(), function(v) { simplified.setNode(v, g.node(v)); });
	  _.forEach(g.edges(), function(e) {
	    var simpleLabel = simplified.edge(e.v, e.w) || { weight: 0, minlen: 1 };
	    var label = g.edge(e);
	    simplified.setEdge(e.v, e.w, {
	      weight: simpleLabel.weight + label.weight,
	      minlen: Math.max(simpleLabel.minlen, label.minlen)
	    });
	  });
	  return simplified;
	}

	function asNonCompoundGraph(g) {
	  var simplified = new Graph({ multigraph: g.isMultigraph() }).setGraph(g.graph());
	  _.forEach(g.nodes(), function(v) {
	    if (!g.children(v).length) {
	      simplified.setNode(v, g.node(v));
	    }
	  });
	  _.forEach(g.edges(), function(e) {
	    simplified.setEdge(e, g.edge(e));
	  });
	  return simplified;
	}

	function successorWeights(g) {
	  var weightMap = _.map(g.nodes(), function(v) {
	    var sucs = {};
	    _.forEach(g.outEdges(v), function(e) {
	      sucs[e.w] = (sucs[e.w] || 0) + g.edge(e).weight;
	    });
	    return sucs;
	  });
	  return _.zipObject(g.nodes(), weightMap);
	}

	function predecessorWeights(g) {
	  var weightMap = _.map(g.nodes(), function(v) {
	    var preds = {};
	    _.forEach(g.inEdges(v), function(e) {
	      preds[e.v] = (preds[e.v] || 0) + g.edge(e).weight;
	    });
	    return preds;
	  });
	  return _.zipObject(g.nodes(), weightMap);
	}

	/*
	 * Finds where a line starting at point ({x, y}) would intersect a rectangle
	 * ({x, y, width, height}) if it were pointing at the rectangle's center.
	 */
	function intersectRect(rect, point) {
	  var x = rect.x;
	  var y = rect.y;

	  // Rectangle intersection algorithm from:
	  // http://math.stackexchange.com/questions/108113/find-edge-between-two-boxes
	  var dx = point.x - x;
	  var dy = point.y - y;
	  var w = rect.width / 2;
	  var h = rect.height / 2;

	  if (!dx && !dy) {
	    throw new Error("Not possible to find intersection inside of the rectangle");
	  }

	  var sx, sy;
	  if (Math.abs(dy) * w > Math.abs(dx) * h) {
	    // Intersection is top or bottom of rect.
	    if (dy < 0) {
	      h = -h;
	    }
	    sx = h * dx / dy;
	    sy = h;
	  } else {
	    // Intersection is left or right of rect.
	    if (dx < 0) {
	      w = -w;
	    }
	    sx = w;
	    sy = w * dy / dx;
	  }

	  return { x: x + sx, y: y + sy };
	}

	/*
	 * Given a DAG with each node assigned "rank" and "order" properties, this
	 * function will produce a matrix with the ids of each node.
	 */
	function buildLayerMatrix(g) {
	  var layering = _.map(_.range(maxRank(g) + 1), function() { return []; });
	  _.forEach(g.nodes(), function(v) {
	    var node = g.node(v);
	    var rank = node.rank;
	    if (!_.isUndefined(rank)) {
	      layering[rank][node.order] = v;
	    }
	  });
	  return layering;
	}

	/*
	 * Adjusts the ranks for all nodes in the graph such that all nodes v have
	 * rank(v) >= 0 and at least one node w has rank(w) = 0.
	 */
	function normalizeRanks(g) {
	  var min = _.min(_.map(g.nodes(), function(v) { return g.node(v).rank; }));
	  _.forEach(g.nodes(), function(v) {
	    var node = g.node(v);
	    if (_.has(node, "rank")) {
	      node.rank -= min;
	    }
	  });
	}

	function removeEmptyRanks(g) {
	  // Ranks may not start at 0, so we need to offset them
	  var offset = _.min(_.map(g.nodes(), function(v) { return g.node(v).rank; }));

	  var layers = [];
	  _.forEach(g.nodes(), function(v) {
	    var rank = g.node(v).rank - offset;
	    if (!layers[rank]) {
	      layers[rank] = [];
	    }
	    layers[rank].push(v);
	  });

	  var delta = 0;
	  var nodeRankFactor = g.graph().nodeRankFactor;
	  _.forEach(layers, function(vs, i) {
	    if (_.isUndefined(vs) && i % nodeRankFactor !== 0) {
	      --delta;
	    } else if (delta) {
	      _.forEach(vs, function(v) { g.node(v).rank += delta; });
	    }
	  });
	}

	function addBorderNode(g, prefix, rank, order) {
	  var node = {
	    width: 0,
	    height: 0
	  };
	  if (arguments.length >= 4) {
	    node.rank = rank;
	    node.order = order;
	  }
	  return addDummyNode(g, "border", node, prefix);
	}

	function maxRank(g) {
	  return _.max(_.map(g.nodes(), function(v) {
	    var rank = g.node(v).rank;
	    if (!_.isUndefined(rank)) {
	      return rank;
	    }
	  }));
	}

	/*
	 * Partition a collection into two groups: `lhs` and `rhs`. If the supplied
	 * function returns true for an entry it goes into `lhs`. Otherwise it goes
	 * into `rhs.
	 */
	function partition(collection, fn) {
	  var result = { lhs: [], rhs: [] };
	  _.forEach(collection, function(value) {
	    if (fn(value)) {
	      result.lhs.push(value);
	    } else {
	      result.rhs.push(value);
	    }
	  });
	  return result;
	}

	/*
	 * Returns a new function that wraps `fn` with a timer. The wrapper logs the
	 * time it takes to execute the function.
	 */
	function time(name, fn) {
	  var start = _.now();
	  try {
	    return fn();
	  } finally {
	    console.log(name + " time: " + (_.now() - start) + "ms");
	  }
	}

	function notime(name, fn) {
	  return fn();
	}
	return util$1;
}

var normalize;
var hasRequiredNormalize;

function requireNormalize () {
	if (hasRequiredNormalize) return normalize;
	hasRequiredNormalize = 1;
	"use strict";

	var _ = requireLodash();
	var util = requireUtil$1();

	normalize = {
	  run: run,
	  undo: undo
	};

	/*
	 * Breaks any long edges in the graph into short segments that span 1 layer
	 * each. This operation is undoable with the denormalize function.
	 *
	 * Pre-conditions:
	 *
	 *    1. The input graph is a DAG.
	 *    2. Each node in the graph has a "rank" property.
	 *
	 * Post-condition:
	 *
	 *    1. All edges in the graph have a length of 1.
	 *    2. Dummy nodes are added where edges have been split into segments.
	 *    3. The graph is augmented with a "dummyChains" attribute which contains
	 *       the first dummy in each chain of dummy nodes produced.
	 */
	function run(g) {
	  g.graph().dummyChains = [];
	  _.forEach(g.edges(), function(edge) { normalizeEdge(g, edge); });
	}

	function normalizeEdge(g, e) {
	  var v = e.v;
	  var vRank = g.node(v).rank;
	  var w = e.w;
	  var wRank = g.node(w).rank;
	  var name = e.name;
	  var edgeLabel = g.edge(e);
	  var labelRank = edgeLabel.labelRank;

	  if (wRank === vRank + 1) return;

	  g.removeEdge(e);

	  var dummy, attrs, i;
	  for (i = 0, ++vRank; vRank < wRank; ++i, ++vRank) {
	    edgeLabel.points = [];
	    attrs = {
	      width: 0, height: 0,
	      edgeLabel: edgeLabel, edgeObj: e,
	      rank: vRank
	    };
	    dummy = util.addDummyNode(g, "edge", attrs, "_d");
	    if (vRank === labelRank) {
	      attrs.width = edgeLabel.width;
	      attrs.height = edgeLabel.height;
	      attrs.dummy = "edge-label";
	      attrs.labelpos = edgeLabel.labelpos;
	    }
	    g.setEdge(v, dummy, { weight: edgeLabel.weight }, name);
	    if (i === 0) {
	      g.graph().dummyChains.push(dummy);
	    }
	    v = dummy;
	  }

	  g.setEdge(v, w, { weight: edgeLabel.weight }, name);
	}

	function undo(g) {
	  _.forEach(g.graph().dummyChains, function(v) {
	    var node = g.node(v);
	    var origLabel = node.edgeLabel;
	    var w;
	    g.setEdge(node.edgeObj, origLabel);
	    while (node.dummy) {
	      w = g.successors(v)[0];
	      g.removeNode(v);
	      origLabel.points.push({ x: node.x, y: node.y });
	      if (node.dummy === "edge-label") {
	        origLabel.x = node.x;
	        origLabel.y = node.y;
	        origLabel.width = node.width;
	        origLabel.height = node.height;
	      }
	      v = w;
	      node = g.node(v);
	    }
	  });
	}
	return normalize;
}

var util;
var hasRequiredUtil;

function requireUtil () {
	if (hasRequiredUtil) return util;
	hasRequiredUtil = 1;
	"use strict";

	var _ = requireLodash();

	util = {
	  longestPath: longestPath,
	  slack: slack
	};

	/*
	 * Initializes ranks for the input graph using the longest path algorithm. This
	 * algorithm scales well and is fast in practice, it yields rather poor
	 * solutions. Nodes are pushed to the lowest layer possible, leaving the bottom
	 * ranks wide and leaving edges longer than necessary. However, due to its
	 * speed, this algorithm is good for getting an initial ranking that can be fed
	 * into other algorithms.
	 *
	 * This algorithm does not normalize layers because it will be used by other
	 * algorithms in most cases. If using this algorithm directly, be sure to
	 * run normalize at the end.
	 *
	 * Pre-conditions:
	 *
	 *    1. Input graph is a DAG.
	 *    2. Input graph node labels can be assigned properties.
	 *
	 * Post-conditions:
	 *
	 *    1. Each node will be assign an (unnormalized) "rank" property.
	 */
	function longestPath(g) {
	  var visited = {};

	  function dfs(v) {
	    var label = g.node(v);
	    if (_.has(visited, v)) {
	      return label.rank;
	    }
	    visited[v] = true;

	    var rank = _.min(_.map(g.outEdges(v), function(e) {
	      return dfs(e.w) - g.edge(e).minlen;
	    }));

	    if (rank === Number.POSITIVE_INFINITY || // return value of _.map([]) for Lodash 3
	        rank === undefined || // return value of _.map([]) for Lodash 4
	        rank === null) { // return value of _.map([null])
	      rank = 0;
	    }

	    return (label.rank = rank);
	  }

	  _.forEach(g.sources(), dfs);
	}

	/*
	 * Returns the amount of slack for the given edge. The slack is defined as the
	 * difference between the length of the edge and its minimum length.
	 */
	function slack(g, e) {
	  return g.node(e.w).rank - g.node(e.v).rank - g.edge(e).minlen;
	}
	return util;
}

var feasibleTree_1;
var hasRequiredFeasibleTree;

function requireFeasibleTree () {
	if (hasRequiredFeasibleTree) return feasibleTree_1;
	hasRequiredFeasibleTree = 1;
	"use strict";

	var _ = requireLodash();
	var Graph = requireGraphlib().Graph;
	var slack = requireUtil().slack;

	feasibleTree_1 = feasibleTree;

	/*
	 * Constructs a spanning tree with tight edges and adjusted the input node's
	 * ranks to achieve this. A tight edge is one that is has a length that matches
	 * its "minlen" attribute.
	 *
	 * The basic structure for this function is derived from Gansner, et al., "A
	 * Technique for Drawing Directed Graphs."
	 *
	 * Pre-conditions:
	 *
	 *    1. Graph must be a DAG.
	 *    2. Graph must be connected.
	 *    3. Graph must have at least one node.
	 *    5. Graph nodes must have been previously assigned a "rank" property that
	 *       respects the "minlen" property of incident edges.
	 *    6. Graph edges must have a "minlen" property.
	 *
	 * Post-conditions:
	 *
	 *    - Graph nodes will have their rank adjusted to ensure that all edges are
	 *      tight.
	 *
	 * Returns a tree (undirected graph) that is constructed using only "tight"
	 * edges.
	 */
	function feasibleTree(g) {
	  var t = new Graph({ directed: false });

	  // Choose arbitrary node from which to start our tree
	  var start = g.nodes()[0];
	  var size = g.nodeCount();
	  t.setNode(start, {});

	  var edge, delta;
	  while (tightTree(t, g) < size) {
	    edge = findMinSlackEdge(t, g);
	    delta = t.hasNode(edge.v) ? slack(g, edge) : -slack(g, edge);
	    shiftRanks(t, g, delta);
	  }

	  return t;
	}

	/*
	 * Finds a maximal tree of tight edges and returns the number of nodes in the
	 * tree.
	 */
	function tightTree(t, g) {
	  function dfs(v) {
	    _.forEach(g.nodeEdges(v), function(e) {
	      var edgeV = e.v,
	        w = (v === edgeV) ? e.w : edgeV;
	      if (!t.hasNode(w) && !slack(g, e)) {
	        t.setNode(w, {});
	        t.setEdge(v, w, {});
	        dfs(w);
	      }
	    });
	  }

	  _.forEach(t.nodes(), dfs);
	  return t.nodeCount();
	}

	/*
	 * Finds the edge with the smallest slack that is incident on tree and returns
	 * it.
	 */
	function findMinSlackEdge(t, g) {
	  return _.minBy(g.edges(), function(e) {
	    if (t.hasNode(e.v) !== t.hasNode(e.w)) {
	      return slack(g, e);
	    }
	  });
	}

	function shiftRanks(t, g, delta) {
	  _.forEach(t.nodes(), function(v) {
	    g.node(v).rank += delta;
	  });
	}
	return feasibleTree_1;
}

var networkSimplex_1;
var hasRequiredNetworkSimplex;

function requireNetworkSimplex () {
	if (hasRequiredNetworkSimplex) return networkSimplex_1;
	hasRequiredNetworkSimplex = 1;
	"use strict";

	var _ = requireLodash();
	var feasibleTree = requireFeasibleTree();
	var slack = requireUtil().slack;
	var initRank = requireUtil().longestPath;
	var preorder = requireGraphlib().alg.preorder;
	var postorder = requireGraphlib().alg.postorder;
	var simplify = requireUtil$1().simplify;

	networkSimplex_1 = networkSimplex;

	// Expose some internals for testing purposes
	networkSimplex.initLowLimValues = initLowLimValues;
	networkSimplex.initCutValues = initCutValues;
	networkSimplex.calcCutValue = calcCutValue;
	networkSimplex.leaveEdge = leaveEdge;
	networkSimplex.enterEdge = enterEdge;
	networkSimplex.exchangeEdges = exchangeEdges;

	/*
	 * The network simplex algorithm assigns ranks to each node in the input graph
	 * and iteratively improves the ranking to reduce the length of edges.
	 *
	 * Preconditions:
	 *
	 *    1. The input graph must be a DAG.
	 *    2. All nodes in the graph must have an object value.
	 *    3. All edges in the graph must have "minlen" and "weight" attributes.
	 *
	 * Postconditions:
	 *
	 *    1. All nodes in the graph will have an assigned "rank" attribute that has
	 *       been optimized by the network simplex algorithm. Ranks start at 0.
	 *
	 *
	 * A rough sketch of the algorithm is as follows:
	 *
	 *    1. Assign initial ranks to each node. We use the longest path algorithm,
	 *       which assigns ranks to the lowest position possible. In general this
	 *       leads to very wide bottom ranks and unnecessarily long edges.
	 *    2. Construct a feasible tight tree. A tight tree is one such that all
	 *       edges in the tree have no slack (difference between length of edge
	 *       and minlen for the edge). This by itself greatly improves the assigned
	 *       rankings by shorting edges.
	 *    3. Iteratively find edges that have negative cut values. Generally a
	 *       negative cut value indicates that the edge could be removed and a new
	 *       tree edge could be added to produce a more compact graph.
	 *
	 * Much of the algorithms here are derived from Gansner, et al., "A Technique
	 * for Drawing Directed Graphs." The structure of the file roughly follows the
	 * structure of the overall algorithm.
	 */
	function networkSimplex(g) {
	  g = simplify(g);
	  initRank(g);
	  var t = feasibleTree(g);
	  initLowLimValues(t);
	  initCutValues(t, g);

	  var e, f;
	  while ((e = leaveEdge(t))) {
	    f = enterEdge(t, g, e);
	    exchangeEdges(t, g, e, f);
	  }
	}

	/*
	 * Initializes cut values for all edges in the tree.
	 */
	function initCutValues(t, g) {
	  var vs = postorder(t, t.nodes());
	  vs = vs.slice(0, vs.length - 1);
	  _.forEach(vs, function(v) {
	    assignCutValue(t, g, v);
	  });
	}

	function assignCutValue(t, g, child) {
	  var childLab = t.node(child);
	  var parent = childLab.parent;
	  t.edge(child, parent).cutvalue = calcCutValue(t, g, child);
	}

	/*
	 * Given the tight tree, its graph, and a child in the graph calculate and
	 * return the cut value for the edge between the child and its parent.
	 */
	function calcCutValue(t, g, child) {
	  var childLab = t.node(child);
	  var parent = childLab.parent;
	  // True if the child is on the tail end of the edge in the directed graph
	  var childIsTail = true;
	  // The graph's view of the tree edge we're inspecting
	  var graphEdge = g.edge(child, parent);
	  // The accumulated cut value for the edge between this node and its parent
	  var cutValue = 0;

	  if (!graphEdge) {
	    childIsTail = false;
	    graphEdge = g.edge(parent, child);
	  }

	  cutValue = graphEdge.weight;

	  _.forEach(g.nodeEdges(child), function(e) {
	    var isOutEdge = e.v === child,
	      other = isOutEdge ? e.w : e.v;

	    if (other !== parent) {
	      var pointsToHead = isOutEdge === childIsTail,
	        otherWeight = g.edge(e).weight;

	      cutValue += pointsToHead ? otherWeight : -otherWeight;
	      if (isTreeEdge(t, child, other)) {
	        var otherCutValue = t.edge(child, other).cutvalue;
	        cutValue += pointsToHead ? -otherCutValue : otherCutValue;
	      }
	    }
	  });

	  return cutValue;
	}

	function initLowLimValues(tree, root) {
	  if (arguments.length < 2) {
	    root = tree.nodes()[0];
	  }
	  dfsAssignLowLim(tree, {}, 1, root);
	}

	function dfsAssignLowLim(tree, visited, nextLim, v, parent) {
	  var low = nextLim;
	  var label = tree.node(v);

	  visited[v] = true;
	  _.forEach(tree.neighbors(v), function(w) {
	    if (!_.has(visited, w)) {
	      nextLim = dfsAssignLowLim(tree, visited, nextLim, w, v);
	    }
	  });

	  label.low = low;
	  label.lim = nextLim++;
	  if (parent) {
	    label.parent = parent;
	  } else {
	    // TODO should be able to remove this when we incrementally update low lim
	    delete label.parent;
	  }

	  return nextLim;
	}

	function leaveEdge(tree) {
	  return _.find(tree.edges(), function(e) {
	    return tree.edge(e).cutvalue < 0;
	  });
	}

	function enterEdge(t, g, edge) {
	  var v = edge.v;
	  var w = edge.w;

	  // For the rest of this function we assume that v is the tail and w is the
	  // head, so if we don't have this edge in the graph we should flip it to
	  // match the correct orientation.
	  if (!g.hasEdge(v, w)) {
	    v = edge.w;
	    w = edge.v;
	  }

	  var vLabel = t.node(v);
	  var wLabel = t.node(w);
	  var tailLabel = vLabel;
	  var flip = false;

	  // If the root is in the tail of the edge then we need to flip the logic that
	  // checks for the head and tail nodes in the candidates function below.
	  if (vLabel.lim > wLabel.lim) {
	    tailLabel = wLabel;
	    flip = true;
	  }

	  var candidates = _.filter(g.edges(), function(edge) {
	    return flip === isDescendant(t, t.node(edge.v), tailLabel) &&
	           flip !== isDescendant(t, t.node(edge.w), tailLabel);
	  });

	  return _.minBy(candidates, function(edge) { return slack(g, edge); });
	}

	function exchangeEdges(t, g, e, f) {
	  var v = e.v;
	  var w = e.w;
	  t.removeEdge(v, w);
	  t.setEdge(f.v, f.w, {});
	  initLowLimValues(t);
	  initCutValues(t, g);
	  updateRanks(t, g);
	}

	function updateRanks(t, g) {
	  var root = _.find(t.nodes(), function(v) { return !g.node(v).parent; });
	  var vs = preorder(t, root);
	  vs = vs.slice(1);
	  _.forEach(vs, function(v) {
	    var parent = t.node(v).parent,
	      edge = g.edge(v, parent),
	      flipped = false;

	    if (!edge) {
	      edge = g.edge(parent, v);
	      flipped = true;
	    }

	    g.node(v).rank = g.node(parent).rank + (flipped ? edge.minlen : -edge.minlen);
	  });
	}

	/*
	 * Returns true if the edge is in the tree.
	 */
	function isTreeEdge(tree, u, v) {
	  return tree.hasEdge(u, v);
	}

	/*
	 * Returns true if the specified node is descendant of the root node per the
	 * assigned low and lim attributes in the tree.
	 */
	function isDescendant(tree, vLabel, rootLabel) {
	  return rootLabel.low <= vLabel.lim && vLabel.lim <= rootLabel.lim;
	}
	return networkSimplex_1;
}

var rank_1;
var hasRequiredRank;

function requireRank () {
	if (hasRequiredRank) return rank_1;
	hasRequiredRank = 1;
	"use strict";

	var rankUtil = requireUtil();
	var longestPath = rankUtil.longestPath;
	var feasibleTree = requireFeasibleTree();
	var networkSimplex = requireNetworkSimplex();

	rank_1 = rank;

	/*
	 * Assigns a rank to each node in the input graph that respects the "minlen"
	 * constraint specified on edges between nodes.
	 *
	 * This basic structure is derived from Gansner, et al., "A Technique for
	 * Drawing Directed Graphs."
	 *
	 * Pre-conditions:
	 *
	 *    1. Graph must be a connected DAG
	 *    2. Graph nodes must be objects
	 *    3. Graph edges must have "weight" and "minlen" attributes
	 *
	 * Post-conditions:
	 *
	 *    1. Graph nodes will have a "rank" attribute based on the results of the
	 *       algorithm. Ranks can start at any index (including negative), we'll
	 *       fix them up later.
	 */
	function rank(g) {
	  switch(g.graph().ranker) {
	  case "network-simplex": networkSimplexRanker(g); break;
	  case "tight-tree": tightTreeRanker(g); break;
	  case "longest-path": longestPathRanker(g); break;
	  default: networkSimplexRanker(g);
	  }
	}

	// A fast and simple ranker, but results are far from optimal.
	var longestPathRanker = longestPath;

	function tightTreeRanker(g) {
	  longestPath(g);
	  feasibleTree(g);
	}

	function networkSimplexRanker(g) {
	  networkSimplex(g);
	}
	return rank_1;
}

var parentDummyChains_1;
var hasRequiredParentDummyChains;

function requireParentDummyChains () {
	if (hasRequiredParentDummyChains) return parentDummyChains_1;
	hasRequiredParentDummyChains = 1;
	var _ = requireLodash();

	parentDummyChains_1 = parentDummyChains;

	function parentDummyChains(g) {
	  var postorderNums = postorder(g);

	  _.forEach(g.graph().dummyChains, function(v) {
	    var node = g.node(v);
	    var edgeObj = node.edgeObj;
	    var pathData = findPath(g, postorderNums, edgeObj.v, edgeObj.w);
	    var path = pathData.path;
	    var lca = pathData.lca;
	    var pathIdx = 0;
	    var pathV = path[pathIdx];
	    var ascending = true;

	    while (v !== edgeObj.w) {
	      node = g.node(v);

	      if (ascending) {
	        while ((pathV = path[pathIdx]) !== lca &&
	               g.node(pathV).maxRank < node.rank) {
	          pathIdx++;
	        }

	        if (pathV === lca) {
	          ascending = false;
	        }
	      }

	      if (!ascending) {
	        while (pathIdx < path.length - 1 &&
	               g.node(pathV = path[pathIdx + 1]).minRank <= node.rank) {
	          pathIdx++;
	        }
	        pathV = path[pathIdx];
	      }

	      g.setParent(v, pathV);
	      v = g.successors(v)[0];
	    }
	  });
	}

	// Find a path from v to w through the lowest common ancestor (LCA). Return the
	// full path and the LCA.
	function findPath(g, postorderNums, v, w) {
	  var vPath = [];
	  var wPath = [];
	  var low = Math.min(postorderNums[v].low, postorderNums[w].low);
	  var lim = Math.max(postorderNums[v].lim, postorderNums[w].lim);
	  var parent;
	  var lca;

	  // Traverse up from v to find the LCA
	  parent = v;
	  do {
	    parent = g.parent(parent);
	    vPath.push(parent);
	  } while (parent &&
	           (postorderNums[parent].low > low || lim > postorderNums[parent].lim));
	  lca = parent;

	  // Traverse from w to LCA
	  parent = w;
	  while ((parent = g.parent(parent)) !== lca) {
	    wPath.push(parent);
	  }

	  return { path: vPath.concat(wPath.reverse()), lca: lca };
	}

	function postorder(g) {
	  var result = {};
	  var lim = 0;

	  function dfs(v) {
	    var low = lim;
	    _.forEach(g.children(v), dfs);
	    result[v] = { low: low, lim: lim++ };
	  }
	  _.forEach(g.children(), dfs);

	  return result;
	}
	return parentDummyChains_1;
}

var nestingGraph;
var hasRequiredNestingGraph;

function requireNestingGraph () {
	if (hasRequiredNestingGraph) return nestingGraph;
	hasRequiredNestingGraph = 1;
	var _ = requireLodash();
	var util = requireUtil$1();

	nestingGraph = {
	  run: run,
	  cleanup: cleanup
	};

	/*
	 * A nesting graph creates dummy nodes for the tops and bottoms of subgraphs,
	 * adds appropriate edges to ensure that all cluster nodes are placed between
	 * these boundries, and ensures that the graph is connected.
	 *
	 * In addition we ensure, through the use of the minlen property, that nodes
	 * and subgraph border nodes to not end up on the same rank.
	 *
	 * Preconditions:
	 *
	 *    1. Input graph is a DAG
	 *    2. Nodes in the input graph has a minlen attribute
	 *
	 * Postconditions:
	 *
	 *    1. Input graph is connected.
	 *    2. Dummy nodes are added for the tops and bottoms of subgraphs.
	 *    3. The minlen attribute for nodes is adjusted to ensure nodes do not
	 *       get placed on the same rank as subgraph border nodes.
	 *
	 * The nesting graph idea comes from Sander, "Layout of Compound Directed
	 * Graphs."
	 */
	function run(g) {
	  var root = util.addDummyNode(g, "root", {}, "_root");
	  var depths = treeDepths(g);
	  var height = _.max(_.values(depths)) - 1; // Note: depths is an Object not an array
	  var nodeSep = 2 * height + 1;

	  g.graph().nestingRoot = root;

	  // Multiply minlen by nodeSep to align nodes on non-border ranks.
	  _.forEach(g.edges(), function(e) { g.edge(e).minlen *= nodeSep; });

	  // Calculate a weight that is sufficient to keep subgraphs vertically compact
	  var weight = sumWeights(g) + 1;

	  // Create border nodes and link them up
	  _.forEach(g.children(), function(child) {
	    dfs(g, root, nodeSep, weight, height, depths, child);
	  });

	  // Save the multiplier for node layers for later removal of empty border
	  // layers.
	  g.graph().nodeRankFactor = nodeSep;
	}

	function dfs(g, root, nodeSep, weight, height, depths, v) {
	  var children = g.children(v);
	  if (!children.length) {
	    if (v !== root) {
	      g.setEdge(root, v, { weight: 0, minlen: nodeSep });
	    }
	    return;
	  }

	  var top = util.addBorderNode(g, "_bt");
	  var bottom = util.addBorderNode(g, "_bb");
	  var label = g.node(v);

	  g.setParent(top, v);
	  label.borderTop = top;
	  g.setParent(bottom, v);
	  label.borderBottom = bottom;

	  _.forEach(children, function(child) {
	    dfs(g, root, nodeSep, weight, height, depths, child);

	    var childNode = g.node(child);
	    var childTop = childNode.borderTop ? childNode.borderTop : child;
	    var childBottom = childNode.borderBottom ? childNode.borderBottom : child;
	    var thisWeight = childNode.borderTop ? weight : 2 * weight;
	    var minlen = childTop !== childBottom ? 1 : height - depths[v] + 1;

	    g.setEdge(top, childTop, {
	      weight: thisWeight,
	      minlen: minlen,
	      nestingEdge: true
	    });

	    g.setEdge(childBottom, bottom, {
	      weight: thisWeight,
	      minlen: minlen,
	      nestingEdge: true
	    });
	  });

	  if (!g.parent(v)) {
	    g.setEdge(root, top, { weight: 0, minlen: height + depths[v] });
	  }
	}

	function treeDepths(g) {
	  var depths = {};
	  function dfs(v, depth) {
	    var children = g.children(v);
	    if (children && children.length) {
	      _.forEach(children, function(child) {
	        dfs(child, depth + 1);
	      });
	    }
	    depths[v] = depth;
	  }
	  _.forEach(g.children(), function(v) { dfs(v, 1); });
	  return depths;
	}

	function sumWeights(g) {
	  return _.reduce(g.edges(), function(acc, e) {
	    return acc + g.edge(e).weight;
	  }, 0);
	}

	function cleanup(g) {
	  var graphLabel = g.graph();
	  g.removeNode(graphLabel.nestingRoot);
	  delete graphLabel.nestingRoot;
	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    if (edge.nestingEdge) {
	      g.removeEdge(e);
	    }
	  });
	}
	return nestingGraph;
}

var addBorderSegments_1;
var hasRequiredAddBorderSegments;

function requireAddBorderSegments () {
	if (hasRequiredAddBorderSegments) return addBorderSegments_1;
	hasRequiredAddBorderSegments = 1;
	var _ = requireLodash();
	var util = requireUtil$1();

	addBorderSegments_1 = addBorderSegments;

	function addBorderSegments(g) {
	  function dfs(v) {
	    var children = g.children(v);
	    var node = g.node(v);
	    if (children.length) {
	      _.forEach(children, dfs);
	    }

	    if (_.has(node, "minRank")) {
	      node.borderLeft = [];
	      node.borderRight = [];
	      for (var rank = node.minRank, maxRank = node.maxRank + 1;
	        rank < maxRank;
	        ++rank) {
	        addBorderNode(g, "borderLeft", "_bl", v, node, rank);
	        addBorderNode(g, "borderRight", "_br", v, node, rank);
	      }
	    }
	  }

	  _.forEach(g.children(), dfs);
	}

	function addBorderNode(g, prop, prefix, sg, sgNode, rank) {
	  var label = { width: 0, height: 0, rank: rank, borderType: prop };
	  var prev = sgNode[prop][rank - 1];
	  var curr = util.addDummyNode(g, "border", label, prefix);
	  sgNode[prop][rank] = curr;
	  g.setParent(curr, sg);
	  if (prev) {
	    g.setEdge(prev, curr, { weight: 1 });
	  }
	}
	return addBorderSegments_1;
}

var coordinateSystem;
var hasRequiredCoordinateSystem;

function requireCoordinateSystem () {
	if (hasRequiredCoordinateSystem) return coordinateSystem;
	hasRequiredCoordinateSystem = 1;
	"use strict";

	var _ = requireLodash();

	coordinateSystem = {
	  adjust: adjust,
	  undo: undo
	};

	function adjust(g) {
	  var rankDir = g.graph().rankdir.toLowerCase();
	  if (rankDir === "lr" || rankDir === "rl") {
	    swapWidthHeight(g);
	  }
	}

	function undo(g) {
	  var rankDir = g.graph().rankdir.toLowerCase();
	  if (rankDir === "bt" || rankDir === "rl") {
	    reverseY(g);
	  }

	  if (rankDir === "lr" || rankDir === "rl") {
	    swapXY(g);
	    swapWidthHeight(g);
	  }
	}

	function swapWidthHeight(g) {
	  _.forEach(g.nodes(), function(v) { swapWidthHeightOne(g.node(v)); });
	  _.forEach(g.edges(), function(e) { swapWidthHeightOne(g.edge(e)); });
	}

	function swapWidthHeightOne(attrs) {
	  var w = attrs.width;
	  attrs.width = attrs.height;
	  attrs.height = w;
	}

	function reverseY(g) {
	  _.forEach(g.nodes(), function(v) { reverseYOne(g.node(v)); });

	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    _.forEach(edge.points, reverseYOne);
	    if (_.has(edge, "y")) {
	      reverseYOne(edge);
	    }
	  });
	}

	function reverseYOne(attrs) {
	  attrs.y = -attrs.y;
	}

	function swapXY(g) {
	  _.forEach(g.nodes(), function(v) { swapXYOne(g.node(v)); });

	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    _.forEach(edge.points, swapXYOne);
	    if (_.has(edge, "x")) {
	      swapXYOne(edge);
	    }
	  });
	}

	function swapXYOne(attrs) {
	  var x = attrs.x;
	  attrs.x = attrs.y;
	  attrs.y = x;
	}
	return coordinateSystem;
}

var initOrder_1;
var hasRequiredInitOrder;

function requireInitOrder () {
	if (hasRequiredInitOrder) return initOrder_1;
	hasRequiredInitOrder = 1;
	"use strict";

	var _ = requireLodash();

	initOrder_1 = initOrder;

	/*
	 * Assigns an initial order value for each node by performing a DFS search
	 * starting from nodes in the first rank. Nodes are assigned an order in their
	 * rank as they are first visited.
	 *
	 * This approach comes from Gansner, et al., "A Technique for Drawing Directed
	 * Graphs."
	 *
	 * Returns a layering matrix with an array per layer and each layer sorted by
	 * the order of its nodes.
	 */
	function initOrder(g) {
	  var visited = {};
	  var simpleNodes = _.filter(g.nodes(), function(v) {
	    return !g.children(v).length;
	  });
	  var maxRank = _.max(_.map(simpleNodes, function(v) { return g.node(v).rank; }));
	  var layers = _.map(_.range(maxRank + 1), function() { return []; });

	  function dfs(v) {
	    if (_.has(visited, v)) return;
	    visited[v] = true;
	    var node = g.node(v);
	    layers[node.rank].push(v);
	    _.forEach(g.successors(v), dfs);
	  }

	  var orderedVs = _.sortBy(simpleNodes, function(v) { return g.node(v).rank; });
	  _.forEach(orderedVs, dfs);

	  return layers;
	}
	return initOrder_1;
}

var crossCount_1;
var hasRequiredCrossCount;

function requireCrossCount () {
	if (hasRequiredCrossCount) return crossCount_1;
	hasRequiredCrossCount = 1;
	"use strict";

	var _ = requireLodash();

	crossCount_1 = crossCount;

	/*
	 * A function that takes a layering (an array of layers, each with an array of
	 * ordererd nodes) and a graph and returns a weighted crossing count.
	 *
	 * Pre-conditions:
	 *
	 *    1. Input graph must be simple (not a multigraph), directed, and include
	 *       only simple edges.
	 *    2. Edges in the input graph must have assigned weights.
	 *
	 * Post-conditions:
	 *
	 *    1. The graph and layering matrix are left unchanged.
	 *
	 * This algorithm is derived from Barth, et al., "Bilayer Cross Counting."
	 */
	function crossCount(g, layering) {
	  var cc = 0;
	  for (var i = 1; i < layering.length; ++i) {
	    cc += twoLayerCrossCount(g, layering[i-1], layering[i]);
	  }
	  return cc;
	}

	function twoLayerCrossCount(g, northLayer, southLayer) {
	  // Sort all of the edges between the north and south layers by their position
	  // in the north layer and then the south. Map these edges to the position of
	  // their head in the south layer.
	  var southPos = _.zipObject(southLayer,
	    _.map(southLayer, function (v, i) { return i; }));
	  var southEntries = _.flatten(_.map(northLayer, function(v) {
	    return _.sortBy(_.map(g.outEdges(v), function(e) {
	      return { pos: southPos[e.w], weight: g.edge(e).weight };
	    }), "pos");
	  }), true);

	  // Build the accumulator tree
	  var firstIndex = 1;
	  while (firstIndex < southLayer.length) firstIndex <<= 1;
	  var treeSize = 2 * firstIndex - 1;
	  firstIndex -= 1;
	  var tree = _.map(new Array(treeSize), function() { return 0; });

	  // Calculate the weighted crossings
	  var cc = 0;
	  _.forEach(southEntries.forEach(function(entry) {
	    var index = entry.pos + firstIndex;
	    tree[index] += entry.weight;
	    var weightSum = 0;
	    while (index > 0) {
	      if (index % 2) {
	        weightSum += tree[index + 1];
	      }
	      index = (index - 1) >> 1;
	      tree[index] += entry.weight;
	    }
	    cc += entry.weight * weightSum;
	  }));

	  return cc;
	}
	return crossCount_1;
}

var barycenter_1;
var hasRequiredBarycenter;

function requireBarycenter () {
	if (hasRequiredBarycenter) return barycenter_1;
	hasRequiredBarycenter = 1;
	var _ = requireLodash();

	barycenter_1 = barycenter;

	function barycenter(g, movable) {
	  return _.map(movable, function(v) {
	    var inV = g.inEdges(v);
	    if (!inV.length) {
	      return { v: v };
	    } else {
	      var result = _.reduce(inV, function(acc, e) {
	        var edge = g.edge(e),
	          nodeU = g.node(e.v);
	        return {
	          sum: acc.sum + (edge.weight * nodeU.order),
	          weight: acc.weight + edge.weight
	        };
	      }, { sum: 0, weight: 0 });

	      return {
	        v: v,
	        barycenter: result.sum / result.weight,
	        weight: result.weight
	      };
	    }
	  });
	}
	return barycenter_1;
}

var resolveConflicts_1;
var hasRequiredResolveConflicts;

function requireResolveConflicts () {
	if (hasRequiredResolveConflicts) return resolveConflicts_1;
	hasRequiredResolveConflicts = 1;
	"use strict";

	var _ = requireLodash();

	resolveConflicts_1 = resolveConflicts;

	/*
	 * Given a list of entries of the form {v, barycenter, weight} and a
	 * constraint graph this function will resolve any conflicts between the
	 * constraint graph and the barycenters for the entries. If the barycenters for
	 * an entry would violate a constraint in the constraint graph then we coalesce
	 * the nodes in the conflict into a new node that respects the contraint and
	 * aggregates barycenter and weight information.
	 *
	 * This implementation is based on the description in Forster, "A Fast and
	 * Simple Hueristic for Constrained Two-Level Crossing Reduction," thought it
	 * differs in some specific details.
	 *
	 * Pre-conditions:
	 *
	 *    1. Each entry has the form {v, barycenter, weight}, or if the node has
	 *       no barycenter, then {v}.
	 *
	 * Returns:
	 *
	 *    A new list of entries of the form {vs, i, barycenter, weight}. The list
	 *    `vs` may either be a singleton or it may be an aggregation of nodes
	 *    ordered such that they do not violate constraints from the constraint
	 *    graph. The property `i` is the lowest original index of any of the
	 *    elements in `vs`.
	 */
	function resolveConflicts(entries, cg) {
	  var mappedEntries = {};
	  _.forEach(entries, function(entry, i) {
	    var tmp = mappedEntries[entry.v] = {
	      indegree: 0,
	      "in": [],
	      out: [],
	      vs: [entry.v],
	      i: i
	    };
	    if (!_.isUndefined(entry.barycenter)) {
	      tmp.barycenter = entry.barycenter;
	      tmp.weight = entry.weight;
	    }
	  });

	  _.forEach(cg.edges(), function(e) {
	    var entryV = mappedEntries[e.v];
	    var entryW = mappedEntries[e.w];
	    if (!_.isUndefined(entryV) && !_.isUndefined(entryW)) {
	      entryW.indegree++;
	      entryV.out.push(mappedEntries[e.w]);
	    }
	  });

	  var sourceSet = _.filter(mappedEntries, function(entry) {
	    return !entry.indegree;
	  });

	  return doResolveConflicts(sourceSet);
	}

	function doResolveConflicts(sourceSet) {
	  var entries = [];

	  function handleIn(vEntry) {
	    return function(uEntry) {
	      if (uEntry.merged) {
	        return;
	      }
	      if (_.isUndefined(uEntry.barycenter) ||
	          _.isUndefined(vEntry.barycenter) ||
	          uEntry.barycenter >= vEntry.barycenter) {
	        mergeEntries(vEntry, uEntry);
	      }
	    };
	  }

	  function handleOut(vEntry) {
	    return function(wEntry) {
	      wEntry["in"].push(vEntry);
	      if (--wEntry.indegree === 0) {
	        sourceSet.push(wEntry);
	      }
	    };
	  }

	  while (sourceSet.length) {
	    var entry = sourceSet.pop();
	    entries.push(entry);
	    _.forEach(entry["in"].reverse(), handleIn(entry));
	    _.forEach(entry.out, handleOut(entry));
	  }

	  return _.map(_.filter(entries, function(entry) { return !entry.merged; }),
	    function(entry) {
	      return _.pick(entry, ["vs", "i", "barycenter", "weight"]);
	    });

	}

	function mergeEntries(target, source) {
	  var sum = 0;
	  var weight = 0;

	  if (target.weight) {
	    sum += target.barycenter * target.weight;
	    weight += target.weight;
	  }

	  if (source.weight) {
	    sum += source.barycenter * source.weight;
	    weight += source.weight;
	  }

	  target.vs = source.vs.concat(target.vs);
	  target.barycenter = sum / weight;
	  target.weight = weight;
	  target.i = Math.min(source.i, target.i);
	  source.merged = true;
	}
	return resolveConflicts_1;
}

var sort_1;
var hasRequiredSort;

function requireSort () {
	if (hasRequiredSort) return sort_1;
	hasRequiredSort = 1;
	var _ = requireLodash();
	var util = requireUtil$1();

	sort_1 = sort;

	function sort(entries, biasRight) {
	  var parts = util.partition(entries, function(entry) {
	    return _.has(entry, "barycenter");
	  });
	  var sortable = parts.lhs,
	    unsortable = _.sortBy(parts.rhs, function(entry) { return -entry.i; }),
	    vs = [],
	    sum = 0,
	    weight = 0,
	    vsIndex = 0;

	  sortable.sort(compareWithBias(!!biasRight));

	  vsIndex = consumeUnsortable(vs, unsortable, vsIndex);

	  _.forEach(sortable, function (entry) {
	    vsIndex += entry.vs.length;
	    vs.push(entry.vs);
	    sum += entry.barycenter * entry.weight;
	    weight += entry.weight;
	    vsIndex = consumeUnsortable(vs, unsortable, vsIndex);
	  });

	  var result = { vs: _.flatten(vs, true) };
	  if (weight) {
	    result.barycenter = sum / weight;
	    result.weight = weight;
	  }
	  return result;
	}

	function consumeUnsortable(vs, unsortable, index) {
	  var last;
	  while (unsortable.length && (last = _.last(unsortable)).i <= index) {
	    unsortable.pop();
	    vs.push(last.vs);
	    index++;
	  }
	  return index;
	}

	function compareWithBias(bias) {
	  return function(entryV, entryW) {
	    if (entryV.barycenter < entryW.barycenter) {
	      return -1;
	    } else if (entryV.barycenter > entryW.barycenter) {
	      return 1;
	    }

	    return !bias ? entryV.i - entryW.i : entryW.i - entryV.i;
	  };
	}
	return sort_1;
}

var sortSubgraph_1;
var hasRequiredSortSubgraph;

function requireSortSubgraph () {
	if (hasRequiredSortSubgraph) return sortSubgraph_1;
	hasRequiredSortSubgraph = 1;
	var _ = requireLodash();
	var barycenter = requireBarycenter();
	var resolveConflicts = requireResolveConflicts();
	var sort = requireSort();

	sortSubgraph_1 = sortSubgraph;

	function sortSubgraph(g, v, cg, biasRight) {
	  var movable = g.children(v);
	  var node = g.node(v);
	  var bl = node ? node.borderLeft : undefined;
	  var br = node ? node.borderRight: undefined;
	  var subgraphs = {};

	  if (bl) {
	    movable = _.filter(movable, function(w) {
	      return w !== bl && w !== br;
	    });
	  }

	  var barycenters = barycenter(g, movable);
	  _.forEach(barycenters, function(entry) {
	    if (g.children(entry.v).length) {
	      var subgraphResult = sortSubgraph(g, entry.v, cg, biasRight);
	      subgraphs[entry.v] = subgraphResult;
	      if (_.has(subgraphResult, "barycenter")) {
	        mergeBarycenters(entry, subgraphResult);
	      }
	    }
	  });

	  var entries = resolveConflicts(barycenters, cg);
	  expandSubgraphs(entries, subgraphs);

	  var result = sort(entries, biasRight);

	  if (bl) {
	    result.vs = _.flatten([bl, result.vs, br], true);
	    if (g.predecessors(bl).length) {
	      var blPred = g.node(g.predecessors(bl)[0]),
	        brPred = g.node(g.predecessors(br)[0]);
	      if (!_.has(result, "barycenter")) {
	        result.barycenter = 0;
	        result.weight = 0;
	      }
	      result.barycenter = (result.barycenter * result.weight +
	                           blPred.order + brPred.order) / (result.weight + 2);
	      result.weight += 2;
	    }
	  }

	  return result;
	}

	function expandSubgraphs(entries, subgraphs) {
	  _.forEach(entries, function(entry) {
	    entry.vs = _.flatten(entry.vs.map(function(v) {
	      if (subgraphs[v]) {
	        return subgraphs[v].vs;
	      }
	      return v;
	    }), true);
	  });
	}

	function mergeBarycenters(target, other) {
	  if (!_.isUndefined(target.barycenter)) {
	    target.barycenter = (target.barycenter * target.weight +
	                         other.barycenter * other.weight) /
	                        (target.weight + other.weight);
	    target.weight += other.weight;
	  } else {
	    target.barycenter = other.barycenter;
	    target.weight = other.weight;
	  }
	}
	return sortSubgraph_1;
}

var buildLayerGraph_1;
var hasRequiredBuildLayerGraph;

function requireBuildLayerGraph () {
	if (hasRequiredBuildLayerGraph) return buildLayerGraph_1;
	hasRequiredBuildLayerGraph = 1;
	var _ = requireLodash();
	var Graph = requireGraphlib().Graph;

	buildLayerGraph_1 = buildLayerGraph;

	/*
	 * Constructs a graph that can be used to sort a layer of nodes. The graph will
	 * contain all base and subgraph nodes from the request layer in their original
	 * hierarchy and any edges that are incident on these nodes and are of the type
	 * requested by the "relationship" parameter.
	 *
	 * Nodes from the requested rank that do not have parents are assigned a root
	 * node in the output graph, which is set in the root graph attribute. This
	 * makes it easy to walk the hierarchy of movable nodes during ordering.
	 *
	 * Pre-conditions:
	 *
	 *    1. Input graph is a DAG
	 *    2. Base nodes in the input graph have a rank attribute
	 *    3. Subgraph nodes in the input graph has minRank and maxRank attributes
	 *    4. Edges have an assigned weight
	 *
	 * Post-conditions:
	 *
	 *    1. Output graph has all nodes in the movable rank with preserved
	 *       hierarchy.
	 *    2. Root nodes in the movable layer are made children of the node
	 *       indicated by the root attribute of the graph.
	 *    3. Non-movable nodes incident on movable nodes, selected by the
	 *       relationship parameter, are included in the graph (without hierarchy).
	 *    4. Edges incident on movable nodes, selected by the relationship
	 *       parameter, are added to the output graph.
	 *    5. The weights for copied edges are aggregated as need, since the output
	 *       graph is not a multi-graph.
	 */
	function buildLayerGraph(g, rank, relationship) {
	  var root = createRootNode(g),
	    result = new Graph({ compound: true }).setGraph({ root: root })
	      .setDefaultNodeLabel(function(v) { return g.node(v); });

	  _.forEach(g.nodes(), function(v) {
	    var node = g.node(v),
	      parent = g.parent(v);

	    if (node.rank === rank || node.minRank <= rank && rank <= node.maxRank) {
	      result.setNode(v);
	      result.setParent(v, parent || root);

	      // This assumes we have only short edges!
	      _.forEach(g[relationship](v), function(e) {
	        var u = e.v === v ? e.w : e.v,
	          edge = result.edge(u, v),
	          weight = !_.isUndefined(edge) ? edge.weight : 0;
	        result.setEdge(u, v, { weight: g.edge(e).weight + weight });
	      });

	      if (_.has(node, "minRank")) {
	        result.setNode(v, {
	          borderLeft: node.borderLeft[rank],
	          borderRight: node.borderRight[rank]
	        });
	      }
	    }
	  });

	  return result;
	}

	function createRootNode(g) {
	  var v;
	  while (g.hasNode((v = _.uniqueId("_root"))));
	  return v;
	}
	return buildLayerGraph_1;
}

var addSubgraphConstraints_1;
var hasRequiredAddSubgraphConstraints;

function requireAddSubgraphConstraints () {
	if (hasRequiredAddSubgraphConstraints) return addSubgraphConstraints_1;
	hasRequiredAddSubgraphConstraints = 1;
	var _ = requireLodash();

	addSubgraphConstraints_1 = addSubgraphConstraints;

	function addSubgraphConstraints(g, cg, vs) {
	  var prev = {},
	    rootPrev;

	  _.forEach(vs, function(v) {
	    var child = g.parent(v),
	      parent,
	      prevChild;
	    while (child) {
	      parent = g.parent(child);
	      if (parent) {
	        prevChild = prev[parent];
	        prev[parent] = child;
	      } else {
	        prevChild = rootPrev;
	        rootPrev = child;
	      }
	      if (prevChild && prevChild !== child) {
	        cg.setEdge(prevChild, child);
	        return;
	      }
	      child = parent;
	    }
	  });

	  /*
	  function dfs(v) {
	    var children = v ? g.children(v) : g.children();
	    if (children.length) {
	      var min = Number.POSITIVE_INFINITY,
	          subgraphs = [];
	      _.each(children, function(child) {
	        var childMin = dfs(child);
	        if (g.children(child).length) {
	          subgraphs.push({ v: child, order: childMin });
	        }
	        min = Math.min(min, childMin);
	      });
	      _.reduce(_.sortBy(subgraphs, "order"), function(prev, curr) {
	        cg.setEdge(prev.v, curr.v);
	        return curr;
	      });
	      return min;
	    }
	    return g.node(v).order;
	  }
	  dfs(undefined);
	  */
	}
	return addSubgraphConstraints_1;
}

var order_1;
var hasRequiredOrder;

function requireOrder () {
	if (hasRequiredOrder) return order_1;
	hasRequiredOrder = 1;
	"use strict";

	var _ = requireLodash();
	var initOrder = requireInitOrder();
	var crossCount = requireCrossCount();
	var sortSubgraph = requireSortSubgraph();
	var buildLayerGraph = requireBuildLayerGraph();
	var addSubgraphConstraints = requireAddSubgraphConstraints();
	var Graph = requireGraphlib().Graph;
	var util = requireUtil$1();

	order_1 = order;

	/*
	 * Applies heuristics to minimize edge crossings in the graph and sets the best
	 * order solution as an order attribute on each node.
	 *
	 * Pre-conditions:
	 *
	 *    1. Graph must be DAG
	 *    2. Graph nodes must be objects with a "rank" attribute
	 *    3. Graph edges must have the "weight" attribute
	 *
	 * Post-conditions:
	 *
	 *    1. Graph nodes will have an "order" attribute based on the results of the
	 *       algorithm.
	 */
	function order(g) {
	  var maxRank = util.maxRank(g),
	    downLayerGraphs = buildLayerGraphs(g, _.range(1, maxRank + 1), "inEdges"),
	    upLayerGraphs = buildLayerGraphs(g, _.range(maxRank - 1, -1, -1), "outEdges");

	  var layering = initOrder(g);
	  assignOrder(g, layering);

	  var bestCC = Number.POSITIVE_INFINITY,
	    best;

	  for (var i = 0, lastBest = 0; lastBest < 4; ++i, ++lastBest) {
	    sweepLayerGraphs(i % 2 ? downLayerGraphs : upLayerGraphs, i % 4 >= 2);

	    layering = util.buildLayerMatrix(g);
	    var cc = crossCount(g, layering);
	    if (cc < bestCC) {
	      lastBest = 0;
	      best = _.cloneDeep(layering);
	      bestCC = cc;
	    }
	  }

	  assignOrder(g, best);
	}

	function buildLayerGraphs(g, ranks, relationship) {
	  return _.map(ranks, function(rank) {
	    return buildLayerGraph(g, rank, relationship);
	  });
	}

	function sweepLayerGraphs(layerGraphs, biasRight) {
	  var cg = new Graph();
	  _.forEach(layerGraphs, function(lg) {
	    var root = lg.graph().root;
	    var sorted = sortSubgraph(lg, root, cg, biasRight);
	    _.forEach(sorted.vs, function(v, i) {
	      lg.node(v).order = i;
	    });
	    addSubgraphConstraints(lg, cg, sorted.vs);
	  });
	}

	function assignOrder(g, layering) {
	  _.forEach(layering, function(layer) {
	    _.forEach(layer, function(v, i) {
	      g.node(v).order = i;
	    });
	  });
	}
	return order_1;
}

var bk;
var hasRequiredBk;

function requireBk () {
	if (hasRequiredBk) return bk;
	hasRequiredBk = 1;
	"use strict";

	var _ = requireLodash();
	var Graph = requireGraphlib().Graph;
	var util = requireUtil$1();

	/*
	 * This module provides coordinate assignment based on Brandes and Köpf, "Fast
	 * and Simple Horizontal Coordinate Assignment."
	 */

	bk = {
	  positionX: positionX,
	  findType1Conflicts: findType1Conflicts,
	  findType2Conflicts: findType2Conflicts,
	  addConflict: addConflict,
	  hasConflict: hasConflict,
	  verticalAlignment: verticalAlignment,
	  horizontalCompaction: horizontalCompaction,
	  alignCoordinates: alignCoordinates,
	  findSmallestWidthAlignment: findSmallestWidthAlignment,
	  balance: balance
	};

	/*
	 * Marks all edges in the graph with a type-1 conflict with the "type1Conflict"
	 * property. A type-1 conflict is one where a non-inner segment crosses an
	 * inner segment. An inner segment is an edge with both incident nodes marked
	 * with the "dummy" property.
	 *
	 * This algorithm scans layer by layer, starting with the second, for type-1
	 * conflicts between the current layer and the previous layer. For each layer
	 * it scans the nodes from left to right until it reaches one that is incident
	 * on an inner segment. It then scans predecessors to determine if they have
	 * edges that cross that inner segment. At the end a final scan is done for all
	 * nodes on the current rank to see if they cross the last visited inner
	 * segment.
	 *
	 * This algorithm (safely) assumes that a dummy node will only be incident on a
	 * single node in the layers being scanned.
	 */
	function findType1Conflicts(g, layering) {
	  var conflicts = {};

	  function visitLayer(prevLayer, layer) {
	    var
	      // last visited node in the previous layer that is incident on an inner
	      // segment.
	      k0 = 0,
	      // Tracks the last node in this layer scanned for crossings with a type-1
	      // segment.
	      scanPos = 0,
	      prevLayerLength = prevLayer.length,
	      lastNode = _.last(layer);

	    _.forEach(layer, function(v, i) {
	      var w = findOtherInnerSegmentNode(g, v),
	        k1 = w ? g.node(w).order : prevLayerLength;

	      if (w || v === lastNode) {
	        _.forEach(layer.slice(scanPos, i +1), function(scanNode) {
	          _.forEach(g.predecessors(scanNode), function(u) {
	            var uLabel = g.node(u),
	              uPos = uLabel.order;
	            if ((uPos < k0 || k1 < uPos) &&
	                !(uLabel.dummy && g.node(scanNode).dummy)) {
	              addConflict(conflicts, u, scanNode);
	            }
	          });
	        });
	        scanPos = i + 1;
	        k0 = k1;
	      }
	    });

	    return layer;
	  }

	  _.reduce(layering, visitLayer);
	  return conflicts;
	}

	function findType2Conflicts(g, layering) {
	  var conflicts = {};

	  function scan(south, southPos, southEnd, prevNorthBorder, nextNorthBorder) {
	    var v;
	    _.forEach(_.range(southPos, southEnd), function(i) {
	      v = south[i];
	      if (g.node(v).dummy) {
	        _.forEach(g.predecessors(v), function(u) {
	          var uNode = g.node(u);
	          if (uNode.dummy &&
	              (uNode.order < prevNorthBorder || uNode.order > nextNorthBorder)) {
	            addConflict(conflicts, u, v);
	          }
	        });
	      }
	    });
	  }


	  function visitLayer(north, south) {
	    var prevNorthPos = -1,
	      nextNorthPos,
	      southPos = 0;

	    _.forEach(south, function(v, southLookahead) {
	      if (g.node(v).dummy === "border") {
	        var predecessors = g.predecessors(v);
	        if (predecessors.length) {
	          nextNorthPos = g.node(predecessors[0]).order;
	          scan(south, southPos, southLookahead, prevNorthPos, nextNorthPos);
	          southPos = southLookahead;
	          prevNorthPos = nextNorthPos;
	        }
	      }
	      scan(south, southPos, south.length, nextNorthPos, north.length);
	    });

	    return south;
	  }

	  _.reduce(layering, visitLayer);
	  return conflicts;
	}

	function findOtherInnerSegmentNode(g, v) {
	  if (g.node(v).dummy) {
	    return _.find(g.predecessors(v), function(u) {
	      return g.node(u).dummy;
	    });
	  }
	}

	function addConflict(conflicts, v, w) {
	  if (v > w) {
	    var tmp = v;
	    v = w;
	    w = tmp;
	  }

	  var conflictsV = conflicts[v];
	  if (!conflictsV) {
	    conflicts[v] = conflictsV = {};
	  }
	  conflictsV[w] = true;
	}

	function hasConflict(conflicts, v, w) {
	  if (v > w) {
	    var tmp = v;
	    v = w;
	    w = tmp;
	  }
	  return _.has(conflicts[v], w);
	}

	/*
	 * Try to align nodes into vertical "blocks" where possible. This algorithm
	 * attempts to align a node with one of its median neighbors. If the edge
	 * connecting a neighbor is a type-1 conflict then we ignore that possibility.
	 * If a previous node has already formed a block with a node after the node
	 * we're trying to form a block with, we also ignore that possibility - our
	 * blocks would be split in that scenario.
	 */
	function verticalAlignment(g, layering, conflicts, neighborFn) {
	  var root = {},
	    align = {},
	    pos = {};

	  // We cache the position here based on the layering because the graph and
	  // layering may be out of sync. The layering matrix is manipulated to
	  // generate different extreme alignments.
	  _.forEach(layering, function(layer) {
	    _.forEach(layer, function(v, order) {
	      root[v] = v;
	      align[v] = v;
	      pos[v] = order;
	    });
	  });

	  _.forEach(layering, function(layer) {
	    var prevIdx = -1;
	    _.forEach(layer, function(v) {
	      var ws = neighborFn(v);
	      if (ws.length) {
	        ws = _.sortBy(ws, function(w) { return pos[w]; });
	        var mp = (ws.length - 1) / 2;
	        for (var i = Math.floor(mp), il = Math.ceil(mp); i <= il; ++i) {
	          var w = ws[i];
	          if (align[v] === v &&
	              prevIdx < pos[w] &&
	              !hasConflict(conflicts, v, w)) {
	            align[w] = v;
	            align[v] = root[v] = root[w];
	            prevIdx = pos[w];
	          }
	        }
	      }
	    });
	  });

	  return { root: root, align: align };
	}

	function horizontalCompaction(g, layering, root, align, reverseSep) {
	  // This portion of the algorithm differs from BK due to a number of problems.
	  // Instead of their algorithm we construct a new block graph and do two
	  // sweeps. The first sweep places blocks with the smallest possible
	  // coordinates. The second sweep removes unused space by moving blocks to the
	  // greatest coordinates without violating separation.
	  var xs = {},
	    blockG = buildBlockGraph(g, layering, root, reverseSep),
	    borderType = reverseSep ? "borderLeft" : "borderRight";

	  function iterate(setXsFunc, nextNodesFunc) {
	    var stack = blockG.nodes();
	    var elem = stack.pop();
	    var visited = {};
	    while (elem) {
	      if (visited[elem]) {
	        setXsFunc(elem);
	      } else {
	        visited[elem] = true;
	        stack.push(elem);
	        stack = stack.concat(nextNodesFunc(elem));
	      }

	      elem = stack.pop();
	    }
	  }

	  // First pass, assign smallest coordinates
	  function pass1(elem) {
	    xs[elem] = blockG.inEdges(elem).reduce(function(acc, e) {
	      return Math.max(acc, xs[e.v] + blockG.edge(e));
	    }, 0);
	  }

	  // Second pass, assign greatest coordinates
	  function pass2(elem) {
	    var min = blockG.outEdges(elem).reduce(function(acc, e) {
	      return Math.min(acc, xs[e.w] - blockG.edge(e));
	    }, Number.POSITIVE_INFINITY);

	    var node = g.node(elem);
	    if (min !== Number.POSITIVE_INFINITY && node.borderType !== borderType) {
	      xs[elem] = Math.max(xs[elem], min);
	    }
	  }

	  iterate(pass1, blockG.predecessors.bind(blockG));
	  iterate(pass2, blockG.successors.bind(blockG));

	  // Assign x coordinates to all nodes
	  _.forEach(align, function(v) {
	    xs[v] = xs[root[v]];
	  });

	  return xs;
	}


	function buildBlockGraph(g, layering, root, reverseSep) {
	  var blockGraph = new Graph(),
	    graphLabel = g.graph(),
	    sepFn = sep(graphLabel.nodesep, graphLabel.edgesep, reverseSep);

	  _.forEach(layering, function(layer) {
	    var u;
	    _.forEach(layer, function(v) {
	      var vRoot = root[v];
	      blockGraph.setNode(vRoot);
	      if (u) {
	        var uRoot = root[u],
	          prevMax = blockGraph.edge(uRoot, vRoot);
	        blockGraph.setEdge(uRoot, vRoot, Math.max(sepFn(g, v, u), prevMax || 0));
	      }
	      u = v;
	    });
	  });

	  return blockGraph;
	}

	/*
	 * Returns the alignment that has the smallest width of the given alignments.
	 */
	function findSmallestWidthAlignment(g, xss) {
	  return _.minBy(_.values(xss), function (xs) {
	    var max = Number.NEGATIVE_INFINITY;
	    var min = Number.POSITIVE_INFINITY;

	    _.forIn(xs, function (x, v) {
	      var halfWidth = width(g, v) / 2;

	      max = Math.max(x + halfWidth, max);
	      min = Math.min(x - halfWidth, min);
	    });

	    return max - min;
	  });
	}

	/*
	 * Align the coordinates of each of the layout alignments such that
	 * left-biased alignments have their minimum coordinate at the same point as
	 * the minimum coordinate of the smallest width alignment and right-biased
	 * alignments have their maximum coordinate at the same point as the maximum
	 * coordinate of the smallest width alignment.
	 */
	function alignCoordinates(xss, alignTo) {
	  var alignToVals = _.values(alignTo),
	    alignToMin = _.min(alignToVals),
	    alignToMax = _.max(alignToVals);

	  _.forEach(["u", "d"], function(vert) {
	    _.forEach(["l", "r"], function(horiz) {
	      var alignment = vert + horiz,
	        xs = xss[alignment],
	        delta;
	      if (xs === alignTo) return;

	      var xsVals = _.values(xs);
	      delta = horiz === "l" ? alignToMin - _.min(xsVals) : alignToMax - _.max(xsVals);

	      if (delta) {
	        xss[alignment] = _.mapValues(xs, function(x) { return x + delta; });
	      }
	    });
	  });
	}

	function balance(xss, align) {
	  return _.mapValues(xss.ul, function(ignore, v) {
	    if (align) {
	      return xss[align.toLowerCase()][v];
	    } else {
	      var xs = _.sortBy(_.map(xss, v));
	      return (xs[1] + xs[2]) / 2;
	    }
	  });
	}

	function positionX(g) {
	  var layering = util.buildLayerMatrix(g);
	  var conflicts = _.merge(
	    findType1Conflicts(g, layering),
	    findType2Conflicts(g, layering));

	  var xss = {};
	  var adjustedLayering;
	  _.forEach(["u", "d"], function(vert) {
	    adjustedLayering = vert === "u" ? layering : _.values(layering).reverse();
	    _.forEach(["l", "r"], function(horiz) {
	      if (horiz === "r") {
	        adjustedLayering = _.map(adjustedLayering, function(inner) {
	          return _.values(inner).reverse();
	        });
	      }

	      var neighborFn = (vert === "u" ? g.predecessors : g.successors).bind(g);
	      var align = verticalAlignment(g, adjustedLayering, conflicts, neighborFn);
	      var xs = horizontalCompaction(g, adjustedLayering,
	        align.root, align.align, horiz === "r");
	      if (horiz === "r") {
	        xs = _.mapValues(xs, function(x) { return -x; });
	      }
	      xss[vert + horiz] = xs;
	    });
	  });

	  var smallestWidth = findSmallestWidthAlignment(g, xss);
	  alignCoordinates(xss, smallestWidth);
	  return balance(xss, g.graph().align);
	}

	function sep(nodeSep, edgeSep, reverseSep) {
	  return function(g, v, w) {
	    var vLabel = g.node(v);
	    var wLabel = g.node(w);
	    var sum = 0;
	    var delta;

	    sum += vLabel.width / 2;
	    if (_.has(vLabel, "labelpos")) {
	      switch (vLabel.labelpos.toLowerCase()) {
	      case "l": delta = -vLabel.width / 2; break;
	      case "r": delta = vLabel.width / 2; break;
	      }
	    }
	    if (delta) {
	      sum += reverseSep ? delta : -delta;
	    }
	    delta = 0;

	    sum += (vLabel.dummy ? edgeSep : nodeSep) / 2;
	    sum += (wLabel.dummy ? edgeSep : nodeSep) / 2;

	    sum += wLabel.width / 2;
	    if (_.has(wLabel, "labelpos")) {
	      switch (wLabel.labelpos.toLowerCase()) {
	      case "l": delta = wLabel.width / 2; break;
	      case "r": delta = -wLabel.width / 2; break;
	      }
	    }
	    if (delta) {
	      sum += reverseSep ? delta : -delta;
	    }
	    delta = 0;

	    return sum;
	  };
	}

	function width(g, v) {
	  return g.node(v).width;
	}
	return bk;
}

var position_1;
var hasRequiredPosition;

function requirePosition () {
	if (hasRequiredPosition) return position_1;
	hasRequiredPosition = 1;
	"use strict";

	var _ = requireLodash();
	var util = requireUtil$1();
	var positionX = requireBk().positionX;

	position_1 = position;

	function position(g) {
	  g = util.asNonCompoundGraph(g);

	  positionY(g);
	  _.forEach(positionX(g), function(x, v) {
	    g.node(v).x = x;
	  });
	}

	function positionY(g) {
	  var layering = util.buildLayerMatrix(g);
	  var rankSep = g.graph().ranksep;
	  var prevY = 0;
	  _.forEach(layering, function(layer) {
	    var maxHeight = _.max(_.map(layer, function(v) { return g.node(v).height; }));
	    _.forEach(layer, function(v) {
	      g.node(v).y = prevY + maxHeight / 2;
	    });
	    prevY += maxHeight + rankSep;
	  });
	}
	return position_1;
}

var layout_1;
var hasRequiredLayout;

function requireLayout () {
	if (hasRequiredLayout) return layout_1;
	hasRequiredLayout = 1;
	"use strict";

	var _ = requireLodash();
	var acyclic = requireAcyclic();
	var normalize = requireNormalize();
	var rank = requireRank();
	var normalizeRanks = requireUtil$1().normalizeRanks;
	var parentDummyChains = requireParentDummyChains();
	var removeEmptyRanks = requireUtil$1().removeEmptyRanks;
	var nestingGraph = requireNestingGraph();
	var addBorderSegments = requireAddBorderSegments();
	var coordinateSystem = requireCoordinateSystem();
	var order = requireOrder();
	var position = requirePosition();
	var util = requireUtil$1();
	var Graph = requireGraphlib().Graph;

	layout_1 = layout;

	function layout(g, opts) {
	  var time = opts && opts.debugTiming ? util.time : util.notime;
	  time("layout", function() {
	    var layoutGraph = 
	      time("  buildLayoutGraph", function() { return buildLayoutGraph(g); });
	    time("  runLayout",        function() { runLayout(layoutGraph, time); });
	    time("  updateInputGraph", function() { updateInputGraph(g, layoutGraph); });
	  });
	}

	function runLayout(g, time) {
	  time("    makeSpaceForEdgeLabels", function() { makeSpaceForEdgeLabels(g); });
	  time("    removeSelfEdges",        function() { removeSelfEdges(g); });
	  time("    acyclic",                function() { acyclic.run(g); });
	  time("    nestingGraph.run",       function() { nestingGraph.run(g); });
	  time("    rank",                   function() { rank(util.asNonCompoundGraph(g)); });
	  time("    injectEdgeLabelProxies", function() { injectEdgeLabelProxies(g); });
	  time("    removeEmptyRanks",       function() { removeEmptyRanks(g); });
	  time("    nestingGraph.cleanup",   function() { nestingGraph.cleanup(g); });
	  time("    normalizeRanks",         function() { normalizeRanks(g); });
	  time("    assignRankMinMax",       function() { assignRankMinMax(g); });
	  time("    removeEdgeLabelProxies", function() { removeEdgeLabelProxies(g); });
	  time("    normalize.run",          function() { normalize.run(g); });
	  time("    parentDummyChains",      function() { parentDummyChains(g); });
	  time("    addBorderSegments",      function() { addBorderSegments(g); });
	  time("    order",                  function() { order(g); });
	  time("    insertSelfEdges",        function() { insertSelfEdges(g); });
	  time("    adjustCoordinateSystem", function() { coordinateSystem.adjust(g); });
	  time("    position",               function() { position(g); });
	  time("    positionSelfEdges",      function() { positionSelfEdges(g); });
	  time("    removeBorderNodes",      function() { removeBorderNodes(g); });
	  time("    normalize.undo",         function() { normalize.undo(g); });
	  time("    fixupEdgeLabelCoords",   function() { fixupEdgeLabelCoords(g); });
	  time("    undoCoordinateSystem",   function() { coordinateSystem.undo(g); });
	  time("    translateGraph",         function() { translateGraph(g); });
	  time("    assignNodeIntersects",   function() { assignNodeIntersects(g); });
	  time("    reversePoints",          function() { reversePointsForReversedEdges(g); });
	  time("    acyclic.undo",           function() { acyclic.undo(g); });
	}

	/*
	 * Copies final layout information from the layout graph back to the input
	 * graph. This process only copies whitelisted attributes from the layout graph
	 * to the input graph, so it serves as a good place to determine what
	 * attributes can influence layout.
	 */
	function updateInputGraph(inputGraph, layoutGraph) {
	  _.forEach(inputGraph.nodes(), function(v) {
	    var inputLabel = inputGraph.node(v);
	    var layoutLabel = layoutGraph.node(v);

	    if (inputLabel) {
	      inputLabel.x = layoutLabel.x;
	      inputLabel.y = layoutLabel.y;

	      if (layoutGraph.children(v).length) {
	        inputLabel.width = layoutLabel.width;
	        inputLabel.height = layoutLabel.height;
	      }
	    }
	  });

	  _.forEach(inputGraph.edges(), function(e) {
	    var inputLabel = inputGraph.edge(e);
	    var layoutLabel = layoutGraph.edge(e);

	    inputLabel.points = layoutLabel.points;
	    if (_.has(layoutLabel, "x")) {
	      inputLabel.x = layoutLabel.x;
	      inputLabel.y = layoutLabel.y;
	    }
	  });

	  inputGraph.graph().width = layoutGraph.graph().width;
	  inputGraph.graph().height = layoutGraph.graph().height;
	}

	var graphNumAttrs = ["nodesep", "edgesep", "ranksep", "marginx", "marginy"];
	var graphDefaults = { ranksep: 50, edgesep: 20, nodesep: 50, rankdir: "tb" };
	var graphAttrs = ["acyclicer", "ranker", "rankdir", "align"];
	var nodeNumAttrs = ["width", "height"];
	var nodeDefaults = { width: 0, height: 0 };
	var edgeNumAttrs = ["minlen", "weight", "width", "height", "labeloffset"];
	var edgeDefaults = {
	  minlen: 1, weight: 1, width: 0, height: 0,
	  labeloffset: 10, labelpos: "r"
	};
	var edgeAttrs = ["labelpos"];

	/*
	 * Constructs a new graph from the input graph, which can be used for layout.
	 * This process copies only whitelisted attributes from the input graph to the
	 * layout graph. Thus this function serves as a good place to determine what
	 * attributes can influence layout.
	 */
	function buildLayoutGraph(inputGraph) {
	  var g = new Graph({ multigraph: true, compound: true });
	  var graph = canonicalize(inputGraph.graph());

	  g.setGraph(_.merge({},
	    graphDefaults,
	    selectNumberAttrs(graph, graphNumAttrs),
	    _.pick(graph, graphAttrs)));

	  _.forEach(inputGraph.nodes(), function(v) {
	    var node = canonicalize(inputGraph.node(v));
	    g.setNode(v, _.defaults(selectNumberAttrs(node, nodeNumAttrs), nodeDefaults));
	    g.setParent(v, inputGraph.parent(v));
	  });

	  _.forEach(inputGraph.edges(), function(e) {
	    var edge = canonicalize(inputGraph.edge(e));
	    g.setEdge(e, _.merge({},
	      edgeDefaults,
	      selectNumberAttrs(edge, edgeNumAttrs),
	      _.pick(edge, edgeAttrs)));
	  });

	  return g;
	}

	/*
	 * This idea comes from the Gansner paper: to account for edge labels in our
	 * layout we split each rank in half by doubling minlen and halving ranksep.
	 * Then we can place labels at these mid-points between nodes.
	 *
	 * We also add some minimal padding to the width to push the label for the edge
	 * away from the edge itself a bit.
	 */
	function makeSpaceForEdgeLabels(g) {
	  var graph = g.graph();
	  graph.ranksep /= 2;
	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    edge.minlen *= 2;
	    if (edge.labelpos.toLowerCase() !== "c") {
	      if (graph.rankdir === "TB" || graph.rankdir === "BT") {
	        edge.width += edge.labeloffset;
	      } else {
	        edge.height += edge.labeloffset;
	      }
	    }
	  });
	}

	/*
	 * Creates temporary dummy nodes that capture the rank in which each edge's
	 * label is going to, if it has one of non-zero width and height. We do this
	 * so that we can safely remove empty ranks while preserving balance for the
	 * label's position.
	 */
	function injectEdgeLabelProxies(g) {
	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    if (edge.width && edge.height) {
	      var v = g.node(e.v);
	      var w = g.node(e.w);
	      var label = { rank: (w.rank - v.rank) / 2 + v.rank, e: e };
	      util.addDummyNode(g, "edge-proxy", label, "_ep");
	    }
	  });
	}

	function assignRankMinMax(g) {
	  var maxRank = 0;
	  _.forEach(g.nodes(), function(v) {
	    var node = g.node(v);
	    if (node.borderTop) {
	      node.minRank = g.node(node.borderTop).rank;
	      node.maxRank = g.node(node.borderBottom).rank;
	      maxRank = _.max(maxRank, node.maxRank);
	    }
	  });
	  g.graph().maxRank = maxRank;
	}

	function removeEdgeLabelProxies(g) {
	  _.forEach(g.nodes(), function(v) {
	    var node = g.node(v);
	    if (node.dummy === "edge-proxy") {
	      g.edge(node.e).labelRank = node.rank;
	      g.removeNode(v);
	    }
	  });
	}

	function translateGraph(g) {
	  var minX = Number.POSITIVE_INFINITY;
	  var maxX = 0;
	  var minY = Number.POSITIVE_INFINITY;
	  var maxY = 0;
	  var graphLabel = g.graph();
	  var marginX = graphLabel.marginx || 0;
	  var marginY = graphLabel.marginy || 0;

	  function getExtremes(attrs) {
	    var x = attrs.x;
	    var y = attrs.y;
	    var w = attrs.width;
	    var h = attrs.height;
	    minX = Math.min(minX, x - w / 2);
	    maxX = Math.max(maxX, x + w / 2);
	    minY = Math.min(minY, y - h / 2);
	    maxY = Math.max(maxY, y + h / 2);
	  }

	  _.forEach(g.nodes(), function(v) { getExtremes(g.node(v)); });
	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    if (_.has(edge, "x")) {
	      getExtremes(edge);
	    }
	  });

	  minX -= marginX;
	  minY -= marginY;

	  _.forEach(g.nodes(), function(v) {
	    var node = g.node(v);
	    node.x -= minX;
	    node.y -= minY;
	  });

	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    _.forEach(edge.points, function(p) {
	      p.x -= minX;
	      p.y -= minY;
	    });
	    if (_.has(edge, "x")) { edge.x -= minX; }
	    if (_.has(edge, "y")) { edge.y -= minY; }
	  });

	  graphLabel.width = maxX - minX + marginX;
	  graphLabel.height = maxY - minY + marginY;
	}

	function assignNodeIntersects(g) {
	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    var nodeV = g.node(e.v);
	    var nodeW = g.node(e.w);
	    var p1, p2;
	    if (!edge.points) {
	      edge.points = [];
	      p1 = nodeW;
	      p2 = nodeV;
	    } else {
	      p1 = edge.points[0];
	      p2 = edge.points[edge.points.length - 1];
	    }
	    edge.points.unshift(util.intersectRect(nodeV, p1));
	    edge.points.push(util.intersectRect(nodeW, p2));
	  });
	}

	function fixupEdgeLabelCoords(g) {
	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    if (_.has(edge, "x")) {
	      if (edge.labelpos === "l" || edge.labelpos === "r") {
	        edge.width -= edge.labeloffset;
	      }
	      switch (edge.labelpos) {
	      case "l": edge.x -= edge.width / 2 + edge.labeloffset; break;
	      case "r": edge.x += edge.width / 2 + edge.labeloffset; break;
	      }
	    }
	  });
	}

	function reversePointsForReversedEdges(g) {
	  _.forEach(g.edges(), function(e) {
	    var edge = g.edge(e);
	    if (edge.reversed) {
	      edge.points.reverse();
	    }
	  });
	}

	function removeBorderNodes(g) {
	  _.forEach(g.nodes(), function(v) {
	    if (g.children(v).length) {
	      var node = g.node(v);
	      var t = g.node(node.borderTop);
	      var b = g.node(node.borderBottom);
	      var l = g.node(_.last(node.borderLeft));
	      var r = g.node(_.last(node.borderRight));

	      node.width = Math.abs(r.x - l.x);
	      node.height = Math.abs(b.y - t.y);
	      node.x = l.x + node.width / 2;
	      node.y = t.y + node.height / 2;
	    }
	  });

	  _.forEach(g.nodes(), function(v) {
	    if (g.node(v).dummy === "border") {
	      g.removeNode(v);
	    }
	  });
	}

	function removeSelfEdges(g) {
	  _.forEach(g.edges(), function(e) {
	    if (e.v === e.w) {
	      var node = g.node(e.v);
	      if (!node.selfEdges) {
	        node.selfEdges = [];
	      }
	      node.selfEdges.push({ e: e, label: g.edge(e) });
	      g.removeEdge(e);
	    }
	  });
	}

	function insertSelfEdges(g) {
	  var layers = util.buildLayerMatrix(g);
	  _.forEach(layers, function(layer) {
	    var orderShift = 0;
	    _.forEach(layer, function(v, i) {
	      var node = g.node(v);
	      node.order = i + orderShift;
	      _.forEach(node.selfEdges, function(selfEdge) {
	        util.addDummyNode(g, "selfedge", {
	          width: selfEdge.label.width,
	          height: selfEdge.label.height,
	          rank: node.rank,
	          order: i + (++orderShift),
	          e: selfEdge.e,
	          label: selfEdge.label
	        }, "_se");
	      });
	      delete node.selfEdges;
	    });
	  });
	}

	function positionSelfEdges(g) {
	  _.forEach(g.nodes(), function(v) {
	    var node = g.node(v);
	    if (node.dummy === "selfedge") {
	      var selfNode = g.node(node.e.v);
	      var x = selfNode.x + selfNode.width / 2;
	      var y = selfNode.y;
	      var dx = node.x - x;
	      var dy = selfNode.height / 2;
	      g.setEdge(node.e, node.label);
	      g.removeNode(v);
	      node.label.points = [
	        { x: x + 2 * dx / 3, y: y - dy },
	        { x: x + 5 * dx / 6, y: y - dy },
	        { x: x +     dx    , y: y },
	        { x: x + 5 * dx / 6, y: y + dy },
	        { x: x + 2 * dx / 3, y: y + dy }
	      ];
	      node.label.x = node.x;
	      node.label.y = node.y;
	    }
	  });
	}

	function selectNumberAttrs(obj, attrs) {
	  return _.mapValues(_.pick(obj, attrs), Number);
	}

	function canonicalize(attrs) {
	  var newAttrs = {};
	  _.forEach(attrs, function(v, k) {
	    newAttrs[k.toLowerCase()] = v;
	  });
	  return newAttrs;
	}
	return layout_1;
}

var debug;
var hasRequiredDebug;

function requireDebug () {
	if (hasRequiredDebug) return debug;
	hasRequiredDebug = 1;
	var _ = requireLodash();
	var util = requireUtil$1();
	var Graph = requireGraphlib().Graph;

	debug = {
	  debugOrdering: debugOrdering
	};

	/* istanbul ignore next */
	function debugOrdering(g) {
	  var layerMatrix = util.buildLayerMatrix(g);

	  var h = new Graph({ compound: true, multigraph: true }).setGraph({});

	  _.forEach(g.nodes(), function(v) {
	    h.setNode(v, { label: v });
	    h.setParent(v, "layer" + g.node(v).rank);
	  });

	  _.forEach(g.edges(), function(e) {
	    h.setEdge(e.v, e.w, {}, e.name);
	  });

	  _.forEach(layerMatrix, function(layer, i) {
	    var layerV = "layer" + i;
	    h.setNode(layerV, { rank: "same" });
	    _.reduce(layer, function(u, v) {
	      h.setEdge(u, v, { style: "invis" });
	      return v;
	    });
	  });

	  return h;
	}
	return debug;
}

var version;
var hasRequiredVersion;

function requireVersion () {
	if (hasRequiredVersion) return version;
	hasRequiredVersion = 1;
	version = "0.8.5";
	return version;
}

/*
Copyright (c) 2012-2014 Chris Pettitt

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

var dagre;
var hasRequiredDagre;

function requireDagre () {
	if (hasRequiredDagre) return dagre;
	hasRequiredDagre = 1;
	dagre = {
	  graphlib: requireGraphlib(),

	  layout: requireLayout(),
	  debug: requireDebug(),
	  util: {
	    time: requireUtil$1().time,
	    notime: requireUtil$1().notime
	  },
	  version: requireVersion()
	};
	return dagre;
}

export { requireDagre as a, requireUniqueId as b, commonjsRequire as c, requireRange as d, requireHas as e, requirePick as f, requireIsPlainObject as g, requireDefaults as h, requireGraphlib$1 as r, v4 as v };
//# sourceMappingURL=index-CElHzHu_.js.map

//# sourceMappingURL=index-CElHzHu_.js.map