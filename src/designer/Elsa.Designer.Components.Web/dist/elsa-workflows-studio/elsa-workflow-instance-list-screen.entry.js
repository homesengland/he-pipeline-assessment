import { r as registerInstance, h } from './index-ea213ee1.js';
import { i as injectHistory } from './index-82ee3393.js';
import { _ as _isIterateeCall, t as toInteger_1, a as _baseSlice, b as _arrayPush, i as isArray_1, d as _copyArray, e as _baseFlatten, f as _arrayMap, g as _baseUnary, h as _cacheHas, j as _SetCache, k as _baseRest, l as last_1, m as _baseIteratee, n as _baseClamp, o as _baseIndexOf, p as _baseFindIndex, q as _baseIsNaN, r as _isIndex, s as get_1, u as _castPath, v as _parent, w as _toKey, x as _compareAscending, y as isSymbol_1, z as identity_1, A as eq_1, B as _arrayFilter, C as _baseTimes, D as _baseProperty, E as _apply, F as findIndex_1, G as findLastIndex_1, c as collection } from './collection-ba09a015.js';
import { _ as _arrayIncludes, a as _arrayIncludesWith, i as isArrayLikeObject_1, b as _flatRest, c as _baseUniq, d as _baseZipObject, e as _baseSet, f as flatten_1, u as union_1, z as zipObject_1 } from './zipObject-76ef4a68.js';
import { c as confirmDialogService } from './index-e19c34cd.js';
import { O as OrderBy, E as EventTypes, b as WorkflowStatus } from './index-0f68dbd6.js';
import { D as DropdownButtonOrigin } from './models-9655958f.js';
import { c as parseQuery } from './utils-db96334c.js';
import { h as hooks } from './moment-fe70f3d6.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import { b as createElsaClient } from './elsa-client-17ed10a4.js';
import './active-router-8e08ab72.js';
import './index-2db7bf78.js';
import './match-path-760e1797.js';
import './location-utils-fea12957.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './axios-middleware.esm-fcda64d5.js';

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeCeil = Math.ceil,
    nativeMax$3 = Math.max;

/**
 * Creates an array of elements split into groups the length of `size`.
 * If `array` can't be split evenly, the final chunk will be the remaining
 * elements.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to process.
 * @param {number} [size=1] The length of each chunk
 * @param- {Object} [guard] Enables use as an iteratee for methods like `_.map`.
 * @returns {Array} Returns the new array of chunks.
 * @example
 *
 * _.chunk(['a', 'b', 'c', 'd'], 2);
 * // => [['a', 'b'], ['c', 'd']]
 *
 * _.chunk(['a', 'b', 'c', 'd'], 3);
 * // => [['a', 'b', 'c'], ['d']]
 */
function chunk(array, size, guard) {
  if ((guard ? _isIterateeCall(array, size, guard) : size === undefined)) {
    size = 1;
  } else {
    size = nativeMax$3(toInteger_1(size), 0);
  }
  var length = array == null ? 0 : array.length;
  if (!length || size < 1) {
    return [];
  }
  var index = 0,
      resIndex = 0,
      result = Array(nativeCeil(length / size));

  while (index < length) {
    result[resIndex++] = _baseSlice(array, index, (index += size));
  }
  return result;
}

var chunk_1 = chunk;

/**
 * Creates an array with all falsey values removed. The values `false`, `null`,
 * `0`, `""`, `undefined`, and `NaN` are falsey.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to compact.
 * @returns {Array} Returns the new array of filtered values.
 * @example
 *
 * _.compact([0, 1, false, 2, '', 3]);
 * // => [1, 2, 3]
 */
function compact(array) {
  var index = -1,
      length = array == null ? 0 : array.length,
      resIndex = 0,
      result = [];

  while (++index < length) {
    var value = array[index];
    if (value) {
      result[resIndex++] = value;
    }
  }
  return result;
}

var compact_1 = compact;

/**
 * Creates a new array concatenating `array` with any additional arrays
 * and/or values.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to concatenate.
 * @param {...*} [values] The values to concatenate.
 * @returns {Array} Returns the new concatenated array.
 * @example
 *
 * var array = [1];
 * var other = _.concat(array, 2, [3], [[4]]);
 *
 * console.log(other);
 * // => [1, 2, 3, [4]]
 *
 * console.log(array);
 * // => [1]
 */
function concat() {
  var length = arguments.length;
  if (!length) {
    return [];
  }
  var args = Array(length - 1),
      array = arguments[0],
      index = length;

  while (index--) {
    args[index - 1] = arguments[index];
  }
  return _arrayPush(isArray_1(array) ? _copyArray(array) : [array], _baseFlatten(args, 1));
}

var concat_1 = concat;

/** Used as the size to enable large array optimizations. */
var LARGE_ARRAY_SIZE = 200;

/**
 * The base implementation of methods like `_.difference` without support
 * for excluding multiple arrays or iteratee shorthands.
 *
 * @private
 * @param {Array} array The array to inspect.
 * @param {Array} values The values to exclude.
 * @param {Function} [iteratee] The iteratee invoked per element.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new array of filtered values.
 */
function baseDifference(array, values, iteratee, comparator) {
  var index = -1,
      includes = _arrayIncludes,
      isCommon = true,
      length = array.length,
      result = [],
      valuesLength = values.length;

  if (!length) {
    return result;
  }
  if (iteratee) {
    values = _arrayMap(values, _baseUnary(iteratee));
  }
  if (comparator) {
    includes = _arrayIncludesWith;
    isCommon = false;
  }
  else if (values.length >= LARGE_ARRAY_SIZE) {
    includes = _cacheHas;
    isCommon = false;
    values = new _SetCache(values);
  }
  outer:
  while (++index < length) {
    var value = array[index],
        computed = iteratee == null ? value : iteratee(value);

    value = (comparator || value !== 0) ? value : 0;
    if (isCommon && computed === computed) {
      var valuesIndex = valuesLength;
      while (valuesIndex--) {
        if (values[valuesIndex] === computed) {
          continue outer;
        }
      }
      result.push(value);
    }
    else if (!includes(values, computed, comparator)) {
      result.push(value);
    }
  }
  return result;
}

var _baseDifference = baseDifference;

/**
 * Creates an array of `array` values not included in the other given arrays
 * using [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
 * for equality comparisons. The order and references of result values are
 * determined by the first array.
 *
 * **Note:** Unlike `_.pullAll`, this method returns a new array.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {...Array} [values] The values to exclude.
 * @returns {Array} Returns the new array of filtered values.
 * @see _.without, _.xor
 * @example
 *
 * _.difference([2, 1], [2, 3]);
 * // => [1]
 */
var difference = _baseRest(function(array, values) {
  return isArrayLikeObject_1(array)
    ? _baseDifference(array, _baseFlatten(values, 1, isArrayLikeObject_1, true))
    : [];
});

var difference_1 = difference;

/**
 * This method is like `_.difference` except that it accepts `iteratee` which
 * is invoked for each element of `array` and `values` to generate the criterion
 * by which they're compared. The order and references of result values are
 * determined by the first array. The iteratee is invoked with one argument:
 * (value).
 *
 * **Note:** Unlike `_.pullAllBy`, this method returns a new array.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {...Array} [values] The values to exclude.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {Array} Returns the new array of filtered values.
 * @example
 *
 * _.differenceBy([2.1, 1.2], [2.3, 3.4], Math.floor);
 * // => [1.2]
 *
 * // The `_.property` iteratee shorthand.
 * _.differenceBy([{ 'x': 2 }, { 'x': 1 }], [{ 'x': 1 }], 'x');
 * // => [{ 'x': 2 }]
 */
var differenceBy = _baseRest(function(array, values) {
  var iteratee = last_1(values);
  if (isArrayLikeObject_1(iteratee)) {
    iteratee = undefined;
  }
  return isArrayLikeObject_1(array)
    ? _baseDifference(array, _baseFlatten(values, 1, isArrayLikeObject_1, true), _baseIteratee(iteratee, 2))
    : [];
});

var differenceBy_1 = differenceBy;

/**
 * This method is like `_.difference` except that it accepts `comparator`
 * which is invoked to compare elements of `array` to `values`. The order and
 * references of result values are determined by the first array. The comparator
 * is invoked with two arguments: (arrVal, othVal).
 *
 * **Note:** Unlike `_.pullAllWith`, this method returns a new array.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {...Array} [values] The values to exclude.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new array of filtered values.
 * @example
 *
 * var objects = [{ 'x': 1, 'y': 2 }, { 'x': 2, 'y': 1 }];
 *
 * _.differenceWith(objects, [{ 'x': 1, 'y': 2 }], _.isEqual);
 * // => [{ 'x': 2, 'y': 1 }]
 */
var differenceWith = _baseRest(function(array, values) {
  var comparator = last_1(values);
  if (isArrayLikeObject_1(comparator)) {
    comparator = undefined;
  }
  return isArrayLikeObject_1(array)
    ? _baseDifference(array, _baseFlatten(values, 1, isArrayLikeObject_1, true), undefined, comparator)
    : [];
});

var differenceWith_1 = differenceWith;

/**
 * Creates a slice of `array` with `n` elements dropped from the beginning.
 *
 * @static
 * @memberOf _
 * @since 0.5.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {number} [n=1] The number of elements to drop.
 * @param- {Object} [guard] Enables use as an iteratee for methods like `_.map`.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * _.drop([1, 2, 3]);
 * // => [2, 3]
 *
 * _.drop([1, 2, 3], 2);
 * // => [3]
 *
 * _.drop([1, 2, 3], 5);
 * // => []
 *
 * _.drop([1, 2, 3], 0);
 * // => [1, 2, 3]
 */
function drop(array, n, guard) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return [];
  }
  n = (guard || n === undefined) ? 1 : toInteger_1(n);
  return _baseSlice(array, n < 0 ? 0 : n, length);
}

var drop_1 = drop;

/**
 * Creates a slice of `array` with `n` elements dropped from the end.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {number} [n=1] The number of elements to drop.
 * @param- {Object} [guard] Enables use as an iteratee for methods like `_.map`.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * _.dropRight([1, 2, 3]);
 * // => [1, 2]
 *
 * _.dropRight([1, 2, 3], 2);
 * // => [1]
 *
 * _.dropRight([1, 2, 3], 5);
 * // => []
 *
 * _.dropRight([1, 2, 3], 0);
 * // => [1, 2, 3]
 */
function dropRight(array, n, guard) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return [];
  }
  n = (guard || n === undefined) ? 1 : toInteger_1(n);
  n = length - n;
  return _baseSlice(array, 0, n < 0 ? 0 : n);
}

var dropRight_1 = dropRight;

/**
 * The base implementation of methods like `_.dropWhile` and `_.takeWhile`
 * without support for iteratee shorthands.
 *
 * @private
 * @param {Array} array The array to query.
 * @param {Function} predicate The function invoked per iteration.
 * @param {boolean} [isDrop] Specify dropping elements instead of taking them.
 * @param {boolean} [fromRight] Specify iterating from right to left.
 * @returns {Array} Returns the slice of `array`.
 */
function baseWhile(array, predicate, isDrop, fromRight) {
  var length = array.length,
      index = fromRight ? length : -1;

  while ((fromRight ? index-- : ++index < length) &&
    predicate(array[index], index, array)) {}

  return isDrop
    ? _baseSlice(array, (fromRight ? 0 : index), (fromRight ? index + 1 : length))
    : _baseSlice(array, (fromRight ? index + 1 : 0), (fromRight ? length : index));
}

var _baseWhile = baseWhile;

/**
 * Creates a slice of `array` excluding elements dropped from the end.
 * Elements are dropped until `predicate` returns falsey. The predicate is
 * invoked with three arguments: (value, index, array).
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {Function} [predicate=_.identity] The function invoked per iteration.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * var users = [
 *   { 'user': 'barney',  'active': true },
 *   { 'user': 'fred',    'active': false },
 *   { 'user': 'pebbles', 'active': false }
 * ];
 *
 * _.dropRightWhile(users, function(o) { return !o.active; });
 * // => objects for ['barney']
 *
 * // The `_.matches` iteratee shorthand.
 * _.dropRightWhile(users, { 'user': 'pebbles', 'active': false });
 * // => objects for ['barney', 'fred']
 *
 * // The `_.matchesProperty` iteratee shorthand.
 * _.dropRightWhile(users, ['active', false]);
 * // => objects for ['barney']
 *
 * // The `_.property` iteratee shorthand.
 * _.dropRightWhile(users, 'active');
 * // => objects for ['barney', 'fred', 'pebbles']
 */
function dropRightWhile(array, predicate) {
  return (array && array.length)
    ? _baseWhile(array, _baseIteratee(predicate, 3), true, true)
    : [];
}

var dropRightWhile_1 = dropRightWhile;

/**
 * Creates a slice of `array` excluding elements dropped from the beginning.
 * Elements are dropped until `predicate` returns falsey. The predicate is
 * invoked with three arguments: (value, index, array).
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {Function} [predicate=_.identity] The function invoked per iteration.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * var users = [
 *   { 'user': 'barney',  'active': false },
 *   { 'user': 'fred',    'active': false },
 *   { 'user': 'pebbles', 'active': true }
 * ];
 *
 * _.dropWhile(users, function(o) { return !o.active; });
 * // => objects for ['pebbles']
 *
 * // The `_.matches` iteratee shorthand.
 * _.dropWhile(users, { 'user': 'barney', 'active': false });
 * // => objects for ['fred', 'pebbles']
 *
 * // The `_.matchesProperty` iteratee shorthand.
 * _.dropWhile(users, ['active', false]);
 * // => objects for ['pebbles']
 *
 * // The `_.property` iteratee shorthand.
 * _.dropWhile(users, 'active');
 * // => objects for ['barney', 'fred', 'pebbles']
 */
function dropWhile(array, predicate) {
  return (array && array.length)
    ? _baseWhile(array, _baseIteratee(predicate, 3), true)
    : [];
}

var dropWhile_1 = dropWhile;

/** Used as references for the maximum length and index of an array. */
var MAX_ARRAY_LENGTH$2 = 4294967295;

/**
 * Converts `value` to an integer suitable for use as the length of an
 * array-like object.
 *
 * **Note:** This method is based on
 * [`ToLength`](http://ecma-international.org/ecma-262/7.0/#sec-tolength).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Lang
 * @param {*} value The value to convert.
 * @returns {number} Returns the converted integer.
 * @example
 *
 * _.toLength(3.2);
 * // => 3
 *
 * _.toLength(Number.MIN_VALUE);
 * // => 0
 *
 * _.toLength(Infinity);
 * // => 4294967295
 *
 * _.toLength('3.2');
 * // => 3
 */
function toLength(value) {
  return value ? _baseClamp(toInteger_1(value), 0, MAX_ARRAY_LENGTH$2) : 0;
}

var toLength_1 = toLength;

/**
 * The base implementation of `_.fill` without an iteratee call guard.
 *
 * @private
 * @param {Array} array The array to fill.
 * @param {*} value The value to fill `array` with.
 * @param {number} [start=0] The start position.
 * @param {number} [end=array.length] The end position.
 * @returns {Array} Returns `array`.
 */
function baseFill(array, value, start, end) {
  var length = array.length;

  start = toInteger_1(start);
  if (start < 0) {
    start = -start > length ? 0 : (length + start);
  }
  end = (end === undefined || end > length) ? length : toInteger_1(end);
  if (end < 0) {
    end += length;
  }
  end = start > end ? 0 : toLength_1(end);
  while (start < end) {
    array[start++] = value;
  }
  return array;
}

var _baseFill = baseFill;

/**
 * Fills elements of `array` with `value` from `start` up to, but not
 * including, `end`.
 *
 * **Note:** This method mutates `array`.
 *
 * @static
 * @memberOf _
 * @since 3.2.0
 * @category Array
 * @param {Array} array The array to fill.
 * @param {*} value The value to fill `array` with.
 * @param {number} [start=0] The start position.
 * @param {number} [end=array.length] The end position.
 * @returns {Array} Returns `array`.
 * @example
 *
 * var array = [1, 2, 3];
 *
 * _.fill(array, 'a');
 * console.log(array);
 * // => ['a', 'a', 'a']
 *
 * _.fill(Array(3), 2);
 * // => [2, 2, 2]
 *
 * _.fill([4, 6, 8, 10], '*', 1, 3);
 * // => [4, '*', '*', 10]
 */
function fill(array, value, start, end) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return [];
  }
  if (start && typeof start != 'number' && _isIterateeCall(array, value, start)) {
    start = 0;
    end = length;
  }
  return _baseFill(array, value, start, end);
}

var fill_1 = fill;

/**
 * Gets the first element of `array`.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @alias first
 * @category Array
 * @param {Array} array The array to query.
 * @returns {*} Returns the first element of `array`.
 * @example
 *
 * _.head([1, 2, 3]);
 * // => 1
 *
 * _.head([]);
 * // => undefined
 */
function head(array) {
  return (array && array.length) ? array[0] : undefined;
}

var head_1 = head;

var first = head_1;

/** Used as references for various `Number` constants. */
var INFINITY = 1 / 0;

/**
 * Recursively flattens `array`.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to flatten.
 * @returns {Array} Returns the new flattened array.
 * @example
 *
 * _.flattenDeep([1, [2, [3, [4]], 5]]);
 * // => [1, 2, 3, 4, 5]
 */
function flattenDeep(array) {
  var length = array == null ? 0 : array.length;
  return length ? _baseFlatten(array, INFINITY) : [];
}

var flattenDeep_1 = flattenDeep;

/**
 * Recursively flatten `array` up to `depth` times.
 *
 * @static
 * @memberOf _
 * @since 4.4.0
 * @category Array
 * @param {Array} array The array to flatten.
 * @param {number} [depth=1] The maximum recursion depth.
 * @returns {Array} Returns the new flattened array.
 * @example
 *
 * var array = [1, [2, [3, [4]], 5]];
 *
 * _.flattenDepth(array, 1);
 * // => [1, 2, [3, [4]], 5]
 *
 * _.flattenDepth(array, 2);
 * // => [1, 2, 3, [4], 5]
 */
function flattenDepth(array, depth) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return [];
  }
  depth = depth === undefined ? 1 : toInteger_1(depth);
  return _baseFlatten(array, depth);
}

var flattenDepth_1 = flattenDepth;

/**
 * The inverse of `_.toPairs`; this method returns an object composed
 * from key-value `pairs`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} pairs The key-value pairs.
 * @returns {Object} Returns the new object.
 * @example
 *
 * _.fromPairs([['a', 1], ['b', 2]]);
 * // => { 'a': 1, 'b': 2 }
 */
function fromPairs(pairs) {
  var index = -1,
      length = pairs == null ? 0 : pairs.length,
      result = {};

  while (++index < length) {
    var pair = pairs[index];
    result[pair[0]] = pair[1];
  }
  return result;
}

var fromPairs_1 = fromPairs;

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeMax$2 = Math.max;

/**
 * Gets the index at which the first occurrence of `value` is found in `array`
 * using [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
 * for equality comparisons. If `fromIndex` is negative, it's used as the
 * offset from the end of `array`.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {*} value The value to search for.
 * @param {number} [fromIndex=0] The index to search from.
 * @returns {number} Returns the index of the matched value, else `-1`.
 * @example
 *
 * _.indexOf([1, 2, 1, 2], 2);
 * // => 1
 *
 * // Search from the `fromIndex`.
 * _.indexOf([1, 2, 1, 2], 2, 2);
 * // => 3
 */
function indexOf(array, value, fromIndex) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return -1;
  }
  var index = fromIndex == null ? 0 : toInteger_1(fromIndex);
  if (index < 0) {
    index = nativeMax$2(length + index, 0);
  }
  return _baseIndexOf(array, value, index);
}

var indexOf_1 = indexOf;

/**
 * Gets all but the last element of `array`.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to query.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * _.initial([1, 2, 3]);
 * // => [1, 2]
 */
function initial(array) {
  var length = array == null ? 0 : array.length;
  return length ? _baseSlice(array, 0, -1) : [];
}

var initial_1 = initial;

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeMin$2 = Math.min;

/**
 * The base implementation of methods like `_.intersection`, without support
 * for iteratee shorthands, that accepts an array of arrays to inspect.
 *
 * @private
 * @param {Array} arrays The arrays to inspect.
 * @param {Function} [iteratee] The iteratee invoked per element.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new array of shared values.
 */
function baseIntersection(arrays, iteratee, comparator) {
  var includes = comparator ? _arrayIncludesWith : _arrayIncludes,
      length = arrays[0].length,
      othLength = arrays.length,
      othIndex = othLength,
      caches = Array(othLength),
      maxLength = Infinity,
      result = [];

  while (othIndex--) {
    var array = arrays[othIndex];
    if (othIndex && iteratee) {
      array = _arrayMap(array, _baseUnary(iteratee));
    }
    maxLength = nativeMin$2(array.length, maxLength);
    caches[othIndex] = !comparator && (iteratee || (length >= 120 && array.length >= 120))
      ? new _SetCache(othIndex && array)
      : undefined;
  }
  array = arrays[0];

  var index = -1,
      seen = caches[0];

  outer:
  while (++index < length && result.length < maxLength) {
    var value = array[index],
        computed = iteratee ? iteratee(value) : value;

    value = (comparator || value !== 0) ? value : 0;
    if (!(seen
          ? _cacheHas(seen, computed)
          : includes(result, computed, comparator)
        )) {
      othIndex = othLength;
      while (--othIndex) {
        var cache = caches[othIndex];
        if (!(cache
              ? _cacheHas(cache, computed)
              : includes(arrays[othIndex], computed, comparator))
            ) {
          continue outer;
        }
      }
      if (seen) {
        seen.push(computed);
      }
      result.push(value);
    }
  }
  return result;
}

var _baseIntersection = baseIntersection;

/**
 * Casts `value` to an empty array if it's not an array like object.
 *
 * @private
 * @param {*} value The value to inspect.
 * @returns {Array|Object} Returns the cast array-like object.
 */
function castArrayLikeObject(value) {
  return isArrayLikeObject_1(value) ? value : [];
}

var _castArrayLikeObject = castArrayLikeObject;

/**
 * Creates an array of unique values that are included in all given arrays
 * using [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
 * for equality comparisons. The order and references of result values are
 * determined by the first array.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @returns {Array} Returns the new array of intersecting values.
 * @example
 *
 * _.intersection([2, 1], [2, 3]);
 * // => [2]
 */
var intersection = _baseRest(function(arrays) {
  var mapped = _arrayMap(arrays, _castArrayLikeObject);
  return (mapped.length && mapped[0] === arrays[0])
    ? _baseIntersection(mapped)
    : [];
});

var intersection_1 = intersection;

/**
 * This method is like `_.intersection` except that it accepts `iteratee`
 * which is invoked for each element of each `arrays` to generate the criterion
 * by which they're compared. The order and references of result values are
 * determined by the first array. The iteratee is invoked with one argument:
 * (value).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {Array} Returns the new array of intersecting values.
 * @example
 *
 * _.intersectionBy([2.1, 1.2], [2.3, 3.4], Math.floor);
 * // => [2.1]
 *
 * // The `_.property` iteratee shorthand.
 * _.intersectionBy([{ 'x': 1 }], [{ 'x': 2 }, { 'x': 1 }], 'x');
 * // => [{ 'x': 1 }]
 */
var intersectionBy = _baseRest(function(arrays) {
  var iteratee = last_1(arrays),
      mapped = _arrayMap(arrays, _castArrayLikeObject);

  if (iteratee === last_1(mapped)) {
    iteratee = undefined;
  } else {
    mapped.pop();
  }
  return (mapped.length && mapped[0] === arrays[0])
    ? _baseIntersection(mapped, _baseIteratee(iteratee, 2))
    : [];
});

var intersectionBy_1 = intersectionBy;

/**
 * This method is like `_.intersection` except that it accepts `comparator`
 * which is invoked to compare elements of `arrays`. The order and references
 * of result values are determined by the first array. The comparator is
 * invoked with two arguments: (arrVal, othVal).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new array of intersecting values.
 * @example
 *
 * var objects = [{ 'x': 1, 'y': 2 }, { 'x': 2, 'y': 1 }];
 * var others = [{ 'x': 1, 'y': 1 }, { 'x': 1, 'y': 2 }];
 *
 * _.intersectionWith(objects, others, _.isEqual);
 * // => [{ 'x': 1, 'y': 2 }]
 */
var intersectionWith = _baseRest(function(arrays) {
  var comparator = last_1(arrays),
      mapped = _arrayMap(arrays, _castArrayLikeObject);

  comparator = typeof comparator == 'function' ? comparator : undefined;
  if (comparator) {
    mapped.pop();
  }
  return (mapped.length && mapped[0] === arrays[0])
    ? _baseIntersection(mapped, undefined, comparator)
    : [];
});

var intersectionWith_1 = intersectionWith;

/** Used for built-in method references. */
var arrayProto$3 = Array.prototype;

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeJoin = arrayProto$3.join;

/**
 * Converts all elements in `array` into a string separated by `separator`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to convert.
 * @param {string} [separator=','] The element separator.
 * @returns {string} Returns the joined string.
 * @example
 *
 * _.join(['a', 'b', 'c'], '~');
 * // => 'a~b~c'
 */
function join(array, separator) {
  return array == null ? '' : nativeJoin.call(array, separator);
}

var join_1 = join;

/**
 * A specialized version of `_.lastIndexOf` which performs strict equality
 * comparisons of values, i.e. `===`.
 *
 * @private
 * @param {Array} array The array to inspect.
 * @param {*} value The value to search for.
 * @param {number} fromIndex The index to search from.
 * @returns {number} Returns the index of the matched value, else `-1`.
 */
function strictLastIndexOf(array, value, fromIndex) {
  var index = fromIndex + 1;
  while (index--) {
    if (array[index] === value) {
      return index;
    }
  }
  return index;
}

var _strictLastIndexOf = strictLastIndexOf;

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeMax$1 = Math.max,
    nativeMin$1 = Math.min;

/**
 * This method is like `_.indexOf` except that it iterates over elements of
 * `array` from right to left.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {*} value The value to search for.
 * @param {number} [fromIndex=array.length-1] The index to search from.
 * @returns {number} Returns the index of the matched value, else `-1`.
 * @example
 *
 * _.lastIndexOf([1, 2, 1, 2], 2);
 * // => 3
 *
 * // Search from the `fromIndex`.
 * _.lastIndexOf([1, 2, 1, 2], 2, 2);
 * // => 1
 */
function lastIndexOf(array, value, fromIndex) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return -1;
  }
  var index = length;
  if (fromIndex !== undefined) {
    index = toInteger_1(fromIndex);
    index = index < 0 ? nativeMax$1(length + index, 0) : nativeMin$1(index, length - 1);
  }
  return value === value
    ? _strictLastIndexOf(array, value, index)
    : _baseFindIndex(array, _baseIsNaN, index, true);
}

var lastIndexOf_1 = lastIndexOf;

/**
 * The base implementation of `_.nth` which doesn't coerce arguments.
 *
 * @private
 * @param {Array} array The array to query.
 * @param {number} n The index of the element to return.
 * @returns {*} Returns the nth element of `array`.
 */
function baseNth(array, n) {
  var length = array.length;
  if (!length) {
    return;
  }
  n += n < 0 ? length : 0;
  return _isIndex(n, length) ? array[n] : undefined;
}

var _baseNth = baseNth;

/**
 * Gets the element at index `n` of `array`. If `n` is negative, the nth
 * element from the end is returned.
 *
 * @static
 * @memberOf _
 * @since 4.11.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {number} [n=0] The index of the element to return.
 * @returns {*} Returns the nth element of `array`.
 * @example
 *
 * var array = ['a', 'b', 'c', 'd'];
 *
 * _.nth(array, 1);
 * // => 'b'
 *
 * _.nth(array, -2);
 * // => 'c';
 */
function nth(array, n) {
  return (array && array.length) ? _baseNth(array, toInteger_1(n)) : undefined;
}

var nth_1 = nth;

/**
 * This function is like `baseIndexOf` except that it accepts a comparator.
 *
 * @private
 * @param {Array} array The array to inspect.
 * @param {*} value The value to search for.
 * @param {number} fromIndex The index to search from.
 * @param {Function} comparator The comparator invoked per element.
 * @returns {number} Returns the index of the matched value, else `-1`.
 */
function baseIndexOfWith(array, value, fromIndex, comparator) {
  var index = fromIndex - 1,
      length = array.length;

  while (++index < length) {
    if (comparator(array[index], value)) {
      return index;
    }
  }
  return -1;
}

var _baseIndexOfWith = baseIndexOfWith;

/** Used for built-in method references. */
var arrayProto$2 = Array.prototype;

/** Built-in value references. */
var splice$1 = arrayProto$2.splice;

/**
 * The base implementation of `_.pullAllBy` without support for iteratee
 * shorthands.
 *
 * @private
 * @param {Array} array The array to modify.
 * @param {Array} values The values to remove.
 * @param {Function} [iteratee] The iteratee invoked per element.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns `array`.
 */
function basePullAll(array, values, iteratee, comparator) {
  var indexOf = comparator ? _baseIndexOfWith : _baseIndexOf,
      index = -1,
      length = values.length,
      seen = array;

  if (array === values) {
    values = _copyArray(values);
  }
  if (iteratee) {
    seen = _arrayMap(array, _baseUnary(iteratee));
  }
  while (++index < length) {
    var fromIndex = 0,
        value = values[index],
        computed = iteratee ? iteratee(value) : value;

    while ((fromIndex = indexOf(seen, computed, fromIndex, comparator)) > -1) {
      if (seen !== array) {
        splice$1.call(seen, fromIndex, 1);
      }
      splice$1.call(array, fromIndex, 1);
    }
  }
  return array;
}

var _basePullAll = basePullAll;

/**
 * This method is like `_.pull` except that it accepts an array of values to remove.
 *
 * **Note:** Unlike `_.difference`, this method mutates `array`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to modify.
 * @param {Array} values The values to remove.
 * @returns {Array} Returns `array`.
 * @example
 *
 * var array = ['a', 'b', 'c', 'a', 'b', 'c'];
 *
 * _.pullAll(array, ['a', 'c']);
 * console.log(array);
 * // => ['b', 'b']
 */
function pullAll(array, values) {
  return (array && array.length && values && values.length)
    ? _basePullAll(array, values)
    : array;
}

var pullAll_1 = pullAll;

/**
 * Removes all given values from `array` using
 * [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
 * for equality comparisons.
 *
 * **Note:** Unlike `_.without`, this method mutates `array`. Use `_.remove`
 * to remove elements from an array by predicate.
 *
 * @static
 * @memberOf _
 * @since 2.0.0
 * @category Array
 * @param {Array} array The array to modify.
 * @param {...*} [values] The values to remove.
 * @returns {Array} Returns `array`.
 * @example
 *
 * var array = ['a', 'b', 'c', 'a', 'b', 'c'];
 *
 * _.pull(array, 'a', 'c');
 * console.log(array);
 * // => ['b', 'b']
 */
var pull = _baseRest(pullAll_1);

var pull_1 = pull;

/**
 * This method is like `_.pullAll` except that it accepts `iteratee` which is
 * invoked for each element of `array` and `values` to generate the criterion
 * by which they're compared. The iteratee is invoked with one argument: (value).
 *
 * **Note:** Unlike `_.differenceBy`, this method mutates `array`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to modify.
 * @param {Array} values The values to remove.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {Array} Returns `array`.
 * @example
 *
 * var array = [{ 'x': 1 }, { 'x': 2 }, { 'x': 3 }, { 'x': 1 }];
 *
 * _.pullAllBy(array, [{ 'x': 1 }, { 'x': 3 }], 'x');
 * console.log(array);
 * // => [{ 'x': 2 }]
 */
function pullAllBy(array, values, iteratee) {
  return (array && array.length && values && values.length)
    ? _basePullAll(array, values, _baseIteratee(iteratee, 2))
    : array;
}

var pullAllBy_1 = pullAllBy;

/**
 * This method is like `_.pullAll` except that it accepts `comparator` which
 * is invoked to compare elements of `array` to `values`. The comparator is
 * invoked with two arguments: (arrVal, othVal).
 *
 * **Note:** Unlike `_.differenceWith`, this method mutates `array`.
 *
 * @static
 * @memberOf _
 * @since 4.6.0
 * @category Array
 * @param {Array} array The array to modify.
 * @param {Array} values The values to remove.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns `array`.
 * @example
 *
 * var array = [{ 'x': 1, 'y': 2 }, { 'x': 3, 'y': 4 }, { 'x': 5, 'y': 6 }];
 *
 * _.pullAllWith(array, [{ 'x': 3, 'y': 4 }], _.isEqual);
 * console.log(array);
 * // => [{ 'x': 1, 'y': 2 }, { 'x': 5, 'y': 6 }]
 */
function pullAllWith(array, values, comparator) {
  return (array && array.length && values && values.length)
    ? _basePullAll(array, values, undefined, comparator)
    : array;
}

var pullAllWith_1 = pullAllWith;

/**
 * The base implementation of `_.at` without support for individual paths.
 *
 * @private
 * @param {Object} object The object to iterate over.
 * @param {string[]} paths The property paths to pick.
 * @returns {Array} Returns the picked elements.
 */
function baseAt(object, paths) {
  var index = -1,
      length = paths.length,
      result = Array(length),
      skip = object == null;

  while (++index < length) {
    result[index] = skip ? undefined : get_1(object, paths[index]);
  }
  return result;
}

var _baseAt = baseAt;

/**
 * The base implementation of `_.unset`.
 *
 * @private
 * @param {Object} object The object to modify.
 * @param {Array|string} path The property path to unset.
 * @returns {boolean} Returns `true` if the property is deleted, else `false`.
 */
function baseUnset(object, path) {
  path = _castPath(path, object);
  object = _parent(object, path);
  return object == null || delete object[_toKey(last_1(path))];
}

var _baseUnset = baseUnset;

/** Used for built-in method references. */
var arrayProto$1 = Array.prototype;

/** Built-in value references. */
var splice = arrayProto$1.splice;

/**
 * The base implementation of `_.pullAt` without support for individual
 * indexes or capturing the removed elements.
 *
 * @private
 * @param {Array} array The array to modify.
 * @param {number[]} indexes The indexes of elements to remove.
 * @returns {Array} Returns `array`.
 */
function basePullAt(array, indexes) {
  var length = array ? indexes.length : 0,
      lastIndex = length - 1;

  while (length--) {
    var index = indexes[length];
    if (length == lastIndex || index !== previous) {
      var previous = index;
      if (_isIndex(index)) {
        splice.call(array, index, 1);
      } else {
        _baseUnset(array, index);
      }
    }
  }
  return array;
}

var _basePullAt = basePullAt;

/**
 * Removes elements from `array` corresponding to `indexes` and returns an
 * array of removed elements.
 *
 * **Note:** Unlike `_.at`, this method mutates `array`.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to modify.
 * @param {...(number|number[])} [indexes] The indexes of elements to remove.
 * @returns {Array} Returns the new array of removed elements.
 * @example
 *
 * var array = ['a', 'b', 'c', 'd'];
 * var pulled = _.pullAt(array, [1, 3]);
 *
 * console.log(array);
 * // => ['a', 'c']
 *
 * console.log(pulled);
 * // => ['b', 'd']
 */
var pullAt = _flatRest(function(array, indexes) {
  var length = array == null ? 0 : array.length,
      result = _baseAt(array, indexes);

  _basePullAt(array, _arrayMap(indexes, function(index) {
    return _isIndex(index, length) ? +index : index;
  }).sort(_compareAscending));

  return result;
});

var pullAt_1 = pullAt;

/**
 * Removes all elements from `array` that `predicate` returns truthy for
 * and returns an array of the removed elements. The predicate is invoked
 * with three arguments: (value, index, array).
 *
 * **Note:** Unlike `_.filter`, this method mutates `array`. Use `_.pull`
 * to pull elements from an array by value.
 *
 * @static
 * @memberOf _
 * @since 2.0.0
 * @category Array
 * @param {Array} array The array to modify.
 * @param {Function} [predicate=_.identity] The function invoked per iteration.
 * @returns {Array} Returns the new array of removed elements.
 * @example
 *
 * var array = [1, 2, 3, 4];
 * var evens = _.remove(array, function(n) {
 *   return n % 2 == 0;
 * });
 *
 * console.log(array);
 * // => [1, 3]
 *
 * console.log(evens);
 * // => [2, 4]
 */
function remove(array, predicate) {
  var result = [];
  if (!(array && array.length)) {
    return result;
  }
  var index = -1,
      indexes = [],
      length = array.length;

  predicate = _baseIteratee(predicate, 3);
  while (++index < length) {
    var value = array[index];
    if (predicate(value, index, array)) {
      result.push(value);
      indexes.push(index);
    }
  }
  _basePullAt(array, indexes);
  return result;
}

var remove_1 = remove;

/** Used for built-in method references. */
var arrayProto = Array.prototype;

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeReverse = arrayProto.reverse;

/**
 * Reverses `array` so that the first element becomes the last, the second
 * element becomes the second to last, and so on.
 *
 * **Note:** This method mutates `array` and is based on
 * [`Array#reverse`](https://mdn.io/Array/reverse).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to modify.
 * @returns {Array} Returns `array`.
 * @example
 *
 * var array = [1, 2, 3];
 *
 * _.reverse(array);
 * // => [3, 2, 1]
 *
 * console.log(array);
 * // => [3, 2, 1]
 */
function reverse(array) {
  return array == null ? array : nativeReverse.call(array);
}

var reverse_1 = reverse;

/**
 * Creates a slice of `array` from `start` up to, but not including, `end`.
 *
 * **Note:** This method is used instead of
 * [`Array#slice`](https://mdn.io/Array/slice) to ensure dense arrays are
 * returned.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to slice.
 * @param {number} [start=0] The start position.
 * @param {number} [end=array.length] The end position.
 * @returns {Array} Returns the slice of `array`.
 */
function slice(array, start, end) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return [];
  }
  if (end && typeof end != 'number' && _isIterateeCall(array, start, end)) {
    start = 0;
    end = length;
  }
  else {
    start = start == null ? 0 : toInteger_1(start);
    end = end === undefined ? length : toInteger_1(end);
  }
  return _baseSlice(array, start, end);
}

var slice_1 = slice;

/** Used as references for the maximum length and index of an array. */
var MAX_ARRAY_LENGTH$1 = 4294967295,
    MAX_ARRAY_INDEX = MAX_ARRAY_LENGTH$1 - 1;

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeFloor = Math.floor,
    nativeMin = Math.min;

/**
 * The base implementation of `_.sortedIndexBy` and `_.sortedLastIndexBy`
 * which invokes `iteratee` for `value` and each element of `array` to compute
 * their sort ranking. The iteratee is invoked with one argument; (value).
 *
 * @private
 * @param {Array} array The sorted array to inspect.
 * @param {*} value The value to evaluate.
 * @param {Function} iteratee The iteratee invoked per element.
 * @param {boolean} [retHighest] Specify returning the highest qualified index.
 * @returns {number} Returns the index at which `value` should be inserted
 *  into `array`.
 */
function baseSortedIndexBy(array, value, iteratee, retHighest) {
  var low = 0,
      high = array == null ? 0 : array.length;
  if (high === 0) {
    return 0;
  }

  value = iteratee(value);
  var valIsNaN = value !== value,
      valIsNull = value === null,
      valIsSymbol = isSymbol_1(value),
      valIsUndefined = value === undefined;

  while (low < high) {
    var mid = nativeFloor((low + high) / 2),
        computed = iteratee(array[mid]),
        othIsDefined = computed !== undefined,
        othIsNull = computed === null,
        othIsReflexive = computed === computed,
        othIsSymbol = isSymbol_1(computed);

    if (valIsNaN) {
      var setLow = retHighest || othIsReflexive;
    } else if (valIsUndefined) {
      setLow = othIsReflexive && (retHighest || othIsDefined);
    } else if (valIsNull) {
      setLow = othIsReflexive && othIsDefined && (retHighest || !othIsNull);
    } else if (valIsSymbol) {
      setLow = othIsReflexive && othIsDefined && !othIsNull && (retHighest || !othIsSymbol);
    } else if (othIsNull || othIsSymbol) {
      setLow = false;
    } else {
      setLow = retHighest ? (computed <= value) : (computed < value);
    }
    if (setLow) {
      low = mid + 1;
    } else {
      high = mid;
    }
  }
  return nativeMin(high, MAX_ARRAY_INDEX);
}

var _baseSortedIndexBy = baseSortedIndexBy;

/** Used as references for the maximum length and index of an array. */
var MAX_ARRAY_LENGTH = 4294967295,
    HALF_MAX_ARRAY_LENGTH = MAX_ARRAY_LENGTH >>> 1;

/**
 * The base implementation of `_.sortedIndex` and `_.sortedLastIndex` which
 * performs a binary search of `array` to determine the index at which `value`
 * should be inserted into `array` in order to maintain its sort order.
 *
 * @private
 * @param {Array} array The sorted array to inspect.
 * @param {*} value The value to evaluate.
 * @param {boolean} [retHighest] Specify returning the highest qualified index.
 * @returns {number} Returns the index at which `value` should be inserted
 *  into `array`.
 */
function baseSortedIndex(array, value, retHighest) {
  var low = 0,
      high = array == null ? low : array.length;

  if (typeof value == 'number' && value === value && high <= HALF_MAX_ARRAY_LENGTH) {
    while (low < high) {
      var mid = (low + high) >>> 1,
          computed = array[mid];

      if (computed !== null && !isSymbol_1(computed) &&
          (retHighest ? (computed <= value) : (computed < value))) {
        low = mid + 1;
      } else {
        high = mid;
      }
    }
    return high;
  }
  return _baseSortedIndexBy(array, value, identity_1, retHighest);
}

var _baseSortedIndex = baseSortedIndex;

/**
 * Uses a binary search to determine the lowest index at which `value`
 * should be inserted into `array` in order to maintain its sort order.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The sorted array to inspect.
 * @param {*} value The value to evaluate.
 * @returns {number} Returns the index at which `value` should be inserted
 *  into `array`.
 * @example
 *
 * _.sortedIndex([30, 50], 40);
 * // => 1
 */
function sortedIndex(array, value) {
  return _baseSortedIndex(array, value);
}

var sortedIndex_1 = sortedIndex;

/**
 * This method is like `_.sortedIndex` except that it accepts `iteratee`
 * which is invoked for `value` and each element of `array` to compute their
 * sort ranking. The iteratee is invoked with one argument: (value).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The sorted array to inspect.
 * @param {*} value The value to evaluate.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {number} Returns the index at which `value` should be inserted
 *  into `array`.
 * @example
 *
 * var objects = [{ 'x': 4 }, { 'x': 5 }];
 *
 * _.sortedIndexBy(objects, { 'x': 4 }, function(o) { return o.x; });
 * // => 0
 *
 * // The `_.property` iteratee shorthand.
 * _.sortedIndexBy(objects, { 'x': 4 }, 'x');
 * // => 0
 */
function sortedIndexBy(array, value, iteratee) {
  return _baseSortedIndexBy(array, value, _baseIteratee(iteratee, 2));
}

var sortedIndexBy_1 = sortedIndexBy;

/**
 * This method is like `_.indexOf` except that it performs a binary
 * search on a sorted `array`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {*} value The value to search for.
 * @returns {number} Returns the index of the matched value, else `-1`.
 * @example
 *
 * _.sortedIndexOf([4, 5, 5, 5, 6], 5);
 * // => 1
 */
function sortedIndexOf(array, value) {
  var length = array == null ? 0 : array.length;
  if (length) {
    var index = _baseSortedIndex(array, value);
    if (index < length && eq_1(array[index], value)) {
      return index;
    }
  }
  return -1;
}

var sortedIndexOf_1 = sortedIndexOf;

/**
 * This method is like `_.sortedIndex` except that it returns the highest
 * index at which `value` should be inserted into `array` in order to
 * maintain its sort order.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The sorted array to inspect.
 * @param {*} value The value to evaluate.
 * @returns {number} Returns the index at which `value` should be inserted
 *  into `array`.
 * @example
 *
 * _.sortedLastIndex([4, 5, 5, 5, 6], 5);
 * // => 4
 */
function sortedLastIndex(array, value) {
  return _baseSortedIndex(array, value, true);
}

var sortedLastIndex_1 = sortedLastIndex;

/**
 * This method is like `_.sortedLastIndex` except that it accepts `iteratee`
 * which is invoked for `value` and each element of `array` to compute their
 * sort ranking. The iteratee is invoked with one argument: (value).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The sorted array to inspect.
 * @param {*} value The value to evaluate.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {number} Returns the index at which `value` should be inserted
 *  into `array`.
 * @example
 *
 * var objects = [{ 'x': 4 }, { 'x': 5 }];
 *
 * _.sortedLastIndexBy(objects, { 'x': 4 }, function(o) { return o.x; });
 * // => 1
 *
 * // The `_.property` iteratee shorthand.
 * _.sortedLastIndexBy(objects, { 'x': 4 }, 'x');
 * // => 1
 */
function sortedLastIndexBy(array, value, iteratee) {
  return _baseSortedIndexBy(array, value, _baseIteratee(iteratee, 2), true);
}

var sortedLastIndexBy_1 = sortedLastIndexBy;

/**
 * This method is like `_.lastIndexOf` except that it performs a binary
 * search on a sorted `array`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {*} value The value to search for.
 * @returns {number} Returns the index of the matched value, else `-1`.
 * @example
 *
 * _.sortedLastIndexOf([4, 5, 5, 5, 6], 5);
 * // => 3
 */
function sortedLastIndexOf(array, value) {
  var length = array == null ? 0 : array.length;
  if (length) {
    var index = _baseSortedIndex(array, value, true) - 1;
    if (eq_1(array[index], value)) {
      return index;
    }
  }
  return -1;
}

var sortedLastIndexOf_1 = sortedLastIndexOf;

/**
 * The base implementation of `_.sortedUniq` and `_.sortedUniqBy` without
 * support for iteratee shorthands.
 *
 * @private
 * @param {Array} array The array to inspect.
 * @param {Function} [iteratee] The iteratee invoked per element.
 * @returns {Array} Returns the new duplicate free array.
 */
function baseSortedUniq(array, iteratee) {
  var index = -1,
      length = array.length,
      resIndex = 0,
      result = [];

  while (++index < length) {
    var value = array[index],
        computed = iteratee ? iteratee(value) : value;

    if (!index || !eq_1(computed, seen)) {
      var seen = computed;
      result[resIndex++] = value === 0 ? 0 : value;
    }
  }
  return result;
}

var _baseSortedUniq = baseSortedUniq;

/**
 * This method is like `_.uniq` except that it's designed and optimized
 * for sorted arrays.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @returns {Array} Returns the new duplicate free array.
 * @example
 *
 * _.sortedUniq([1, 1, 2]);
 * // => [1, 2]
 */
function sortedUniq(array) {
  return (array && array.length)
    ? _baseSortedUniq(array)
    : [];
}

var sortedUniq_1 = sortedUniq;

/**
 * This method is like `_.uniqBy` except that it's designed and optimized
 * for sorted arrays.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {Function} [iteratee] The iteratee invoked per element.
 * @returns {Array} Returns the new duplicate free array.
 * @example
 *
 * _.sortedUniqBy([1.1, 1.2, 2.3, 2.4], Math.floor);
 * // => [1.1, 2.3]
 */
function sortedUniqBy(array, iteratee) {
  return (array && array.length)
    ? _baseSortedUniq(array, _baseIteratee(iteratee, 2))
    : [];
}

var sortedUniqBy_1 = sortedUniqBy;

/**
 * Gets all but the first element of `array`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to query.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * _.tail([1, 2, 3]);
 * // => [2, 3]
 */
function tail(array) {
  var length = array == null ? 0 : array.length;
  return length ? _baseSlice(array, 1, length) : [];
}

var tail_1 = tail;

/**
 * Creates a slice of `array` with `n` elements taken from the beginning.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {number} [n=1] The number of elements to take.
 * @param- {Object} [guard] Enables use as an iteratee for methods like `_.map`.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * _.take([1, 2, 3]);
 * // => [1]
 *
 * _.take([1, 2, 3], 2);
 * // => [1, 2]
 *
 * _.take([1, 2, 3], 5);
 * // => [1, 2, 3]
 *
 * _.take([1, 2, 3], 0);
 * // => []
 */
function take(array, n, guard) {
  if (!(array && array.length)) {
    return [];
  }
  n = (guard || n === undefined) ? 1 : toInteger_1(n);
  return _baseSlice(array, 0, n < 0 ? 0 : n);
}

var take_1 = take;

/**
 * Creates a slice of `array` with `n` elements taken from the end.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {number} [n=1] The number of elements to take.
 * @param- {Object} [guard] Enables use as an iteratee for methods like `_.map`.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * _.takeRight([1, 2, 3]);
 * // => [3]
 *
 * _.takeRight([1, 2, 3], 2);
 * // => [2, 3]
 *
 * _.takeRight([1, 2, 3], 5);
 * // => [1, 2, 3]
 *
 * _.takeRight([1, 2, 3], 0);
 * // => []
 */
function takeRight(array, n, guard) {
  var length = array == null ? 0 : array.length;
  if (!length) {
    return [];
  }
  n = (guard || n === undefined) ? 1 : toInteger_1(n);
  n = length - n;
  return _baseSlice(array, n < 0 ? 0 : n, length);
}

var takeRight_1 = takeRight;

/**
 * Creates a slice of `array` with elements taken from the end. Elements are
 * taken until `predicate` returns falsey. The predicate is invoked with
 * three arguments: (value, index, array).
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {Function} [predicate=_.identity] The function invoked per iteration.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * var users = [
 *   { 'user': 'barney',  'active': true },
 *   { 'user': 'fred',    'active': false },
 *   { 'user': 'pebbles', 'active': false }
 * ];
 *
 * _.takeRightWhile(users, function(o) { return !o.active; });
 * // => objects for ['fred', 'pebbles']
 *
 * // The `_.matches` iteratee shorthand.
 * _.takeRightWhile(users, { 'user': 'pebbles', 'active': false });
 * // => objects for ['pebbles']
 *
 * // The `_.matchesProperty` iteratee shorthand.
 * _.takeRightWhile(users, ['active', false]);
 * // => objects for ['fred', 'pebbles']
 *
 * // The `_.property` iteratee shorthand.
 * _.takeRightWhile(users, 'active');
 * // => []
 */
function takeRightWhile(array, predicate) {
  return (array && array.length)
    ? _baseWhile(array, _baseIteratee(predicate, 3), false, true)
    : [];
}

var takeRightWhile_1 = takeRightWhile;

/**
 * Creates a slice of `array` with elements taken from the beginning. Elements
 * are taken until `predicate` returns falsey. The predicate is invoked with
 * three arguments: (value, index, array).
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Array
 * @param {Array} array The array to query.
 * @param {Function} [predicate=_.identity] The function invoked per iteration.
 * @returns {Array} Returns the slice of `array`.
 * @example
 *
 * var users = [
 *   { 'user': 'barney',  'active': false },
 *   { 'user': 'fred',    'active': false },
 *   { 'user': 'pebbles', 'active': true }
 * ];
 *
 * _.takeWhile(users, function(o) { return !o.active; });
 * // => objects for ['barney', 'fred']
 *
 * // The `_.matches` iteratee shorthand.
 * _.takeWhile(users, { 'user': 'barney', 'active': false });
 * // => objects for ['barney']
 *
 * // The `_.matchesProperty` iteratee shorthand.
 * _.takeWhile(users, ['active', false]);
 * // => objects for ['barney', 'fred']
 *
 * // The `_.property` iteratee shorthand.
 * _.takeWhile(users, 'active');
 * // => []
 */
function takeWhile(array, predicate) {
  return (array && array.length)
    ? _baseWhile(array, _baseIteratee(predicate, 3))
    : [];
}

var takeWhile_1 = takeWhile;

/**
 * This method is like `_.union` except that it accepts `iteratee` which is
 * invoked for each element of each `arrays` to generate the criterion by
 * which uniqueness is computed. Result values are chosen from the first
 * array in which the value occurs. The iteratee is invoked with one argument:
 * (value).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {Array} Returns the new array of combined values.
 * @example
 *
 * _.unionBy([2.1], [1.2, 2.3], Math.floor);
 * // => [2.1, 1.2]
 *
 * // The `_.property` iteratee shorthand.
 * _.unionBy([{ 'x': 1 }], [{ 'x': 2 }, { 'x': 1 }], 'x');
 * // => [{ 'x': 1 }, { 'x': 2 }]
 */
var unionBy = _baseRest(function(arrays) {
  var iteratee = last_1(arrays);
  if (isArrayLikeObject_1(iteratee)) {
    iteratee = undefined;
  }
  return _baseUniq(_baseFlatten(arrays, 1, isArrayLikeObject_1, true), _baseIteratee(iteratee, 2));
});

var unionBy_1 = unionBy;

/**
 * This method is like `_.union` except that it accepts `comparator` which
 * is invoked to compare elements of `arrays`. Result values are chosen from
 * the first array in which the value occurs. The comparator is invoked
 * with two arguments: (arrVal, othVal).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new array of combined values.
 * @example
 *
 * var objects = [{ 'x': 1, 'y': 2 }, { 'x': 2, 'y': 1 }];
 * var others = [{ 'x': 1, 'y': 1 }, { 'x': 1, 'y': 2 }];
 *
 * _.unionWith(objects, others, _.isEqual);
 * // => [{ 'x': 1, 'y': 2 }, { 'x': 2, 'y': 1 }, { 'x': 1, 'y': 1 }]
 */
var unionWith = _baseRest(function(arrays) {
  var comparator = last_1(arrays);
  comparator = typeof comparator == 'function' ? comparator : undefined;
  return _baseUniq(_baseFlatten(arrays, 1, isArrayLikeObject_1, true), undefined, comparator);
});

var unionWith_1 = unionWith;

/**
 * Creates a duplicate-free version of an array, using
 * [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
 * for equality comparisons, in which only the first occurrence of each element
 * is kept. The order of result values is determined by the order they occur
 * in the array.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @returns {Array} Returns the new duplicate free array.
 * @example
 *
 * _.uniq([2, 1, 2]);
 * // => [2, 1]
 */
function uniq(array) {
  return (array && array.length) ? _baseUniq(array) : [];
}

var uniq_1 = uniq;

/**
 * This method is like `_.uniq` except that it accepts `iteratee` which is
 * invoked for each element in `array` to generate the criterion by which
 * uniqueness is computed. The order of result values is determined by the
 * order they occur in the array. The iteratee is invoked with one argument:
 * (value).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {Array} Returns the new duplicate free array.
 * @example
 *
 * _.uniqBy([2.1, 1.2, 2.3], Math.floor);
 * // => [2.1, 1.2]
 *
 * // The `_.property` iteratee shorthand.
 * _.uniqBy([{ 'x': 1 }, { 'x': 2 }, { 'x': 1 }], 'x');
 * // => [{ 'x': 1 }, { 'x': 2 }]
 */
function uniqBy(array, iteratee) {
  return (array && array.length) ? _baseUniq(array, _baseIteratee(iteratee, 2)) : [];
}

var uniqBy_1 = uniqBy;

/**
 * This method is like `_.uniq` except that it accepts `comparator` which
 * is invoked to compare elements of `array`. The order of result values is
 * determined by the order they occur in the array.The comparator is invoked
 * with two arguments: (arrVal, othVal).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new duplicate free array.
 * @example
 *
 * var objects = [{ 'x': 1, 'y': 2 }, { 'x': 2, 'y': 1 }, { 'x': 1, 'y': 2 }];
 *
 * _.uniqWith(objects, _.isEqual);
 * // => [{ 'x': 1, 'y': 2 }, { 'x': 2, 'y': 1 }]
 */
function uniqWith(array, comparator) {
  comparator = typeof comparator == 'function' ? comparator : undefined;
  return (array && array.length) ? _baseUniq(array, undefined, comparator) : [];
}

var uniqWith_1 = uniqWith;

/* Built-in method references for those with the same name as other `lodash` methods. */
var nativeMax = Math.max;

/**
 * This method is like `_.zip` except that it accepts an array of grouped
 * elements and creates an array regrouping the elements to their pre-zip
 * configuration.
 *
 * @static
 * @memberOf _
 * @since 1.2.0
 * @category Array
 * @param {Array} array The array of grouped elements to process.
 * @returns {Array} Returns the new array of regrouped elements.
 * @example
 *
 * var zipped = _.zip(['a', 'b'], [1, 2], [true, false]);
 * // => [['a', 1, true], ['b', 2, false]]
 *
 * _.unzip(zipped);
 * // => [['a', 'b'], [1, 2], [true, false]]
 */
function unzip(array) {
  if (!(array && array.length)) {
    return [];
  }
  var length = 0;
  array = _arrayFilter(array, function(group) {
    if (isArrayLikeObject_1(group)) {
      length = nativeMax(group.length, length);
      return true;
    }
  });
  return _baseTimes(length, function(index) {
    return _arrayMap(array, _baseProperty(index));
  });
}

var unzip_1 = unzip;

/**
 * This method is like `_.unzip` except that it accepts `iteratee` to specify
 * how regrouped values should be combined. The iteratee is invoked with the
 * elements of each group: (...group).
 *
 * @static
 * @memberOf _
 * @since 3.8.0
 * @category Array
 * @param {Array} array The array of grouped elements to process.
 * @param {Function} [iteratee=_.identity] The function to combine
 *  regrouped values.
 * @returns {Array} Returns the new array of regrouped elements.
 * @example
 *
 * var zipped = _.zip([1, 2], [10, 20], [100, 200]);
 * // => [[1, 10, 100], [2, 20, 200]]
 *
 * _.unzipWith(zipped, _.add);
 * // => [3, 30, 300]
 */
function unzipWith(array, iteratee) {
  if (!(array && array.length)) {
    return [];
  }
  var result = unzip_1(array);
  if (iteratee == null) {
    return result;
  }
  return _arrayMap(result, function(group) {
    return _apply(iteratee, undefined, group);
  });
}

var unzipWith_1 = unzipWith;

/**
 * Creates an array excluding all given values using
 * [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
 * for equality comparisons.
 *
 * **Note:** Unlike `_.pull`, this method returns a new array.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {Array} array The array to inspect.
 * @param {...*} [values] The values to exclude.
 * @returns {Array} Returns the new array of filtered values.
 * @see _.difference, _.xor
 * @example
 *
 * _.without([2, 1, 2, 3], 1, 2);
 * // => [3]
 */
var without = _baseRest(function(array, values) {
  return isArrayLikeObject_1(array)
    ? _baseDifference(array, values)
    : [];
});

var without_1 = without;

/**
 * The base implementation of methods like `_.xor`, without support for
 * iteratee shorthands, that accepts an array of arrays to inspect.
 *
 * @private
 * @param {Array} arrays The arrays to inspect.
 * @param {Function} [iteratee] The iteratee invoked per element.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new array of values.
 */
function baseXor(arrays, iteratee, comparator) {
  var length = arrays.length;
  if (length < 2) {
    return length ? _baseUniq(arrays[0]) : [];
  }
  var index = -1,
      result = Array(length);

  while (++index < length) {
    var array = arrays[index],
        othIndex = -1;

    while (++othIndex < length) {
      if (othIndex != index) {
        result[index] = _baseDifference(result[index] || array, arrays[othIndex], iteratee, comparator);
      }
    }
  }
  return _baseUniq(_baseFlatten(result, 1), iteratee, comparator);
}

var _baseXor = baseXor;

/**
 * Creates an array of unique values that is the
 * [symmetric difference](https://en.wikipedia.org/wiki/Symmetric_difference)
 * of the given arrays. The order of result values is determined by the order
 * they occur in the arrays.
 *
 * @static
 * @memberOf _
 * @since 2.4.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @returns {Array} Returns the new array of filtered values.
 * @see _.difference, _.without
 * @example
 *
 * _.xor([2, 1], [2, 3]);
 * // => [1, 3]
 */
var xor = _baseRest(function(arrays) {
  return _baseXor(_arrayFilter(arrays, isArrayLikeObject_1));
});

var xor_1 = xor;

/**
 * This method is like `_.xor` except that it accepts `iteratee` which is
 * invoked for each element of each `arrays` to generate the criterion by
 * which by which they're compared. The order of result values is determined
 * by the order they occur in the arrays. The iteratee is invoked with one
 * argument: (value).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @param {Function} [iteratee=_.identity] The iteratee invoked per element.
 * @returns {Array} Returns the new array of filtered values.
 * @example
 *
 * _.xorBy([2.1, 1.2], [2.3, 3.4], Math.floor);
 * // => [1.2, 3.4]
 *
 * // The `_.property` iteratee shorthand.
 * _.xorBy([{ 'x': 1 }], [{ 'x': 2 }, { 'x': 1 }], 'x');
 * // => [{ 'x': 2 }]
 */
var xorBy = _baseRest(function(arrays) {
  var iteratee = last_1(arrays);
  if (isArrayLikeObject_1(iteratee)) {
    iteratee = undefined;
  }
  return _baseXor(_arrayFilter(arrays, isArrayLikeObject_1), _baseIteratee(iteratee, 2));
});

var xorBy_1 = xorBy;

/**
 * This method is like `_.xor` except that it accepts `comparator` which is
 * invoked to compare elements of `arrays`. The order of result values is
 * determined by the order they occur in the arrays. The comparator is invoked
 * with two arguments: (arrVal, othVal).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Array
 * @param {...Array} [arrays] The arrays to inspect.
 * @param {Function} [comparator] The comparator invoked per element.
 * @returns {Array} Returns the new array of filtered values.
 * @example
 *
 * var objects = [{ 'x': 1, 'y': 2 }, { 'x': 2, 'y': 1 }];
 * var others = [{ 'x': 1, 'y': 1 }, { 'x': 1, 'y': 2 }];
 *
 * _.xorWith(objects, others, _.isEqual);
 * // => [{ 'x': 2, 'y': 1 }, { 'x': 1, 'y': 1 }]
 */
var xorWith = _baseRest(function(arrays) {
  var comparator = last_1(arrays);
  comparator = typeof comparator == 'function' ? comparator : undefined;
  return _baseXor(_arrayFilter(arrays, isArrayLikeObject_1), undefined, comparator);
});

var xorWith_1 = xorWith;

/**
 * Creates an array of grouped elements, the first of which contains the
 * first elements of the given arrays, the second of which contains the
 * second elements of the given arrays, and so on.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Array
 * @param {...Array} [arrays] The arrays to process.
 * @returns {Array} Returns the new array of grouped elements.
 * @example
 *
 * _.zip(['a', 'b'], [1, 2], [true, false]);
 * // => [['a', 1, true], ['b', 2, false]]
 */
var zip = _baseRest(unzip_1);

var zip_1 = zip;

/**
 * This method is like `_.zipObject` except that it supports property paths.
 *
 * @static
 * @memberOf _
 * @since 4.1.0
 * @category Array
 * @param {Array} [props=[]] The property identifiers.
 * @param {Array} [values=[]] The property values.
 * @returns {Object} Returns the new object.
 * @example
 *
 * _.zipObjectDeep(['a.b[0].c', 'a.b[1].d'], [1, 2]);
 * // => { 'a': { 'b': [{ 'c': 1 }, { 'd': 2 }] } }
 */
function zipObjectDeep(props, values) {
  return _baseZipObject(props || [], values || [], _baseSet);
}

var zipObjectDeep_1 = zipObjectDeep;

/**
 * This method is like `_.zip` except that it accepts `iteratee` to specify
 * how grouped values should be combined. The iteratee is invoked with the
 * elements of each group: (...group).
 *
 * @static
 * @memberOf _
 * @since 3.8.0
 * @category Array
 * @param {...Array} [arrays] The arrays to process.
 * @param {Function} [iteratee=_.identity] The function to combine
 *  grouped values.
 * @returns {Array} Returns the new array of grouped elements.
 * @example
 *
 * _.zipWith([1, 2], [10, 20], [100, 200], function(a, b, c) {
 *   return a + b + c;
 * });
 * // => [111, 222]
 */
var zipWith = _baseRest(function(arrays) {
  var length = arrays.length,
      iteratee = length > 1 ? arrays[length - 1] : undefined;

  iteratee = typeof iteratee == 'function' ? (arrays.pop(), iteratee) : undefined;
  return unzipWith_1(arrays, iteratee);
});

var zipWith_1 = zipWith;

var array = {
  'chunk': chunk_1,
  'compact': compact_1,
  'concat': concat_1,
  'difference': difference_1,
  'differenceBy': differenceBy_1,
  'differenceWith': differenceWith_1,
  'drop': drop_1,
  'dropRight': dropRight_1,
  'dropRightWhile': dropRightWhile_1,
  'dropWhile': dropWhile_1,
  'fill': fill_1,
  'findIndex': findIndex_1,
  'findLastIndex': findLastIndex_1,
  'first': first,
  'flatten': flatten_1,
  'flattenDeep': flattenDeep_1,
  'flattenDepth': flattenDepth_1,
  'fromPairs': fromPairs_1,
  'head': head_1,
  'indexOf': indexOf_1,
  'initial': initial_1,
  'intersection': intersection_1,
  'intersectionBy': intersectionBy_1,
  'intersectionWith': intersectionWith_1,
  'join': join_1,
  'last': last_1,
  'lastIndexOf': lastIndexOf_1,
  'nth': nth_1,
  'pull': pull_1,
  'pullAll': pullAll_1,
  'pullAllBy': pullAllBy_1,
  'pullAllWith': pullAllWith_1,
  'pullAt': pullAt_1,
  'remove': remove_1,
  'reverse': reverse_1,
  'slice': slice_1,
  'sortedIndex': sortedIndex_1,
  'sortedIndexBy': sortedIndexBy_1,
  'sortedIndexOf': sortedIndexOf_1,
  'sortedLastIndex': sortedLastIndex_1,
  'sortedLastIndexBy': sortedLastIndexBy_1,
  'sortedLastIndexOf': sortedLastIndexOf_1,
  'sortedUniq': sortedUniq_1,
  'sortedUniqBy': sortedUniqBy_1,
  'tail': tail_1,
  'take': take_1,
  'takeRight': takeRight_1,
  'takeRightWhile': takeRightWhile_1,
  'takeWhile': takeWhile_1,
  'union': union_1,
  'unionBy': unionBy_1,
  'unionWith': unionWith_1,
  'uniq': uniq_1,
  'uniqBy': uniqBy_1,
  'uniqWith': uniqWith_1,
  'unzip': unzip_1,
  'unzipWith': unzipWith_1,
  'without': without_1,
  'xor': xor_1,
  'xorBy': xorBy_1,
  'xorWith': xorWith_1,
  'zip': zip_1,
  'zipObject': zipObject_1,
  'zipObjectDeep': zipObjectDeep_1,
  'zipWith': zipWith_1
};

const resources = {
  'en': {
    'default': {
      'Title': 'Workflow Instances',
      'Search': 'Search',
      'Untitled': 'Untitled',
      'Table': {
        'Id': 'ID',
        'CorrelationId': 'Correlation ID',
        'Workflow': 'Workflow',
        'Version': 'Version',
        'InstanceName': 'Instance Name',
        'Status': 'Status',
        'Created': 'Created',
        'Finished': 'Finished',
        'LastExecuted': 'Last Executed',
        'Faulted': 'Faulted',
        'ContextMenu': {
          'View': 'View',
          'Cancel': 'Cancel',
          'Delete': 'Delete',
          'Retry': 'Retry'
        }
      },
      'CancelDialog': {
        'Title': 'Cancel Workflow Instance',
        'Message': 'Are you sure you wish to cancel this workflow instance?'
      },
      'DeleteDialog': {
        'Title': 'Delete Workflow Instance',
        'Message': 'Are you sure you wish to permanently delete this workflow instance?'
      },
      'BulkCancelDialog': {
        'Title': 'Cancel Selected Workflow Instances',
        'Message': 'Are you sure you wish to cancel all selected workflow instances?'
      },
      'BulkDeleteDialog': {
        'Title': 'Delete Selected Workflow Instances',
        'Message': 'Are you sure you wish to permanently delete all selected workflow instances?'
      },
      'BulkRetryDialog': {
        'Title': 'Retry Selected Workflow Instances',
        'Message': 'Are you sure you wish to retry all selected workflow instances?'
      },
      'Pager': {
        'Display': 'Showing {{Start}} to {{End}} of {{Results}} results'
      },
      'BulkActions': {
        'Title': 'Bulk Actions',
        'Actions': {
          'Cancel': 'Cancel',
          'Delete': 'Delete',
          'Retry': 'Retry'
        }
      },
      'Filters': {
        'Workflow': {
          'Label': 'Workflow',
          'All': 'All'
        },
        'Status': {
          'All': 'All',
          'Label': 'Status',
          'Running': 'Running',
          'Idle': 'Idle',
          'Suspended': 'Suspended',
          'Faulted': 'Faulted',
          'Finished': 'Finished',
          'Cancelled': 'Cancelled'
        },
        'Sort': {
          'Label': 'Sort',
          'SelectedLabel': 'Sort by: {{Key}}',
          'Finished': 'Finished',
          'LastExecuted': 'Last Executed',
          'Started': 'Started'
        },
        'PageSize': {
          'SelectedLabel': 'Page size: {{Size}}',
        }
      }
    }
  },
  'zh-CN': {
    'default': {
      'Title': '工作流实例',
      'Search': '搜索',
      'Untitled': '无题',
      'Table': {
        'Id': 'ID',
        'CorrelationId': '相关 ID',
        'Workflow': '工作流程',
        'Version': '版本',
        'InstanceName': '实例名称',
        'Status': '状态',
        'Created': '已创建',
        'Finished': '已完成',
        'LastExecuted': '最后执行',
        'Faulted': '故障',
        'ContextMenu': {
          'View': '查看',
          'Cancel': '取消',
          'Delete': '删除'
        }
      },
      'CancelDialog': {
        'Title': '取消工作流实例',
        'Message': '你确定要取消这个工作流程实例吗？'
      },
      'DeleteDialog': {
        'Title': '删除工作流实例',
        'Message': '你确定要永久删除这个工作流程实例吗？'
      },
      'BulkCancelDialog': {
        'Title': '取消选定的工作流实例',
        'Message': '你确定要取消所有选定的工作流程实例吗？'
      },
      'BulkDeleteDialog': {
        'Title': '删除选定的工作流实例',
        'Message': '你确定要永久删除所有选定的工作流实例吗？'
      },
      'Pager': {
        'Display': '显示 {{Start}} 到 {{End}} 的 {{Results}} 结果'
      },
      'BulkActions': {
        'Title': '批量操作',
        'Actions': {
          'Cancel': '取消',
          'Delete': '删除'
        }
      },
      'Filters': {
        'Workflow': {
          'Label': '工作流程',
          'All': '所有'
        },
        'Status': {
          'All': '所有',
          'Label': '状态',
          'Running': '运行中',
          'Idle': '闲置',
          'Suspended': '已暂停',
          'Faulted': '故障',
          'Finished': '已完成',
          'Cancelled': '已取消'
        },
        'Sort': {
          'Label': '排序',
          'SelectedLabel': '根据{{Key}}排序',
          'Finished': '已完成',
          'LastExecuted': '最后执行',
          'Started': '开始'
        },
        'PageSize': {
          'SelectedLabel': '页面大小: {{Size}}',
        }
      }
    }
  },
  'nl-NL': {
    'default': {}
  },
  'fa-IR': {
    'default': {
      'Title': 'جریان ها',
      'Search': 'جستجو',
      'Untitled': 'بی نام',
      'Table': {
        'Id': 'شناسه',
        'CorrelationId': 'Correlation ID',
        'Workflow': 'فرآیند',
        'Version': 'ویرایش',
        'InstanceName': 'نام جریان',
        'Status': 'وضعیت',
        'Created': 'ایجاد شده',
        'Finished': 'پایان یافته ',
        'LastExecuted': 'آخرین اجرا',
        'Faulted': 'با خطا مواجه شده',
        'ContextMenu': {
          'View': 'مشاهده',
          'Cancel': 'لغو',
          'Delete': 'حذف',
          'Retry': 'تلاش مجدد'
        }
      },
      'CancelDialog': {
        'Title': 'لغو اجرای فرآیند',
        'Message': 'آیا از لغو اجرای این فرآیند اطمینان دارید؟'
      },
      'DeleteDialog': {
        'Title': 'حذف جریان فرآیند',
        'Message': 'آیا از حذف این اجرای فرآیند اطمینان دارید؟'
      },
      'BulkCancelDialog': {
        'Title': 'لغو اجراهای انتخاب شده',
        'Message': 'آیا از لغو همه اجراهای انتخاب شده اطمینان دارید؟'
      },
      'BulkDeleteDialog': {
        'Title': 'حذف اجراهای انتخاب شده',
        'Message': 'آیا از حذف همه اجراهای انتخاب شده اطمینان دارید؟?'
      },
      'BulkRetryDialog': {
        'Title': 'اجرای مجدد اجراهای انتخاب شده',
        'Message': 'آیااز اجرای مجدد اجراهای انتخاب شده اطمینان دارید؟'
      },
      'Pager': {
        'Display': '{{Start}} تا {{End}} از {{Results}} نتیجه'
      },
      'BulkActions': {
        'Title': 'عملیات گروهی',
        'Actions': {
          'Cancel': 'لغو',
          'Delete': 'حذف',
          'Retry': 'اجرای مجدد'
        }
      },
      'Filters': {
        'Workflow': {
          'Label': 'فرآیند',
          'All': 'همه'
        },
        'Status': {
          'All': 'همه',
          'Label': 'وضعیت',
          'Running': 'در حال اجرا',
          'Idle': 'Idle',
          'Suspended': 'تعلیف شده',
          'Faulted': 'با خطا مواجه شده',
          'Finished': 'پایان یافته',
          'Cancelled': 'لغو شده'
        },
        'Sort': {
          'Label': 'مرتب سازی',
          'SelectedLabel': 'بر اساس: {{Key}}',
          'Finished': 'پایان یافته',
          'LastExecuted': 'آخرین اجرا',
          'Started': 'شروع شده'
        },
        'PageSize': {
          'SelectedLabel': 'تعداد در صفحه: {{Size}}',
        }
      }
    }
  },
  'de-DE': {
    'default': {
      'Title': 'Ablaufinstanzen',
      'Search': 'Suchen',
      'Untitled': 'Unbenannt',
      'Table': {
        'Id': 'ID',
        'CorrelationId': 'Correlation ID',
        'Workflow': 'Ablauf',
        'Version': 'Version',
        'InstanceName': 'Instanzname',
        'Status': 'Status',
        'Created': 'Erstellt',
        'Finished': 'Abgeschlossen',
        'LastExecuted': 'Zuletzt ausgeführt',
        'Faulted': 'Fehlgeschlagen',
        'ContextMenu': {
          'View': 'Ansehen',
          'Cancel': 'Abbrechen',
          'Delete': 'Entfernen',
          'Retry': 'Nochmal ausführen'
        }
      },
      'CancelDialog': {
        'Title': 'Ablaufinstanz abbrechen',
        'Message': 'Sind Sie sicher, dass Sie die Instanz abbrechen wollen?'
      },
      'DeleteDialog': {
        'Title': 'Ablauf entfernen',
        'Message': 'Sind Sie sicher, dass Sie die Instanz unwiderruflich entfernen wollen?'
      },
      'BulkCancelDialog': {
        'Title': 'Ausgewählte Instanzen abbrechen',
        'Message': 'Sind Sie sicher, dass Sie die ausgewählten Instanzen abbrechen wollen?'
      },
      'BulkDeleteDialog': {
        'Title': 'Ausgewählte Instanzen entfernen',
        'Message': 'Sind Sie sicher, dass Sie die ausgewählten Instanzen entfernen wollen?'
      },
      'BulkRetryDialog': {
        'Title': 'Ausgewählte Instanzen nochmal ausführen',
        'Message': 'Sind Sie sicher, dass Sie die ausgwählten Instanzen noch einmal ausführen wollen?'
      },
      'Pager': {
        'Display': 'Zeige {{Start}} bis {{End}} von {{Results}} Ergebnissen'
      },
      'BulkActions': {
        'Title': 'Massenaktionen',
        'Actions': {
          'Cancel': 'Abbrechen',
          'Delete': 'Entfernen',
          'Retry': 'Erneut ausführen'
        }
      },
      'Filters': {
        'Workflow': {
          'Label': 'Ablauf',
          'All': 'Alle'
        },
        'Status': {
          'All': 'Alle',
          'Label': 'Status',
          'Running': 'Laufend',
          'Idle': 'Inaktive',
          'Suspended': 'Angehalten',
          'Faulted': 'Fehlgeschlagen',
          'Finished': 'Abgeschlossen',
          'Cancelled': 'Abgebrochen'
        },
        'Sort': {
          'Label': 'Sortierung',
          'SelectedLabel': 'Sortieren nach: {{Key}}',
          'Finished': 'Abgeschlossen',
          'LastExecuted': 'Zuletzt ausgeführt',
          'Started': 'Gestartet'
        },
        'PageSize': {
          'SelectedLabel': 'Seitengröße: {{Size}}',
        }
      }
    }
  },
};

let ElsaWorkflowInstanceListScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.orderBy = OrderBy.Started;
    this.workflowBlueprints = [];
    this.workflowInstances = { items: [], page: 1, pageSize: 50, totalCount: 0 };
    this.selectedOrderByState = OrderBy.Started;
    this.selectedWorkflowInstanceIds = [];
    this.currentPage = 0;
    this.currentPageSize = ElsaWorkflowInstanceListScreen.DEFAULT_PAGE_SIZE;
    this.t = (key, options) => this.i18next.t(key, options);
    this.getSelectAllState = () => {
      const { items } = this.workflowInstances;
      for (let i = 0; i < items.length; i++) {
        if (!this.selectedWorkflowInstanceIds.includes(items[i].id)) {
          return false;
        }
      }
      return true;
    };
    this.setSelectAllIndeterminateState = () => {
      if (this.selectAllCheckboxEl) {
        const selectedItems = this.workflowInstances.items.filter(item => this.selectedWorkflowInstanceIds.includes(item.id));
        this.selectAllCheckboxEl.indeterminate = selectedItems.length != 0 && selectedItems.length != this.workflowInstances.items.length;
      }
    };
    this.onPaged = async (e) => {
      this.currentPage = e.detail.page;
      await this.loadWorkflowInstances();
    };
  }
  connectedCallback() {
    if (!!this.history)
      this.unlistenRouteChanged = this.history.listen(e => this.routeChanged(e));
  }
  disconnectedCallback() {
    if (!!this.unlistenRouteChanged)
      this.unlistenRouteChanged();
  }
  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
    this.selectedWorkflowId = this.workflowId;
    this.selectedCorrelationId = this.correlationId;
    this.selectedWorkflowStatus = this.workflowStatus;
    this.selectedOrderByState = this.orderBy;
    if (!!this.history)
      this.applyQueryString(this.history.location.search);
    await this.loadWorkflowBlueprints();
    await this.loadWorkflowInstances();
    const t = this.t;
    let bulkActions = [{
        text: t('BulkActions.Actions.Cancel').toString(),
        name: 'Cancel',
      }, {
        text: t('BulkActions.Actions.Delete').toString(),
        name: 'Delete',
      }, {
        text: t('BulkActions.Actions.Retry').toString(),
        name: 'Retry',
      }];
    await eventBus.emit(EventTypes.WorkflowInstanceBulkActionsLoading, this, { sender: this, bulkActions });
    this.bulkActions = bulkActions;
  }
  async getSelectedWorkflowInstanceIds() {
    return this.selectedWorkflowInstanceIds;
  }
  async refresh() {
    await this.loadWorkflowInstances();
    this.updateSelectAllChecked();
  }
  async handleWorkflowIdChanged(value) {
    this.selectedWorkflowId = value;
    await this.loadWorkflowInstances();
  }
  async handleCorrelationIdChanged(value) {
    this.selectedCorrelationId = value;
    await this.loadWorkflowInstances();
  }
  async handleWorkflowStatusChanged(value) {
    this.selectedWorkflowStatus = value;
    await this.loadWorkflowInstances();
  }
  async handlePageSizeChanged(value) {
    this.currentPageSize = value;
    this.currentPageSize = isNaN(this.currentPageSize) ? ElsaWorkflowInstanceListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.currentPageSize = Math.max(Math.min(this.currentPageSize, ElsaWorkflowInstanceListScreen.MAX_PAGE_SIZE), ElsaWorkflowInstanceListScreen.MIN_PAGE_SIZE);
    await this.loadWorkflowInstances();
  }
  async handleOrderByChanged(value) {
    this.selectedOrderByState = value;
    await this.loadWorkflowInstances();
  }
  applyQueryString(queryString) {
    var _a;
    const query = parseQuery(queryString);
    this.selectedWorkflowId = query.workflow;
    this.correlationId = query.correlationId;
    this.selectedWorkflowStatus = query.status;
    this.selectedOrderByState = (_a = query.orderBy) !== null && _a !== void 0 ? _a : OrderBy.Started;
    this.currentPage = !!query.page ? parseInt(query.page) : 0;
    this.currentPage = isNaN(this.currentPage) ? ElsaWorkflowInstanceListScreen.START_PAGE : this.currentPage;
    this.currentPageSize = !!query.pageSize ? parseInt(query.pageSize) : ElsaWorkflowInstanceListScreen.DEFAULT_PAGE_SIZE;
    this.currentPageSize = isNaN(this.currentPageSize) ? ElsaWorkflowInstanceListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.currentPageSize = Math.max(Math.min(this.currentPageSize, ElsaWorkflowInstanceListScreen.MAX_PAGE_SIZE), ElsaWorkflowInstanceListScreen.MIN_PAGE_SIZE);
  }
  async loadWorkflowBlueprints() {
    const elsaClient = await this.createClient();
    this.workflowBlueprints = await elsaClient.workflowRegistryApi.listAll({ allVersions: true });
  }
  async loadWorkflowInstances() {
    this.currentPage = isNaN(this.currentPage) ? ElsaWorkflowInstanceListScreen.START_PAGE : this.currentPage;
    this.currentPage = Math.max(this.currentPage, ElsaWorkflowInstanceListScreen.START_PAGE);
    this.currentPageSize = isNaN(this.currentPageSize) ? ElsaWorkflowInstanceListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    const elsaClient = await this.createClient();
    this.workflowInstances = await elsaClient.workflowInstancesApi.list(this.currentPage, this.currentPageSize, this.selectedWorkflowId, this.selectedWorkflowStatus, this.selectedOrderByState, this.currentSearchTerm, this.correlationId);
    const maxPage = Math.floor(this.workflowInstances.totalCount / this.currentPageSize);
    if (this.currentPage > maxPage) {
      this.currentPage = maxPage;
      this.workflowInstances = await elsaClient.workflowInstancesApi.list(this.currentPage, this.currentPageSize, this.selectedWorkflowId, this.selectedWorkflowStatus, this.selectedOrderByState, this.currentSearchTerm, this.correlationId);
    }
    this.setSelectAllIndeterminateState();
  }
  createClient() {
    return createElsaClient(this.serverUrl);
  }
  getLatestWorkflowBlueprintVersions() {
    const groups = collection.groupBy(this.workflowBlueprints, 'id');
    return collection.map(groups, x => array.first(collection.orderBy(x, 'version', 'desc')));
  }
  buildFilterUrl(workflowId, workflowStatus, orderBy, pageSize, correlationId) {
    const filters = {};
    if (!!correlationId)
      filters['correlationId'] = correlationId;
    if (!!workflowId)
      filters['workflow'] = workflowId;
    if (!!workflowStatus)
      filters['status'] = workflowStatus;
    if (!!orderBy)
      filters['orderBy'] = orderBy;
    if (!!this.currentPage)
      filters['page'] = this.currentPage.toString();
    let newPageSize = !!pageSize ? pageSize : this.currentPageSize;
    newPageSize = Math.max(Math.min(newPageSize, 100), ElsaWorkflowInstanceListScreen.MIN_PAGE_SIZE);
    filters['pageSize'] = newPageSize.toString();
    if (newPageSize != this.currentPageSize)
      filters['page'] = Math.floor(this.currentPage * this.currentPageSize / newPageSize).toString();
    const queryString = collection.map(filters, (v, k) => `${k}=${v}`).join('&');
    return `${this.basePath}/workflow-instances?${queryString}`;
  }
  getStatusColor(status) {
    switch (status) {
      default:
      case WorkflowStatus.Idle:
        return "gray";
      case WorkflowStatus.Running:
        return "rose";
      case WorkflowStatus.Suspended:
        return "blue";
      case WorkflowStatus.Finished:
        return "green";
      case WorkflowStatus.Faulted:
        return "red";
      case WorkflowStatus.Cancelled:
        return "yellow";
    }
  }
  updateSelectAllChecked() {
    if (this.workflowInstances.items.length == 0) {
      this.selectAllChecked = false;
      return;
    }
    this.selectAllChecked = this.workflowInstances.items.findIndex(x => this.selectedWorkflowInstanceIds.findIndex(id => id == x.id) < 0) < 0;
  }
  async routeChanged(e) {
    if (!e.pathname.toLowerCase().endsWith('workflow-instances'))
      return;
    this.applyQueryString(e.search);
    await this.loadWorkflowInstances();
  }
  onSelectAllCheckChange(e) {
    const checkBox = e.target;
    const isChecked = checkBox.checked;
    this.selectAllChecked = isChecked;
    if (isChecked) {
      let itemsToAdd = [];
      this.workflowInstances.items.forEach(item => {
        if (!this.selectedWorkflowInstanceIds.includes(item.id)) {
          itemsToAdd.push(item.id);
        }
      });
      if (itemsToAdd.length > 0) {
        this.selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds.concat(itemsToAdd);
      }
    }
    else {
      const currentItems = this.workflowInstances.items.map(x => x.id);
      this.selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds.filter(item => {
        return !currentItems.includes(item);
      });
    }
  }
  onWorkflowInstanceCheckChange(e, workflowInstance) {
    const checkBox = e.target;
    const isChecked = checkBox.checked;
    if (isChecked)
      this.selectedWorkflowInstanceIds = [...this.selectedWorkflowInstanceIds, workflowInstance.id];
    else
      this.selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds.filter(x => x != workflowInstance.id);
    this.setSelectAllIndeterminateState();
  }
  async onCancelClick(e, workflowInstance) {
    const t = this.t;
    const result = await confirmDialogService.show(t('CancelDialog.Title').toString(), t('CancelDialog.Message').toString());
    if (!result)
      return;
    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.cancel(workflowInstance.id);
    await this.loadWorkflowInstances();
  }
  async onDeleteClick(e, workflowInstance) {
    const t = this.t;
    const result = await confirmDialogService.show(t('DeleteDialog.Title').toString(), t('DeleteDialog.Message').toString());
    if (!result)
      return;
    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.delete(workflowInstance.id);
    await this.loadWorkflowInstances();
  }
  async onRetryClick(e, workflowInstance) {
    const t = this.t;
    const result = await confirmDialogService.show(t('RetryDialog.Title').toString(), t('RetryDialog.Message').toString());
    if (!result)
      return;
    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.retry(workflowInstance.id);
    await this.loadWorkflowInstances();
  }
  async onBulkCancel() {
    const t = this.t;
    const result = await confirmDialogService.show(t('BulkCancelDialog.Title').toString(), t('BulkCancelDialog.Message').toString());
    if (!result)
      return;
    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.bulkCancel({ workflowInstanceIds: this.selectedWorkflowInstanceIds });
    this.selectedWorkflowInstanceIds = [];
    await this.loadWorkflowInstances();
    this.currentPage = 0;
  }
  async onBulkDelete() {
    const t = this.t;
    const result = await confirmDialogService.show(t('BulkDeleteDialog.Title').toString(), t('BulkDeleteDialog.Message').toString());
    if (!result)
      return;
    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.bulkDelete({ workflowInstanceIds: this.selectedWorkflowInstanceIds });
    this.selectedWorkflowInstanceIds = [];
    await this.loadWorkflowInstances();
    this.currentPage = 0;
  }
  async onBulkRetry() {
    const t = this.t;
    const result = await confirmDialogService.show(t('BulkRetryDialog.Title').toString(), t('BulkRetryDialog.Message').toString());
    if (!result)
      return;
    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.bulkRetry({ workflowInstanceIds: this.selectedWorkflowInstanceIds });
    this.selectedWorkflowInstanceIds = [];
    await this.loadWorkflowInstances();
    this.currentPage = 0;
  }
  async onBulkActionSelected(e) {
    const action = e.detail;
    switch (action.name) {
      case 'Cancel':
        await this.onBulkCancel();
        break;
      case 'Delete':
        await this.onBulkDelete();
        break;
      case 'Retry':
        await this.onBulkRetry();
        break;
      default:
        action.handler();
    }
    this.updateSelectAllChecked();
  }
  async onSearch(e) {
    e.preventDefault();
    const form = e.currentTarget;
    const formData = new FormData(form);
    const searchTerm = formData.get('searchTerm');
    this.currentSearchTerm = searchTerm.toString();
    await this.loadWorkflowInstances();
  }
  render() {
    const basePath = this.basePath;
    const workflowInstances = this.workflowInstances.items;
    const workflowBlueprints = this.workflowBlueprints;
    const totalCount = this.workflowInstances.totalCount;
    const t = this.t;
    const renderViewIcon = function () {
      return (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", xmlns: "http://www.w3.org/2000/svg", fill: "none", stroke: "currentColor", "stroke-width": "2", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { d: "M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" }), h("path", { d: "M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" })));
    };
    const renderCancelIcon = function () {
      return (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M21 12a9 9 0 11-18 0 9 9 0 0118 0z" }), h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M9 10a1 1 0 011-1h4a1 1 0 011 1v4a1 1 0 01-1 1h-4a1 1 0 01-1-1v-4z" })));
    };
    const renderDeleteIcon = function () {
      return (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("line", { x1: "4", y1: "7", x2: "20", y2: "7" }), h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }), h("line", { x1: "14", y1: "11", x2: "14", y2: "17" }), h("path", { d: "M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" }), h("path", { d: "M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" })));
    };
    const renderRetryIcon = function () {
      return (h("svg", { class: "elsa-h-5 w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("path", { d: "M12 17l-2 2l2 2m-2 -2h9a2 2 0 0 0 1.75 -2.75l-.55 -1" }), h("path", { d: "M12 17l-2 2l2 2m-2 -2h9a2 2 0 0 0 1.75 -2.75l-.55 -1", transform: "rotate(120 12 13)" }), h("path", { d: "M12 17l-2 2l2 2m-2 -2h9a2 2 0 0 0 1.75 -2.75l-.55 -1", transform: "rotate(240 12 13)" })));
    };
    return (h("div", null, h("div", { class: "elsa-relative elsa-z-10 elsa-flex-shrink-0 elsa-flex elsa-h-16 elsa-bg-white elsa-border-b elsa-border-gray-200" }, h("div", { class: "elsa-flex-1 elsa-px-4 elsa-flex elsa-justify-between sm:elsa-px-6 lg:elsa-px-8" }, h("div", { class: "elsa-flex-1 elsa-flex" }, h("form", { class: "elsa-w-full elsa-flex md:ml-0", onSubmit: e => this.onSearch(e) }, h("label", { htmlFor: "search_field", class: "elsa-sr-only" }, "Search"), h("div", { class: "elsa-relative elsa-w-full elsa-text-gray-400 focus-within:elsa-text-gray-600" }, h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-left-0 elsa-flex elsa-items-center elsa-pointer-events-none" }, h("svg", { class: "elsa-h-5 elsa-w-5", fill: "currentColor", viewBox: "0 0 20 20" }, h("path", { "fill-rule": "evenodd", "clip-rule": "evenodd", d: "M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" }))), h("input", { name: "searchTerm", class: "elsa-block elsa-w-full elsa-h-full elsa-pl-8 elsa-pr-3 elsa-py-2 elsa-rounded-md elsa-text-gray-900 elsa-placeholder-gray-500 focus:elsa-placeholder-gray-400 sm:elsa-text-sm elsa-border-0 focus:elsa-outline-none focus:elsa-ring-0", placeholder: t('Search').toString(), type: "search" })))))), h("div", { class: "elsa-p-8 elsa-flex elsa-content-end elsa-justify-right elsa-bg-white elsa-space-x-4" }, h("div", { class: "elsa-flex-shrink-0" }, this.renderBulkActions()), h("div", { class: "elsa-flex-1" }, "\u00A0"), this.renderPageSizeFilter(), this.renderWorkflowFilter(), this.renderStatusFilter(), this.renderOrderByFilter()), h("div", { class: "elsa-mt-8 sm:elsa-block" }, h("div", { class: "elsa-align-middle elsa-inline-block elsa-min-w-full elsa-border-b elsa-border-gray-200" }, h("table", { class: "elsa-min-w-full" }, h("thead", null, h("tr", { class: "elsa-border-t elsa-border-gray-200" }, h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h("input", { type: "checkbox", value: "true", checked: this.getSelectAllState(), onChange: e => this.onSelectAllCheckChange(e), ref: el => this.selectAllCheckboxEl = el, class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.Id')), h("th", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.CorrelationId')), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.Workflow')), h("th", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.Version')), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.InstanceName')), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.Status')), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.Created')), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.Finished')), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.LastExecuted')), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, t('Table.Faulted')), h("th", { class: "elsa-pr-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }))), h("tbody", { class: "elsa-bg-white elsa-divide-y elsa-divide-gray-100" }, workflowInstances.map(workflowInstance => {
      var _a;
      const workflowBlueprint = (_a = workflowBlueprints.find(x => x.versionId == workflowInstance.definitionVersionId)) !== null && _a !== void 0 ? _a : {
        name: 'Not Found',
        displayName: '(Workflow definition not found)'
      };
      const displayName = workflowBlueprint.displayName || workflowBlueprint.name || '(Untitled)';
      const statusColor = this.getStatusColor(workflowInstance.workflowStatus);
      const instanceViewUrl = `${basePath}/workflow-instances/${workflowInstance.id}`;
      const correlationId = !!workflowInstance.correlationId ? workflowInstance.correlationId : '';
      const correlationListViewUrl = `${basePath}/workflow-instances?correlationId=${correlationId}`;
      const blueprintViewUrl = `${basePath}/workflow-registry/${workflowInstance.definitionId}`;
      const instanceName = !workflowInstance.name ? '' : workflowInstance.name;
      const isSelected = this.selectedWorkflowInstanceIds.findIndex(x => x === workflowInstance.id) >= 0;
      const createdAt = hooks(workflowInstance.createdAt);
      const finishedAt = !!workflowInstance.finishedAt ? hooks(workflowInstance.finishedAt) : null;
      const lastExecutedAt = !!workflowInstance.lastExecutedAt ? hooks(workflowInstance.lastExecutedAt) : null;
      const faultedAt = !!workflowInstance.faultedAt ? hooks(workflowInstance.faultedAt) : null;
      const isFaulted = workflowInstance.workflowStatus == WorkflowStatus.Faulted;
      const contextMenuItems = [
        { text: t('Table.ContextMenu.View').toString(), anchorUrl: instanceViewUrl, icon: renderViewIcon() },
        {
          text: t('Table.ContextMenu.Cancel').toString(),
          clickHandler: e => this.onCancelClick(e, workflowInstance),
          icon: renderCancelIcon()
        },
        ...[isFaulted ? {
            text: t('Table.ContextMenu.Retry').toString(),
            clickHandler: e => this.onRetryClick(e, workflowInstance),
            icon: renderRetryIcon()
          } : null],
        {
          text: t('Table.ContextMenu.Delete').toString(),
          clickHandler: e => this.onDeleteClick(e, workflowInstance),
          icon: renderDeleteIcon()
        }
      ].filter(x => x != null);
      return h("tr", null, h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("input", { type: "checkbox", value: workflowInstance.id, checked: isSelected, onChange: e => this.onWorkflowInstanceCheckChange(e, workflowInstance), class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })), h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("stencil-route-link", { url: instanceViewUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, workflowInstance.id)), h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("stencil-route-link", { url: correlationListViewUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, correlationId)), h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900 elsa-text-left" }, h("stencil-route-link", { url: blueprintViewUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, displayName)), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-right elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-uppercase" }, workflowInstance.version), h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900 elsa-text-left" }, h("stencil-route-link", { url: instanceViewUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, instanceName)), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-right" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, h("div", { class: `flex-shrink-0 elsa-w-2-5 elsa-h-2-5 elsa-rounded-full elsa-bg-${statusColor}-600` }), h("span", null, workflowInstance.workflowStatus))), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-left" }, createdAt.format('DD-MM-YYYY HH:mm:ss')), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-left" }, !!finishedAt ? finishedAt.format('DD-MM-YYYY HH:mm:ss') : '-'), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-left" }, !!lastExecutedAt ? lastExecutedAt.format('DD-MM-YYYY HH:mm:ss') : '-'), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-left" }, !!faultedAt ? faultedAt.format('DD-MM-YYYY HH:mm:ss') : '-'), h("td", { class: "elsa-pr-6" }, h("elsa-context-menu", { history: this.history, menuItems: contextMenuItems })));
    }))), h("elsa-pager", { page: this.currentPage, pageSize: this.currentPageSize, totalCount: totalCount, history: this.history, onPaged: this.onPaged, culture: this.culture })))));
  }
  renderBulkActions() {
    const bulkActionIcon = h("svg", { class: "elsa-mr-3 elsa-h-5 elsa-w-5 elsa-text-gray-400", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "1", d: "M13 10V3L4 14h7v7l9-11h-7z" }));
    const t = this.t;
    const actions = this.bulkActions;
    return h("elsa-dropdown-button", { text: t('BulkActions.Title').toString(), items: actions, icon: bulkActionIcon, origin: DropdownButtonOrigin.TopLeft, onItemSelected: e => this.onBulkActionSelected(e) });
  }
  renderWorkflowFilter() {
    const t = this.t;
    const latestWorkflowBlueprints = this.getLatestWorkflowBlueprintVersions();
    const selectedCorrelationId = this.selectedCorrelationId;
    const selectedWorkflowId = this.selectedWorkflowId;
    const selectedWorkflow = latestWorkflowBlueprints.find(x => x.id == selectedWorkflowId);
    const selectedWorkflowText = !selectedWorkflowId ? t('Filters.Workflow.Label').toString() : !!selectedWorkflow && (selectedWorkflow.name || selectedWorkflow.displayName) ? (selectedWorkflow.displayName || selectedWorkflow.name) : t('Untitled').toString();
    const selectedWorkflowStatus = this.selectedWorkflowStatus;
    const selectedOrderBy = this.selectedOrderByState;
    const history = this.history;
    let items = latestWorkflowBlueprints.map(x => {
      const displayName = !!x.displayName && x.displayName.length > 0 ? x.displayName : x.name || t('Untitled').toString();
      const item = { text: displayName, value: x.id, isSelected: x.id == selectedWorkflowId };
      if (!!history)
        item.url = this.buildFilterUrl(x.id, selectedWorkflowStatus, selectedOrderBy, null, selectedCorrelationId);
      return item;
    });
    const allItem = { text: t('Filters.Workflow.All').toString(), value: null, isSelected: !selectedWorkflowId };
    if (!!history)
      allItem.url = this.buildFilterUrl(null, selectedWorkflowStatus, selectedOrderBy, null, selectedCorrelationId);
    items = [allItem, ...items];
    const renderIcon = function () {
      return h("svg", { class: "elsa-mr-3 elsa-h-5 elsa-w-5 elsa-text-gray-400", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("rect", { x: "4", y: "4", width: "6", height: "6", rx: "1" }), h("rect", { x: "14", y: "4", width: "6", height: "6", rx: "1" }), h("rect", { x: "4", y: "14", width: "6", height: "6", rx: "1" }), h("rect", { x: "14", y: "14", width: "6", height: "6", rx: "1" }));
    };
    return h("elsa-dropdown-button", { text: selectedWorkflowText, items: items, icon: renderIcon(), origin: DropdownButtonOrigin.TopRight, onItemSelected: e => this.handleWorkflowIdChanged(e.detail.value) });
  }
  renderStatusFilter() {
    const t = this.t;
    const selectedCorrelationId = this.correlationId;
    const selectedWorkflowStatus = this.selectedWorkflowStatus;
    const selectedWorkflowStatusText = !!selectedWorkflowStatus ? selectedWorkflowStatus : t('Filters.Status.Label').toString();
    const statuses = [null, WorkflowStatus.Running, WorkflowStatus.Suspended, WorkflowStatus.Finished, WorkflowStatus.Faulted, WorkflowStatus.Cancelled, WorkflowStatus.Idle];
    const history = this.history;
    const items = statuses.map(x => {
      const text = x !== null && x !== void 0 ? x : t('Filters.Status.All').toString();
      const item = { text: text, isSelected: x == selectedWorkflowStatus, value: x };
      if (!!history)
        item.url = this.buildFilterUrl(this.selectedWorkflowId, x, this.selectedOrderByState, null, selectedCorrelationId);
      return item;
    });
    const renderIcon = function () {
      return h("svg", { class: "elsa-mr-3 elsa-h-5 elsa-w-5 elsa-text-gray-400", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("circle", { cx: "12", cy: "12", r: "10" }), h("polygon", { points: "10 8 16 12 10 16 10 8" }));
    };
    return h("elsa-dropdown-button", { text: selectedWorkflowStatusText, items: items, icon: renderIcon(), origin: DropdownButtonOrigin.TopRight, onItemSelected: e => this.handleWorkflowStatusChanged(e.detail.value) });
  }
  renderPageSizeFilter() {
    const t = this.t;
    const selectedCorrelationId = this.correlationId;
    const currentPageSize = this.currentPageSize;
    const currentPageSizeText = t('Filters.PageSize.SelectedLabel', { Size: currentPageSize }).toString();
    const pageSizes = [5, 10, 15, 20, 30, 50, 100];
    const history = this.history;
    const items = pageSizes.map(x => {
      const text = "" + x;
      const item = { text: text, isSelected: x == currentPageSize, value: x };
      if (!!history)
        item.url = this.buildFilterUrl(this.selectedWorkflowId, this.selectedWorkflowStatus, this.selectedOrderByState, x, selectedCorrelationId);
      return item;
    });
    const renderIcon = function () {
      return h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-400 elsa-mr-2", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("line", { x1: "9", y1: "6", x2: "20", y2: "6" }), h("line", { x1: "9", y1: "12", x2: "20", y2: "12" }), h("line", { x1: "9", y1: "18", x2: "20", y2: "18" }), h("line", { x1: "5", y1: "6", x2: "5", y2: "6.01" }), h("line", { x1: "5", y1: "12", x2: "5", y2: "12.01" }), h("line", { x1: "5", y1: "18", x2: "5", y2: "18.01" }));
    };
    return h("elsa-dropdown-button", { text: currentPageSizeText, items: items, icon: renderIcon(), origin: DropdownButtonOrigin.TopRight, onItemSelected: e => this.handlePageSizeChanged(e.detail.value) });
  }
  renderOrderByFilter() {
    const t = this.t;
    const selectedCorrelationId = this.correlationId;
    const selectedOrderBy = this.selectedOrderByState;
    const selectedOrderByText = !!selectedOrderBy ? t('Filters.Sort.SelectedLabel', { Key: selectedOrderBy }).toString() : t('Filters.Sort.Label').toString();
    const orderByValues = [OrderBy.Finished, OrderBy.LastExecuted, OrderBy.Started];
    const history = this.history;
    const items = orderByValues.map(x => {
      const item = { text: x, value: x, isSelected: x == selectedOrderBy };
      if (!!history)
        item.url = this.buildFilterUrl(this.selectedWorkflowId, this.selectedWorkflowStatus, x, null, selectedCorrelationId);
      return item;
    });
    const renderIcon = function () {
      return h("svg", { class: "elsa-mr-3 elsa-h-5 elsa-w-5 elsa-text-gray-400", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M3 4h13M3 8h9m-9 4h9m5-4v12m0 0l-4-4m4 4l4-4" }));
    };
    return h("elsa-dropdown-button", { text: selectedOrderByText, items: items, icon: renderIcon(), origin: DropdownButtonOrigin.TopRight, onItemSelected: e => this.handleOrderByChanged(e.detail.value) });
  }
  static get watchers() { return {
    "workflowId": ["handleWorkflowIdChanged"],
    "correlationId": ["handleCorrelationIdChanged"],
    "workflowStatus": ["handleWorkflowStatusChanged"],
    "currentPageSize": ["handlePageSizeChanged"],
    "orderBy": ["handleOrderByChanged"]
  }; }
};
ElsaWorkflowInstanceListScreen.DEFAULT_PAGE_SIZE = 15;
ElsaWorkflowInstanceListScreen.MIN_PAGE_SIZE = 5;
ElsaWorkflowInstanceListScreen.MAX_PAGE_SIZE = 100;
ElsaWorkflowInstanceListScreen.START_PAGE = 0;
Tunnel.injectProps(ElsaWorkflowInstanceListScreen, ['serverUrl', 'culture', 'basePath']);
injectHistory(ElsaWorkflowInstanceListScreen);

export { ElsaWorkflowInstanceListScreen as elsa_workflow_instance_list_screen };
