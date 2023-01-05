/**
 * Virtual DOM patching algorithm based on Snabbdom by
 * Simon Friis Vindum (@paldepind)
 * Licensed under the MIT License
 * https://github.com/snabbdom/snabbdom/blob/master/LICENSE
 *
 * Modified for Stencil's renderer and slot projection
 */
let e = !1, t = !1;

const n = {}, isComplexType = e => "object" === (
// https://jsperf.com/typeof-fn-object/5
e = typeof e) || "function" === e, h = (e, t, ...n) => {
  let l = null, s = null, r = !1, c = !1;
  const i = [], walk = t => {
    for (let n = 0; n < t.length; n++) l = t[n], Array.isArray(l) ? walk(l) : null != l && "boolean" != typeof l && ((r = "function" != typeof e && !isComplexType(l)) && (l += ""), 
    r && c ? 
    // If the previous child was simple (string), we merge both
    i[i.length - 1]._$$text$$_ += l : 
    // Append a new vNode, if it's text, we create a text vNode
    i.push(r ? newVNode(null, l) : l), c = r);
  };
  if (walk(n), t) {
    // normalize class / classname attributes
    t.key && (s = t.key);
    {
      const e = t.className || t.class;
      e && (t.class = "object" != typeof e ? e : Object.keys(e).filter((t => e[t])).join(" "));
    }
  }
  if ("function" == typeof e) 
  // nodeName is a functional component
  return e(null === t ? {} : t, i, o);
  const u = newVNode(e, null);
  return u._$$attrs$$_ = t, i.length > 0 && (u._$$children$$_ = i), u._$$key$$_ = s, 
  u;
}, newVNode = (e, t) => {
  const n = {
    _$$flags$$_: 0,
    _$$tag$$_: e,
    _$$text$$_: t,
    _$$elm$$_: null,
    _$$children$$_: null,
    _$$attrs$$_: null,
    _$$key$$_: null
  };
  return n;
}, l = {}, o = {
  forEach: (e, t) => e.map(convertToPublic).forEach(t),
  map: (e, t) => e.map(convertToPublic).map(t).map(convertToPrivate)
}, convertToPublic = e => ({
  vattrs: e._$$attrs$$_,
  vchildren: e._$$children$$_,
  vkey: e._$$key$$_,
  vname: e._$$name$$_,
  vtag: e._$$tag$$_,
  vtext: e._$$text$$_
}), convertToPrivate = e => {
  if ("function" == typeof e.vtag) {
    const t = Object.assign({}, e.vattrs);
    return e.vkey && (t.key = e.vkey), e.vname && (t.name = e.vname), h(e.vtag, t, ...e.vchildren || []);
  }
  const t = newVNode(e.vtag, e.vtext);
  return t._$$attrs$$_ = e.vattrs, t._$$children$$_ = e.vchildren, t._$$key$$_ = e.vkey, 
  t._$$name$$_ = e.vname, t;
}, createEvent = (e, t, n) => {
  const l = (e => getHostRef(e)._$$hostElement$$_)(e);
  return {
    emit: e => emitEvent(l, t, {
      bubbles: !!(4 /* EVENT_FLAGS.Bubbles */ & n),
      composed: !!(2 /* EVENT_FLAGS.Composed */ & n),
      cancelable: !!(1 /* EVENT_FLAGS.Cancellable */ & n),
      detail: e
    })
  };
}, emitEvent = (e, t, n) => {
  const l = a.ce(t, n);
  return e.dispatchEvent(l), l;
}, setAccessor = (e, t, n, l, o, s) => {
  if (n !== l) {
    let r = isMemberInElement(e, t), c = t.toLowerCase();
    if ("class" === t) {
      const t = e.classList, o = parseClassList(n), s = parseClassList(l);
      t.remove(...o.filter((e => e && !s.includes(e)))), t.add(...s.filter((e => e && !o.includes(e))));
    } else if ("style" === t) {
      for (const t in n) l && null != l[t] || (t.includes("-") ? e.style.removeProperty(t) : e.style[t] = "");
      for (const t in l) n && l[t] === n[t] || (t.includes("-") ? e.style.setProperty(t, l[t]) : e.style[t] = l[t]);
    } else if ("key" === t) ; else if (r || "o" !== t[0] || "n" !== t[1]) {
      // Set property if it exists and it's not a SVG
      const c = isComplexType(l);
      if ((r || c && null !== l) && !o) try {
        if (e.tagName.includes("-")) e[t] = l; else {
          const o = null == l ? "" : l;
          // Workaround for Safari, moving the <input> caret when re-assigning the same valued
                    "list" === t ? r = !1 : null != n && e[t] == o || (e[t] = o);
        }
      } catch (e) {}
      null == l || !1 === l ? !1 === l && "" !== e.getAttribute(t) || e.removeAttribute(t) : (!r || 4 /* VNODE_FLAGS.isHost */ & s || o) && !c && (l = !0 === l ? "" : l, 
      e.setAttribute(t, l));
    } else 
    // Event Handlers
    // so if the member name starts with "on" and the 3rd characters is
    // a capital letter, and it's not already a member on the element,
    // then we're assuming it's an event listener
    // on- prefixed events
    // allows to be explicit about the dom event to listen without any magic
    // under the hood:
    // <my-cmp on-click> // listens for "click"
    // <my-cmp on-Click> // listens for "Click"
    // <my-cmp on-ionChange> // listens for "ionChange"
    // <my-cmp on-EVENTS> // listens for "EVENTS"
    t = "-" === t[2] ? t.slice(3) : isMemberInElement(i, c) ? c.slice(2) : c[2] + t.slice(3), 
    n && a.rel(e, t, n, !1), l && a.ael(e, t, l, !1);
  }
}, s = /\s/, parseClassList = e => e ? e.split(s) : [], updateElement = (e, t, l, o) => {
  // if the element passed in is a shadow root, which is a document fragment
  // then we want to be adding attrs/props to the shadow root's "host" element
  // if it's not a shadow root, then we add attrs/props to the same element
  const s = 11 /* NODE_TYPE.DocumentFragment */ === t._$$elm$$_.nodeType && t._$$elm$$_.host ? t._$$elm$$_.host : t._$$elm$$_, r = e && e._$$attrs$$_ || n, c = t._$$attrs$$_ || n;
  // remove attributes no longer present on the vnode by setting them to undefined
  for (o in r) o in c || setAccessor(s, o, r[o], void 0, l, t._$$flags$$_);
  // add new & update changed attributes
  for (o in c) setAccessor(s, o, r[o], c[o], l, t._$$flags$$_);
}, createElm = (t, n, l, o) => {
  // tslint:disable-next-line: prefer-const
  const s = n._$$children$$_[l];
  let r, c, i = 0;
  if (null !== s._$$text$$_) 
  // create text node
  r = s._$$elm$$_ = u.createTextNode(s._$$text$$_); else {
    if (e || (e = "svg" === s._$$tag$$_), 
    // create element
    r = s._$$elm$$_ = u.createElementNS(e ? "http://www.w3.org/2000/svg" : "http://www.w3.org/1999/xhtml", s._$$tag$$_), 
    e && "foreignObject" === s._$$tag$$_ && (e = !1), updateElement(null, s, e), s._$$children$$_) for (i = 0; i < s._$$children$$_.length; ++i) 
    // create the node
    c = createElm(t, s, i), 
    // return node could have been null
    c && 
    // append our new node
    r.appendChild(c);
    "svg" === s._$$tag$$_ ? 
    // Only reset the SVG context when we're exiting <svg> element
    e = !1 : "foreignObject" === r.tagName && (
    // Reenter SVG context when we're exiting <foreignObject> element
    e = !0);
  }
  return r;
}, addVnodes = (e, t, n, l, o, s) => {
  let r, c = e;
  for (;o <= s; ++o) l[o] && (r = createElm(null, n, o), r && (l[o]._$$elm$$_ = r, 
  c.insertBefore(r, t)));
}, removeVnodes = (e, t, n, l, o) => {
  for (;t <= n; ++t) (l = e[t]) && 
  // remove the vnode's element from the dom
  l._$$elm$$_.remove();
}, isSameVnode = (e, t) => 
// compare if two vnode to see if they're "technically" the same
// need to have the same element tag, and same key to be the same
e._$$tag$$_ === t._$$tag$$_ && e._$$key$$_ === t._$$key$$_, patch = (t, n) => {
  const l = n._$$elm$$_ = t._$$elm$$_, o = t._$$children$$_, s = n._$$children$$_, r = n._$$tag$$_, c = n._$$text$$_;
  null === c ? (
  // test if we're rendering an svg element, or still rendering nodes inside of one
  // only add this to the when the compiler sees we're using an svg somewhere
  e = "svg" === r || "foreignObject" !== r && e, 
  // either this is the first render of an element OR it's an update
  // AND we already know it's possible it could have changed
  // this updates the element's css classes, attrs, props, listeners, etc.
  updateElement(t, n, e), null !== o && null !== s ? 
  // looks like there's child vnodes for both the old and new vnodes
  // so we need to call `updateChildren` to reconcile them
  ((e, t, n, l) => {
    let o, s, r = 0, c = 0, i = 0, u = 0, a = t.length - 1, f = t[0], d = t[a], m = l.length - 1, $ = l[0], p = l[m];
    for (;r <= a && c <= m; ) if (null == f) 
    // VNode might have been moved left
    f = t[++r]; else if (null == d) d = t[--a]; else if (null == $) $ = l[++c]; else if (null == p) p = l[--m]; else if (isSameVnode(f, $)) 
    // if the start nodes are the same then we should patch the new VNode
    // onto the old one, and increment our `newStartIdx` and `oldStartIdx`
    // indices to reflect that. We don't need to move any DOM Nodes around
    // since things are matched up in order.
    patch(f, $), f = t[++r], $ = l[++c]; else if (isSameVnode(d, p)) 
    // likewise, if the end nodes are the same we patch new onto old and
    // decrement our end indices, and also likewise in this case we don't
    // need to move any DOM Nodes.
    patch(d, p), d = t[--a], p = l[--m]; else if (isSameVnode(f, p)) patch(f, p), 
    // We need to move the element for `oldStartVnode` into a position which
    // will be appropriate for `newEndVnode`. For this we can use
    // `.insertBefore` and `oldEndVnode.$elm$.nextSibling`. If there is a
    // sibling for `oldEndVnode.$elm$` then we want to move the DOM node for
    // `oldStartVnode` between `oldEndVnode` and it's sibling, like so:
    // <old-start-node />
    // <some-intervening-node />
    // <old-end-node />
    // <!-- ->              <-- `oldStartVnode.$elm$` should be inserted here
    // <next-sibling />
    // If instead `oldEndVnode.$elm$` has no sibling then we just want to put
    // the node for `oldStartVnode` at the end of the children of
    // `parentElm`. Luckily, `Node.nextSibling` will return `null` if there
    // aren't any siblings, and passing `null` to `Node.insertBefore` will
    // append it to the children of the parent element.
    e.insertBefore(f._$$elm$$_, d._$$elm$$_.nextSibling), f = t[++r], p = l[--m]; else if (isSameVnode(d, $)) patch(d, $), 
    // We've already checked above if `oldStartVnode` and `newStartVnode` are
    // the same node, so since we're here we know that they are not. Thus we
    // can move the element for `oldEndVnode` _before_ the element for
    // `oldStartVnode`, leaving `oldStartVnode` to be reconciled in the
    // future.
    e.insertBefore(d._$$elm$$_, f._$$elm$$_), d = t[--a], $ = l[++c]; else {
      for (
      // Here we do some checks to match up old and new nodes based on the
      // `$key$` attribute, which is set by putting a `key="my-key"` attribute
      // in the JSX for a DOM element in the implementation of a Stencil
      // component.
      // First we check to see if there are any nodes in the array of old
      // children which have the same key as the first node in the new
      // children.
      i = -1, u = r; u <= a; ++u) if (t[u] && null !== t[u]._$$key$$_ && t[u]._$$key$$_ === $._$$key$$_) {
        i = u;
        break;
      }
      i >= 0 ? (
      // We found a node in the old children which matches up with the first
      // node in the new children! So let's deal with that
      s = t[i], s._$$tag$$_ !== $._$$tag$$_ ? 
      // the tag doesn't match so we'll need a new DOM element
      o = createElm(t && t[c], n, i) : (patch(s, $), 
      // invalidate the matching old node so that we won't try to update it
      // again later on
      t[i] = void 0, o = s._$$elm$$_), $ = l[++c]) : (
      // We either didn't find an element in the old children that matches
      // the key of the first new child OR the build is not using `key`
      // attributes at all. In either case we need to create a new element
      // for the new node.
      o = createElm(t && t[c], n, c), $ = l[++c]), o && f._$$elm$$_.parentNode.insertBefore(o, f._$$elm$$_);
    }
    r > a ? 
    // we have some more new nodes to add which don't match up with old nodes
    addVnodes(e, null == l[m + 1] ? null : l[m + 1]._$$elm$$_, n, l, c, m) : c > m && 
    // there are nodes in the `oldCh` array which no longer correspond to nodes
    // in the new array, so lets remove them (which entails cleaning up the
    // relevant DOM nodes)
    removeVnodes(t, r, a);
  })(l, o, n, s) : null !== s ? (
  // no old child vnodes, but there are new child vnodes to add
  null !== t._$$text$$_ && (
  // the old vnode was text, so be sure to clear it out
  l.textContent = ""), 
  // add the new vnode children
  addVnodes(l, null, n, s, 0, s.length - 1)) : null !== o && 
  // no new child vnodes, but there are old child vnodes to remove
  removeVnodes(o, 0, o.length - 1), e && "svg" === r && (e = !1)) : t._$$text$$_ !== c && (
  // update the text content for the text only vnode
  // and also only if the text is different than before
  l.data = c);
}, renderVdom = (e, t) => {
  const n = e._$$hostElement$$_, o = e._$$vnode$$_ || newVNode(null, null), s = (e => e && e._$$tag$$_ === l)(t) ? t : h(null, null, t);
  s._$$tag$$_ = null, s._$$flags$$_ |= 4 /* VNODE_FLAGS.isHost */ , e._$$vnode$$_ = s, 
  s._$$elm$$_ = o._$$elm$$_ = n, 
  // synchronous patch
  patch(o, s);
}, attachToAncestor = (e, t) => {
  t && !e._$$onRenderResolve$$_ && t["s-p"] && t["s-p"].push(new Promise((t => e._$$onRenderResolve$$_ = t)));
}, scheduleUpdate = (e, t) => {
  if (e._$$flags$$_ |= 16 /* HOST_FLAGS.isQueuedForUpdate */ , 4 /* HOST_FLAGS.isWaitingForChildren */ & e._$$flags$$_) return void (e._$$flags$$_ |= 512 /* HOST_FLAGS.needsRerender */);
  attachToAncestor(e, e._$$ancestorComponent$$_);
  return m((() => dispatchHooks(e, t)));
}, dispatchHooks = (e, t) => {
  const n = e._$$hostElement$$_, l = (e._$$cmpMeta$$_._$$tagName$$_, () => {}), o = e._$$lazyInstance$$_;
  let s;
  return t ? (e._$$flags$$_ |= 256 /* HOST_FLAGS.isListenReady */ , e._$$queuedListeners$$_ && (e._$$queuedListeners$$_.map((([e, t]) => safeCall(o, e, t))), 
  e._$$queuedListeners$$_ = null), emitLifecycleEvent(n, "componentWillLoad"), s = safeCall(o, "componentWillLoad")) : emitLifecycleEvent(n, "componentWillUpdate"), 
  emitLifecycleEvent(n, "componentWillRender"), l(), then(s, (() => updateComponent(e, o)));
}, updateComponent = async (e, t, n) => {
  // updateComponent
  const l = e._$$hostElement$$_, o = (e._$$cmpMeta$$_._$$tagName$$_, () => {}), s = l["s-rc"], r = (e._$$cmpMeta$$_._$$tagName$$_, 
  () => {});
  callRender(e, t), s && (
  // ok, so turns out there are some child host elements
  // waiting on this parent element to load
  // let's fire off all update callbacks waiting
  s.map((e => e())), l["s-rc"] = void 0), r(), o();
  {
    const t = l["s-p"], postUpdate = () => postUpdateComponent(e);
    0 === t.length ? postUpdate() : (Promise.all(t).then(postUpdate), e._$$flags$$_ |= 4 /* HOST_FLAGS.isWaitingForChildren */ , 
    t.length = 0);
  }
}, callRender = (e, t, n) => {
  try {
    t = t.render(), e._$$flags$$_ &= -17 /* HOST_FLAGS.isQueuedForUpdate */ , e._$$flags$$_ |= 2 /* HOST_FLAGS.hasRendered */ , 
    renderVdom(e, t);
  } catch (t) {
    consoleError(t, e._$$hostElement$$_);
  }
  return null;
}, postUpdateComponent = e => {
  e._$$cmpMeta$$_._$$tagName$$_;
  const t = e._$$hostElement$$_, endPostUpdate = () => {}, n = e._$$ancestorComponent$$_;
  emitLifecycleEvent(t, "componentDidRender"), 64 /* HOST_FLAGS.hasLoadedComponent */ & e._$$flags$$_ ? (emitLifecycleEvent(t, "componentDidUpdate"), 
  endPostUpdate()) : (e._$$flags$$_ |= 64 /* HOST_FLAGS.hasLoadedComponent */ , 
  // DOM WRITE!
  addHydratedFlag(t), emitLifecycleEvent(t, "componentDidLoad"), endPostUpdate(), 
  e._$$onReadyResolve$$_(t), n || appDidLoad()), e._$$onRenderResolve$$_ && (e._$$onRenderResolve$$_(), 
  e._$$onRenderResolve$$_ = void 0), 512 /* HOST_FLAGS.needsRerender */ & e._$$flags$$_ && nextTick((() => scheduleUpdate(e, !1))), 
  e._$$flags$$_ &= -517 /* HOST_FLAGS.needsRerender */;
}
// ( •_•)
// ( •_•)>⌐■-■
// (⌐■_■)
, appDidLoad = e => {
  addHydratedFlag(u.documentElement), nextTick((() => emitEvent(i, "appload", {
    detail: {
      namespace: "custom-elsa"
    }
  })));
}, safeCall = (e, t, n) => {
  if (e && e[t]) try {
    return e[t](n);
  } catch (e) {
    consoleError(e);
  }
}, then = (e, t) => e && e.then ? e.then(t) : t(), emitLifecycleEvent = (e, t) => {
  emitEvent(e, "stencil_" + t, {
    bubbles: !0,
    composed: !0,
    detail: {
      namespace: "custom-elsa"
    }
  });
}, addHydratedFlag = e => e.classList.add("hydrated"), setValue = (e, t, n, l) => {
  // check our new property value against our internal value
  const o = getHostRef(e), s = o._$$instanceValues$$_.get(t), r = o._$$flags$$_, c = o._$$lazyInstance$$_;
  n = ((e, t) => (
  // ensure this value is of the correct prop type
  null == e || isComplexType(e), e))(n);
  8 /* HOST_FLAGS.isConstructingInstance */ & r && void 0 !== s || !(n !== s && !(Number.isNaN(s) && Number.isNaN(n))) || (
  // gadzooks! the property's value has changed!!
  // set our new value!
  o._$$instanceValues$$_.set(t, n), c && 2 /* HOST_FLAGS.hasRendered */ == (18 /* HOST_FLAGS.isQueuedForUpdate */ & r) && 
  // looks like this value actually changed, so we've got work to do!
  // but only if we've already rendered, otherwise just chill out
  // queue that we need to do an update, but don't worry about queuing
  // up millions cuz this function ensures it only runs once
  scheduleUpdate(o, !1));
}, proxyComponent = (e, t, n) => {
  if (t._$$members$$_) {
    // It's better to have a const than two Object.entries()
    const l = Object.entries(t._$$members$$_), o = e.prototype;
    l.map((([e, [t]]) => {
      (31 /* MEMBER_FLAGS.Prop */ & t || 2 /* PROXY_FLAGS.proxyState */ & n && 32 /* MEMBER_FLAGS.State */ & t) && 
      // proxyComponent - prop
      Object.defineProperty(o, e, {
        get() {
          // proxyComponent, get value
          return ((e, t) => getHostRef(e)._$$instanceValues$$_.get(t))(this, e);
        },
        set(t) {
          // proxyComponent, set value
          setValue(this, e, t);
        },
        configurable: !0,
        enumerable: !0
      });
    }));
  }
  return e;
}, connectedCallback = e => {
  if (0 == (1 /* PLATFORM_FLAGS.isTmpDisconnected */ & a._$$flags$$_)) {
    const t = getHostRef(e), n = t._$$cmpMeta$$_, l = (n._$$tagName$$_, () => {});
    if (1 /* HOST_FLAGS.hasConnected */ & t._$$flags$$_) 
    // not the first time this has connected
    // reattach any event listeners to the host
    // since they would have been removed when disconnected
    addHostEventListeners(e, t, n._$$listeners$$_); else {
      // first time this component has connected
      t._$$flags$$_ |= 1 /* HOST_FLAGS.hasConnected */;
      {
        // find the first ancestor component (if there is one) and register
        // this component as one of the actively loading child components for its ancestor
        let n = e;
        for (;n = n.parentNode || n.host; ) 
        // climb up the ancestors looking for the first
        // component that hasn't finished its lifecycle update yet
        if (n["s-p"]) {
          // we found this components first ancestor component
          // keep a reference to this component's ancestor component
          attachToAncestor(t, t._$$ancestorComponent$$_ = n);
          break;
        }
      }
      // Lazy properties
      // https://developers.google.com/web/fundamentals/web-components/best-practices#lazy-properties
            n._$$members$$_ && Object.entries(n._$$members$$_).map((([t, [n]]) => {
        if (31 /* MEMBER_FLAGS.Prop */ & n && e.hasOwnProperty(t)) {
          const n = e[t];
          delete e[t], e[t] = n;
        }
      })), (async (e, t, n, l, o) => {
        // initializeComponent
        if (0 == (32 /* HOST_FLAGS.hasInitializedComponent */ & t._$$flags$$_)) {
          if (
          // we haven't initialized this element yet
          t._$$flags$$_ |= 32 /* HOST_FLAGS.hasInitializedComponent */ , (
          // lazy loaded components
          // request the component's implementation to be
          // wired up with the host element
          o = loadModule(n)).then) {
            // Await creates a micro-task avoid if possible
            const endLoad = () => {};
            o = await o, endLoad();
          }
          if (!o) throw Error(`Constructor for "${n._$$tagName$$_}#${t._$$modeName$$_}" was not found`);
          o.isProxied || (proxyComponent(o, n, 2 /* PROXY_FLAGS.proxyState */), o.isProxied = !0);
          const e = (n._$$tagName$$_, () => {});
          // ok, time to construct the instance
          // but let's keep track of when we start and stop
          // so that the getters/setters don't incorrectly step on data
                    t._$$flags$$_ |= 8 /* HOST_FLAGS.isConstructingInstance */;
          // construct the lazy-loaded component implementation
          // passing the hostRef is very important during
          // construction in order to directly wire together the
          // host element and the lazy-loaded instance
          try {
            new o(t);
          } catch (e) {
            consoleError(e);
          }
          t._$$flags$$_ &= -9 /* HOST_FLAGS.isConstructingInstance */ , e();
        }
        // we've successfully created a lazy instance
                const s = t._$$ancestorComponent$$_, schedule = () => scheduleUpdate(t, !0);
        s && s["s-rc"] ? 
        // this is the initial load and this component it has an ancestor component
        // but the ancestor component has NOT fired its will update lifecycle yet
        // so let's just cool our jets and wait for the ancestor to continue first
        // this will get fired off when the ancestor component
        // finally gets around to rendering its lazy self
        // fire off the initial update
        s["s-rc"].push(schedule) : schedule();
      })(0, t, n);
    }
    l();
  }
}, bootstrapLazy = (e, t = {}) => {
  const endBootstrap = () => {}, n = [], l = t.exclude || [], o = i.customElements, s = u.head, r =  s.querySelector("meta[charset]"), c =  u.createElement("style"), f = [];
  let d, m = !0;
  Object.assign(a, t), a._$$resourcesUrl$$_ = new URL(t.resourcesUrl || "./", u.baseURI).href, 
  e.map((e => {
    e[1].map((t => {
      const s = {
        _$$flags$$_: t[0],
        _$$tagName$$_: t[1],
        _$$members$$_: t[2],
        _$$listeners$$_: t[3]
      };
      s._$$members$$_ = t[2], s._$$listeners$$_ = t[3];
      const r = s._$$tagName$$_, c = class extends HTMLElement {
        // StencilLazyHost
        constructor(e) {
          // @ts-ignore
          super(e), registerHost(e = this, s);
        }
        connectedCallback() {
          d && (clearTimeout(d), d = null), m ? 
          // connectedCallback will be processed once all components have been registered
          f.push(this) : a.jmp((() => connectedCallback(this)));
        }
        disconnectedCallback() {
          a.jmp((() => (e => {
            if (0 == (1 /* PLATFORM_FLAGS.isTmpDisconnected */ & a._$$flags$$_)) {
              const t = getHostRef(e);
              t._$$rmListeners$$_ && (t._$$rmListeners$$_.map((e => e())), t._$$rmListeners$$_ = void 0);
            }
          })(this)));
        }
        componentOnReady() {
          return getHostRef(this)._$$onReadyPromise$$_;
        }
      };
      s._$$lazyBundleId$$_ = e[0], l.includes(r) || o.get(r) || (n.push(r), o.define(r, proxyComponent(c, s, 1 /* PROXY_FLAGS.isElementConstructor */)));
    }));
  })), c.innerHTML = n + "{visibility:hidden}.hydrated{visibility:inherit}", c.setAttribute("data-styles", ""), 
  s.insertBefore(c, r ? r.nextSibling : s.firstChild), 
  // Process deferred connectedCallbacks now all components have been registered
  m = !1, f.length ? f.map((e => e.connectedCallback())) : a.jmp((() => d = setTimeout(appDidLoad, 30))), 
  // Fallback appLoad event
  endBootstrap();
}, addHostEventListeners = (e, t, n, l) => {
  n && n.map((([n, l, o]) => {
    const s = getHostListenerTarget(e, n), r = hostListenerProxy(t, o), c = hostListenerOpts(n);
    a.ael(s, l, r, c), (t._$$rmListeners$$_ = t._$$rmListeners$$_ || []).push((() => a.rel(s, l, r, c)));
  }));
}, hostListenerProxy = (e, t) => n => {
  try {
    256 /* HOST_FLAGS.isListenReady */ & e._$$flags$$_ ? 
    // instance is ready, let's call it's member method for this event
    e._$$lazyInstance$$_[t](n) : (e._$$queuedListeners$$_ = e._$$queuedListeners$$_ || []).push([ t, n ]);
  } catch (e) {
    consoleError(e);
  }
}, getHostListenerTarget = (e, t) => 16 /* LISTENER_FLAGS.TargetBody */ & t ? u.body : e, hostListenerOpts = e => 0 != (2 /* LISTENER_FLAGS.Capture */ & e), r =  new WeakMap, getHostRef = e => r.get(e), registerInstance = (e, t) => r.set(t._$$lazyInstance$$_ = e, t), registerHost = (e, t) => {
  const n = {
    _$$flags$$_: 0,
    _$$hostElement$$_: e,
    _$$cmpMeta$$_: t,
    _$$instanceValues$$_: new Map
  };
  return n._$$onReadyPromise$$_ = new Promise((e => n._$$onReadyResolve$$_ = e)), 
  e["s-p"] = [], e["s-rc"] = [], addHostEventListeners(e, n, t._$$listeners$$_), r.set(e, n);
}, isMemberInElement = (e, t) => t in e, consoleError = (e, t) => (0, console.error)(e, t), c =  new Map, loadModule = (e, t, n) => {
  // loadModuleImport
  const l = e._$$tagName$$_.replace(/-/g, "_"), o = e._$$lazyBundleId$$_, s = c.get(o);
  return s ? s[l] : import(
  /* @vite-ignore */
  /* webpackInclude: /\.entry\.js$/ */
  /* webpackExclude: /\.system\.entry\.js$/ */
  /* webpackMode: "lazy" */
  `./${o}.entry.js`).then((e => (c.set(o, e), e[l])), consoleError)
  /*!__STENCIL_STATIC_IMPORT_SWITCH__*/;
}, i = "undefined" != typeof window ? window : {}, u = i.document || {
  head: {}
}, a = {
  _$$flags$$_: 0,
  _$$resourcesUrl$$_: "",
  jmp: e => e(),
  raf: e => requestAnimationFrame(e),
  ael: (e, t, n, l) => e.addEventListener(t, n, l),
  rel: (e, t, n, l) => e.removeEventListener(t, n, l),
  ce: (e, t) => new CustomEvent(e, t)
}, promiseResolve = e => Promise.resolve(e), f = [], d = [], queueTask = (e, n) => l => {
  e.push(l), t || (t = !0, n && 4 /* PLATFORM_FLAGS.queueSync */ & a._$$flags$$_ ? nextTick(flush) : a.raf(flush));
}, consume = e => {
  for (let t = 0; t < e.length; t++) try {
    e[t](performance.now());
  } catch (e) {
    consoleError(e);
  }
  e.length = 0;
}, flush = () => {
  // always force a bunch of medium callbacks to run, but still have
  // a throttle on how many can run in a certain time
  // DOM READS!!!
  consume(f), consume(d), (t = f.length > 0) && 
  // still more to do yet, but we've run out of time
  // let's let this thing cool off and try again in the next tick
  a.raf(flush);
}, nextTick =  e => promiseResolve().then(e), m =  queueTask(d, !0);

export { bootstrapLazy as b, createEvent as c, h, promiseResolve as p, registerInstance as r }