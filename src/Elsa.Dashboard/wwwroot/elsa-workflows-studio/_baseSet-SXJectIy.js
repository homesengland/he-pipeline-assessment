import './_commonjsHelpers-Cf5sKic0.js';
import { p as require_baseIndexOf, I as requireIsArrayLike, J as requireIsObjectLike, e as require_baseFlatten, K as require_overRest, L as require_setToString, M as require_Set, N as require_setToArray, h as require_SetCache, k as require_cacheHas, l as require_baseRest, O as require_baseAssignValue, B as requireEq, v as require_castPath, t as require_isIndex, P as requireIsObject, x as require_toKey } from './collection-B4sYCr2r.js';

var _arrayIncludes;
var hasRequired_arrayIncludes;

function require_arrayIncludes () {
	if (hasRequired_arrayIncludes) return _arrayIncludes;
	hasRequired_arrayIncludes = 1;
	var baseIndexOf = require_baseIndexOf();

	/**
	 * A specialized version of `_.includes` for arrays without support for
	 * specifying an index to search from.
	 *
	 * @private
	 * @param {Array} [array] The array to inspect.
	 * @param {*} target The value to search for.
	 * @returns {boolean} Returns `true` if `target` is found, else `false`.
	 */
	function arrayIncludes(array, value) {
	  var length = array == null ? 0 : array.length;
	  return !!length && baseIndexOf(array, value, 0) > -1;
	}

	_arrayIncludes = arrayIncludes;
	return _arrayIncludes;
}

/**
 * This function is like `arrayIncludes` except that it accepts a comparator.
 *
 * @private
 * @param {Array} [array] The array to inspect.
 * @param {*} target The value to search for.
 * @param {Function} comparator The comparator invoked per element.
 * @returns {boolean} Returns `true` if `target` is found, else `false`.
 */

var _arrayIncludesWith;
var hasRequired_arrayIncludesWith;

function require_arrayIncludesWith () {
	if (hasRequired_arrayIncludesWith) return _arrayIncludesWith;
	hasRequired_arrayIncludesWith = 1;
	function arrayIncludesWith(array, value, comparator) {
	  var index = -1,
	      length = array == null ? 0 : array.length;

	  while (++index < length) {
	    if (comparator(value, array[index])) {
	      return true;
	    }
	  }
	  return false;
	}

	_arrayIncludesWith = arrayIncludesWith;
	return _arrayIncludesWith;
}

var isArrayLikeObject_1;
var hasRequiredIsArrayLikeObject;

function requireIsArrayLikeObject () {
	if (hasRequiredIsArrayLikeObject) return isArrayLikeObject_1;
	hasRequiredIsArrayLikeObject = 1;
	var isArrayLike = requireIsArrayLike(),
	    isObjectLike = requireIsObjectLike();

	/**
	 * This method is like `_.isArrayLike` except that it also checks if `value`
	 * is an object.
	 *
	 * @static
	 * @memberOf _
	 * @since 4.0.0
	 * @category Lang
	 * @param {*} value The value to check.
	 * @returns {boolean} Returns `true` if `value` is an array-like object,
	 *  else `false`.
	 * @example
	 *
	 * _.isArrayLikeObject([1, 2, 3]);
	 * // => true
	 *
	 * _.isArrayLikeObject(document.body.children);
	 * // => true
	 *
	 * _.isArrayLikeObject('abc');
	 * // => false
	 *
	 * _.isArrayLikeObject(_.noop);
	 * // => false
	 */
	function isArrayLikeObject(value) {
	  return isObjectLike(value) && isArrayLike(value);
	}

	isArrayLikeObject_1 = isArrayLikeObject;
	return isArrayLikeObject_1;
}

var flatten_1;
var hasRequiredFlatten;

function requireFlatten () {
	if (hasRequiredFlatten) return flatten_1;
	hasRequiredFlatten = 1;
	var baseFlatten = require_baseFlatten();

	/**
	 * Flattens `array` a single level deep.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.1.0
	 * @category Array
	 * @param {Array} array The array to flatten.
	 * @returns {Array} Returns the new flattened array.
	 * @example
	 *
	 * _.flatten([1, [2, [3, [4]], 5]]);
	 * // => [1, 2, [3, [4]], 5]
	 */
	function flatten(array) {
	  var length = array == null ? 0 : array.length;
	  return length ? baseFlatten(array, 1) : [];
	}

	flatten_1 = flatten;
	return flatten_1;
}

var _flatRest;
var hasRequired_flatRest;

function require_flatRest () {
	if (hasRequired_flatRest) return _flatRest;
	hasRequired_flatRest = 1;
	var flatten = requireFlatten(),
	    overRest = require_overRest(),
	    setToString = require_setToString();

	/**
	 * A specialized version of `baseRest` which flattens the rest array.
	 *
	 * @private
	 * @param {Function} func The function to apply a rest parameter to.
	 * @returns {Function} Returns the new function.
	 */
	function flatRest(func) {
	  return setToString(overRest(func, undefined, flatten), func + '');
	}

	_flatRest = flatRest;
	return _flatRest;
}

/**
 * This method returns `undefined`.
 *
 * @static
 * @memberOf _
 * @since 2.3.0
 * @category Util
 * @example
 *
 * _.times(2, _.noop);
 * // => [undefined, undefined]
 */

var noop_1;
var hasRequiredNoop;

function requireNoop () {
	if (hasRequiredNoop) return noop_1;
	hasRequiredNoop = 1;
	function noop() {
	  // No operation performed.
	}

	noop_1 = noop;
	return noop_1;
}

var _createSet;
var hasRequired_createSet;

function require_createSet () {
	if (hasRequired_createSet) return _createSet;
	hasRequired_createSet = 1;
	var Set = require_Set(),
	    noop = requireNoop(),
	    setToArray = require_setToArray();

	/** Used as references for various `Number` constants. */
	var INFINITY = 1 / 0;

	/**
	 * Creates a set object of `values`.
	 *
	 * @private
	 * @param {Array} values The values to add to the set.
	 * @returns {Object} Returns the new set.
	 */
	var createSet = !(Set && (1 / setToArray(new Set([,-0]))[1]) == INFINITY) ? noop : function(values) {
	  return new Set(values);
	};

	_createSet = createSet;
	return _createSet;
}

var _baseUniq;
var hasRequired_baseUniq;

function require_baseUniq () {
	if (hasRequired_baseUniq) return _baseUniq;
	hasRequired_baseUniq = 1;
	var SetCache = require_SetCache(),
	    arrayIncludes = require_arrayIncludes(),
	    arrayIncludesWith = require_arrayIncludesWith(),
	    cacheHas = require_cacheHas(),
	    createSet = require_createSet(),
	    setToArray = require_setToArray();

	/** Used as the size to enable large array optimizations. */
	var LARGE_ARRAY_SIZE = 200;

	/**
	 * The base implementation of `_.uniqBy` without support for iteratee shorthands.
	 *
	 * @private
	 * @param {Array} array The array to inspect.
	 * @param {Function} [iteratee] The iteratee invoked per element.
	 * @param {Function} [comparator] The comparator invoked per element.
	 * @returns {Array} Returns the new duplicate free array.
	 */
	function baseUniq(array, iteratee, comparator) {
	  var index = -1,
	      includes = arrayIncludes,
	      length = array.length,
	      isCommon = true,
	      result = [],
	      seen = result;

	  if (comparator) {
	    isCommon = false;
	    includes = arrayIncludesWith;
	  }
	  else if (length >= LARGE_ARRAY_SIZE) {
	    var set = iteratee ? null : createSet(array);
	    if (set) {
	      return setToArray(set);
	    }
	    isCommon = false;
	    includes = cacheHas;
	    seen = new SetCache;
	  }
	  else {
	    seen = iteratee ? [] : result;
	  }
	  outer:
	  while (++index < length) {
	    var value = array[index],
	        computed = iteratee ? iteratee(value) : value;

	    value = (comparator || value !== 0) ? value : 0;
	    if (isCommon && computed === computed) {
	      var seenIndex = seen.length;
	      while (seenIndex--) {
	        if (seen[seenIndex] === computed) {
	          continue outer;
	        }
	      }
	      if (iteratee) {
	        seen.push(computed);
	      }
	      result.push(value);
	    }
	    else if (!includes(seen, computed, comparator)) {
	      if (seen !== result) {
	        seen.push(computed);
	      }
	      result.push(value);
	    }
	  }
	  return result;
	}

	_baseUniq = baseUniq;
	return _baseUniq;
}

var union_1;
var hasRequiredUnion;

function requireUnion () {
	if (hasRequiredUnion) return union_1;
	hasRequiredUnion = 1;
	var baseFlatten = require_baseFlatten(),
	    baseRest = require_baseRest(),
	    baseUniq = require_baseUniq(),
	    isArrayLikeObject = requireIsArrayLikeObject();

	/**
	 * Creates an array of unique values, in order, from all given arrays using
	 * [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
	 * for equality comparisons.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.1.0
	 * @category Array
	 * @param {...Array} [arrays] The arrays to inspect.
	 * @returns {Array} Returns the new array of combined values.
	 * @example
	 *
	 * _.union([2], [1, 2]);
	 * // => [2, 1]
	 */
	var union = baseRest(function(arrays) {
	  return baseUniq(baseFlatten(arrays, 1, isArrayLikeObject, true));
	});

	union_1 = union;
	return union_1;
}

var _assignValue;
var hasRequired_assignValue;

function require_assignValue () {
	if (hasRequired_assignValue) return _assignValue;
	hasRequired_assignValue = 1;
	var baseAssignValue = require_baseAssignValue(),
	    eq = requireEq();

	/** Used for built-in method references. */
	var objectProto = Object.prototype;

	/** Used to check objects for own properties. */
	var hasOwnProperty = objectProto.hasOwnProperty;

	/**
	 * Assigns `value` to `key` of `object` if the existing value is not equivalent
	 * using [`SameValueZero`](http://ecma-international.org/ecma-262/7.0/#sec-samevaluezero)
	 * for equality comparisons.
	 *
	 * @private
	 * @param {Object} object The object to modify.
	 * @param {string} key The key of the property to assign.
	 * @param {*} value The value to assign.
	 */
	function assignValue(object, key, value) {
	  var objValue = object[key];
	  if (!(hasOwnProperty.call(object, key) && eq(objValue, value)) ||
	      (value === undefined && !(key in object))) {
	    baseAssignValue(object, key, value);
	  }
	}

	_assignValue = assignValue;
	return _assignValue;
}

/**
 * This base implementation of `_.zipObject` which assigns values using `assignFunc`.
 *
 * @private
 * @param {Array} props The property identifiers.
 * @param {Array} values The property values.
 * @param {Function} assignFunc The function to assign values.
 * @returns {Object} Returns the new object.
 */

var _baseZipObject;
var hasRequired_baseZipObject;

function require_baseZipObject () {
	if (hasRequired_baseZipObject) return _baseZipObject;
	hasRequired_baseZipObject = 1;
	function baseZipObject(props, values, assignFunc) {
	  var index = -1,
	      length = props.length,
	      valsLength = values.length,
	      result = {};

	  while (++index < length) {
	    var value = index < valsLength ? values[index] : undefined;
	    assignFunc(result, props[index], value);
	  }
	  return result;
	}

	_baseZipObject = baseZipObject;
	return _baseZipObject;
}

var zipObject_1;
var hasRequiredZipObject;

function requireZipObject () {
	if (hasRequiredZipObject) return zipObject_1;
	hasRequiredZipObject = 1;
	var assignValue = require_assignValue(),
	    baseZipObject = require_baseZipObject();

	/**
	 * This method is like `_.fromPairs` except that it accepts two arrays,
	 * one of property identifiers and one of corresponding values.
	 *
	 * @static
	 * @memberOf _
	 * @since 0.4.0
	 * @category Array
	 * @param {Array} [props=[]] The property identifiers.
	 * @param {Array} [values=[]] The property values.
	 * @returns {Object} Returns the new object.
	 * @example
	 *
	 * _.zipObject(['a', 'b'], [1, 2]);
	 * // => { 'a': 1, 'b': 2 }
	 */
	function zipObject(props, values) {
	  return baseZipObject(props || [], values || [], assignValue);
	}

	zipObject_1 = zipObject;
	return zipObject_1;
}

var _baseSet;
var hasRequired_baseSet;

function require_baseSet () {
	if (hasRequired_baseSet) return _baseSet;
	hasRequired_baseSet = 1;
	var assignValue = require_assignValue(),
	    castPath = require_castPath(),
	    isIndex = require_isIndex(),
	    isObject = requireIsObject(),
	    toKey = require_toKey();

	/**
	 * The base implementation of `_.set`.
	 *
	 * @private
	 * @param {Object} object The object to modify.
	 * @param {Array|string} path The path of the property to set.
	 * @param {*} value The value to set.
	 * @param {Function} [customizer] The function to customize path creation.
	 * @returns {Object} Returns `object`.
	 */
	function baseSet(object, path, value, customizer) {
	  if (!isObject(object)) {
	    return object;
	  }
	  path = castPath(path, object);

	  var index = -1,
	      length = path.length,
	      lastIndex = length - 1,
	      nested = object;

	  while (nested != null && ++index < length) {
	    var key = toKey(path[index]),
	        newValue = value;

	    if (key === '__proto__' || key === 'constructor' || key === 'prototype') {
	      return object;
	    }

	    if (index != lastIndex) {
	      var objValue = nested[key];
	      newValue = customizer ? customizer(objValue, key, nested) : undefined;
	      if (newValue === undefined) {
	        newValue = isObject(objValue)
	          ? objValue
	          : (isIndex(path[index + 1]) ? [] : {});
	      }
	    }
	    assignValue(nested, key, newValue);
	    nested = nested[key];
	  }
	  return object;
	}

	_baseSet = baseSet;
	return _baseSet;
}

export { require_arrayIncludesWith as a, requireIsArrayLikeObject as b, require_flatRest as c, require_baseUniq as d, require_baseSet as e, require_baseZipObject as f, requireZipObject as g, requireUnion as h, requireFlatten as i, require_assignValue as j, require_arrayIncludes as r };
//# sourceMappingURL=_baseSet-SXJectIy.js.map

//# sourceMappingURL=_baseSet-SXJectIy.js.map