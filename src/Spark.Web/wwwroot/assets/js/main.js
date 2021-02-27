!function(e) {
    var t = {};

    function n(i) {
        if (t[i]) return t[i].exports;
        const r = t[i] = { i: i, l: !1, exports: {} };
        return e[i].call(r.exports, r, r.exports, n), r.l = !0, r.exports;
    }

    n.m = e, n.c = t, n.d =
        function(e, t, i) { n.o(e, t) || Object.defineProperty(e, t, { enumerable: !0, get: i }) }, n.r = function(e) {
        "undefined" != typeof Symbol &&
            Symbol.toStringTag &&
            Object.defineProperty(e, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(e,
            "__esModule",
            { value: !0 });
    }, n.t = function(e, t) {
        if (1 & t && (e = n(e)), 8 & t) return e;
        if (4 & t && "object" == typeof e && e && e.__esModule) return e;
        const i = Object.create(null);
        if (n.r(i), Object.defineProperty(i, "default", { enumerable: !0, value: e }), 2 & t && "string" != typeof e)
            for (let r in e) n.d(i, r, function(t) { return e[t] }.bind(null, r));
        return i;
    }, n.n = function(e) {
        const t = e && e.__esModule ? function() { return e.default } : function() { return e };
        return n.d(t, "a", t), t;
    }, n.o = function(e, t) { return Object.prototype.hasOwnProperty.call(e, t) }, n.p = "", n(n.s = 0);
}([
    function(e, t, n) {
        "use strict";
        n.r(t);
        n(1);
    }, function(e, t, n) {
/*!
  * Bootstrap v4.3.1 (https://getbootstrap.com/)
  * Copyright 2011-2019 The Bootstrap Authors (https://github.com/twbs/bootstrap/graphs/contributors)
  * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
  */
        !function(e, t, n) {
            "use strict";

            function i(e, t) {
                for (let n = 0; n < t.length; n++) {
                    var i = t[n];
                    i.enumerable = i.enumerable || !1, i.configurable =
                        !0, "value" in i && (i.writable = !0), Object.defineProperty(e, i.key, i);
                }
            }

            function r(e, t, n) { return t && i(e.prototype, t), n && i(e, n), e }

            function o(e, t, n) {
                return t in e
                    ? Object.defineProperty(e, t, { value: n, enumerable: !0, configurable: !0, writable: !0 })
                    : e[t] = n, e;
            }

            function s(e) {
                for (let t = 1; t < arguments.length; t++) {
                    var n = null != arguments[t] ? arguments[t] : {};
                    let i = Object.keys(n);
                    "function" == typeof Object.getOwnPropertySymbols &&
                    (i = i.concat(Object.getOwnPropertySymbols(n).filter((function(e) {
                        return Object.getOwnPropertyDescriptor(n, e).enumerable;
                    })))), i.forEach((function(t) { o(e, t, n[t]) }));
                }
                return e;
            }

            t = t && t.hasOwnProperty("default") ? t.default : t, n = n && n.hasOwnProperty("default") ? n.default : n;
            var a = "transitionend";

            function l(e) {
                var n = this, i = !1;
                return t(this).one(u.TRANSITION_END, (function() { i = !0 })), setTimeout((function() {
                        i || u.triggerTransitionEnd(n);
                    }),
                    e), this;
            }

            var u = {
                TRANSITION_END: "bsTransitionEnd",
                getUID: function(e) {
                    do {
                        e += ~~(1e6 * Math.random());
                    } while (document.getElementById(e));
                    return e;
                },
                getSelectorFromElement: function(e) {
                    var t = e.getAttribute("data-target");
                    if (!t || "#" === t) {
                        const n = e.getAttribute("href");
                        t = n && "#" !== n ? n.trim() : "";
                    }
                    try {
                        return document.querySelector(t) ? t : null;
                    } catch (e) {
                        return null;
                    }
                },
                getTransitionDurationFromElement: function(e) {
                    if (!e) return 0;
                    var n = t(e).css("transition-duration"), i = t(e).css("transition-delay");
                    const r = parseFloat(n);
                    const o = parseFloat(i);
                    return r || o
                        ? (n = n.split(",")[0], i = i.split(",")[0], 1e3 * (parseFloat(n) + parseFloat(i)))
                        : 0;
                },
                reflow: function(e) { return e.offsetHeight },
                triggerTransitionEnd: function(e) { t(e).trigger(a) },
                supportsTransitionEnd: function() { return Boolean(a) },
                isElement: function(e) { return(e[0] || e).nodeType },
                typeCheckConfig: function(e, t, n) {
                    for (let i in n)
                        if (Object.prototype.hasOwnProperty.call(n, i)) {
                            const r = n[i];
                            const o = t[i];
                            const s = o && u.isElement(o)
                                ? "element"
                                : (a = o, {}.toString.call(a).match(/\s([a-z]+)/i)[1].toLowerCase());
                            if (!new RegExp(r).test(s))
                                throw new Error(e.toUpperCase() +
                                    ': Option "' +
                                    i +
                                    '" provided type "' +
                                    s +
                                    '" but expected type "' +
                                    r +
                                    '".');
                        }
                    var a;
                },
                findShadowRoot: function(e) {
                    if (!document.documentElement.attachShadow) return null;
                    if ("function" == typeof e.getRootNode) {
                        const t = e.getRootNode();
                        return t instanceof ShadowRoot ? t : null;
                    }
                    return e instanceof ShadowRoot ? e : e.parentNode ? u.findShadowRoot(e.parentNode) : null;
                }
            };
            t.fn.emulateTransitionEnd = l, t.event.special[u.TRANSITION_END] = {
                bindType: a,
                delegateType: a,
                handle: function(e) { if (t(e.target).is(this)) return e.handleObj.handler.apply(this, arguments) }
            };
            var c = t.fn.alert,
                f = { CLOSE: "close.bs.alert", CLOSED: "closed.bs.alert", CLICK_DATA_API: "click.bs.alert.data-api" },
                d = "alert",
                h = "fade",
                p = "show",
                g = function() {
                    function e(e) { this._element = e }

                    const n = e.prototype;
                    return n.close = function(e) {
                        var t = this._element;
                        e && (t = this._getRootElement(e)), this._triggerCloseEvent(t).isDefaultPrevented() ||
                            this._removeElement(t);
                    }, n.dispose =
                        function() {
                            t.removeData(this._element, "bs.alert"), this._element = null;
                        }, n._getRootElement =
                        function(e) {
                            const n = u.getSelectorFromElement(e);
                            var i = !1;
                            return n && (i = document.querySelector(n)), i || (i = t(e).closest(`.${d}`)[0]), i;
                        }, n._triggerCloseEvent = function(e) {
                        const n = t.Event(f.CLOSE);
                        return t(e).trigger(n), n;
                    }, n._removeElement = function(e) {
                        var n = this;
                        if (t(e).removeClass(p), t(e).hasClass(h)) {
                            const i = u.getTransitionDurationFromElement(e);
                            t(e).one(u.TRANSITION_END, (function(t) { return n._destroyElement(e, t) }))
                                .emulateTransitionEnd(i);
                        } else this._destroyElement(e);
                    }, n._destroyElement =
                        function(e) { t(e).detach().trigger(f.CLOSED).remove() }, e._jQueryInterface =
                        function(n) {
                            return this.each((function() {
                                const i = t(this);
                                var r = i.data("bs.alert");
                                r || (r = new e(this), i.data("bs.alert", r)), "close" === n && r[n](this);
                            }));
                        }, e._handleDismiss =
                        function(e) { return function(t) { t && t.preventDefault(), e.close(this) } }, r(e,
                        null,
                        [{ key: "VERSION", get: function() { return"4.3.1" } }]), e;
                }();
            t(document).on(f.CLICK_DATA_API, '[data-dismiss="alert"]', g._handleDismiss(new g)), t.fn.alert =
                g._jQueryInterface, t.fn.alert.Constructor = g, t.fn.alert.noConflict = function() {
                return t.fn.alert = c, g._jQueryInterface;
            };
            var m = t.fn.button,
                v = "active",
                y = "btn",
                b = "focus",
                _ = '[data-toggle^="button"]',
                E = '[data-toggle="buttons"]',
                w = 'input:not([type="hidden"])',
                T = ".active",
                C = ".btn",
                x = {
                    CLICK_DATA_API: "click.bs.button.data-api",
                    FOCUS_BLUR_DATA_API: "focus.bs.button.data-api blur.bs.button.data-api"
                },
                S = function() {
                    function e(e) { this._element = e }

                    const n = e.prototype;
                    return n.toggle = function() {
                        var e = !0, n = !0;
                        const i = t(this._element).closest(E)[0];
                        if (i) {
                            const r = this._element.querySelector(w);
                            if (r) {
                                if ("radio" === r.type)
                                    if (r.checked && this._element.classList.contains(v)) e = !1;
                                    else {
                                        const o = i.querySelector(T);
                                        o && t(o).removeClass(v);
                                    }
                                if (e) {
                                    if (r.hasAttribute("disabled") ||
                                        i.hasAttribute("disabled") ||
                                        r.classList.contains("disabled") ||
                                        i.classList.contains("disabled")) return;
                                    r.checked = !this._element.classList.contains(v), t(r).trigger("change");
                                }
                                r.focus(), n = !1;
                            }
                        }
                        n && this._element.setAttribute("aria-pressed", !this._element.classList.contains(v)), e &&
                            t(this._element).toggleClass(v);
                    }, n.dispose =
                        function() { t.removeData(this._element, "bs.button"), this._element = null }, e
                        ._jQueryInterface =
                        function(n) {
                            return this.each((function() {
                                var i = t(this).data("bs.button");
                                i || (i = new e(this), t(this).data("bs.button", i)), "toggle" === n && i[n]();
                            }));
                        }, r(e, null, [{ key: "VERSION", get: function() { return"4.3.1" } }]), e;
                }();
            t(document).on(x.CLICK_DATA_API,
                _,
                (function(e) {
                    e.preventDefault();
                    var n = e.target;
                    t(n).hasClass(y) || (n = t(n).closest(C)), S._jQueryInterface.call(t(n), "toggle");
                })).on(x.FOCUS_BLUR_DATA_API,
                _,
                (function(e) {
                    const n = t(e.target).closest(C)[0];
                    t(n).toggleClass(b, /^focus(in)?$/.test(e.type));
                })), t.fn.button = S._jQueryInterface, t.fn.button.Constructor = S, t.fn.button.noConflict =
                function() { return t.fn.button = m, S._jQueryInterface };
            var D = "carousel",
                A = ".bs.carousel",
                N = t.fn[D],
                I = { interval: 5e3, keyboard: !0, slide: !1, pause: "hover", wrap: !0, touch: !0 },
                O =
                {
                    interval: "(number|boolean)",
                    keyboard: "boolean",
                    slide: "(boolean|string)",
                    pause: "(string|boolean)",
                    wrap: "boolean",
                    touch: "boolean"
                },
                k = "next",
                L = "prev",
                j = "left",
                P = "right",
                H =
                {
                    SLIDE: "slide.bs.carousel",
                    SLID: "slid.bs.carousel",
                    KEYDOWN: "keydown.bs.carousel",
                    MOUSEENTER: "mouseenter.bs.carousel",
                    MOUSELEAVE: "mouseleave.bs.carousel",
                    TOUCHSTART: "touchstart.bs.carousel",
                    TOUCHMOVE: "touchmove.bs.carousel",
                    TOUCHEND: "touchend.bs.carousel",
                    POINTERDOWN: "pointerdown.bs.carousel",
                    POINTERUP: "pointerup.bs.carousel",
                    DRAG_START: "dragstart.bs.carousel",
                    LOAD_DATA_API: "load.bs.carousel.data-api",
                    CLICK_DATA_API: "click.bs.carousel.data-api"
                },
                R = "carousel",
                M = "active",
                q = "slide",
                F = "carousel-item-right",
                W = "carousel-item-left",
                B = "carousel-item-next",
                U = "carousel-item-prev",
                V = "pointer-event",
                K =
                {
                    ACTIVE: ".active",
                    ACTIVE_ITEM: ".active.carousel-item",
                    ITEM: ".carousel-item",
                    ITEM_IMG: ".carousel-item img",
                    NEXT_PREV: ".carousel-item-next, .carousel-item-prev",
                    INDICATORS: ".carousel-indicators",
                    DATA_SLIDE: "[data-slide], [data-slide-to]",
                    DATA_RIDE: '[data-ride="carousel"]'
                },
                $ = { TOUCH: "touch", PEN: "pen" },
                Q = function() {
                    function e(e, t) {
                        this._items = null, this._interval = null, this._activeElement = null, this._isPaused =
                            !1, this._isSliding = !1, this.touchTimeout = null, this.touchStartX = 0, this.touchDeltaX =
                            0, this._config = this._getConfig(t), this._element = e, this._indicatorsElement =
                            this._element.querySelector(K.INDICATORS), this._touchSupported =
                            "ontouchstart" in document.documentElement || navigator.maxTouchPoints > 0, this
                            ._pointerEvent =
                            Boolean(window.PointerEvent || window.MSPointerEvent), this._addEventListeners();
                    }

                    const n = e.prototype;
                    return n.next = function() { this._isSliding || this._slide(k) }, n.nextWhenVisible =
                        function() {
                            !document.hidden &&
                                t(this._element).is(":visible") &&
                                "hidden" !== t(this._element).css("visibility") &&
                                this.next();
                        }, n.prev = function() { this._isSliding || this._slide(L) }, n.pause =
                        function(e) {
                            e || (this._isPaused = !0), this._element.querySelector(K.NEXT_PREV) &&
                                    (u.triggerTransitionEnd(this._element), this.cycle(!0)), clearInterval(
                                    this._interval),
                                this
                                    ._interval = null;
                        }, n.cycle = function(e) {
                        e || (this._isPaused = !1),
                            this._interval && (clearInterval(this._interval), this._interval = null),
                            this._config.interval &&
                                !this._isPaused &&
                                (this._interval =
                                    setInterval(
                                        (document.visibilityState ? this.nextWhenVisible : this.next).bind(this),
                                        this._config.interval));
                    }, n.to = function(e) {
                        var n = this;
                        this._activeElement = this._element.querySelector(K.ACTIVE_ITEM);
                        const i = this._getItemIndex(this._activeElement);
                        if (!(e > this._items.length - 1 || e < 0))
                            if (this._isSliding) t(this._element).one(H.SLID, (function() { return n.to(e) }));
                            else {
                                if (i === e) return this.pause(), void this.cycle();
                                const r = e > i ? k : L;
                                this._slide(r, this._items[e]);
                            }
                    }, n.dispose = function() {
                        t(this._element).off(A), t.removeData(this._element, "bs.carousel"), this._items =
                            null, this._config = null, this._element = null, this._interval = null, this._isPaused =
                            null, this._isSliding = null, this._activeElement = null, this._indicatorsElement = null;
                    }, n._getConfig =
                        function(e) { return e = s({}, I, e), u.typeCheckConfig(D, e, O), e }, n._handleSwipe =
                        function() {
                            const e = Math.abs(this.touchDeltaX);
                            if (!(e <= 40)) {
                                const t = e / this.touchDeltaX;
                                t > 0 && this.prev(), t < 0 && this.next();
                            }
                        }, n._addEventListeners = function() {
                        var e = this;
                        this._config.keyboard && t(this._element).on(H.KEYDOWN, (function(t) { return e._keydown(t) })),
                            "hover" === this._config.pause &&
                                t(this._element).on(H.MOUSEENTER, (function(t) { return e.pause(t) }))
                                .on(H.MOUSELEAVE, (function(t) { return e.cycle(t) })), this._config.touch &&
                                this._addTouchEventListeners();
                    }, n._addTouchEventListeners = function() {
                        var e = this;
                        if (this._touchSupported) {
                            var n = function(t) {
                                    e._pointerEvent && $[t.originalEvent.pointerType.toUpperCase()]
                                        ? e.touchStartX = t.originalEvent.clientX
                                        : e._pointerEvent || (e.touchStartX = t.originalEvent.touches[0].clientX);
                                },
                                i = function(t) {
                                    e._pointerEvent &&
                                            $[t.originalEvent.pointerType.toUpperCase()] &&
                                            (e.touchDeltaX = t.originalEvent.clientX - e.touchStartX), e._handleSwipe(),
                                        "hover" === e._config.pause &&
                                        (e.pause(), e.touchTimeout && clearTimeout(e.touchTimeout), e.touchTimeout =
                                            setTimeout((function(t) { return e.cycle(t) }), 500 + e._config.interval));
                                };
                            t(this._element.querySelectorAll(K.ITEM_IMG))
                                .on(H.DRAG_START, (function(e) { return e.preventDefault() })), this._pointerEvent
                                ? (t(this._element).on(H.POINTERDOWN, (function(e) { return n(e) })),
                                    t(this._element).on(H.POINTERUP, (function(e) { return i(e) })),
                                    this._element.classList.add(V))
                                : (t(this._element).on(H.TOUCHSTART, (function(e) { return n(e) })), t(this._element)
                                    .on(H.TOUCHMOVE,
                                        (function(t) {
                                            return function(t) {
                                                t.originalEvent.touches && t.originalEvent.touches.length > 1
                                                    ? e.touchDeltaX = 0
                                                    : e.touchDeltaX = t.originalEvent.touches[0].clientX -
                                                    e.touchStartX;
                                            }(t);
                                        })), t(this._element).on(H.TOUCHEND, (function(e) { return i(e) })));
                        }
                    }, n._keydown = function(e) {
                        if (!/input|textarea/i.test(e.target.tagName))
                            switch (e.which) {
                            case 37:
                                e.preventDefault(), this.prev();
                                break;
                            case 39:
                                e.preventDefault(), this.next();
                            }
                    }, n._getItemIndex = function(e) {
                        return this._items =
                            e && e.parentNode ? [].slice.call(e.parentNode.querySelectorAll(K.ITEM)) : [], this._items
                            .indexOf(e);
                    }, n._getItemByDirection = function(e, t) {
                        const n = e === k;
                        const i = e === L;
                        const r = this._getItemIndex(t);
                        const o = this._items.length - 1;
                        if ((i && 0 === r || n && r === o) && !this._config.wrap) return t;
                        const s = (r + (e === L ? -1 : 1)) % this._items.length;
                        return-1 === s ? this._items[this._items.length - 1] : this._items[s];
                    }, n._triggerSlideEvent = function(e, n) {
                        const i = this._getItemIndex(e);
                        const r = this._getItemIndex(this._element.querySelector(K.ACTIVE_ITEM));
                        const o = t.Event(H.SLIDE, { relatedTarget: e, direction: n, from: r, to: i });
                        return t(this._element).trigger(o), o;
                    }, n._setActiveIndicatorElement = function(e) {
                        if (this._indicatorsElement) {
                            const n = [].slice.call(this._indicatorsElement.querySelectorAll(K.ACTIVE));
                            t(n).removeClass(M);
                            const i = this._indicatorsElement.children[this._getItemIndex(e)];
                            i && t(i).addClass(M);
                        }
                    }, n._slide = function(e, n) {
                        var i, r, o, s = this, a = this._element.querySelector(K.ACTIVE_ITEM);
                        const l = this._getItemIndex(a);
                        var c = n || a && this._getItemByDirection(e, a);
                        const f = this._getItemIndex(c);
                        const d = Boolean(this._interval);
                        if (e === k ? (i = W, r = B, o = j) : (i = F, r = U, o = P), c && t(c).hasClass(M))
                            this._isSliding = !1;
                        else if (!this._triggerSlideEvent(c, o).isDefaultPrevented() && a && c) {
                            this._isSliding = !0, d && this.pause(), this._setActiveIndicatorElement(c);
                            var h = t.Event(H.SLID, { relatedTarget: c, direction: o, from: l, to: f });
                            if (t(this._element).hasClass(q)) {
                                t(c).addClass(r), u.reflow(c), t(a).addClass(i), t(c).addClass(i);
                                const p = parseInt(c.getAttribute("data-interval"), 10);
                                p
                                    ? (this._config.defaultInterval =
                                        this._config.defaultInterval || this._config.interval, this._config.interval =
                                        p)
                                    : this._config.interval = this._config.defaultInterval || this._config.interval;
                                const g = u.getTransitionDurationFromElement(a);
                                t(a).one(u.TRANSITION_END,
                                    (function() {
                                        t(c).removeClass(i + " " + r).addClass(M), t(a)
                                                .removeClass(M + " " + r + " " + i),
                                            s._isSliding =
                                                !1, setTimeout((function() { return t(s._element).trigger(h) }), 0);
                                    })).emulateTransitionEnd(g);
                            } else
                                t(a).removeClass(M), t(c).addClass(M), this._isSliding =
                                    !1, t(this._element).trigger(h);
                            d && this.cycle();
                        }
                    }, e._jQueryInterface = function(n) {
                        return this.each((function() {
                            var i = t(this).data("bs.carousel"), r = s({}, I, t(this).data());
                            "object" == typeof n && (r = s({}, r, n));
                            const o = "string" == typeof n ? n : r.slide;
                            if (i || (i = new e(this, r), t(this).data("bs.carousel", i)), "number" == typeof n)
                                i.to(n);
                            else if ("string" == typeof o) {
                                if (void 0 === i[o]) throw new TypeError(`No method named "${o}"`);
                                i[o]();
                            } else r.interval && r.ride && (i.pause(), i.cycle());
                        }));
                    }, e._dataApiClickHandler = function(n) {
                        const i = u.getSelectorFromElement(this);
                        if (i) {
                            const r = t(i)[0];
                            if (r && t(r).hasClass(R)) {
                                const o = s({}, t(r).data(), t(this).data());
                                const a = this.getAttribute("data-slide-to");
                                a && (o.interval = !1), e._jQueryInterface.call(t(r), o), a &&
                                    t(r).data("bs.carousel").to(a), n.preventDefault();
                            }
                        }
                    }, r(e,
                        null,
                        [
                            { key: "VERSION", get: function() { return"4.3.1" } },
                            { key: "Default", get: function() { return I } }
                        ]), e;
                }();
            t(document).on(H.CLICK_DATA_API, K.DATA_SLIDE, Q._dataApiClickHandler), t(window).on(H.LOAD_DATA_API,
                (function() {
                    for (var e = [].slice.call(document.querySelectorAll(K.DATA_RIDE)), n = 0, i = e.length;
                        n < i;
                        n++) {
                        const r = t(e[n]);
                        Q._jQueryInterface.call(r, r.data());
                    }
                })), t.fn[D] = Q._jQueryInterface, t.fn[D].Constructor = Q, t.fn[D].noConflict = function() {
                return t.fn[D] = N, Q._jQueryInterface;
            };
            var Y = "collapse",
                z = t.fn[Y],
                X = { toggle: !0, parent: "" },
                G = { toggle: "boolean", parent: "(string|element)" },
                J = {
                    SHOW: "show.bs.collapse",
                    SHOWN: "shown.bs.collapse",
                    HIDE: "hide.bs.collapse",
                    HIDDEN: "hidden.bs.collapse",
                    CLICK_DATA_API: "click.bs.collapse.data-api"
                },
                Z = "show",
                ee = "collapse",
                te = "collapsing",
                ne = "collapsed",
                ie = "width",
                re = "height",
                oe = { ACTIVES: ".show, .collapsing", DATA_TOGGLE: '[data-toggle="collapse"]' },
                se = function() {
                    function e(e, t) {
                        this._isTransitioning = !1, this._element = e, this._config =
                            this._getConfig(t), this._triggerArray =
                            [].slice.call(document.querySelectorAll(
                                `[data-toggle="collapse"][href="#${e.id}"],[data-toggle="collapse"][data-target="#${e.id
                                }"]`));
                        for (var n = [].slice.call(document.querySelectorAll(oe.DATA_TOGGLE)), i = 0, r = n.length;
                            i < r;
                            i++) {
                            const o = n[i];
                            const s = u.getSelectorFromElement(o);
                            const a = [].slice.call(document.querySelectorAll(s))
                                .filter((function(t) { return t === e }));
                            null !== s && a.length > 0 && (this._selector = s, this._triggerArray.push(o));
                        }
                        this._parent =
                            this._config.parent ? this._getParent() : null, this._config.parent ||
                            this._addAriaAndCollapsedClass(this._element, this._triggerArray), this._config.toggle &&
                            this.toggle();
                    }

                    const n = e.prototype;
                    return n.toggle = function() { t(this._element).hasClass(Z) ? this.hide() : this.show() }, n.show =
                            function() {
                                var n, i, r = this;
                                if (!(this._isTransitioning ||
                                    t(this._element).hasClass(Z) ||
                                    (this._parent &&
                                        0 ===
                                        (n = [].slice.call(this._parent.querySelectorAll(oe.ACTIVES)).filter(
                                            (function(e) {
                                                return"string" == typeof r._config.parent
                                                    ? e.getAttribute("data-parent") === r._config.parent
                                                    : e.classList.contains(ee);
                                            }))).length &&
                                        (n = null), n &&
                                        (i = t(n).not(this._selector).data("bs.collapse")) &&
                                        i._isTransitioning))) {
                                    const o = t.Event(J.SHOW);
                                    if (t(this._element).trigger(o), !o.isDefaultPrevented()) {
                                        n &&
                                        (e._jQueryInterface.call(t(n).not(this._selector), "hide"), i ||
                                            t(n).data("bs.collapse", null));
                                        var s = this._getDimension();
                                        t(this._element).removeClass(ee).addClass(te), this._element.style[s] =
                                            0, this._triggerArray.length &&
                                            t(this._triggerArray).removeClass(ne).attr("aria-expanded", !0), this
                                            .setTransitioning(!0);
                                        const a = `scroll${s[0].toUpperCase() + s.slice(1)}`;
                                        const l = u.getTransitionDurationFromElement(this._element);
                                        t(this._element).one(u.TRANSITION_END,
                                            (function() {
                                                t(r._element).removeClass(te).addClass(ee).addClass(Z), r._element.style
                                                    [s] =
                                                    "", r.setTransitioning(!1), t(r._element).trigger(J.SHOWN);
                                            })).emulateTransitionEnd(l), this._element.style[s] =
                                            this._element[a] + "px";
                                    }
                                }
                            }, n.hide = function() {
                            var e = this;
                            if (!this._isTransitioning && t(this._element).hasClass(Z)) {
                                const n = t.Event(J.HIDE);
                                if (t(this._element).trigger(n), !n.isDefaultPrevented()) {
                                    const i = this._getDimension();
                                    this._element.style[i] =
                                            this._element.getBoundingClientRect()[i] + "px", u.reflow(this._element),
                                        t(this._element).addClass(te).removeClass(ee).removeClass(Z);
                                    const r = this._triggerArray.length;
                                    if (r > 0)
                                        for (let o = 0; o < r; o++) {
                                            const s = this._triggerArray[o];
                                            const a = u.getSelectorFromElement(s);
                                            null !== a &&
                                            (t([].slice.call(document.querySelectorAll(a))).hasClass(Z) ||
                                                t(s).addClass(ne).attr("aria-expanded", !1));
                                        }
                                    this.setTransitioning(!0), this._element.style[i] = "";
                                    const l = u.getTransitionDurationFromElement(this._element);
                                    t(this._element)
                                        .one(u.TRANSITION_END,
                                            (function() {
                                                e.setTransitioning(!1), t(e._element).removeClass(te).addClass(ee)
                                                    .trigger(J.HIDDEN);
                                            })).emulateTransitionEnd(l);
                                }
                            }
                        }, n.setTransitioning = function(e) { this._isTransitioning = e }, n.dispose =
                            function() {
                                t.removeData(this._element, "bs.collapse"), this._config = null, this._parent =
                                    null, this._element = null, this._triggerArray = null, this._isTransitioning = null;
                            }, n._getConfig =
                            function(e) {
                                return(e = s({}, X, e)).toggle = Boolean(e.toggle), u.typeCheckConfig(Y, e, G), e;
                            },
                        n._getDimension = function() { return t(this._element).hasClass(ie) ? ie : re }, n._getParent =
                            function() {
                                var n, i = this;
                                u.isElement(this._config.parent)
                                    ? (n = this._config.parent, void 0 !== this._config.parent.jquery &&
                                        (n = this._config.parent[0]))
                                    : n = document.querySelector(this._config.parent);
                                const r = `[data-toggle="collapse"][data-parent="${this._config.parent}"]`;
                                const o = [].slice.call(n.querySelectorAll(r));
                                return t(o).each((function(t, n) {
                                    i._addAriaAndCollapsedClass(e._getTargetFromElement(n), [n]);
                                })), n;
                            }, n._addAriaAndCollapsedClass = function(e, n) {
                            const i = t(e).hasClass(Z);
                            n.length && t(n).toggleClass(ne, !i).attr("aria-expanded", i);
                        }, e._getTargetFromElement = function(e) {
                            const t = u.getSelectorFromElement(e);
                            return t ? document.querySelector(t) : null;
                        }, e._jQueryInterface = function(n) {
                            return this.each((function() {
                                const i = t(this);
                                var r = i.data("bs.collapse");
                                const o = s({}, X, i.data(), "object" == typeof n && n ? n : {});
                                if (!r && o.toggle && /show|hide/.test(n) && (o.toggle = !1), r ||
                                    (r = new e(this, o), i.data("bs.collapse", r)), "string" == typeof n) {
                                    if (void 0 === r[n]) throw new TypeError(`No method named "${n}"`);
                                    r[n]();
                                }
                            }));
                        }, r(e,
                            null,
                            [
                                { key: "VERSION", get: function() { return"4.3.1" } },
                                { key: "Default", get: function() { return X } }
                            ]), e;
                }();
            t(document).on(J.CLICK_DATA_API,
                oe.DATA_TOGGLE,
                (function(e) {
                    "A" === e.currentTarget.tagName && e.preventDefault();
                    var n = t(this);
                    const i = u.getSelectorFromElement(this);
                    const r = [].slice.call(document.querySelectorAll(i));
                    t(r).each((function() {
                        const e = t(this);
                        const i = e.data("bs.collapse") ? "toggle" : n.data();
                        se._jQueryInterface.call(e, i);
                    }));
                })), t.fn[Y] = se._jQueryInterface, t.fn[Y].Constructor = se, t.fn[Y].noConflict = function() {
                return t.fn[Y] = z, se._jQueryInterface;
            };
            var ae = "dropdown",
                le = t.fn[ae],
                ue = new RegExp("38|40|27"),
                ce =
                {
                    HIDE: "hide.bs.dropdown",
                    HIDDEN: "hidden.bs.dropdown",
                    SHOW: "show.bs.dropdown",
                    SHOWN: "shown.bs.dropdown",
                    CLICK: "click.bs.dropdown",
                    CLICK_DATA_API: "click.bs.dropdown.data-api",
                    KEYDOWN_DATA_API: "keydown.bs.dropdown.data-api",
                    KEYUP_DATA_API: "keyup.bs.dropdown.data-api"
                },
                fe = "disabled",
                de = "show",
                he = "dropup",
                pe = "dropright",
                ge = "dropleft",
                me = "dropdown-menu-right",
                ve = "position-static",
                ye = '[data-toggle="dropdown"]',
                be = ".dropdown form",
                _e = ".dropdown-menu",
                Ee = ".navbar-nav",
                we = ".dropdown-menu .dropdown-item:not(.disabled):not(:disabled)",
                Te = "top-start",
                Ce = "top-end",
                xe = "bottom-start",
                Se = "bottom-end",
                De = "right-start",
                Ae = "left-start",
                Ne = { offset: 0, flip: !0, boundary: "scrollParent", reference: "toggle", display: "dynamic" },
                Ie = {
                    offset: "(number|string|function)",
                    flip: "boolean",
                    boundary: "(string|element)",
                    reference: "(string|element)",
                    display: "string"
                },
                Oe = function() {
                    function e(e, t) {
                        this._element = e, this._popper = null, this._config = this._getConfig(t), this._menu =
                            this._getMenuElement(), this._inNavbar = this._detectNavbar(), this._addEventListeners();
                    }

                    const i = e.prototype;
                    return i.toggle = function() {
                        if (!this._element.disabled && !t(this._element).hasClass(fe)) {
                            const i = e._getParentFromElement(this._element);
                            const r = t(this._menu).hasClass(de);
                            if (e._clearMenus(), !r) {
                                const o = { relatedTarget: this._element };
                                const s = t.Event(ce.SHOW, o);
                                if (t(i).trigger(s), !s.isDefaultPrevented()) {
                                    if (!this._inNavbar) {
                                        if (void 0 === n)
                                            throw new TypeError(
                                                "Bootstrap's dropdowns require Popper.js (https://popper.js.org/)");
                                        let a = this._element;
                                        "parent" === this._config.reference
                                                ? a = i
                                                : u.isElement(this._config.reference) &&
                                                (a = this._config.reference, void 0 !== this._config.reference.jquery &&
                                                    (a = this._config.reference[0])),
                                            "scrollParent" !== this._config.boundary && t(i).addClass(ve),
                                            this._popper =
                                                new n(a, this._menu, this._getPopperConfig());
                                    }
                                    "ontouchstart" in document.documentElement &&
                                            0 === t(i).closest(Ee).length &&
                                            t(document.body).children().on("mouseover", null, t.noop), this._element
                                            .focus(),
                                        this._element.setAttribute("aria-expanded", !0), t(this._menu).toggleClass(de),
                                        t(i)
                                            .toggleClass(de).trigger(t.Event(ce.SHOWN, o));
                                }
                            }
                        }
                    }, i.show = function() {
                        if (!(this._element.disabled || t(this._element).hasClass(fe) || t(this._menu).hasClass(de))) {
                            const n = { relatedTarget: this._element };
                            const i = t.Event(ce.SHOW, n);
                            const r = e._getParentFromElement(this._element);
                            t(r).trigger(i), i.isDefaultPrevented() ||
                                (t(this._menu).toggleClass(de), t(r).toggleClass(de).trigger(t.Event(ce.SHOWN, n)));
                        }
                    }, i.hide = function() {
                        if (!this._element.disabled && !t(this._element).hasClass(fe) && t(this._menu).hasClass(de)) {
                            const n = { relatedTarget: this._element };
                            const i = t.Event(ce.HIDE, n);
                            const r = e._getParentFromElement(this._element);
                            t(r).trigger(i), i.isDefaultPrevented() ||
                                (t(this._menu).toggleClass(de), t(r).toggleClass(de).trigger(t.Event(ce.HIDDEN, n)));
                        }
                    }, i.dispose =
                        function() {
                            t.removeData(this._element, "bs.dropdown"), t(this._element).off(".bs.dropdown"),
                                this._element = null, this._menu =
                                    null, null !== this._popper && (this._popper.destroy(), this._popper = null);
                        }, i.update =
                        function() {
                            this._inNavbar =
                                this._detectNavbar(), null !== this._popper && this._popper.scheduleUpdate();
                        }, i._addEventListeners = function() {
                        var e = this;
                        t(this._element).on(ce.CLICK,
                            (function(t) { t.preventDefault(), t.stopPropagation(), e.toggle() }));
                    }, i._getConfig =
                        function(e) {
                            return e = s({}, this.constructor.Default, t(this._element).data(), e), u.typeCheckConfig(
                                ae,
                                e,
                                this.constructor.DefaultType), e;
                        }, i._getMenuElement = function() {
                        if (!this._menu) {
                            const t = e._getParentFromElement(this._element);
                            t && (this._menu = t.querySelector(_e));
                        }
                        return this._menu;
                    }, i._getPlacement = function() {
                        const e = t(this._element.parentNode);
                        var n = xe;
                        return e.hasClass(he)
                            ? (n = Te, t(this._menu).hasClass(me) && (n = Ce))
                            : e.hasClass(pe)
                            ? n = De
                            : e.hasClass(ge)
                            ? n = Ae
                            : t(this._menu).hasClass(me) && (n = Se), n;
                    }, i._detectNavbar =
                        function() { return t(this._element).closest(".navbar").length > 0 }, i._getOffset =
                        function() {
                            var e = this;
                            const t = {};
                            return"function" == typeof this._config.offset
                                ? t.fn = function(t) {
                                    return t.offsets =
                                        s({}, t.offsets, e._config.offset(t.offsets, e._element) || {}), t;
                                }
                                : t.offset = this._config.offset, t;
                        }, i._getPopperConfig = function() {
                        const e = {
                            placement: this._getPlacement(),
                            modifiers: {
                                offset: this._getOffset(),
                                flip: { enabled: this._config.flip },
                                preventOverflow: { boundariesElement: this._config.boundary }
                            }
                        };
                        return"static" === this._config.display && (e.modifiers.applyStyle = { enabled: !1 }), e;
                    }, e._jQueryInterface = function(n) {
                        return this.each((function() {
                            var i = t(this).data("bs.dropdown");
                            if (i || (i = new e(this, "object" == typeof n ? n : null), t(this).data("bs.dropdown", i)),
                                "string" == typeof n) {
                                if (void 0 === i[n]) throw new TypeError(`No method named "${n}"`);
                                i[n]();
                            }
                        }));
                    }, e._clearMenus = function(n) {
                        if (!n || 3 !== n.which && ("keyup" !== n.type || 9 === n.which))
                            for (var i = [].slice.call(document.querySelectorAll(ye)), r = 0, o = i.length;
                                r < o;
                                r++) {
                                const s = e._getParentFromElement(i[r]);
                                const a = t(i[r]).data("bs.dropdown");
                                const l = { relatedTarget: i[r] };
                                if (n && "click" === n.type && (l.clickEvent = n), a) {
                                    const u = a._menu;
                                    if (t(s).hasClass(de) &&
                                        !(n &&
                                            ("click" === n.type && /input|textarea/i.test(n.target.tagName) ||
                                                "keyup" === n.type && 9 === n.which) &&
                                            t.contains(s, n.target))) {
                                        const c = t.Event(ce.HIDE, l);
                                        t(s).trigger(c), c.isDefaultPrevented() ||
                                        ("ontouchstart" in document.documentElement &&
                                                t(document.body).children().off("mouseover", null, t.noop),
                                            i[r].setAttribute("aria-expanded", "false"), t(u).removeClass(de), t(s)
                                                .removeClass(de).trigger(t.Event(ce.HIDDEN, l)));
                                    }
                                }
                            }
                    }, e._getParentFromElement = function(e) {
                        var t;
                        const n = u.getSelectorFromElement(e);
                        return n && (t = document.querySelector(n)), t || e.parentNode;
                    }, e._dataApiKeydownHandler = function(n) {
                        if ((/input|textarea/i.test(n.target.tagName)
                                ? !(32 === n.which ||
                                    27 !== n.which &&
                                    (40 !== n.which && 38 !== n.which || t(n.target).closest(_e).length))
                                : ue.test(n.which)) &&
                            (n.preventDefault(), n.stopPropagation(), !this.disabled && !t(this).hasClass(fe))) {
                            const i = e._getParentFromElement(this);
                            const r = t(i).hasClass(de);
                            if (r && (!r || 27 !== n.which && 32 !== n.which)) {
                                const o = [].slice.call(i.querySelectorAll(we));
                                if (0 !== o.length) {
                                    let s = o.indexOf(n.target);
                                    38 === n.which && s > 0 && s--, 40 === n.which && s < o.length - 1 && s++, s < 0 &&
                                        (s = 0), o[s].focus();
                                }
                            } else {
                                if (27 === n.which) {
                                    const a = i.querySelector(ye);
                                    t(a).trigger("focus");
                                }
                                t(this).trigger("click");
                            }
                        }
                    }, r(e,
                        null,
                        [
                            { key: "VERSION", get: function() { return"4.3.1" } },
                            { key: "Default", get: function() { return Ne } },
                            { key: "DefaultType", get: function() { return Ie } }
                        ]), e;
                }();
            t(document).on(ce.KEYDOWN_DATA_API, ye, Oe._dataApiKeydownHandler)
                .on(ce.KEYDOWN_DATA_API, _e, Oe._dataApiKeydownHandler)
                .on(ce.CLICK_DATA_API + " " + ce.KEYUP_DATA_API, Oe._clearMenus)
                .on(ce.CLICK_DATA_API,
                    ye,
                    (function(e) {
                        e.preventDefault(), e.stopPropagation(), Oe._jQueryInterface.call(t(this), "toggle");
                    })).on(ce.CLICK_DATA_API, be, (function(e) { e.stopPropagation() })), t.fn[ae] =
                Oe._jQueryInterface, t.fn[ae].Constructor = Oe, t.fn[ae].noConflict = function() {
                return t.fn[ae] = le, Oe._jQueryInterface;
            };
            var ke = t.fn.modal,
                Le = { backdrop: !0, keyboard: !0, focus: !0, show: !0 },
                je = { backdrop: "(boolean|string)", keyboard: "boolean", focus: "boolean", show: "boolean" },
                Pe =
                {
                    HIDE: "hide.bs.modal",
                    HIDDEN: "hidden.bs.modal",
                    SHOW: "show.bs.modal",
                    SHOWN: "shown.bs.modal",
                    FOCUSIN: "focusin.bs.modal",
                    RESIZE: "resize.bs.modal",
                    CLICK_DISMISS: "click.dismiss.bs.modal",
                    KEYDOWN_DISMISS: "keydown.dismiss.bs.modal",
                    MOUSEUP_DISMISS: "mouseup.dismiss.bs.modal",
                    MOUSEDOWN_DISMISS: "mousedown.dismiss.bs.modal",
                    CLICK_DATA_API: "click.bs.modal.data-api"
                },
                He = "modal-dialog-scrollable",
                Re = "modal-scrollbar-measure",
                Me = "modal-backdrop",
                qe = "modal-open",
                Fe = "fade",
                We = "show",
                Be = {
                    DIALOG: ".modal-dialog",
                    MODAL_BODY: ".modal-body",
                    DATA_TOGGLE: '[data-toggle="modal"]',
                    DATA_DISMISS: '[data-dismiss="modal"]',
                    FIXED_CONTENT: ".fixed-top, .fixed-bottom, .is-fixed, .sticky-top",
                    STICKY_CONTENT: ".sticky-top"
                },
                Ue = function() {
                    function e(e, t) {
                        this._config = this._getConfig(t), this._element = e, this._dialog =
                            e.querySelector(Be.DIALOG), this._backdrop = null, this._isShown =
                            !1, this._isBodyOverflowing =
                            !1, this._ignoreBackdropClick = !1, this._isTransitioning = !1, this._scrollbarWidth = 0;
                    }

                    const n = e.prototype;
                    return n.toggle = function(e) { return this._isShown ? this.hide() : this.show(e) }, n.show =
                            function(e) {
                                var n = this;
                                if (!this._isShown && !this._isTransitioning) {
                                    t(this._element).hasClass(Fe) && (this._isTransitioning = !0);
                                    const i = t.Event(Pe.SHOW, { relatedTarget: e });
                                    t(this._element).trigger(i), this._isShown ||
                                        i.isDefaultPrevented() ||
                                        (this._isShown =
                                                !0, this._checkScrollbar(), this._setScrollbar(), this._adjustDialog(),
                                            this._setEscapeEvent(), this._setResizeEvent(), t(this._element).on(
                                                Pe.CLICK_DISMISS,
                                                Be.DATA_DISMISS,
                                                (function(e) { return n.hide(e) })), t(this._dialog).on(
                                                Pe.MOUSEDOWN_DISMISS,
                                                (function() {
                                                    t(n._element).one(Pe.MOUSEUP_DISMISS,
                                                        (function(e) {
                                                            t(e.target).is(n._element) && (n._ignoreBackdropClick = !0);
                                                        }));
                                                })), this._showBackdrop((function() { return n._showElement(e) })));
                                }
                            }, n.hide = function(e) {
                            var n = this;
                            if (e && e.preventDefault(), this._isShown && !this._isTransitioning) {
                                const i = t.Event(Pe.HIDE);
                                if (t(this._element).trigger(i), this._isShown && !i.isDefaultPrevented()) {
                                    this._isShown = !1;
                                    const r = t(this._element).hasClass(Fe);
                                    if (r && (this._isTransitioning = !0), this._setEscapeEvent(),
                                        this._setResizeEvent(),
                                        t(document).off(Pe.FOCUSIN), t(this._element).removeClass(We), t(this._element)
                                            .off(Pe.CLICK_DISMISS), t(this._dialog).off(Pe.MOUSEDOWN_DISMISS), r) {
                                        const o = u.getTransitionDurationFromElement(this._element);
                                        t(this._element).one(u.TRANSITION_END, (function(e) { return n._hideModal(e) }))
                                            .emulateTransitionEnd(o);
                                    } else this._hideModal();
                                }
                            }
                        }, n.dispose =
                            function() {
                                [window, this._element, this._dialog].forEach((function(e) {
                                        return t(e).off(".bs.modal");
                                    })),
                                    t(document).off(Pe.FOCUSIN), t.removeData(this._element, "bs.modal"), this._config =
                                        null, this._element = null, this._dialog = null, this._backdrop =
                                        null, this._isShown =
                                        null, this._isBodyOverflowing = null, this._ignoreBackdropClick =
                                        null, this._isTransitioning = null, this._scrollbarWidth = null;
                            }, n.handleUpdate = function() { this._adjustDialog() }, n._getConfig = function(e) {
                            return e = s({}, Le, e), u.typeCheckConfig("modal", e, je), e;
                        }, n._showElement = function(e) {
                            var n = this;
                            const i = t(this._element).hasClass(Fe);
                            this._element.parentNode && this._element.parentNode.nodeType === Node.ELEMENT_NODE ||
                                    document.body.appendChild(this._element), this._element.style.display =
                                    "block", this._element.removeAttribute("aria-hidden"),
                                this._element.setAttribute("aria-modal", !0), t(this._dialog).hasClass(He)
                                    ? this._dialog.querySelector(Be.MODAL_BODY).scrollTop = 0
                                    : this._element.scrollTop = 0, i && u.reflow(this._element), t(this._element)
                                    .addClass(We),
                                this._config.focus && this._enforceFocus();
                            var r = t.Event(Pe.SHOWN, { relatedTarget: e });
                            const o = function() {
                                n._config.focus && n._element.focus(), n._isTransitioning =
                                    !1, t(n._element).trigger(r);
                            };
                            if (i) {
                                const s = u.getTransitionDurationFromElement(this._dialog);
                                t(this._dialog).one(u.TRANSITION_END, o).emulateTransitionEnd(s);
                            } else o();
                        }, n._enforceFocus = function() {
                            var e = this;
                            t(document).off(Pe.FOCUSIN).on(Pe.FOCUSIN,
                                (function(n) {
                                    document !== n.target &&
                                        e._element !== n.target &&
                                        0 === t(e._element).has(n.target).length &&
                                        e._element.focus();
                                }));
                        }, n._setEscapeEvent = function() {
                            var e = this;
                            this._isShown && this._config.keyboard
                                ? t(this._element).on(Pe.KEYDOWN_DISMISS,
                                    (function(t) { 27 === t.which && (t.preventDefault(), e.hide()) }))
                                : this._isShown || t(this._element).off(Pe.KEYDOWN_DISMISS);
                        }, n._setResizeEvent = function() {
                            var e = this;
                            this._isShown
                                ? t(window).on(Pe.RESIZE, (function(t) { return e.handleUpdate(t) }))
                                : t(window).off(Pe.RESIZE);
                        }, n._hideModal = function() {
                            var e = this;
                            this._element.style.display =
                                    "none", this._element.setAttribute("aria-hidden", !0),
                                this._element.removeAttribute("aria-modal"), this._isTransitioning =
                                    !1, this._showBackdrop((function() {
                                    t(document.body).removeClass(qe), e._resetAdjustments(), e._resetScrollbar(), t(
                                            e._element)
                                        .trigger(Pe.HIDDEN);
                                }));
                        }, n._removeBackdrop =
                            function() { this._backdrop && (t(this._backdrop).remove(), this._backdrop = null) },
                        n._showBackdrop = function(e) {
                            var n = this;
                            const i = t(this._element).hasClass(Fe) ? Fe : "";
                            if (this._isShown && this._config.backdrop) {
                                if (this._backdrop = document.createElement("div"), this._backdrop.className =
                                        Me, i && this._backdrop.classList.add(i), t(this._backdrop)
                                        .appendTo(document.body),
                                    t(this._element).on(Pe.CLICK_DISMISS,
                                        (function(e) {
                                            n._ignoreBackdropClick
                                                ? n._ignoreBackdropClick = !1
                                                : e.target === e.currentTarget &&
                                                ("static" === n._config.backdrop ? n._element.focus() : n.hide());
                                        })), i && u.reflow(this._backdrop), t(this._backdrop).addClass(We), !e) return;
                                if (!i) return void e();
                                const r = u.getTransitionDurationFromElement(this._backdrop);
                                t(this._backdrop).one(u.TRANSITION_END, e).emulateTransitionEnd(r);
                            } else if (!this._isShown && this._backdrop) {
                                t(this._backdrop).removeClass(We);
                                const o = function() { n._removeBackdrop(), e && e() };
                                if (t(this._element).hasClass(Fe)) {
                                    const s = u.getTransitionDurationFromElement(this._backdrop);
                                    t(this._backdrop).one(u.TRANSITION_END, o).emulateTransitionEnd(s);
                                } else o();
                            } else e && e();
                        }, n._adjustDialog = function() {
                            const e = this._element.scrollHeight > document.documentElement.clientHeight;
                            !this._isBodyOverflowing &&
                                    e &&
                                    (this._element.style.paddingLeft = this._scrollbarWidth + "px"),
                                this._isBodyOverflowing &&
                                    !e &&
                                    (this._element.style.paddingRight = this._scrollbarWidth + "px");
                        }, n._resetAdjustments = function() {
                            this._element.style.paddingLeft = "", this._element.style.paddingRight = "";
                        }, n._checkScrollbar = function() {
                            const e = document.body.getBoundingClientRect();
                            this._isBodyOverflowing = e.left + e.right < window.innerWidth, this._scrollbarWidth =
                                this._getScrollbarWidth();
                        }, n._setScrollbar = function() {
                            var e = this;
                            if (this._isBodyOverflowing) {
                                const n = [].slice.call(document.querySelectorAll(Be.FIXED_CONTENT));
                                const i = [].slice.call(document.querySelectorAll(Be.STICKY_CONTENT));
                                t(n).each((function(n, i) {
                                    const r = i.style.paddingRight;
                                    const o = t(i).css("padding-right");
                                    t(i).data("padding-right", r).css("padding-right",
                                        parseFloat(o) + e._scrollbarWidth + "px");
                                })), t(i).each((function(n, i) {
                                    const r = i.style.marginRight;
                                    const o = t(i).css("margin-right");
                                    t(i).data("margin-right", r).css("margin-right",
                                        parseFloat(o) - e._scrollbarWidth + "px");
                                }));
                                const r = document.body.style.paddingRight;
                                const o = t(document.body).css("padding-right");
                                t(document.body).data("padding-right", r)
                                    .css("padding-right", parseFloat(o) + this._scrollbarWidth + "px");
                            }
                            t(document.body).addClass(qe);
                        }, n._resetScrollbar = function() {
                            const e = [].slice.call(document.querySelectorAll(Be.FIXED_CONTENT));
                            t(e).each((function(e, n) {
                                const i = t(n).data("padding-right");
                                t(n).removeData("padding-right"), n.style.paddingRight = i || "";
                            }));
                            const n = [].slice.call(document.querySelectorAll(`${Be.STICKY_CONTENT}`));
                            t(n).each((function(e, n) {
                                const i = t(n).data("margin-right");
                                void 0 !== i && t(n).css("margin-right", i).removeData("margin-right");
                            }));
                            const i = t(document.body).data("padding-right");
                            t(document.body).removeData("padding-right"), document.body.style.paddingRight = i || "";
                        }, n._getScrollbarWidth = function() {
                            const e = document.createElement("div");
                            e.className = Re, document.body.appendChild(e);
                            const t = e.getBoundingClientRect().width - e.clientWidth;
                            return document.body.removeChild(e), t;
                        }, e._jQueryInterface = function(n, i) {
                            return this.each((function() {
                                var r = t(this).data("bs.modal");
                                const o = s({}, Le, t(this).data(), "object" == typeof n && n ? n : {});
                                if (r || (r = new e(this, o), t(this).data("bs.modal", r)), "string" == typeof n) {
                                    if (void 0 === r[n]) throw new TypeError(`No method named "${n}"`);
                                    r[n](i);
                                } else o.show && r.show(i);
                            }));
                        }, r(e,
                            null,
                            [
                                { key: "VERSION", get: function() { return"4.3.1" } },
                                { key: "Default", get: function() { return Le } }
                            ]), e;
                }();
            t(document).on(Pe.CLICK_DATA_API,
                Be.DATA_TOGGLE,
                (function(e) {
                    var n, i = this;
                    const r = u.getSelectorFromElement(this);
                    r && (n = document.querySelector(r));
                    const o = t(n).data("bs.modal") ? "toggle" : s({}, t(n).data(), t(this).data());
                    "A" !== this.tagName && "AREA" !== this.tagName || e.preventDefault();
                    var a = t(n).one(Pe.SHOW,
                        (function(e) {
                            e.isDefaultPrevented() ||
                                a.one(Pe.HIDDEN, (function() { t(i).is(":visible") && i.focus() }));
                        }));
                    Ue._jQueryInterface.call(t(n), o, this);
                })), t.fn.modal = Ue._jQueryInterface, t.fn.modal.Constructor = Ue, t.fn.modal.noConflict = function() {
                return t.fn.modal = ke, Ue._jQueryInterface;
            };
            var Ve = ["background", "cite", "href", "itemtype", "longdesc", "poster", "src", "xlink:href"],
                Ke = {
                    "*": ["class", "dir", "id", "lang", "role", /^aria-[\w-]*$/i],
                    a: ["target", "href", "title", "rel"],
                    area: [],
                    b: [],
                    br: [],
                    col: [],
                    code: [],
                    div: [],
                    em: [],
                    hr: [],
                    h1: [],
                    h2: [],
                    h3: [],
                    h4: [],
                    h5: [],
                    h6: [],
                    i: [],
                    img: ["src", "alt", "title", "width", "height"],
                    li: [],
                    ol: [],
                    p: [],
                    pre: [],
                    s: [],
                    small: [],
                    span: [],
                    sub: [],
                    sup: [],
                    strong: [],
                    u: [],
                    ul: []
                },
                $e = /^(?:(?:https?|mailto|ftp|tel|file):|[^&:/?#]*(?:[/?#]|$))/gi,
                Qe =
                    /^data:(?:image\/(?:bmp|gif|jpeg|jpg|png|tiff|webp)|video\/(?:mpeg|mp4|ogg|webm)|audio\/(?:mp3|oga|ogg|opus));base64,[a-z0-9+/]+=*$/i;

            function Ye(e, t, n) {
                if (0 === e.length) return e;
                if (n && "function" == typeof n) return n(e);
                for (var i = (new window.DOMParser).parseFromString(e, "text/html"),
                    r = Object.keys(t),
                    o = [].slice.call(i.body.querySelectorAll("*")),
                    s = function(e, n) {
                        var i = o[e];
                        const s = i.nodeName.toLowerCase();
                        if (-1 === r.indexOf(i.nodeName.toLowerCase())) return i.parentNode.removeChild(i), "continue";
                        const a = [].slice.call(i.attributes);
                        var l = [].concat(t["*"] || [], t[s] || []);
                        a.forEach((function(e) {
                            (function(e, t) {
                                    const n = e.nodeName.toLowerCase();
                                    if (-1 !== t.indexOf(n))
                                        return-1 === Ve.indexOf(n) ||
                                            Boolean(e.nodeValue.match($e) || e.nodeValue.match(Qe));
                                    for (var i = t.filter((function(e) { return e instanceof RegExp })),
                                        r = 0,
                                        o = i.length;
                                        r < o;
                                        r++) if (n.match(i[r])) return!0;
                                    return!1;
                                })(e, l) ||
                                i.removeAttribute(e.nodeName);
                        }));
                    },
                    a = 0,
                    l = o.length;
                    a < l;
                    a++) s(a);
                return i.body.innerHTML;
            }

            var ze = "tooltip",
                Xe = t.fn.tooltip,
                Ge = new RegExp("(^|\\s)bs-tooltip\\S+", "g"),
                Je = ["sanitize", "whiteList", "sanitizeFn"],
                Ze =
                {
                    animation: "boolean",
                    template: "string",
                    title: "(string|element|function)",
                    trigger: "string",
                    delay: "(number|object)",
                    html: "boolean",
                    selector: "(string|boolean)",
                    placement: "(string|function)",
                    offset: "(number|string|function)",
                    container: "(string|element|boolean)",
                    fallbackPlacement: "(string|array)",
                    boundary: "(string|element)",
                    sanitize: "boolean",
                    sanitizeFn: "(null|function)",
                    whiteList: "object"
                },
                et = { AUTO: "auto", TOP: "top", RIGHT: "right", BOTTOM: "bottom", LEFT: "left" },
                tt =
                {
                    animation: !0,
                    template:
                        '<div class="tooltip" role="tooltip"><div class="arrow"></div><div class="tooltip-inner"></div></div>',
                    trigger: "hover focus",
                    title: "",
                    delay: 0,
                    html: !1,
                    selector: !1,
                    placement: "top",
                    offset: 0,
                    container: !1,
                    fallbackPlacement: "flip",
                    boundary: "scrollParent",
                    sanitize: !0,
                    sanitizeFn: null,
                    whiteList: Ke
                },
                nt = "show",
                it = "out",
                rt = {
                    HIDE: "hide.bs.tooltip",
                    HIDDEN: "hidden.bs.tooltip",
                    SHOW: "show.bs.tooltip",
                    SHOWN: "shown.bs.tooltip",
                    INSERTED: "inserted.bs.tooltip",
                    CLICK: "click.bs.tooltip",
                    FOCUSIN: "focusin.bs.tooltip",
                    FOCUSOUT: "focusout.bs.tooltip",
                    MOUSEENTER: "mouseenter.bs.tooltip",
                    MOUSELEAVE: "mouseleave.bs.tooltip"
                },
                ot = "fade",
                st = "show",
                at = ".tooltip-inner",
                lt = ".arrow",
                ut = "hover",
                ct = "focus",
                ft = "click",
                dt = "manual",
                ht = function() {
                    function e(e, t) {
                        if (void 0 === n)
                            throw new TypeError("Bootstrap's tooltips require Popper.js (https://popper.js.org/)");
                        this._isEnabled = !0, this._timeout = 0, this._hoverState = "", this._activeTrigger =
                            {}, this._popper = null, this.element = e, this.config = this._getConfig(t), this.tip =
                            null, this._setListeners();
                    }

                    const i = e.prototype;
                    return i.enable = function() { this._isEnabled = !0 }, i.disable =
                        function() { this._isEnabled = !1 }, i.toggleEnabled =
                        function() { this._isEnabled = !this._isEnabled }, i.toggle = function(e) {
                        if (this._isEnabled)
                            if (e) {
                                const n = this.constructor.DATA_KEY;
                                let i = t(e.currentTarget).data(n);
                                i ||
                                (i = new this.constructor(e.currentTarget, this._getDelegateConfig()), t(
                                    e.currentTarget).data(n, i)), i._activeTrigger.click =
                                    !i._activeTrigger.click, i._isWithActiveTrigger()
                                    ? i._enter(null, i)
                                    : i._leave(null, i);
                            } else {
                                if (t(this.getTipElement()).hasClass(st)) return void this._leave(null, this);
                                this._enter(null, this);
                            }
                    }, i.dispose = function() {
                        clearTimeout(this._timeout), t.removeData(this.element, this.constructor.DATA_KEY),
                            t(this.element).off(this.constructor.EVENT_KEY),
                            t(this.element).closest(".modal").off("hide.bs.modal"), this.tip && t(this.tip).remove(),
                            this._isEnabled = null, this._timeout = null, this._hoverState = null, this._activeTrigger =
                                null, null !== this._popper && this._popper.destroy(), this._popper =
                                null, this.element =
                                null, this.config = null, this.tip = null;
                    }, i.show = function() {
                        var e = this;
                        if ("none" === t(this.element).css("display"))
                            throw new Error("Please use show on visible elements");
                        var i = t.Event(this.constructor.Event.SHOW);
                        if (this.isWithContent() && this._isEnabled) {
                            t(this.element).trigger(i);
                            var r = u.findShadowRoot(this.element),
                                o = t.contains(null !== r ? r : this.element.ownerDocument.documentElement,
                                    this.element);
                            if (i.isDefaultPrevented() || !o) return;
                            var s = this.getTipElement(), a = u.getUID(this.constructor.NAME);
                            s.setAttribute("id", a), this.element.setAttribute("aria-describedby", a),
                                this.setContent(),
                                this.config.animation && t(s).addClass(ot);
                            var l = "function" == typeof this.config.placement
                                    ? this.config.placement.call(this, s, this.element)
                                    : this.config.placement,
                                c = this._getAttachment(l);
                            this.addAttachmentClass(c);
                            var f = this._getContainer();
                            t(s).data(this.constructor.DATA_KEY, this),
                                t.contains(this.element.ownerDocument.documentElement, this.tip) || t(s).appendTo(f),
                                t(this.element).trigger(this.constructor.Event.INSERTED), this._popper = new n(
                                    this.element,
                                    s,
                                    {
                                        placement: c,
                                        modifiers: {
                                            offset: this._getOffset(),
                                            flip: { behavior: this.config.fallbackPlacement },
                                            arrow: { element: lt },
                                            preventOverflow: { boundariesElement: this.config.boundary }
                                        },
                                        onCreate: function(t) {
                                            t.originalPlacement !== t.placement && e._handlePopperPlacementChange(t);
                                        },
                                        onUpdate: function(t) { return e._handlePopperPlacementChange(t) }
                                    }), t(s).addClass(st), "ontouchstart" in document.documentElement &&
                                    t(document.body).children().on("mouseover", null, t.noop);
                            var d = function() {
                                e.config.animation && e._fixTransition();
                                const n = e._hoverState;
                                e._hoverState =
                                        null, t(e.element).trigger(e.constructor.Event.SHOWN),
                                    n === it && e._leave(null, e);
                            };
                            if (t(this.tip).hasClass(ot)) {
                                var h = u.getTransitionDurationFromElement(this.tip);
                                t(this.tip).one(u.TRANSITION_END, d).emulateTransitionEnd(h);
                            } else d();
                        }
                    }, i.hide = function(e) {
                        var n = this, i = this.getTipElement();
                        const r = t.Event(this.constructor.Event.HIDE);
                        const o = function() {
                            n._hoverState !== nt && i.parentNode && i.parentNode.removeChild(i), n._cleanTipClass(),
                                n.element.removeAttribute("aria-describedby"), t(n.element)
                                    .trigger(n.constructor.Event.HIDDEN), null !== n._popper && n._popper.destroy(),
                                e &&
                                    e();
                        };
                        if (t(this.element).trigger(r), !r.isDefaultPrevented()) {
                            if (t(i).removeClass(st), "ontouchstart" in document.documentElement &&
                                t(document.body).children().off("mouseover", null, t.noop), this._activeTrigger[ft] =
                                !1, this._activeTrigger[ct] = !1, this._activeTrigger[ut] =
                                !1, t(this.tip).hasClass(ot)) {
                                const s = u.getTransitionDurationFromElement(i);
                                t(i).one(u.TRANSITION_END, o).emulateTransitionEnd(s);
                            } else o();
                            this._hoverState = "";
                        }
                    }, i.update =
                        function() { null !== this._popper && this._popper.scheduleUpdate() }, i.isWithContent =
                        function() { return Boolean(this.getTitle()) }, i.addAttachmentClass =
                        function(e) { t(this.getTipElement()).addClass(`bs-tooltip-${e}`) }, i.getTipElement =
                        function() {
                            return this.tip = this.tip || t(this.config.template)[0], this.tip;
                        }, i.setContent = function() {
                        const e = this.getTipElement();
                        this.setElementContent(t(e.querySelectorAll(at)), this.getTitle()), t(e)
                            .removeClass(ot + " " + st);
                    }, i.setElementContent = function(e, n) {
                        "object" != typeof n || !n.nodeType && !n.jquery
                            ? this.config.html
                            ? (this.config.sanitize && (n = Ye(n, this.config.whiteList, this.config.sanitizeFn)), e
                                .html(n)
                            )
                            : e.text(n)
                            : this.config.html
                            ? t(n).parent().is(e) || e.empty().append(n)
                            : e.text(t(n).text());
                    }, i.getTitle = function() {
                        var e = this.element.getAttribute("data-original-title");
                        return e ||
                        (e = "function" == typeof this.config.title
                            ? this.config.title.call(this.element)
                            : this.config.title), e;
                    }, i._getOffset = function() {
                        var e = this;
                        const t = {};
                        return"function" == typeof this.config.offset
                            ? t.fn = function(t) {
                                return t.offsets = s({}, t.offsets, e.config.offset(t.offsets, e.element) || {}), t;
                            }
                            : t.offset = this.config.offset, t;
                    }, i._getContainer =
                        function() {
                            return!1 === this.config.container
                                ? document.body
                                : u.isElement(this.config.container)
                                ? t(this.config.container)
                                : t(document).find(this.config.container);
                        }, i._getAttachment = function(e) { return et[e.toUpperCase()] }, i._setListeners = function() {
                        var e = this;
                        this.config.trigger.split(" ").forEach((function(n) {
                                if ("click" === n)
                                    t(e.element).on(e.constructor.Event.CLICK,
                                        e.config.selector,
                                        (function(t) { return e.toggle(t) }));
                                else if (n !== dt) {
                                    const i = n === ut ? e.constructor.Event.MOUSEENTER : e.constructor.Event.FOCUSIN;
                                    const r = n === ut ? e.constructor.Event.MOUSELEAVE : e.constructor.Event.FOCUSOUT;
                                    t(e.element).on(i, e.config.selector, (function(t) { return e._enter(t) })).on(r,
                                        e.config.selector,
                                        (function(t) { return e._leave(t) }));
                                }
                            })), t(this.element).closest(".modal")
                                .on("hide.bs.modal", (function() { e.element && e.hide() })),
                            this.config.selector
                                ? this.config = s({}, this.config, { trigger: "manual", selector: "" })
                                : this._fixTitle();
                    }, i._fixTitle = function() {
                        const e = typeof this.element.getAttribute("data-original-title");
                        (this.element.getAttribute("title") || "string" !== e) &&
                        (this.element.setAttribute("data-original-title", this.element.getAttribute("title") || ""),
                            this.element.setAttribute("title", ""));
                    }, i._enter = function(e, n) {
                        const i = this.constructor.DATA_KEY;
                        (n = n || t(e.currentTarget).data(i)) ||
                            (n = new this.constructor(e.currentTarget, this._getDelegateConfig()), t(e.currentTarget)
                                .data(i, n)), e && (n._activeTrigger["focusin" === e.type ? ct : ut] = !0),
                            t(n.getTipElement()).hasClass(st) || n._hoverState === nt
                                ? n._hoverState = nt
                                : (clearTimeout(n._timeout), n._hoverState = nt, n.config.delay && n.config.delay.show
                                    ? n._timeout = setTimeout((function() { n._hoverState === nt && n.show() }),
                                        n.config.delay.show)
                                    : n.show());
                    }, i._leave = function(e, n) {
                        const i = this.constructor.DATA_KEY;
                        (n = n || t(e.currentTarget).data(i)) ||
                            (n = new this.constructor(e.currentTarget, this._getDelegateConfig()), t(e.currentTarget)
                                .data(i, n)), e && (n._activeTrigger["focusout" === e.type ? ct : ut] = !1),
                            n._isWithActiveTrigger() ||
                            (clearTimeout(n._timeout), n._hoverState = it, n.config.delay && n.config.delay.hide
                                ? n._timeout =
                                setTimeout((function() { n._hoverState === it && n.hide() }), n.config.delay.hide)
                                : n.hide());
                    }, i._isWithActiveTrigger = function() {
                        for (let e in this._activeTrigger) if (this._activeTrigger[e]) return!0;
                        return!1;
                    }, i._getConfig = function(e) {
                        var n = t(this.element).data();
                        return Object.keys(n).forEach((function(e) { -1 !== Je.indexOf(e) && delete n[e] })),
                            "number" ==
                                typeof(e = s({}, this.constructor.Default, n, "object" == typeof e && e ? e : {}))
                                .delay &&
                                (e.delay = { show: e.delay, hide: e.delay }),
                            "number" == typeof e.title && (e.title = e.title.toString()),
                            "number" == typeof e.content &&
                                (e.content = e.content.toString()), u.typeCheckConfig(ze,
                                e,
                                this.constructor.DefaultType),
                            e.sanitize && (e.template = Ye(e.template, e.whiteList, e.sanitizeFn)), e;
                    }, i._getDelegateConfig = function() {
                        const e = {};
                        if (this.config)
                            for (let t in this.config)
                                this.constructor.Default[t] !== this.config[t] && (e[t] = this.config[t]);
                        return e;
                    }, i._cleanTipClass = function() {
                        const e = t(this.getTipElement());
                        const n = e.attr("class").match(Ge);
                        null !== n && n.length && e.removeClass(n.join(""));
                    }, i._handlePopperPlacementChange = function(e) {
                        const t = e.instance;
                        this.tip =
                            t.popper, this._cleanTipClass(), this.addAttachmentClass(this._getAttachment(e.placement));
                    }, i._fixTransition = function() {
                        const e = this.getTipElement();
                        const n = this.config.animation;
                        null === e.getAttribute("x-placement") &&
                        (t(e).removeClass(ot), this.config.animation =
                            !1, this.hide(), this.show(), this.config.animation = n);
                    }, e._jQueryInterface = function(n) {
                        return this.each((function() {
                            var i = t(this).data("bs.tooltip");
                            const r = "object" == typeof n && n;
                            if ((i || !/dispose|hide/.test(n)) &&
                                (i || (i = new e(this, r), t(this).data("bs.tooltip", i)), "string" == typeof n)) {
                                if (void 0 === i[n]) throw new TypeError(`No method named "${n}"`);
                                i[n]();
                            }
                        }));
                    }, r(e,
                        null,
                        [
                            { key: "VERSION", get: function() { return"4.3.1" } },
                            { key: "Default", get: function() { return tt } },
                            { key: "NAME", get: function() { return ze } },
                            { key: "DATA_KEY", get: function() { return"bs.tooltip" } },
                            { key: "Event", get: function() { return rt } },
                            { key: "EVENT_KEY", get: function() { return".bs.tooltip" } },
                            { key: "DefaultType", get: function() { return Ze } }
                        ]), e;
                }();
            t.fn.tooltip = ht._jQueryInterface, t.fn.tooltip.Constructor = ht, t.fn.tooltip.noConflict = function() {
                return t.fn.tooltip = Xe, ht._jQueryInterface;
            };
            var pt = "popover",
                gt = t.fn.popover,
                mt = new RegExp("(^|\\s)bs-popover\\S+", "g"),
                vt =
                    s({},
                        ht.Default,
                        {
                            placement: "right",
                            trigger: "click",
                            content: "",
                            template:
                                '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-header"></h3><div class="popover-body"></div></div>'
                        }),
                yt = s({}, ht.DefaultType, { content: "(string|element|function)" }),
                bt = "fade",
                _t = "show",
                Et = ".popover-header",
                wt = ".popover-body",
                Tt = {
                    HIDE: "hide.bs.popover",
                    HIDDEN: "hidden.bs.popover",
                    SHOW: "show.bs.popover",
                    SHOWN: "shown.bs.popover",
                    INSERTED: "inserted.bs.popover",
                    CLICK: "click.bs.popover",
                    FOCUSIN: "focusin.bs.popover",
                    FOCUSOUT: "focusout.bs.popover",
                    MOUSEENTER: "mouseenter.bs.popover",
                    MOUSELEAVE: "mouseleave.bs.popover"
                },
                Ct = function(e) {
                    var n, i;

                    function o() { return e.apply(this, arguments) || this }

                    i = e, (n = o).prototype = Object.create(i.prototype), n.prototype.constructor = n, n.__proto__ = i;
                    const s = o.prototype;
                    return s.isWithContent =
                        function() { return this.getTitle() || this._getContent() }, s.addAttachmentClass =
                        function(e) { t(this.getTipElement()).addClass(`bs-popover-${e}`) }, s.getTipElement =
                        function() {
                            return this.tip = this.tip || t(this.config.template)[0], this.tip;
                        }, s.setContent =
                        function() {
                            const e = t(this.getTipElement());
                            this.setElementContent(e.find(Et), this.getTitle());
                            var n = this._getContent();
                            "function" == typeof n && (n = n.call(this.element)), this.setElementContent(e.find(wt), n),
                                e.removeClass(bt + " " + _t);
                        }, s._getContent = function() {
                        return this.element.getAttribute("data-content") || this.config.content;
                    }, s._cleanTipClass = function() {
                        const e = t(this.getTipElement());
                        const n = e.attr("class").match(mt);
                        null !== n && n.length > 0 && e.removeClass(n.join(""));
                    }, o._jQueryInterface = function(e) {
                        return this.each((function() {
                            var n = t(this).data("bs.popover");
                            const i = "object" == typeof e ? e : null;
                            if ((n || !/dispose|hide/.test(e)) &&
                                (n || (n = new o(this, i), t(this).data("bs.popover", n)), "string" == typeof e)) {
                                if (void 0 === n[e]) throw new TypeError(`No method named "${e}"`);
                                n[e]();
                            }
                        }));
                    }, r(o,
                        null,
                        [
                            { key: "VERSION", get: function() { return"4.3.1" } },
                            { key: "Default", get: function() { return vt } },
                            { key: "NAME", get: function() { return pt } },
                            { key: "DATA_KEY", get: function() { return"bs.popover" } },
                            { key: "Event", get: function() { return Tt } },
                            { key: "EVENT_KEY", get: function() { return".bs.popover" } },
                            { key: "DefaultType", get: function() { return yt } }
                        ]), o;
                }(ht);
            t.fn.popover = Ct._jQueryInterface, t.fn.popover.Constructor = Ct, t.fn.popover.noConflict = function() {
                return t.fn.popover = gt, Ct._jQueryInterface;
            };
            var xt = "scrollspy",
                St = t.fn[xt],
                Dt = { offset: 10, method: "auto", target: "" },
                At = { offset: "number", method: "string", target: "(string|element)" },
                Nt =
                {
                    ACTIVATE: "activate.bs.scrollspy",
                    SCROLL: "scroll.bs.scrollspy",
                    LOAD_DATA_API: "load.bs.scrollspy.data-api"
                },
                It = "dropdown-item",
                Ot = "active",
                kt = {
                    DATA_SPY: '[data-spy="scroll"]',
                    ACTIVE: ".active",
                    NAV_LIST_GROUP: ".nav, .list-group",
                    NAV_LINKS: ".nav-link",
                    NAV_ITEMS: ".nav-item",
                    LIST_ITEMS: ".list-group-item",
                    DROPDOWN: ".dropdown",
                    DROPDOWN_ITEMS: ".dropdown-item",
                    DROPDOWN_TOGGLE: ".dropdown-toggle"
                },
                Lt = "offset",
                jt = "position",
                Pt = function() {
                    function e(e, n) {
                        var i = this;
                        this._element = e, this._scrollElement = "BODY" === e.tagName ? window : e, this._config =
                                this._getConfig(n), this._selector =
                                this._config.target +
                                " " +
                                kt.NAV_LINKS +
                                "," +
                                this._config.target +
                                " " +
                                kt.LIST_ITEMS +
                                "," +
                                this._config.target +
                                " " +
                                kt.DROPDOWN_ITEMS, this._offsets = [], this._targets = [], this._activeTarget =
                                null, this._scrollHeight =
                                0, t(this._scrollElement).on(Nt.SCROLL, (function(e) { return i._process(e) })), this
                                .refresh(),
                            this._process();
                    }

                    const n = e.prototype;
                    return n.refresh = function() {
                        var e = this;
                        const n = this._scrollElement === this._scrollElement.window ? Lt : jt;
                        var i = "auto" === this._config.method ? n : this._config.method,
                            r = i === jt ? this._getScrollTop() : 0;
                        this._offsets = [], this._targets = [], this._scrollHeight = this._getScrollHeight(), [].slice
                            .call(document.querySelectorAll(this._selector)).map((function(e) {
                                var n;
                                const o = u.getSelectorFromElement(e);
                                if (o && (n = document.querySelector(o)), n) {
                                    const s = n.getBoundingClientRect();
                                    if (s.width || s.height) return[t(n)[i]().top + r, o];
                                }
                                return null;
                            })).filter((function(e) { return e })).sort((function(e, t) { return e[0] - t[0] }))
                            .forEach((function(t) { e._offsets.push(t[0]), e._targets.push(t[1]) }));
                    }, n.dispose = function() {
                        t.removeData(this._element, "bs.scrollspy"), t(this._scrollElement).off(".bs.scrollspy"),
                            this._element = null, this._scrollElement = null, this._config = null, this._selector =
                                null, this._offsets = null, this._targets = null, this._activeTarget =
                                null, this._scrollHeight = null;
                    }, n._getConfig = function(e) {
                        if ("string" != typeof(e = s({}, Dt, "object" == typeof e && e ? e : {})).target) {
                            let n = t(e.target).attr("id");
                            n || (n = u.getUID(xt), t(e.target).attr("id", n)), e.target = `#${n}`;
                        }
                        return u.typeCheckConfig(xt, e, At), e;
                    }, n._getScrollTop =
                        function() {
                            return this._scrollElement === window
                                ? this._scrollElement.pageYOffset
                                : this._scrollElement.scrollTop;
                        }, n._getScrollHeight =
                        function() {
                            return this._scrollElement.scrollHeight ||
                                Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
                        }, n._getOffsetHeight = function() {
                        return this._scrollElement === window
                            ? window.innerHeight
                            : this._scrollElement.getBoundingClientRect().height;
                    }, n._process = function() {
                        const e = this._getScrollTop() + this._config.offset;
                        const t = this._getScrollHeight();
                        const n = this._config.offset + t - this._getOffsetHeight();
                        if (this._scrollHeight !== t && this.refresh(), e >= n) {
                            const i = this._targets[this._targets.length - 1];
                            this._activeTarget !== i && this._activate(i);
                        } else {
                            if (this._activeTarget && e < this._offsets[0] && this._offsets[0] > 0)
                                return this._activeTarget = null, void this._clear();
                            for (let r = this._offsets.length; r--;)
                                this._activeTarget !== this._targets[r] &&
                                    e >= this._offsets[r] &&
                                    (void 0 === this._offsets[r + 1] || e < this._offsets[r + 1]) &&
                                    this._activate(this._targets[r]);
                        }
                    }, n._activate = function(e) {
                        this._activeTarget = e, this._clear();
                        const n = this._selector.split(",").map((function(t) {
                            return t + '[data-target="' + e + '"],' + t + '[href="' + e + '"]';
                        }));
                        const i = t([].slice.call(document.querySelectorAll(n.join(","))));
                        i.hasClass(It)
                            ? (i.closest(kt.DROPDOWN).find(kt.DROPDOWN_TOGGLE).addClass(Ot), i.addClass(Ot))
                            : (i.addClass(Ot), i.parents(kt.NAV_LIST_GROUP).prev(kt.NAV_LINKS + ", " + kt.LIST_ITEMS)
                                .addClass(Ot), i.parents(kt.NAV_LIST_GROUP).prev(kt.NAV_ITEMS).children(kt.NAV_LINKS)
                                .addClass(Ot)), t(this._scrollElement).trigger(Nt.ACTIVATE, { relatedTarget: e });
                    }, n._clear =
                        function() {
                            [].slice.call(document.querySelectorAll(this._selector))
                                .filter((function(e) { return e.classList.contains(Ot) })).forEach((function(e) {
                                    return e.classList.remove(Ot);
                                }));
                        }, e._jQueryInterface = function(n) {
                        return this.each((function() {
                            var i = t(this).data("bs.scrollspy");
                            if (i || (i = new e(this, "object" == typeof n && n), t(this).data("bs.scrollspy", i)),
                                "string" == typeof n) {
                                if (void 0 === i[n]) throw new TypeError(`No method named "${n}"`);
                                i[n]();
                            }
                        }));
                    }, r(e,
                        null,
                        [
                            { key: "VERSION", get: function() { return"4.3.1" } },
                            { key: "Default", get: function() { return Dt } }
                        ]), e;
                }();
            t(window).on(Nt.LOAD_DATA_API,
                (function() {
                    for (var e = [].slice.call(document.querySelectorAll(kt.DATA_SPY)), n = e.length; n--;) {
                        const i = t(e[n]);
                        Pt._jQueryInterface.call(i, i.data());
                    }
                })), t.fn[xt] = Pt._jQueryInterface, t.fn[xt].Constructor = Pt, t.fn[xt].noConflict = function() {
                return t.fn[xt] = St, Pt._jQueryInterface;
            };
            var Ht = t.fn.tab,
                Rt =
                {
                    HIDE: "hide.bs.tab",
                    HIDDEN: "hidden.bs.tab",
                    SHOW: "show.bs.tab",
                    SHOWN: "shown.bs.tab",
                    CLICK_DATA_API: "click.bs.tab.data-api"
                },
                Mt = "dropdown-menu",
                qt = "active",
                Ft = "disabled",
                Wt = "fade",
                Bt = "show",
                Ut = ".dropdown",
                Vt = ".nav, .list-group",
                Kt = ".active",
                $t = "> li > .active",
                Qt = '[data-toggle="tab"], [data-toggle="pill"], [data-toggle="list"]',
                Yt = ".dropdown-toggle",
                zt = "> .dropdown-menu .active",
                Xt = function() {
                    function e(e) { this._element = e }

                    const n = e.prototype;
                    return n.show = function() {
                        var e = this;
                        if (!(this._element.parentNode &&
                            this._element.parentNode.nodeType === Node.ELEMENT_NODE &&
                            t(this._element).hasClass(qt) ||
                            t(this._element).hasClass(Ft))) {
                            var i;
                            const r = t(this._element).closest(Vt)[0];
                            const o = u.getSelectorFromElement(this._element);
                            if (r) {
                                const s = "UL" === r.nodeName || "OL" === r.nodeName ? $t : Kt;
                                i = (i = t.makeArray(t(r).find(s)))[i.length - 1];
                            }
                            const a = t.Event(Rt.HIDE, { relatedTarget: this._element });
                            const l = t.Event(Rt.SHOW, { relatedTarget: i });
                            if (i && t(i).trigger(a), t(this._element).trigger(l), !l.isDefaultPrevented() &&
                                !a.isDefaultPrevented()) {
                                let n;
                                o && (n = document.querySelector(o)), this._activate(this._element, r);
                                const c = function() {
                                    const n = t.Event(Rt.HIDDEN, { relatedTarget: e._element });
                                    const r = t.Event(Rt.SHOWN, { relatedTarget: i });
                                    t(i).trigger(n), t(e._element).trigger(r);
                                };
                                n ? this._activate(n, n.parentNode, c) : c();
                            }
                        }
                    }, n.dispose =
                        function() { t.removeData(this._element, "bs.tab"), this._element = null }, n._activate =
                        function(e, n, i) {
                            var r = this,
                                o = (!n || "UL" !== n.nodeName && "OL" !== n.nodeName
                                    ? t(n).children(Kt)
                                    : t(n).find($t))[0];
                            const s = i && o && t(o).hasClass(Wt);
                            const a = function() { return r._transitionComplete(e, o, i) };
                            if (o && s) {
                                const l = u.getTransitionDurationFromElement(o);
                                t(o).removeClass(Bt).one(u.TRANSITION_END, a).emulateTransitionEnd(l);
                            } else a();
                        }, n._transitionComplete = function(e, n, i) {
                        if (n) {
                            t(n).removeClass(qt);
                            const r = t(n.parentNode).find(zt)[0];
                            r && t(r).removeClass(qt), "tab" === n.getAttribute("role") &&
                                n.setAttribute("aria-selected", !1);
                        }
                        if (t(e).addClass(qt), "tab" === e.getAttribute("role") && e.setAttribute("aria-selected", !0),
                            u.reflow(e), e.classList.contains(Wt) && e.classList.add(Bt), e.parentNode &&
                                t(e.parentNode).hasClass(Mt)) {
                            const o = t(e).closest(Ut)[0];
                            if (o) {
                                const s = [].slice.call(o.querySelectorAll(Yt));
                                t(s).addClass(qt);
                            }
                            e.setAttribute("aria-expanded", !0);
                        }
                        i && i();
                    }, e._jQueryInterface = function(n) {
                        return this.each((function() {
                            const i = t(this);
                            var r = i.data("bs.tab");
                            if (r || (r = new e(this), i.data("bs.tab", r)), "string" == typeof n) {
                                if (void 0 === r[n]) throw new TypeError(`No method named "${n}"`);
                                r[n]();
                            }
                        }));
                    }, r(e, null, [{ key: "VERSION", get: function() { return"4.3.1" } }]), e;
                }();
            t(document).on(Rt.CLICK_DATA_API,
                Qt,
                (function(e) { e.preventDefault(), Xt._jQueryInterface.call(t(this), "show") })), t.fn.tab =
                Xt._jQueryInterface, t.fn.tab.Constructor = Xt, t.fn.tab.noConflict = function() {
                return t.fn.tab = Ht, Xt._jQueryInterface;
            };
            var Gt = t.fn.toast,
                Jt =
                {
                    CLICK_DISMISS: "click.dismiss.bs.toast",
                    HIDE: "hide.bs.toast",
                    HIDDEN: "hidden.bs.toast",
                    SHOW: "show.bs.toast",
                    SHOWN: "shown.bs.toast"
                },
                Zt = "fade",
                en = "hide",
                tn = "show",
                nn = "showing",
                rn = { animation: "boolean", autohide: "boolean", delay: "number" },
                on = { animation: !0, autohide: !0, delay: 500 },
                sn = '[data-dismiss="toast"]',
                an = function() {
                    function e(e, t) {
                        this._element = e, this._config = this._getConfig(t), this._timeout =
                            null, this._setListeners();
                    }

                    const n = e.prototype;
                    return n.show = function() {
                        var e = this;
                        t(this._element).trigger(Jt.SHOW), this._config.animation && this._element.classList.add(Zt);
                        const n = function() {
                            e._element.classList.remove(nn), e._element.classList.add(tn), t(e._element)
                                    .trigger(Jt.SHOWN),
                                e._config.autohide && e.hide();
                        };
                        if (this._element.classList.remove(en), this._element.classList.add(nn), this._config.animation
                        ) {
                            const i = u.getTransitionDurationFromElement(this._element);
                            t(this._element).one(u.TRANSITION_END, n).emulateTransitionEnd(i);
                        } else n();
                    }, n.hide = function(e) {
                        var n = this;
                        this._element.classList.contains(tn) &&
                        (t(this._element).trigger(Jt.HIDE), e
                            ? this._close()
                            : this._timeout = setTimeout((function() { n._close() }), this._config.delay));
                    }, n.dispose =
                        function() {
                            clearTimeout(this._timeout), this._timeout =
                                    null, this._element.classList.contains(tn) && this._element.classList.remove(tn),
                                t(this._element).off(Jt.CLICK_DISMISS), t.removeData(this._element, "bs.toast"), this
                                    ._element =
                                    null, this._config = null;
                        }, n._getConfig =
                        function(e) {
                            return e =
                                s({}, on, t(this._element).data(), "object" == typeof e && e ? e : {}), u
                                .typeCheckConfig(
                                    "toast",
                                    e,
                                    this.constructor.DefaultType), e;
                        }, n._setListeners = function() {
                        var e = this;
                        t(this._element).on(Jt.CLICK_DISMISS, sn, (function() { return e.hide(!0) }));
                    }, n._close = function() {
                        var e = this;
                        const n = function() { e._element.classList.add(en), t(e._element).trigger(Jt.HIDDEN) };
                        if (this._element.classList.remove(tn), this._config.animation) {
                            const i = u.getTransitionDurationFromElement(this._element);
                            t(this._element).one(u.TRANSITION_END, n).emulateTransitionEnd(i);
                        } else n();
                    }, e._jQueryInterface = function(n) {
                        return this.each((function() {
                            const i = t(this);
                            var r = i.data("bs.toast");
                            if (r || (r = new e(this, "object" == typeof n && n), i.data("bs.toast", r)), "string" ==
                                typeof n) {
                                if (void 0 === r[n]) throw new TypeError(`No method named "${n}"`);
                                r[n](this);
                            }
                        }));
                    }, r(e,
                        null,
                        [
                            { key: "VERSION", get: function() { return"4.3.1" } },
                            { key: "DefaultType", get: function() { return rn } },
                            { key: "Default", get: function() { return on } }
                        ]), e;
                }();
            t.fn.toast = an._jQueryInterface, t.fn.toast.Constructor = an, t.fn.toast.noConflict =
                function() { return t.fn.toast = Gt, an._jQueryInterface }, function() {
                if (void 0 === t)
                    throw new TypeError(
                        "Bootstrap's JavaScript requires jQuery. jQuery must be included before Bootstrap's JavaScript.");
                const e = t.fn.jquery.split(" ")[0].split(".");
                if (e[0] < 2 && e[1] < 9 || 1 === e[0] && 9 === e[1] && e[2] < 1 || e[0] >= 4)
                    throw new Error("Bootstrap's JavaScript requires at least jQuery v1.9.1 but less than v4.0.0");
            }(), e.Util = u, e.Alert = g, e.Button = S, e.Carousel = Q, e.Collapse = se, e.Dropdown = Oe, e.Modal =
                Ue, e.Popover = Ct, e.Scrollspy = Pt, e.Tab = Xt, e.Toast = an, e.Tooltip =
                ht, Object.defineProperty(e, "__esModule", { value: !0 });
        }(t, n(2), n(3));
    }, function(e, t, n) {
        var i;
/*!
 * jQuery JavaScript Library v3.4.1
 * https://jquery.com/
 *
 * Includes Sizzle.js
 * https://sizzlejs.com/
 *
 * Copyright JS Foundation and other contributors
 * Released under the MIT license
 * https://jquery.org/license
 *
 * Date: 2019-05-01T21:04Z
 */
        !function(t, n) {
            "use strict";
            "object" == typeof e.exports
                ? e.exports = t.document
                ? n(t, !0)
                : function(e) {
                    if (!e.document) throw new Error("jQuery requires a window with a document");
                    return n(e);
                }
                : n(t);
        }("undefined" != typeof window ? window : this,
            (function(n, r) {
                "use strict";
                var o = [],
                    s = n.document,
                    a = Object.getPrototypeOf,
                    l = o.slice,
                    u = o.concat,
                    c = o.push,
                    f = o.indexOf,
                    d = {},
                    h = d.toString,
                    p = d.hasOwnProperty,
                    g = p.toString,
                    m = g.call(Object),
                    v = {},
                    y = function(e) { return"function" == typeof e && "number" != typeof e.nodeType },
                    b = function(e) { return null != e && e === e.window },
                    _ = { type: !0, src: !0, nonce: !0, noModule: !0 };

                function E(e, t, n) {
                    var i, r;
                    const o = (n = n || s).createElement("script");
                    if (o.text =
                        e, t) for (i in _) (r = t[i] || t.getAttribute && t.getAttribute(i)) && o.setAttribute(i, r);
                    n.head.appendChild(o).parentNode.removeChild(o);
                }

                function w(e) {
                    return null == e
                        ? e + ""
                        : "object" == typeof e || "function" == typeof e
                        ? d[h.call(e)] || "object"
                        : typeof e;
                }

                var T = function(e, t) { return new T.fn.init(e, t) }, C = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;

                function x(e) {
                    const t = !!e && "length" in e && e.length;
                    const n = w(e);
                    return!y(e) && !b(e) && ("array" === n || 0 === t || "number" == typeof t && t > 0 && t - 1 in e);
                }

                T.fn = T.prototype = {
                    jquery: "3.4.1",
                    constructor: T,
                    length: 0,
                    toArray: function() { return l.call(this) },
                    get: function(e) { return null == e ? l.call(this) : e < 0 ? this[e + this.length] : this[e] },
                    pushStack: function(e) {
                        const t = T.merge(this.constructor(), e);
                        return t.prevObject = this, t;
                    },
                    each: function(e) { return T.each(this, e) },
                    map: function(e) {
                        return this.pushStack(T.map(this, (function(t, n) { return e.call(t, n, t) })));
                    },
                    slice: function() { return this.pushStack(l.apply(this, arguments)) },
                    first: function() { return this.eq(0) },
                    last: function() { return this.eq(-1) },
                    eq: function(e) {
                        const t = this.length;
                        const n = +e + (e < 0 ? t : 0);
                        return this.pushStack(n >= 0 && n < t ? [this[n]] : []);
                    },
                    end: function() { return this.prevObject || this.constructor() },
                    push: c,
                    sort: o.sort,
                    splice: o.splice
                }, T.extend = T.fn.extend = function() {
                    var e, t, n, i, r, o, s = arguments[0] || {}, a = 1;
                    const l = arguments.length;
                    var u = !1;
                    for ("boolean" == typeof s && (u = s, s = arguments[a] || {}, a++), "object" == typeof s ||
                            y(s) ||
                            (s = {}), a === l && (s = this, a--);
                        a < l;
                        a++)
                        if (null != (e = arguments[a]))
                            for (t in e)
                                i = e[t], "__proto__" !== t &&
                                    s !== i &&
                                    (u && i && (T.isPlainObject(i) || (r = Array.isArray(i)))
                                        ? (n = s[t], o =
                                            r && !Array.isArray(n) ? [] : r || T.isPlainObject(n) ? n : {}, r =
                                            !1, s[t] = T.extend(u, o, i))
                                        : void 0 !== i && (s[t] = i));
                    return s;
                }, T.extend({
                    expando: `jQuery${(`3.4.1${Math.random()}`).replace(/\D/g, "")}`,
                    isReady: !0,
                    error: function(e) { throw new Error(e) },
                    noop: function() {},
                    isPlainObject: function(e) {
                        var t, n;
                        return!(!e || "[object Object]" !== h.call(e)) &&
                        (!(t = a(e)) ||
                            "function" == typeof(n = p.call(t, "constructor") && t.constructor) && g.call(n) === m);
                    },
                    isEmptyObject: function(e) {
                        var t;
                        for (t in e) return!1;
                        return!0;
                    },
                    globalEval: function(e, t) { E(e, { nonce: t && t.nonce }) },
                    each: function(e, t) {
                        var n, i = 0;
                        if (x(e)) for (n = e.length; i < n && !1 !== t.call(e[i], i, e[i]); i++);
                        else for (i in e) if (!1 === t.call(e[i], i, e[i])) break;
                        return e;
                    },
                    trim: function(e) { return null == e ? "" : (e + "").replace(C, "") },
                    makeArray: function(e, t) {
                        const n = t || [];
                        return null != e &&
                            (x(Object(e)) ? T.merge(n, "string" == typeof e ? [e] : e) : c.call(n, e)), n;
                    },
                    inArray: function(e, t, n) { return null == t ? -1 : f.call(t, e, n) },
                    merge: function(e, t) {
                        for (var n = +t.length, i = 0, r = e.length; i < n; i++) e[r++] = t[i];
                        return e.length = r, e;
                    },
                    grep: function(e, t, n) {
                        for (var i = [], r = 0, o = e.length, s = !n; r < o; r++) !t(e[r], r) !== s && i.push(e[r]);
                        return i;
                    },
                    map: function(e, t, n) {
                        var i, r, o = 0;
                        const s = [];
                        if (x(e)) for (i = e.length; o < i; o++) null != (r = t(e[o], o, n)) && s.push(r);
                        else for (o in e) null != (r = t(e[o], o, n)) && s.push(r);
                        return u.apply([], s);
                    },
                    guid: 1,
                    support: v
                }), "function" == typeof Symbol && (T.fn[Symbol.iterator] = o[Symbol.iterator]), T.each(
                    "Boolean Number String Function Array Date RegExp Object Error Symbol".split(" "),
                    (function(e, t) { d[`[object ${t}]`] = t.toLowerCase() }));
                var S =
/*!
 * Sizzle CSS Selector Engine v2.3.4
 * https://sizzlejs.com/
 *
 * Copyright JS Foundation and other contributors
 * Released under the MIT license
 * https://js.foundation/
 *
 * Date: 2019-04-08
 */
                    function(e) {
                        var t,
                            n,
                            i,
                            r,
                            o,
                            s,
                            a,
                            l,
                            u,
                            c,
                            f,
                            d,
                            h,
                            p,
                            g,
                            m,
                            v,
                            y,
                            b,
                            _ = `sizzle${1 * new Date}`,
                            E = e.document,
                            w = 0,
                            T = 0,
                            C = le(),
                            x = le(),
                            S = le(),
                            D = le(),
                            A = function(e, t) { return e === t && (f = !0), 0 },
                            N = {}.hasOwnProperty,
                            I = [],
                            O = I.pop,
                            k = I.push,
                            L = I.push,
                            j = I.slice,
                            P = function(e, t) {
                                for (var n = 0, i = e.length; n < i; n++) if (e[n] === t) return n;
                                return-1;
                            },
                            H =
                                "checked|selected|async|autofocus|autoplay|controls|defer|disabled|hidden|ismap|loop|multiple|open|readonly|required|scoped",
                            R = "[\\x20\\t\\r\\n\\f]",
                            M = "(?:\\\\.|[\\w-]|[^\0-\\xa0])+",
                            q = `\\[${R}*(${M})(?:${R}*([*^$|!~]?=)${R
                                }*(?:'((?:\\\\.|[^\\\\'])*)'|"((?:\\\\.|[^\\\\"])*)"|(${M}))|)${R}*\\]`,
                            F = `:(${M
                                })(?:\\((('((?:\\\\.|[^\\\\'])*)'|"((?:\\\\.|[^\\\\"])*)")|((?:\\\\.|[^\\\\()[\\]]|${q
                                })*)|.*)\\)|)`,
                            W = new RegExp(R + "+", "g"),
                            B = new RegExp(`^${R}+|((?:^|[^\\\\])(?:\\\\.)*)${R}+$`, "g"),
                            U = new RegExp(`^${R}*,${R}*`),
                            V = new RegExp(`^${R}*([>+~]|${R})${R}*`),
                            K = new RegExp(R + "|>"),
                            $ = new RegExp(F),
                            Q = new RegExp(`^${M}$`),
                            Y = {
                                ID: new RegExp(`^#(${M})`),
                                CLASS: new RegExp(`^\\.(${M})`),
                                TAG: new RegExp(`^(${M}|[*])`),
                                ATTR: new RegExp(`^${q}`),
                                PSEUDO: new RegExp(`^${F}`),
                                CHILD: new RegExp(
                                    `^:(only|first|last|nth|nth-last)-(child|of-type)(?:\\(${R
                                    }*(even|odd|(([+-]|)(\\d*)n|)${R}*(?:([+-]|)${R}*(\\d+)|))${R
                                    }*\\)|)`,
                                    "i"),
                                bool: new RegExp(`^(?:${H})$`, "i"),
                                needsContext: new RegExp(
                                    `^${R}*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:\\(${R}*((?:-\\d)?\\d*)${R
                                    }*\\)|)(?=[^-]|$)`,
                                    "i")
                            },
                            z = /HTML$/i,
                            X = /^(?:input|select|textarea|button)$/i,
                            G = /^h\d$/i,
                            J = /^[^{]+\{\s*\[native \w/,
                            Z = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/,
                            ee = /[+~]/,
                            te = new RegExp(`\\\\([\\da-f]{1,6}${R}?|(${R})|.)`, "ig"),
                            ne = function(e, t, n) {
                                const i = "0x" + t - 65536;
                                return i != i || n
                                    ? t
                                    : i < 0
                                    ? String.fromCharCode(i + 65536)
                                    : String.fromCharCode(i >> 10 | 55296, 1023 & i | 56320);
                            },
                            ie = /([\0-\x1f\x7f]|^-?\d)|^-$|[^\0-\x1f\x7f-\uFFFF\w-]/g,
                            re = function(e, t) {
                                return t
                                    ? "\0" === e
                                    ? "�"
                                    : e.slice(0, -1) + "\\" + e.charCodeAt(e.length - 1).toString(16) + " "
                                    : `\\${e}`;
                            },
                            oe = function() { d() },
                            se = _e((function(e) {
                                    return!0 === e.disabled && "fieldset" === e.nodeName.toLowerCase();
                                }),
                                { dir: "parentNode", next: "legend" });
                        try {
                            L.apply(I = j.call(E.childNodes), E.childNodes), I[E.childNodes.length].nodeType;
                        } catch (e) {
                            L = {
                                apply: I.length
                                    ? function(e, t) { k.apply(e, j.call(t)) }
                                    : function(e, t) {
                                        for (var n = e.length, i = 0; e[n++] = t[i++];);
                                        e.length = n - 1;
                                    }
                            };
                        }

                        function ae(e, t, i, r) {
                            var o, a, u, c, f, p, v, y = t && t.ownerDocument, w = t ? t.nodeType : 9;
                            if (i = i || [], "string" != typeof e || !e || 1 !== w && 9 !== w && 11 !== w) return i;
                            if (!r && ((t ? t.ownerDocument || t : E) !== h && d(t), t = t || h, g)) {
                                if (11 !== w && (f = Z.exec(e)))
                                    if (o = f[1]) {
                                        if (9 === w) {
                                            if (!(u = t.getElementById(o))) return i;
                                            if (u.id === o) return i.push(u), i;
                                        } else if (y && (u = y.getElementById(o)) && b(t, u) && u.id === o)
                                            return i.push(u), i;
                                    } else {
                                        if (f[2]) return L.apply(i, t.getElementsByTagName(e)), i;
                                        if ((o = f[3]) && n.getElementsByClassName && t.getElementsByClassName)
                                            return L.apply(i, t.getElementsByClassName(o)), i;
                                    }
                                if (n.qsa &&
                                    !D[e + " "] &&
                                    (!m || !m.test(e)) &&
                                    (1 !== w || "object" !== t.nodeName.toLowerCase())) {
                                    if (v = e, y = t, 1 === w && K.test(e)) {
                                        for ((c = t.getAttribute("id"))
                                                ? c = c.replace(ie, re)
                                                : t.setAttribute("id", c = _), a = (p = s(e)).length;
                                            a--;
                                        ) p[a] = `#${c} ${be(p[a])}`;
                                        v = p.join(","), y = ee.test(e) && ve(t.parentNode) || t;
                                    }
                                    try {
                                        return L.apply(i, y.querySelectorAll(v)), i;
                                    } catch (t) {
                                        D(e, !0);
                                    } finally {
                                        c === _ && t.removeAttribute("id");
                                    }
                                }
                            }
                            return l(e.replace(B, "$1"), t, i, r);
                        }

                        function le() {
                            var e = [];
                            return function t(n, r) {
                                return e.push(n + " ") > i.cacheLength && delete t[e.shift()], t[n + " "] = r;
                            };
                        }

                        function ue(e) { return e[_] = !0, e }

                        function ce(e) {
                            var t = h.createElement("fieldset");
                            try {
                                return!!e(t);
                            } catch (e) {
                                return!1;
                            } finally {
                                t.parentNode && t.parentNode.removeChild(t), t = null;
                            }
                        }

                        function fe(e, t) { for (var n = e.split("|"), r = n.length; r--;) i.attrHandle[n[r]] = t }

                        function de(e, t) {
                            var n = t && e;
                            const i = n && 1 === e.nodeType && 1 === t.nodeType && e.sourceIndex - t.sourceIndex;
                            if (i) return i;
                            if (n) for (; n = n.nextSibling;) if (n === t) return-1;
                            return e ? 1 : -1;
                        }

                        function he(e) {
                            return function(t) { return"input" === t.nodeName.toLowerCase() && t.type === e };
                        }

                        function pe(e) {
                            return function(t) {
                                const n = t.nodeName.toLowerCase();
                                return("input" === n || "button" === n) && t.type === e;
                            };
                        }

                        function ge(e) {
                            return function(t) {
                                return"form" in t
                                    ? t.parentNode && !1 === t.disabled
                                    ? "label" in t
                                    ? "label" in t.parentNode
                                    ? t.parentNode.disabled === e
                                    : t.disabled === e
                                    : t.isDisabled === e || t.isDisabled !== !e && se(t) === e
                                    : t.disabled === e
                                    : "label" in t && t.disabled === e;
                            };
                        }

                        function me(e) {
                            return ue((function(t) {
                                return t = +t, ue((function(n, i) {
                                    for (var r, o = e([], n.length, t), s = o.length; s--;)
                                        n[r = o[s]] && (n[r] = !(i[r] = n[r]));
                                }));
                            }));
                        }

                        function ve(e) { return e && void 0 !== e.getElementsByTagName && e }

                        for (t in n = ae.support = {}, o = ae.isXML = function(e) {
                            const t = e.namespaceURI;
                            const n = (e.ownerDocument || e).documentElement;
                            return!z.test(t || n && n.nodeName || "HTML");
                        }, d = ae.setDocument = function(e) {
                            var t, r;
                            const s = e ? e.ownerDocument || e : E;
                            return s !== h && 9 === s.nodeType && s.documentElement
                                ? (p = (h = s).documentElement, g =
                                        !o(h), E !== h &&
                                        (r = h.defaultView) &&
                                        r.top !== r &&
                                        (r.addEventListener
                                            ? r.addEventListener("unload", oe, !1)
                                            : r.attachEvent && r.attachEvent("onunload", oe)), n.attributes =
                                        ce((function(e) { return e.className = "i", !e.getAttribute("className") })),
                                    n.getElementsByTagName =
                                        ce((function(e) {
                                            return e.appendChild(h.createComment("")), !e.getElementsByTagName("*")
                                                .length;
                                        })), n.getElementsByClassName = J.test(h.getElementsByClassName), n.getById =
                                        ce((function(e) {
                                            return p.appendChild(e).id =
                                                _, !h.getElementsByName || !h.getElementsByName(_).length;
                                        })), n.getById
                                        ? (i.filter.ID = function(e) {
                                            var t = e.replace(te, ne);
                                            return function(e) { return e.getAttribute("id") === t };
                                        }, i.find.ID = function(e, t) {
                                            if (void 0 !== t.getElementById && g) {
                                                const n = t.getElementById(e);
                                                return n ? [n] : [];
                                            }
                                        })
                                        : (i.filter.ID = function(e) {
                                            var t = e.replace(te, ne);
                                            return function(e) {
                                                const n = void 0 !== e.getAttributeNode && e.getAttributeNode("id");
                                                return n && n.value === t;
                                            };
                                        }, i.find.ID = function(e, t) {
                                            if (void 0 !== t.getElementById && g) {
                                                let o = t.getElementById(e);
                                                if (o) {
                                                    let n;
                                                    if ((n = o.getAttributeNode("id")) && n.value === e) return[o];
                                                    let i;
                                                    let r;
                                                    for (r = t.getElementsByName(e), i = 0; o = r[i++];)
                                                        if ((n = o.getAttributeNode("id")) && n.value === e) return[o];
                                                }
                                                return[];
                                            }
                                        }), i.find.TAG = n.getElementsByTagName
                                        ? function(e, t) {
                                            return void 0 !== t.getElementsByTagName
                                                ? t.getElementsByTagName(e)
                                                : n.qsa
                                                ? t.querySelectorAll(e)
                                                : void 0;
                                        }
                                        : function(e, t) {
                                            var n;
                                            var r = 0;
                                            const o = t.getElementsByTagName(e);
                                            if ("*" === e) {
                                                const i = [];
                                                for (; n = o[r++];) 1 === n.nodeType && i.push(n);
                                                return i;
                                            }
                                            return o;
                                        }, i.find.CLASS =
                                        n.getElementsByClassName &&
                                        function(e, t) {
                                            if (void 0 !== t.getElementsByClassName && g)
                                                return t.getElementsByClassName(e);
                                        }, v = [], m = [], (n.qsa = J.test(h.querySelectorAll)) &&
                                    (ce((function(e) {
                                        p.appendChild(e).innerHTML =
                                                `<a id='${_}'></a><select id='${_
                                                }-\r\\' msallowcapture=''><option selected=''></option></select>`,
                                            e.querySelectorAll("[msallowcapture^='']").length &&
                                                m.push(`[*^$]=${R}*(?:''|"")`),
                                            e.querySelectorAll("[selected]").length || m.push(`\\[${R}*(?:value|${H})`),
                                            e.querySelectorAll(`[id~=${_}-]`).length || m.push("~="),
                                            e.querySelectorAll(":checked").length || m.push(":checked"), e
                                                .querySelectorAll(`a#${_}+*`).length ||
                                                m.push(".#.+[+~]");
                                    })), ce((function(e) {
                                        e.innerHTML =
                                            "<a href='' disabled='disabled'></a><select disabled='disabled'><option/></select>";
                                        const t = h.createElement("input");
                                        t.setAttribute("type", "hidden"), e.appendChild(t).setAttribute("name", "D"),
                                            e.querySelectorAll("[name=d]").length && m.push(`name${R}*[*^$|!~]?=`),
                                            2 !== e.querySelectorAll(":enabled").length &&
                                                m.push(":enabled", ":disabled"),
                                            p.appendChild(e).disabled =
                                                !0, 2 !== e.querySelectorAll(":disabled").length &&
                                                m.push(":enabled", ":disabled"), e.querySelectorAll("*,:x"), m.push(
                                                ",.*:");
                                    }))), (n.matchesSelector =
                                            J.test(y = p.matches ||
                                                p.webkitMatchesSelector ||
                                                p.mozMatchesSelector ||
                                                p.oMatchesSelector ||
                                                p.msMatchesSelector)) &&
                                        ce((function(e) {
                                            n.disconnectedMatch =
                                                y.call(e, "*"), y.call(e, "[s!='']:x"), v.push("!=", F);
                                        })), m = m.length && new RegExp(m.join("|")), v =
                                        v.length && new RegExp(v.join("|")), t = J.test(p.compareDocumentPosition), b =
                                        t || J.test(p.contains)
                                        ? function(e, t) {
                                            const n = 9 === e.nodeType ? e.documentElement : e;
                                            const i = t && t.parentNode;
                                            return e === i ||
                                                !(!i ||
                                                    1 !== i.nodeType ||
                                                    !(n.contains
                                                        ? n.contains(i)
                                                        : e.compareDocumentPosition &&
                                                        16 & e.compareDocumentPosition(i)));
                                        }
                                        : function(e, t) {
                                            if (t) for (; t = t.parentNode;) if (t === e) return!0;
                                            return!1;
                                        }, A = t
                                        ? function(e, t) {
                                            if (e === t) return f = !0, 0;
                                            var i = !e.compareDocumentPosition - !t.compareDocumentPosition;
                                            return i ||
                                            (1 &
                                                (i = (e.ownerDocument || e) === (t.ownerDocument || t)
                                                    ? e.compareDocumentPosition(t)
                                                    : 1) ||
                                                !n.sortDetached && t.compareDocumentPosition(e) === i
                                                ? e === h || e.ownerDocument === E && b(E, e)
                                                ? -1
                                                : t === h || t.ownerDocument === E && b(E, t)
                                                ? 1
                                                : c
                                                ? P(c, e) - P(c, t)
                                                : 0
                                                : 4 & i
                                                ? -1
                                                : 1);
                                        }
                                        : function(e, t) {
                                            if (e === t) return f = !0, 0;
                                            var n, i = 0;
                                            const r = e.parentNode;
                                            const o = t.parentNode;
                                            const s = [e];
                                            const a = [t];
                                            if (!r || !o)
                                                return e === h
                                                    ? -1
                                                    : t === h
                                                    ? 1
                                                    : r
                                                    ? -1
                                                    : o
                                                    ? 1
                                                    : c
                                                    ? P(c, e) - P(c, t)
                                                    : 0;
                                            if (r === o) return de(e, t);
                                            for (n = e; n = n.parentNode;) s.unshift(n);
                                            for (n = t; n = n.parentNode;) a.unshift(n);
                                            for (; s[i] === a[i];) i++;
                                            return i ? de(s[i], a[i]) : s[i] === E ? -1 : a[i] === E ? 1 : 0;
                                        }, h)
                                : h;
                        }, ae.matches = function(e, t) { return ae(e, null, null, t) }, ae.matchesSelector =
                            function(e, t) {
                                if ((e.ownerDocument || e) !== h && d(e), n.matchesSelector &&
                                    g &&
                                    !D[t + " "] &&
                                    (!v || !v.test(t)) &&
                                    (!m || !m.test(t)))
                                    try {
                                        const i = y.call(e, t);
                                        if (i || n.disconnectedMatch || e.document && 11 !== e.document.nodeType)
                                            return i;
                                    } catch (e) {
                                        D(t, !0);
                                    }
                                return ae(t, h, null, [e]).length > 0;
                            }, ae.contains =
                            function(e, t) { return(e.ownerDocument || e) !== h && d(e), b(e, t) }, ae.attr =
                            function(e, t) {
                                (e.ownerDocument || e) !== h && d(e);
                                const r = i.attrHandle[t.toLowerCase()];
                                var o = r && N.call(i.attrHandle, t.toLowerCase()) ? r(e, t, !g) : void 0;
                                return void 0 !== o
                                    ? o
                                    : n.attributes || !g
                                    ? e.getAttribute(t)
                                    : (o = e.getAttributeNode(t)) && o.specified
                                    ? o.value
                                    : null;
                            }, ae.escape = function(e) { return(e + "").replace(ie, re) }, ae.error = function(e) {
                            throw new Error(`Syntax error, unrecognized expression: ${e}`);
                        }, ae.uniqueSort = function(e) {
                            var t;
                            var r = 0,
                                o = 0;
                            if (f = !n.detectDuplicates, c = !n.sortStable && e.slice(0), e.sort(A), f) {
                                const i = [];
                                for (; t = e[o++];) t === e[o] && (r = i.push(o));
                                for (; r--;) e.splice(i[r], 1);
                            }
                            return c = null, e;
                        }, r = ae.getText = function(e) {
                            var t, n = "", i = 0;
                            const o = e.nodeType;
                            if (o) {
                                if (1 === o || 9 === o || 11 === o) {
                                    if ("string" == typeof e.textContent) return e.textContent;
                                    for (e = e.firstChild; e; e = e.nextSibling) n += r(e);
                                } else if (3 === o || 4 === o) return e.nodeValue;
                            } else for (; t = e[i++];) n += r(t);
                            return n;
                        }, (i = ae.selectors = {
                                cacheLength: 50,
                                createPseudo: ue,
                                match: Y,
                                attrHandle: {},
                                find: {},
                                relative:
                                {
                                    ">": { dir: "parentNode", first: !0 },
                                    " ": { dir: "parentNode" },
                                    "+": { dir: "previousSibling", first: !0 },
                                    "~": { dir: "previousSibling" }
                                },
                                preFilter: {
                                    ATTR: function(e) {
                                        return e[1] = e[1].replace(te, ne), e[3] =
                                            (e[3] || e[4] || e[5] || "").replace(te, ne), "~=" === e[2] &&
                                            (e[3] = ` ${e[3]} `), e.slice(0, 4);
                                    },
                                    CHILD: function(e) {
                                        return e[1] = e[1].toLowerCase(), "nth" === e[1].slice(0, 3)
                                            ? (e[3] || ae.error(e[0]), e[4] =
                                                +(e[4]
                                                    ? e[5] + (e[6] || 1)
                                                    : 2 * ("even" === e[3] || "odd" === e[3])), e[5
                                            ] = +(e[7] + e[8] || "odd" === e[3]))
                                            : e[3] && ae.error(e[0]), e;
                                    },
                                    PSEUDO: function(e) {
                                        var t;
                                        const n = !e[6] && e[2];
                                        return Y.CHILD.test(e[0])
                                            ? null
                                            : (e[3]
                                                ? e[2] = e[4] || e[5] || ""
                                                : n &&
                                                $.test(n) &&
                                                (t = s(n, !0)) &&
                                                (t = n.indexOf(")", n.length - t) - n.length) &&
                                                (e[0] = e[0].slice(0, t), e[2] = n.slice(0, t)), e.slice(0, 3));
                                    }
                                },
                                filter: {
                                    TAG: function(e) {
                                        var t = e.replace(te, ne).toLowerCase();
                                        return"*" === e
                                            ? function() { return!0 }
                                            : function(e) { return e.nodeName && e.nodeName.toLowerCase() === t };
                                    },
                                    CLASS: function(e) {
                                        var t = C[e + " "];
                                        return t ||
                                            (t = new RegExp(`(^|${R})${e}(${R}|$)`)) &&
                                            C(e,
                                                (function(e) {
                                                    return t.test("string" == typeof e.className && e.className ||
                                                        void 0 !== e.getAttribute && e.getAttribute("class") ||
                                                        "");
                                                }));
                                    },
                                    ATTR: function(e, t, n) {
                                        return function(i) {
                                            var r = ae.attr(i, e);
                                            return null == r
                                                ? "!=" === t
                                                : !t ||
                                                (r += "", "=" === t
                                                    ? r === n
                                                    : "!=" === t
                                                    ? r !== n
                                                    : "^=" === t
                                                    ? n && 0 === r.indexOf(n)
                                                    : "*=" === t
                                                    ? n && r.indexOf(n) > -1
                                                    : "$=" === t
                                                    ? n && r.slice(-n.length) === n
                                                    : "~=" === t
                                                    ? (` ${r.replace(W, " ")} `).indexOf(n) > -1
                                                    : "|=" === t && (r === n || r.slice(0, n.length + 1) === n + "-"));
                                        };
                                    },
                                    CHILD: function(e, t, n, i, r) {
                                        var o = "nth" !== e.slice(0, 3),
                                            s = "last" !== e.slice(-4),
                                            a = "of-type" === t;
                                        return 1 === i && 0 === r
                                            ? function(e) { return!!e.parentNode }
                                            : function(t, n, l) {
                                                var u,
                                                    c,
                                                    f,
                                                    d,
                                                    h,
                                                    p,
                                                    g = o !== s ? "nextSibling" : "previousSibling",
                                                    m = t.parentNode,
                                                    v = a && t.nodeName.toLowerCase(),
                                                    y = !l && !a,
                                                    b = !1;
                                                if (m) {
                                                    if (o) {
                                                        for (; g;) {
                                                            for (d = t; d = d[g];)
                                                                if (a
                                                                    ? d.nodeName.toLowerCase() === v
                                                                    : 1 === d.nodeType)
                                                                    return!1;
                                                            p = g = "only" === e && !p && "nextSibling";
                                                        }
                                                        return!0;
                                                    }
                                                    if (p = [s ? m.firstChild : m.lastChild], s && y) {
                                                        for (b = (h =
                                                                    (u = (c = (f =
                                                                                (d = m)[_] || (d[_] = {}))[d.uniqueID
                                                                            ] ||
                                                                            (f[d.uniqueID] = {}))[e] ||
                                                                        [])[0] ===
                                                                    w &&
                                                                    u[1]) &&
                                                                u[2], d = h && m.childNodes[h];
                                                            d = ++h && d && d[g] || (b = h = 0) || p.pop();
                                                        )
                                                            if (1 === d.nodeType && ++b && d === t) {
                                                                c[e] = [w, h, b];
                                                                break;
                                                            }
                                                    } else if (y &&
                                                    (b = h = (u =
                                                            (c = (f = (d = t)[_] || (d[_] = {}))[d.uniqueID] ||
                                                                (f[d.uniqueID] = {}))[e] ||
                                                            [])[0] ===
                                                        w &&
                                                        u[1]), !1 === b)
                                                        for (;
                                                            (d = ++h && d && d[g] || (b = h = 0) || p.pop()) &&
                                                            ((a ? d.nodeName.toLowerCase() !== v : 1 !== d.nodeType) ||
                                                                !++b ||
                                                                (y &&
                                                                ((c = (f = d[_] || (d[_] = {}))[d.uniqueID] ||
                                                                    (f[d.uniqueID] = {}))[e] = [w, b]), d !== t));
                                                        );
                                                    return(b -= r) === i || b % i == 0 && b / i >= 0;
                                                }
                                            };
                                    },
                                    PSEUDO: function(e, t) {
                                        var n,
                                            r = i.pseudos[e] ||
                                                i.setFilters[e.toLowerCase()] ||
                                                ae.error(`unsupported pseudo: ${e}`);
                                        return r[_]
                                            ? r(t)
                                            : r.length > 1
                                            ? (n = [e, e, "", t], i.setFilters.hasOwnProperty(e.toLowerCase())
                                                ? ue((function(e, n) {
                                                    for (var i, o = r(e, t), s = o.length; s--;)
                                                        e[i = P(e, o[s])] = !(n[i] = o[s]);
                                                }))
                                                : function(e) { return r(e, 0, n) })
                                            : r;
                                    }
                                },
                                pseudos: {
                                    not: ue((function(e) {
                                        var t = [], n = [], i = a(e.replace(B, "$1"));
                                        return i[_]
                                            ? ue((function(e, t, n, r) {
                                                for (var o, s = i(e, null, r, []), a = e.length; a--;)
                                                    (o = s[a]) && (e[a] = !(t[a] = o));
                                            }))
                                            : function(e, r, o) {
                                                return t[0] = e, i(t, null, o, n), t[0] = null, !n.pop();
                                            };
                                    })),
                                    has: ue((function(e) { return function(t) { return ae(e, t).length > 0 } })),
                                    contains: ue((function(e) {
                                        return e = e.replace(te, ne), function(t) {
                                            return(t.textContent || r(t)).indexOf(e) > -1;
                                        };
                                    })),
                                    lang: ue((function(e) {
                                        return Q.test(e || "") || ae.error(`unsupported lang: ${e}`), e =
                                            e.replace(te, ne).toLowerCase(), function(t) {
                                            var n;
                                            do {
                                                if (n = g
                                                    ? t.lang
                                                    : t.getAttribute("xml:lang") || t.getAttribute("lang"))
                                                    return(n = n.toLowerCase()) === e || 0 === n.indexOf(e + "-");
                                            } while ((t = t.parentNode) && 1 === t.nodeType);
                                            return!1;
                                        };
                                    })),
                                    target: function(t) {
                                        const n = e.location && e.location.hash;
                                        return n && n.slice(1) === t.id;
                                    },
                                    root: function(e) { return e === p },
                                    focus: function(e) {
                                        return e === h.activeElement &&
                                            (!h.hasFocus || h.hasFocus()) &&
                                            !!(e.type || e.href || ~e.tabIndex);
                                    },
                                    enabled: ge(!1),
                                    disabled: ge(!0),
                                    checked: function(e) {
                                        const t = e.nodeName.toLowerCase();
                                        return"input" === t && !!e.checked || "option" === t && !!e.selected;
                                    },
                                    selected: function(e) {
                                        return e.parentNode && e.parentNode.selectedIndex, !0 === e.selected;
                                    },
                                    empty: function(e) {
                                        for (e = e.firstChild; e; e = e.nextSibling) if (e.nodeType < 6) return!1;
                                        return!0;
                                    },
                                    parent: function(e) { return!i.pseudos.empty(e) },
                                    header: function(e) { return G.test(e.nodeName) },
                                    input: function(e) { return X.test(e.nodeName) },
                                    button: function(e) {
                                        const t = e.nodeName.toLowerCase();
                                        return"input" === t && "button" === e.type || "button" === t;
                                    },
                                    text: function(e) {
                                        var t;
                                        return"input" === e.nodeName.toLowerCase() &&
                                            "text" === e.type &&
                                            (null == (t = e.getAttribute("type")) || "text" === t.toLowerCase());
                                    },
                                    first: me((function() { return[0] })),
                                    last: me((function(e, t) { return[t - 1] })),
                                    eq: me((function(e, t, n) { return[n < 0 ? n + t : n] })),
                                    even: me((function(e, t) {
                                        for (let n = 0; n < t; n += 2) e.push(n);
                                        return e;
                                    })),
                                    odd: me((function(e, t) {
                                        for (let n = 1; n < t; n += 2) e.push(n);
                                        return e;
                                    })),
                                    lt: me((function(e, t, n) {
                                        for (let i = n < 0 ? n + t : n > t ? t : n; --i >= 0;) e.push(i);
                                        return e;
                                    })),
                                    gt: me((function(e, t, n) {
                                        for (let i = n < 0 ? n + t : n; ++i < t;) e.push(i);
                                        return e;
                                    }))
                                }
                            }).pseudos.nth =
                            i.pseudos.eq, { radio: !0, checkbox: !0, file: !0, password: !0, image: !0 })
                            i.pseudos[t] = he(t);
                        for (t in{ submit: !0, reset: !0 }) i.pseudos[t] = pe(t);

                        function ye() {}

                        function be(e) {
                            for (var t = 0, n = e.length, i = ""; t < n; t++) i += e[t].value;
                            return i;
                        }

                        function _e(e, t, n) {
                            var i = t.dir, r = t.next, o = r || i, s = n && "parentNode" === o, a = T++;
                            return t.first
                                ? function(t, n, r) {
                                    for (; t = t[i];) if (1 === t.nodeType || s) return e(t, n, r);
                                    return!1;
                                }
                                : function(t, n, l) {
                                    var u, c, f;
                                    const d = [w, a];
                                    if (l) {
                                        for (; t = t[i];) if ((1 === t.nodeType || s) && e(t, n, l)) return!0;
                                    } else
                                        for (; t = t[i];)
                                            if (1 === t.nodeType || s)
                                                if (c =
                                                    (f = t[_] || (t[_] = {}))[t.uniqueID] || (f[t.uniqueID] = {}), r &&
                                                    r === t.nodeName.toLowerCase()) t = t[i] || t;
                                                else {
                                                    if ((u = c[o]) && u[0] === w && u[1] === a) return d[2] = u[2];
                                                    if (c[o] = d, d[2] = e(t, n, l)) return!0;
                                                }
                                    return!1;
                                };
                        }

                        function Ee(e) {
                            return e.length > 1
                                ? function(t, n, i) {
                                    for (let r = e.length; r--;) if (!e[r](t, n, i)) return!1;
                                    return!0;
                                }
                                : e[0];
                        }

                        function we(e, t, n, i, r) {
                            for (var o, s = [], a = 0, l = e.length, u = null != t; a < l; a++)
                                (o = e[a]) && (n && !n(o, i, r) || (s.push(o), u && t.push(a)));
                            return s;
                        }

                        function Te(e, t, n, i, r, o) {
                            return i && !i[_] && (i = Te(i)), r && !r[_] && (r = Te(r, o)), ue((function(o, s, a, l) {
                                var u, c, f;
                                const d = [];
                                const h = [];
                                const p = s.length;
                                const g = o ||
                                    function(e, t, n) {
                                        for (var i = 0, r = t.length; i < r; i++) ae(e, t[i], n);
                                        return n;
                                    }(t || "*", a.nodeType ? [a] : a, []);
                                const m = !e || !o && t ? g : we(g, d, e, a, l);
                                var v = n ? r || (o ? e : p || i) ? [] : s : m;
                                if (n && n(m, v, a, l), i)
                                    for (u = we(v, h), i(u, [], a, l), c = u.length; c--;)
                                        (f = u[c]) && (v[h[c]] = !(m[h[c]] = f));
                                if (o) {
                                    if (r || e) {
                                        if (r) {
                                            for (u = [], c = v.length; c--;) (f = v[c]) && u.push(m[c] = f);
                                            r(null, v = [], u, l);
                                        }
                                        for (c = v.length; c--;)
                                            (f = v[c]) && (u = r ? P(o, f) : d[c]) > -1 && (o[u] = !(s[u] = f));
                                    }
                                } else
                                    v = we(v === s ? v.splice(p, v.length) : v), r ? r(null, s, v, l) : L.apply(s, v);
                            }));
                        }

                        function Ce(e) {
                            for (var t,
                                n,
                                r,
                                o = e.length,
                                s = i.relative[e[0].type],
                                a = s || i.relative[" "],
                                l = s ? 1 : 0,
                                c = _e((function(e) { return e === t }), a, !0),
                                f = _e((function(e) { return P(t, e) > -1 }), a, !0),
                                d = [
                                    function(e, n, i) {
                                        const r = !s && (i || n !== u) || ((t = n).nodeType ? c(e, n, i) : f(e, n, i));
                                        return t = null, r;
                                    }
                                ];
                                l < o;
                                l++)
                                if (n = i.relative[e[l].type]) d = [_e(Ee(d), n)];
                                else {
                                    if ((n = i.filter[e[l].type].apply(null, e[l].matches))[_]) {
                                        for (r = ++l; r < o && !i.relative[e[r].type]; r++);
                                        return Te(l > 1 && Ee(d),
                                            l > 1 &&
                                            be(e.slice(0, l - 1).concat({ value: " " === e[l - 2].type ? "*" : "" }))
                                            .replace(B, "$1"),
                                            n,
                                            l < r && Ce(e.slice(l, r)),
                                            r < o && Ce(e = e.slice(r)),
                                            r < o && be(e));
                                    }
                                    d.push(n);
                                }
                            return Ee(d);
                        }

                        return ye.prototype = i.filters = i.pseudos, i.setFilters = new ye, s = ae.tokenize =
                                function(e, t) {
                                    var n, r, o, s, a, l, u;
                                    const c = x[e + " "];
                                    if (c) return t ? 0 : c.slice(0);
                                    for (a = e, l = [], u = i.preFilter; a;) {
                                        for (s in n && !(r = U.exec(a)) ||
                                            (r && (a = a.slice(r[0].length) || a), l.push(o = [])), n =
                                            !1, (r = V.exec(a)) &&
                                        (n = r.shift(), o.push({ value: n, type: r[0].replace(B, " ") }), a =
                                            a.slice(n.length)), i.filter)
                                            !(r = Y[s].exec(a)) ||
                                                u[s] && !(r = u[s](r)) ||
                                                (n = r.shift(), o.push({ value: n, type: s, matches: r }), a =
                                                    a.slice(n.length));
                                        if (!n) break;
                                    }
                                    return t ? a.length : a ? ae.error(e) : x(e, l).slice(0);
                                }, a = ae.compile = function(e, t) {
                                var n;
                                var a = S[e + " "];
                                if (!a) {
                                    const r = [];
                                    const o = [];
                                    for (t || (t = s(e)), n = t.length; n--;) (a = Ce(t[n]))[_] ? r.push(a) : o.push(a);
                                    (a = S(e,
                                        function(e, t) {
                                            var n = t.length > 0, r = e.length > 0;
                                            const o = function(o, s, a, l, c) {
                                                var f, p, m, v = 0, y = "0";
                                                const b = o && [];
                                                var _ = [];
                                                const E = u;
                                                const T = o || r && i.find.TAG("*", c);
                                                const C = w += null == E ? 1 : Math.random() || .1;
                                                const x = T.length;
                                                for (c && (u = s === h || s || c); y !== x && null != (f = T[y]); y++) {
                                                    if (r && f) {
                                                        for (p = 0, s || f.ownerDocument === h || (d(f), a = !g);
                                                            m = e[p++];
                                                        )
                                                            if (m(f, s || h, a)) {
                                                                l.push(f);
                                                                break;
                                                            }
                                                        c && (w = C);
                                                    }
                                                    n && ((f = !m && f) && v--, o && b.push(f));
                                                }
                                                if (v += y, n && y !== v) {
                                                    for (p = 0; m = t[p++];) m(b, _, s, a);
                                                    if (o) {
                                                        if (v > 0) for (; y--;) b[y] || _[y] || (_[y] = O.call(l));
                                                        _ = we(_);
                                                    }
                                                    L.apply(l, _), c &&
                                                        !o &&
                                                        _.length > 0 &&
                                                        v + t.length > 1 &&
                                                        ae.uniqueSort(l);
                                                }
                                                return c && (w = C, u = E), b;
                                            };
                                            return n ? ue(o) : o;
                                        }(o, r))).selector = e;
                                }
                                return a;
                            }, l = ae.select = function(e, t, n, r) {
                                var o, l, u, c, f;
                                const d = "function" == typeof e && e;
                                const h = !r && s(e = d.selector || e);
                                if (n = n || [], 1 === h.length) {
                                    if ((l = h[0] = h[0].slice(0)).length > 2 &&
                                        "ID" === (u = l[0]).type &&
                                        9 === t.nodeType &&
                                        g &&
                                        i.relative[l[1].type]) {
                                        if (!(t = (i.find.ID(u.matches[0].replace(te, ne), t) || [])[0])) return n;
                                        d && (t = t.parentNode), e = e.slice(l.shift().value.length);
                                    }
                                    for (o = Y.needsContext.test(e) ? 0 : l.length;
                                        o-- && (u = l[o], !i.relative[c = u.type]);
                                    )
                                        if ((f = i.find[c]) &&
                                        (r = f(u.matches[0].replace(te, ne),
                                            ee.test(l[0].type) && ve(t.parentNode) || t))) {
                                            if (l.splice(o, 1), !(e = r.length && be(l))) return L.apply(n, r), n;
                                            break;
                                        }
                                }
                                return(d || a(e, h))(r, t, !g, n, !t || ee.test(e) && ve(t.parentNode) || t), n;
                            }, n.sortStable = _.split("").sort(A).join("") === _, n.detectDuplicates =
                                !!f, d(), n.sortDetached =
                                ce((function(e) { return 1 & e.compareDocumentPosition(h.createElement("fieldset")) })),
                            ce((function(e) {
                                    return e.innerHTML = "<a href='#'></a>", "#" === e.firstChild.getAttribute("href");
                                })) ||
                                fe("type|href|height|width",
                                    (function(e, t, n) {
                                        if (!n) return e.getAttribute(t, "type" === t.toLowerCase() ? 1 : 2);
                                    })), n.attributes &&
                                ce((function(e) {
                                    return e.innerHTML =
                                        "<input/>", e.firstChild.setAttribute("value", ""), "" ===
                                        e.firstChild.getAttribute("value");
                                })) ||
                                fe("value",
                                    (function(e, t, n) {
                                        if (!n && "input" === e.nodeName.toLowerCase()) return e.defaultValue;
                                    })), ce((function(e) { return null == e.getAttribute("disabled") })) ||
                                fe(H,
                                    (function(e, t, n) {
                                        var i;
                                        if (!n)
                                            return!0 === e[t]
                                                ? t.toLowerCase()
                                                : (i = e.getAttributeNode(t)) && i.specified
                                                ? i.value
                                                : null;
                                    })), ae;
                    }(n);
                T.find = S, T.expr = S.selectors, T.expr[":"] = T.expr.pseudos, T.uniqueSort = T.unique =
                    S.uniqueSort, T.text = S.getText, T.isXMLDoc = S.isXML, T.contains = S.contains, T.escapeSelector =
                    S.escape;
                var D = function(e, t, n) {
                        for (var i = [], r = void 0 !== n; (e = e[t]) && 9 !== e.nodeType;)
                            if (1 === e.nodeType) {
                                if (r && T(e).is(n)) break;
                                i.push(e);
                            }
                        return i;
                    },
                    A = function(e, t) {
                        for (var n = []; e; e = e.nextSibling) 1 === e.nodeType && e !== t && n.push(e);
                        return n;
                    },
                    N = T.expr.match.needsContext;

                function I(e, t) { return e.nodeName && e.nodeName.toLowerCase() === t.toLowerCase() }

                var O = /^<([a-z][^\/\0>:\x20\t\r\n\f]*)[\x20\t\r\n\f]*\/?>(?:<\/\1>|)$/i;

                function k(e, t, n) {
                    return y(t)
                        ? T.grep(e, (function(e, i) { return!!t.call(e, i, e) !== n }))
                        : t.nodeType
                        ? T.grep(e, (function(e) { return e === t !== n }))
                        : "string" != typeof t
                        ? T.grep(e, (function(e) { return f.call(t, e) > -1 !== n }))
                        : T.filter(t, e, n);
                }

                T.filter = function(e, t, n) {
                    const i = t[0];
                    return n && (e = `:not(${e})`), 1 === t.length && 1 === i.nodeType
                        ? T.find.matchesSelector(i, e)
                        ? [i]
                        : []
                        : T.find.matches(e, T.grep(t, (function(e) { return 1 === e.nodeType })));
                }, T.fn.extend({
                    find: function(e) {
                        var t, n, i = this.length, r = this;
                        if ("string" != typeof e)
                            return this.pushStack(T(e).filter((function() {
                                for (t = 0; t < i; t++) if (T.contains(r[t], this)) return!0;
                            })));
                        for (n = this.pushStack([]), t = 0; t < i; t++) T.find(e, r[t], n);
                        return i > 1 ? T.uniqueSort(n) : n;
                    },
                    filter: function(e) { return this.pushStack(k(this, e || [], !1)) },
                    not: function(e) { return this.pushStack(k(this, e || [], !0)) },
                    is: function(e) { return!!k(this, "string" == typeof e && N.test(e) ? T(e) : e || [], !1).length }
                });
                var L, j = /^(?:\s*(<[\w\W]+>)[^>]*|#([\w-]+))$/;
                (T.fn.init = function(e, t, n) {
                    var i, r;
                    if (!e) return this;
                    if (n = n || L, "string" == typeof e) {
                        if (!(i =
                                "<" === e[0] && ">" === e[e.length - 1] && e.length >= 3
                                ? [null, e, null]
                                : j.exec(e)) ||
                            !i[1] && t) return!t || t.jquery ? (t || n).find(e) : this.constructor(t).find(e);
                        if (i[1]) {
                            if (t = t instanceof T ? t[0] : t, T.merge(this,
                                T.parseHTML(i[1], t && t.nodeType ? t.ownerDocument || t : s, !0)), O.test(i[1]) &&
                                T.isPlainObject(t)) for (i in t) y(this[i]) ? this[i](t[i]) : this.attr(i, t[i]);
                            return this;
                        }
                        return(r = s.getElementById(i[2])) && (this[0] = r, this.length = 1), this;
                    }
                    return e.nodeType
                        ? (this[0] = e, this.length = 1, this)
                        : y(e)
                        ? void 0 !== n.ready
                        ? n.ready(e)
                        : e(T)
                        : T.makeArray(e, this);
                }).prototype = T.fn, L = T(s);
                var P = /^(?:parents|prev(?:Until|All))/, H = { children: !0, contents: !0, next: !0, prev: !0 };

                function R(e, t) {
                    for (; (e = e[t]) && 1 !== e.nodeType;);
                    return e;
                }

                T.fn.extend({
                    has: function(e) {
                        var t = T(e, this), n = t.length;
                        return this.filter((function() {
                            for (let e = 0; e < n; e++) if (T.contains(this, t[e])) return!0;
                        }));
                    },
                    closest: function(e, t) {
                        var n, i = 0;
                        const r = this.length;
                        const o = [];
                        const s = "string" != typeof e && T(e);
                        if (!N.test(e))
                            for (; i < r; i++)
                                for (n = this[i]; n && n !== t; n = n.parentNode)
                                    if (n.nodeType < 11 &&
                                        (s ? s.index(n) > -1 : 1 === n.nodeType && T.find.matchesSelector(n, e))) {
                                        o.push(n);
                                        break;
                                    }
                        return this.pushStack(o.length > 1 ? T.uniqueSort(o) : o);
                    },
                    index: function(e) {
                        return e
                            ? "string" == typeof e
                            ? f.call(T(e), this[0])
                            : f.call(this, e.jquery ? e[0] : e)
                            : this[0] && this[0].parentNode
                            ? this.first().prevAll().length
                            : -1;
                    },
                    add: function(e, t) { return this.pushStack(T.uniqueSort(T.merge(this.get(), T(e, t)))) },
                    addBack: function(e) { return this.add(null == e ? this.prevObject : this.prevObject.filter(e)) }
                }), T.each({
                        parent: function(e) {
                            const t = e.parentNode;
                            return t && 11 !== t.nodeType ? t : null;
                        },
                        parents: function(e) { return D(e, "parentNode") },
                        parentsUntil: function(e, t, n) { return D(e, "parentNode", n) },
                        next: function(e) { return R(e, "nextSibling") },
                        prev: function(e) { return R(e, "previousSibling") },
                        nextAll: function(e) { return D(e, "nextSibling") },
                        prevAll: function(e) { return D(e, "previousSibling") },
                        nextUntil: function(e, t, n) { return D(e, "nextSibling", n) },
                        prevUntil: function(e, t, n) { return D(e, "previousSibling", n) },
                        siblings: function(e) { return A((e.parentNode || {}).firstChild, e) },
                        children: function(e) { return A(e.firstChild) },
                        contents: function(e) {
                            return void 0 !== e.contentDocument
                                ? e.contentDocument
                                : (I(e, "template") && (e = e.content || e), T.merge([], e.childNodes));
                        }
                    },
                    (function(e, t) {
                        T.fn[e] = function(n, i) {
                            var r = T.map(this, t, n);
                            return"Until" !== e.slice(-5) && (i = n), i && "string" == typeof i && (r = T.filter(i, r)),
                                this.length > 1 && (H[e] || T.uniqueSort(r), P.test(e) && r.reverse()), this.pushStack(
                                    r);
                        };
                    }));
                var M = /[^\x20\t\r\n\f]+/g;

                function q(e) { return e }

                function F(e) { throw e }

                function W(e, t, n, i) {
                    var r;
                    try {
                        e && y(r = e.promise)
                            ? r.call(e).done(t).fail(n)
                            : e && y(r = e.then)
                            ? r.call(e, t, n)
                            : t.apply(void 0, [e].slice(i));
                    } catch (e) {
                        n.apply(void 0, [e]);
                    }
                }

                T.Callbacks = function(e) {
                    e = "string" == typeof e
                        ? function(e) {
                            var t = {};
                            return T.each(e.match(M) || [], (function(e, n) { t[n] = !0 })), t;
                        }(e)
                        : T.extend({}, e);
                    var t,
                        n,
                        i,
                        r,
                        o = [],
                        s = [],
                        a = -1,
                        l = function() {
                            for (r = r || e.once, i = t = !0; s.length; a = -1)
                                for (n = s.shift(); ++a < o.length;)
                                    !1 === o[a].apply(n[0], n[1]) && e.stopOnFalse && (a = o.length, n = !1);
                            e.memory || (n = !1), t = !1, r && (o = n ? [] : "");
                        },
                        u = {
                            add: function() {
                                return o &&
                                (n && !t && (a = o.length - 1, s.push(n)), function t(n) {
                                    T.each(n,
                                        (function(n, i) {
                                            y(i)
                                                ? e.unique && u.has(i) || o.push(i)
                                                : i && i.length && "string" !== w(i) && t(i);
                                        }));
                                }(arguments), n && !t && l()), this;
                            },
                            remove: function() {
                                return T.each(arguments,
                                    (function(e, t) {
                                        for (let n; (n = T.inArray(t, o, n)) > -1;) o.splice(n, 1), n <= a && a--;
                                    })), this;
                            },
                            has: function(e) { return e ? T.inArray(e, o) > -1 : o.length > 0 },
                            empty: function() { return o && (o = []), this },
                            disable: function() { return r = s = [], o = n = "", this },
                            disabled: function() { return!o },
                            lock: function() { return r = s = [], n || t || (o = n = ""), this },
                            locked: function() { return!!r },
                            fireWith: function(e, n) {
                                return r || (n = [e, (n = n || []).slice ? n.slice() : n], s.push(n), t || l()), this;
                            },
                            fire: function() { return u.fireWith(this, arguments), this },
                            fired: function() { return!!i }
                        };
                    return u;
                }, T.extend({
                    Deferred: function(e) {
                        var t = [
                                ["notify", "progress", T.Callbacks("memory"), T.Callbacks("memory"), 2],
                                [
                                    "resolve", "done", T.Callbacks("once memory"), T.Callbacks("once memory"), 0,
                                    "resolved"
                                ],
                                [
                                    "reject", "fail", T.Callbacks("once memory"), T.Callbacks("once memory"), 1,
                                    "rejected"
                                ]
                            ],
                            i = "pending",
                            r = {
                                state: function() { return i },
                                always: function() { return o.done(arguments).fail(arguments), this },
                                catch: function(e) { return r.then(null, e) },
                                pipe: function() {
                                    var e = arguments;
                                    return T.Deferred((function(n) {
                                        T.each(t,
                                            (function(t, i) {
                                                var r = y(e[i[4]]) && e[i[4]];
                                                o[i[1]]((function() {
                                                    const e = r && r.apply(this, arguments);
                                                    e && y(e.promise)
                                                        ? e.promise().progress(n.notify).done(n.resolve).fail(n.reject)
                                                        : n[i[0] + "With"](this, r ? [e] : arguments);
                                                }));
                                            })), e = null;
                                    })).promise();
                                },
                                then: function(e, i, r) {
                                    var o = 0;

                                    function s(e, t, i, r) {
                                        return function() {
                                            var a = this,
                                                l = arguments,
                                                u = function() {
                                                    var n, u;
                                                    if (!(e < o)) {
                                                        if ((n = i.apply(a, l)) === t.promise())
                                                            throw new TypeError("Thenable self-resolution");
                                                        u = n &&
                                                                ("object" == typeof n || "function" == typeof n) &&
                                                                n.then,
                                                            y(u)
                                                                ? r
                                                                ? u.call(n, s(o, t, q, r), s(o, t, F, r))
                                                                : (o++, u.call(n,
                                                                    s(o, t, q, r),
                                                                    s(o, t, F, r),
                                                                    s(o, t, q, t.notifyWith)))
                                                                : (i !== q && (a = void 0, l = [n]), (r || t.resolveWith
                                                                )(a, l));
                                                    }
                                                },
                                                c = r
                                                    ? u
                                                    : function() {
                                                        try {
                                                            u();
                                                        } catch (n) {
                                                            T.Deferred.exceptionHook &&
                                                                T.Deferred.exceptionHook(n, c.stackTrace), e + 1 >= o &&
                                                                (i !== F && (a = void 0, l = [n]), t.rejectWith(a, l));
                                                        }
                                                    };
                                            e
                                                ? c()
                                                : (T.Deferred.getStackHook &&
                                                    (c.stackTrace = T.Deferred.getStackHook()), n
                                                    .setTimeout(c));
                                        };
                                    }

                                    return T.Deferred((function(n) {
                                        t[0][3].add(s(0, n, y(r) ? r : q, n.notifyWith)),
                                            t[1][3].add(s(0, n, y(e) ? e : q)), t[2][3].add(s(0, n, y(i) ? i : F));
                                    })).promise();
                                },
                                promise: function(e) { return null != e ? T.extend(e, r) : r }
                            },
                            o = {};
                        return T.each(t,
                            (function(e, n) {
                                const s = n[2];
                                var a = n[5];
                                r[n[1]] =
                                    s.add, a &&
                                    s.add((function() { i = a }),
                                        t[3 - e][2].disable,
                                        t[3 - e][3].disable,
                                        t[0][2].lock,
                                        t[0][3].lock), s.add(n[3].fire), o[n[0]] = function() {
                                    return o[n[0] + "With"](this === o ? void 0 : this, arguments), this;
                                }, o[n[0] + "With"] = s.fireWith;
                            })), r.promise(o), e && e.call(o, o), o;
                    },
                    when: function(e) {
                        var t = arguments.length, n = t, i = Array(n), r = l.call(arguments), o = T.Deferred();
                        const s = function(e) {
                            return function(n) {
                                i[e] = this, r[e] =
                                    arguments.length > 1 ? l.call(arguments) : n, --t || o.resolveWith(i, r);
                            };
                        };
                        if (t <= 1 &&
                            (W(e, o.done(s(n)).resolve, o.reject, !t), "pending" === o.state() || y(r[n] && r[n].then)))
                            return o.then();
                        for (; n--;) W(r[n], s(n), o.reject);
                        return o.promise();
                    }
                });
                var B = /^(Eval|Internal|Range|Reference|Syntax|Type|URI)Error$/;
                T.Deferred.exceptionHook =
                    function(e, t) {
                        n.console &&
                            n.console.warn &&
                            e &&
                            B.test(e.name) &&
                            n.console.warn(`jQuery.Deferred exception: ${e.message}`, e.stack, t);
                    }, T.readyException = function(e) { n.setTimeout((function() { throw e })) };
                var U = T.Deferred();

                function V() {
                    s.removeEventListener("DOMContentLoaded", V), n.removeEventListener("load", V), T.ready();
                }

                T.fn.ready =
                    function(e) { return U.then(e).catch((function(e) { T.readyException(e) })), this }, T.extend({
                    isReady: !1,
                    readyWait: 1,
                    ready: function(e) {
                        (!0 === e ? --T.readyWait : T.isReady) ||
                            (T.isReady = !0, !0 !== e && --T.readyWait > 0 || U.resolveWith(s, [T]));
                    }
                }), T.ready.then =
                    U.then, "complete" === s.readyState || "loading" !== s.readyState && !s.documentElement.doScroll
                    ? n.setTimeout(T.ready)
                    : (s.addEventListener("DOMContentLoaded", V), n.addEventListener("load", V));
                var K = function(e, t, n, i, r, o, s) {
                        var a = 0;
                        const l = e.length;
                        var u = null == n;
                        if ("object" === w(n)) for (a in r = !0, n) K(e, t, a, n[a], !0, o, s);
                        else if (void 0 !== i &&
                        (r = !0, y(i) || (s = !0), u &&
                            (s
                                ? (t.call(e, i), t = null)
                                : (u = t, t = function(e, t, n) { return u.call(T(e), n) })),
                            t))
                            for (; a < l; a++) t(e[a], n, s ? i : i.call(e[a], a, t(e[a], n)));
                        return r ? e : u ? t.call(e) : l ? t(e[0], n) : o;
                    },
                    $ = /^-ms-/,
                    Q = /-([a-z])/g;

                function Y(e, t) { return t.toUpperCase() }

                function z(e) { return e.replace($, "ms-").replace(Q, Y) }

                var X = function(e) { return 1 === e.nodeType || 9 === e.nodeType || !+e.nodeType };

                function G() { this.expando = T.expando + G.uid++ }

                G.uid = 1, G.prototype = {
                    cache: function(e) {
                        var t = e[this.expando];
                        return t ||
                        (t = {}, X(e) &&
                        (e.nodeType
                            ? e[this.expando] = t
                            : Object.defineProperty(e, this.expando, { value: t, configurable: !0 }))), t;
                    },
                    set: function(e, t, n) {
                        var i;
                        const r = this.cache(e);
                        if ("string" == typeof t) r[z(t)] = n;
                        else for (i in t) r[z(i)] = t[i];
                        return r;
                    },
                    get: function(e, t) {
                        return void 0 === t ? this.cache(e) : e[this.expando] && e[this.expando][z(t)];
                    },
                    access: function(e, t, n) {
                        return void 0 === t || t && "string" == typeof t && void 0 === n
                            ? this.get(e, t)
                            : (this.set(e, t, n), void 0 !== n ? n : t);
                    },
                    remove: function(e, t) {
                        var n;
                        const i = e[this.expando];
                        if (void 0 !== i) {
                            if (void 0 !== t) {
                                n = (t = Array.isArray(t) ? t.map(z) : (t = z(t)) in i ? [t] : t.match(M) || []).length;
                                for (; n--;) delete i[t[n]];
                            }
                            (void 0 === t || T.isEmptyObject(i)) &&
                                (e.nodeType ? e[this.expando] = void 0 : delete e[this.expando]);
                        }
                    },
                    hasData: function(e) {
                        const t = e[this.expando];
                        return void 0 !== t && !T.isEmptyObject(t);
                    }
                };
                var J = new G, Z = new G, ee = /^(?:\{[\w\W]*\}|\[[\w\W]*\])$/, te = /[A-Z]/g;

                function ne(e, t, n) {
                    var i;
                    if (void 0 === n && 1 === e.nodeType)
                        if (i =
                            `data-${t.replace(te, "-$&").toLowerCase()}`, "string" == typeof(n = e.getAttribute(i))) {
                            try {
                                n = function(e) {
                                    return"true" === e ||
                                        "false" !== e &&
                                        ("null" === e ? null : e === +e + "" ? +e : ee.test(e) ? JSON.parse(e) : e);
                                }(n);
                            } catch (e) {
                            }
                            Z.set(e, t, n);
                        } else n = void 0;
                    return n;
                }

                T.extend({
                    hasData: function(e) { return Z.hasData(e) || J.hasData(e) },
                    data: function(e, t, n) { return Z.access(e, t, n) },
                    removeData: function(e, t) { Z.remove(e, t) },
                    _data: function(e, t, n) { return J.access(e, t, n) },
                    _removeData: function(e, t) { J.remove(e, t) }
                }), T.fn.extend({
                    data: function(e, t) {
                        var n, i, r, o = this[0];
                        const s = o && o.attributes;
                        if (void 0 === e) {
                            if (this.length && (r = Z.get(o), 1 === o.nodeType && !J.get(o, "hasDataAttrs"))) {
                                for (n = s.length; n--;)
                                    s[n] &&
                                        0 === (i = s[n].name).indexOf("data-") &&
                                        (i = z(i.slice(5)), ne(o, i, r[i]));
                                J.set(o, "hasDataAttrs", !0);
                            }
                            return r;
                        }
                        return"object" == typeof e
                            ? this.each((function() { Z.set(this, e) }))
                            : K(this,
                                (function(t) {
                                    var n;
                                    if (o && void 0 === t)
                                        return void 0 !== (n = Z.get(o, e))
                                            ? n
                                            : void 0 !== (n = ne(o, e))
                                            ? n
                                            : void 0;
                                    this.each((function() { Z.set(this, e, t) }));
                                }),
                                null,
                                t,
                                arguments.length > 1,
                                null,
                                !0);
                    },
                    removeData: function(e) { return this.each((function() { Z.remove(this, e) })) }
                }), T.extend({
                    queue: function(e, t, n) {
                        var i;
                        if (e)
                            return t = (t || "fx") + "queue", i =
                                J.get(e, t), n &&
                                (!i || Array.isArray(n) ? i = J.access(e, t, T.makeArray(n)) : i.push(n)), i || [];
                    },
                    dequeue: function(e, t) {
                        t = t || "fx";
                        const n = T.queue(e, t);
                        var i = n.length, r = n.shift();
                        const o = T._queueHooks(e, t);
                        "inprogress" === r && (r = n.shift(), i--), r &&
                        ("fx" === t && n.unshift("inprogress"), delete o.stop, r.call(e,
                            (function() { T.dequeue(e, t) }),
                            o)), !i && o && o.empty.fire();
                    },
                    _queueHooks: function(e, t) {
                        var n = t + "queueHooks";
                        return J.get(e, n) ||
                            J.access(e,
                                n,
                                {
                                    empty: T.Callbacks("once memory")
                                        .add((function() { J.remove(e, [t + "queue", n]) }))
                                });
                    }
                }), T.fn.extend({
                    queue: function(e, t) {
                        var n = 2;
                        return"string" != typeof e && (t = e, e = "fx", n--), arguments.length < n
                            ? T.queue(this[0], e)
                            : void 0 === t
                            ? this
                            : this.each((function() {
                                const n = T.queue(this, e, t);
                                T._queueHooks(this, e), "fx" === e && "inprogress" !== n[0] && T.dequeue(this, e);
                            }));
                    },
                    dequeue: function(e) { return this.each((function() { T.dequeue(this, e) })) },
                    clearQueue: function(e) { return this.queue(e || "fx", []) },
                    promise: function(e, t) {
                        var n, i = 1, r = T.Deferred(), o = this, s = this.length;
                        const a = function() { --i || r.resolveWith(o, [o]) };
                        for ("string" != typeof e && (t = e, e = void 0), e = e || "fx"; s--;)
                            (n = J.get(o[s], e + "queueHooks")) && n.empty && (i++, n.empty.add(a));
                        return a(), r.promise(t);
                    }
                });
                var ie = /[+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|)/.source,
                    re = new RegExp(`^(?:([+-])=|)(${ie})([a-z%]*)$`, "i"),
                    oe = ["Top", "Right", "Bottom", "Left"],
                    se = s.documentElement,
                    ae = function(e) { return T.contains(e.ownerDocument, e) },
                    le = { composed: !0 };
                se.getRootNode &&
                (ae = function(e) {
                    return T.contains(e.ownerDocument, e) || e.getRootNode(le) === e.ownerDocument;
                });
                var ue = function(e, t) {
                        return"none" === (e = t || e).style.display ||
                            "" === e.style.display && ae(e) && "none" === T.css(e, "display");
                    },
                    ce = function(e, t, n, i) {
                        var r, o;
                        const s = {};
                        for (o in t) s[o] = e.style[o], e.style[o] = t[o];
                        for (o in r = n.apply(e, i || []), t) e.style[o] = s[o];
                        return r;
                    };

                function fe(e, t, n, i) {
                    var r, o, s = 20;
                    const a = i ? function() { return i.cur() } : function() { return T.css(e, t, "") };
                    var l = a(),
                        u = n && n[3] || (T.cssNumber[t] ? "" : "px"),
                        c = e.nodeType && (T.cssNumber[t] || "px" !== u && +l) && re.exec(T.css(e, t));
                    if (c && c[3] !== u) {
                        for (l /= 2, u = u || c[3], c = +l || 1; s--;)
                            T.style(e, t, c + u), (1 - o) * (1 - (o = a() / l || .5)) <= 0 && (s = 0), c /= o;
                        c *= 2, T.style(e, t, c + u), n = n || [];
                    }
                    return n &&
                    (c = +c || +l || 0, r =
                        n[1] ? c + (n[1] + 1) * n[2] : +n[2], i && (i.unit = u, i.start = c, i.end = r)), r;
                }

                var de = {};

                function he(e) {
                    var t;
                    const n = e.ownerDocument;
                    const i = e.nodeName;
                    var r = de[i];
                    return r ||
                    (t = n.body.appendChild(n.createElement(i)), r =
                        T.css(t, "display"), t.parentNode.removeChild(t), "none" === r && (r = "block"), de[i] = r, r);
                }

                function pe(e, t) {
                    for (var n, i, r = [], o = 0, s = e.length; o < s; o++)
                        (i = e[o]).style &&
                        (n = i.style.display, t
                            ? ("none" === n && (r[o] = J.get(i, "display") || null, r[o] || (i.style.display = "")),
                                "" === i.style.display && ue(i) && (r[o] = he(i)))
                            : "none" !== n && (r[o] = "none", J.set(i, "display", n)));
                    for (o = 0; o < s; o++) null != r[o] && (e[o].style.display = r[o]);
                    return e;
                }

                T.fn.extend({
                    show: function() { return pe(this, !0) },
                    hide: function() { return pe(this) },
                    toggle: function(e) {
                        return"boolean" == typeof e
                            ? e
                            ? this.show()
                            : this.hide()
                            : this.each((function() { ue(this) ? T(this).show() : T(this).hide() }));
                    }
                });
                var ge = /^(?:checkbox|radio)$/i,
                    me = /<([a-z][^\/\0>\x20\t\r\n\f]*)/i,
                    ve = /^$|^module$|\/(?:java|ecma)script/i,
                    ye = {
                        option: [1, "<select multiple='multiple'>", "</select>"],
                        thead: [1, "<table>", "</table>"],
                        col: [2, "<table><colgroup>", "</colgroup></table>"],
                        tr: [2, "<table><tbody>", "</tbody></table>"],
                        td: [3, "<table><tbody><tr>", "</tr></tbody></table>"],
                        _default: [0, "", ""]
                    };

                function be(e, t) {
                    var n;
                    return n = void 0 !== e.getElementsByTagName
                        ? e.getElementsByTagName(t || "*")
                        : void 0 !== e.querySelectorAll
                        ? e.querySelectorAll(t || "*")
                        : [], void 0 === t || t && I(e, t) ? T.merge([e], n) : n;
                }

                function _e(e, t) {
                    for (var n = 0, i = e.length; n < i; n++)
                        J.set(e[n], "globalEval", !t || J.get(t[n], "globalEval"));
                }

                ye.optgroup = ye.option, ye.tbody = ye.tfoot = ye.colgroup = ye.caption = ye.thead, ye.th = ye.td;
                var Ee, we, Te = /<|&#?\w+;/;

                function Ce(e, t, n, i, r) {
                    for (var o, s, a, l, u, c, f = t.createDocumentFragment(), d = [], h = 0, p = e.length; h < p; h++)
                        if ((o = e[h]) || 0 === o)
                            if ("object" === w(o)) T.merge(d, o.nodeType ? [o] : o);
                            else if (Te.test(o)) {
                                for (s = s || f.appendChild(t.createElement("div")), a =
                                        (me.exec(o) || ["", ""])[1].toLowerCase(), l =
                                        ye[a] || ye._default, s.innerHTML =
                                        l[1] + T.htmlPrefilter(o) + l[2], c = l[0];
                                    c--;
                                ) s = s.lastChild;
                                T.merge(d, s.childNodes), (s = f.firstChild).textContent = "";
                            } else d.push(t.createTextNode(o));
                    for (f.textContent = "", h = 0; o = d[h++];)
                        if (i && T.inArray(o, i) > -1) r && r.push(o);
                        else if (u = ae(o), s =
                            be(f.appendChild(o), "script"), u && _e(s), n)
                            for (c = 0; o = s[c++];) ve.test(o.type || "") && n.push(o);
                    return f;
                }

                Ee = s.createDocumentFragment().appendChild(s.createElement("div")),
                    (we = s.createElement("input")).setAttribute("type", "radio"),
                    we.setAttribute("checked", "checked"), we.setAttribute("name", "t"), Ee.appendChild(we),
                    v.checkClone = Ee.cloneNode(!0).cloneNode(!0).lastChild.checked, Ee.innerHTML =
                        "<textarea>x</textarea>", v.noCloneChecked = !!Ee.cloneNode(!0).lastChild.defaultValue;
                var xe = /^key/, Se = /^(?:mouse|pointer|contextmenu|drag|drop)|click/, De = /^([^.]*)(?:\.(.+)|)/;

                function Ae() { return!0 }

                function Ne() { return!1 }

                function Ie(e, t) {
                    return e ===
                        function() {
                            try {
                                return s.activeElement;
                            } catch (e) {
                            }
                        }() ==
                        ("focus" === t);
                }

                function Oe(e, t, n, i, r, o) {
                    var s, a;
                    if ("object" == typeof t) {
                        for (a in"string" != typeof n && (i = i || n, n = void 0), t) Oe(e, a, n, i, t[a], o);
                        return e;
                    }
                    if (null == i && null == r
                            ? (r = n, i = n = void 0)
                            : null == r && ("string" == typeof n ? (r = i, i = void 0) : (r = i, i = n, n = void 0)),
                        !1 === r) r = Ne;
                    else if (!r) return e;
                    return 1 === o &&
                    (s = r, (r = function(e) { return T().off(e), s.apply(this, arguments) }).guid =
                        s.guid || (s.guid = T.guid++)), e.each((function() { T.event.add(this, t, r, i, n) }));
                }

                function ke(e, t, n) {
                    n
                        ? (J.set(e, t, !1), T.event.add(e,
                            t,
                            {
                                namespace: !1,
                                handler: function(e) {
                                    var i, r, o = J.get(this, t);
                                    if (1 & e.isTrigger && this[t]) {
                                        if (o.length) (T.event.special[t] || {}).delegateType && e.stopPropagation();
                                        else if (o = l.call(arguments), J.set(this, t, o), i =
                                            n(this, t), this[t](), o !== (r = J.get(this, t)) || i
                                            ? J.set(this, t, !1)
                                            : r = {}, o !== r)
                                            return e.stopImmediatePropagation(), e.preventDefault(), r.value;
                                    } else
                                        o.length &&
                                        (J.set(this,
                                            t,
                                            {
                                                value: T.event.trigger(T.extend(o[0], T.Event.prototype),
                                                    o.slice(1),
                                                    this)
                                            }), e.stopImmediatePropagation());
                                }
                            }))
                        : void 0 === J.get(e, t) && T.event.add(e, t, Ae);
                }

                T.event = {
                    global: {},
                    add: function(e, t, n, i, r) {
                        var o, s, a, l, u, c, f, d, h, p, g, m = J.get(e);
                        if (m)
                            for
                            (n.handler && (n = (o = n).handler, r = o.selector), r && T.find.matchesSelector(se, r),
                                n.guid || (n.guid = T.guid++), (l = m.events) || (l = m.events = {}), (s = m.handle) ||
                                (s = m.handle = function(t) {
                                    return void 0 !== T && T.event.triggered !== t.type
                                        ? T.event.dispatch.apply(e, arguments)
                                        : void 0;
                                }), u = (t = (t || "").match(M) || [""]).length;
                                u--;
                            )
                                h = g = (a = De.exec(t[u]) || [])[1], p = (a[2] || "").split(".").sort(), h &&
                                (f = T.event.special[h] || {}, h = (r ? f.delegateType : f.bindType) || h, f =
                                    T.event.special[h] || {}, c =
                                    T.extend({
                                            type: h,
                                            origType: g,
                                            data: i,
                                            handler: n,
                                            guid: n.guid,
                                            selector: r,
                                            needsContext: r && T.expr.match.needsContext.test(r),
                                            namespace: p.join(".")
                                        },
                                        o), (d = l[h]) ||
                                ((d = l[h] = []).delegateCount = 0, f.setup && !1 !== f.setup.call(e, i, p, s) ||
                                    e.addEventListener && e.addEventListener(h, s)), f.add &&
                                    (f.add.call(e, c), c.handler.guid || (c.handler.guid = n.guid)), r
                                    ? d.splice(d.delegateCount++, 0, c)
                                    : d.push(c), T.event.global[h] = !0);
                    },
                    remove: function(e, t, n, i, r) {
                        var o, s, a, l, u, c, f, d, h, p, g, m = J.hasData(e) && J.get(e);
                        if (m && (l = m.events)) {
                            for (u = (t = (t || "").match(M) || [""]).length; u--;)
                                if (h = g = (a = De.exec(t[u]) || [])[1], p = (a[2] || "").split(".").sort(), h) {
                                    for (f = T.event.special[h] || {}, d =
                                            l[h = (i ? f.delegateType : f.bindType) || h] || [], a =
                                            a[2] && new RegExp(`(^|\\.)${p.join("\\.(?:.*\\.|)")}(\\.|$)`), s = o =
                                            d.length;
                                        o--;
                                    )
                                        c = d[o], !r && g !== c.origType ||
                                            n && n.guid !== c.guid ||
                                            a && !a.test(c.namespace) ||
                                            i && i !== c.selector && ("**" !== i || !c.selector) ||
                                            (d.splice(o, 1), c.selector && d.delegateCount--, f.remove &&
                                                f.remove.call(e, c));
                                    s &&
                                        !d.length &&
                                        (f.teardown && !1 !== f.teardown.call(e, p, m.handle) ||
                                            T.removeEvent(e, h, m.handle), delete l[h]);
                                } else for (h in l) T.event.remove(e, h + t[u], n, i, !0);
                            T.isEmptyObject(l) && J.remove(e, "handle events");
                        }
                    },
                    dispatch: function(e) {
                        var t, n, i, r, o, s;
                        const a = T.event.fix(e);
                        const l = new Array(arguments.length);
                        const u = (J.get(this, "events") || {})[a.type] || [];
                        const c = T.event.special[a.type] || {};
                        for (l[0] = a, t = 1; t < arguments.length; t++) l[t] = arguments[t];
                        if (a.delegateTarget = this, !c.preDispatch || !1 !== c.preDispatch.call(this, a)) {
                            for (s = T.event.handlers.call(this, a, u), t = 0;
                                (r = s[t++]) && !a.isPropagationStopped();
                            )
                                for (a.currentTarget = r.elem, n = 0;
                                    (o = r.handlers[n++]) && !a.isImmediatePropagationStopped();
                                )
                                    a.rnamespace && !1 !== o.namespace && !a.rnamespace.test(o.namespace) ||
                                    (a.handleObj = o, a.data =
                                        o.data, void 0 !==
                                        (i = ((T.event.special[o.origType] || {}).handle || o.handler)
                                            .apply(r.elem, l)) &&
                                        !1 === (a.result = i) &&
                                        (a.preventDefault(), a.stopPropagation()));
                            return c.postDispatch && c.postDispatch.call(this, a), a.result;
                        }
                    },
                    handlers: function(e, t) {
                        var n, i, r, o, s;
                        const a = [];
                        const l = t.delegateCount;
                        var u = e.target;
                        if (l && u.nodeType && !("click" === e.type && e.button >= 1))
                            for (; u !== this; u = u.parentNode || this)
                                if (1 === u.nodeType && ("click" !== e.type || !0 !== u.disabled)) {
                                    for (o = [], s = {}, n = 0; n < l; n++)
                                        void 0 === s[r = (i = t[n]).selector + " "] &&
                                        (s[r] = i.needsContext
                                            ? T(r, this).index(u) > -1
                                            : T.find(r, this, null, [u]).length), s[r] && o.push(i);
                                    o.length && a.push({ elem: u, handlers: o });
                                }
                        return u = this, l < t.length && a.push({ elem: u, handlers: t.slice(l) }), a;
                    },
                    addProp: function(e, t) {
                        Object.defineProperty(T.Event.prototype,
                            e,
                            {
                                enumerable: !0,
                                configurable: !0,
                                get: y(t)
                                    ? function() { if (this.originalEvent) return t(this.originalEvent) }
                                    : function() { if (this.originalEvent) return this.originalEvent[e] },
                                set: function(t) {
                                    Object.defineProperty(this,
                                        e,
                                        { enumerable: !0, configurable: !0, writable: !0, value: t });
                                }
                            });
                    },
                    fix: function(e) { return e[T.expando] ? e : new T.Event(e) },
                    special: {
                        load: { noBubble: !0 },
                        click: {
                            setup: function(e) {
                                const t = this || e;
                                return ge.test(t.type) && t.click && I(t, "input") && ke(t, "click", Ae), !1;
                            },
                            trigger: function(e) {
                                const t = this || e;
                                return ge.test(t.type) && t.click && I(t, "input") && ke(t, "click"), !0;
                            },
                            _default: function(e) {
                                const t = e.target;
                                return ge.test(t.type) && t.click && I(t, "input") && J.get(t, "click") || I(t, "a");
                            }
                        },
                        beforeunload: {
                            postDispatch: function(e) {
                                void 0 !== e.result && e.originalEvent && (e.originalEvent.returnValue = e.result);
                            }
                        }
                    }
                }, T.removeEvent = function(e, t, n) { e.removeEventListener && e.removeEventListener(t, n) }, T.Event =
                    function(e, t) {
                        if (!(this instanceof T.Event)) return new T.Event(e, t);
                        e && e.type
                            ? (this.originalEvent = e, this.type = e.type, this.isDefaultPrevented =
                                    e.defaultPrevented || void 0 === e.defaultPrevented && !1 === e.returnValue
                                    ? Ae
                                    : Ne,
                                this.target =
                                    e.target && 3 === e.target.nodeType ? e.target.parentNode : e.target,
                                this.currentTarget = e.currentTarget, this.relatedTarget = e.relatedTarget)
                            : this.type = e, t && T.extend(this, t), this.timeStamp =
                            e && e.timeStamp || Date.now(), this[T.expando] = !0;
                    }, T.Event.prototype = {
                    constructor: T.Event,
                    isDefaultPrevented: Ne,
                    isPropagationStopped: Ne,
                    isImmediatePropagationStopped: Ne,
                    isSimulated: !1,
                    preventDefault: function() {
                        const e = this.originalEvent;
                        this.isDefaultPrevented = Ae, e && !this.isSimulated && e.preventDefault();
                    },
                    stopPropagation: function() {
                        const e = this.originalEvent;
                        this.isPropagationStopped = Ae, e && !this.isSimulated && e.stopPropagation();
                    },
                    stopImmediatePropagation: function() {
                        const e = this.originalEvent;
                        this.isImmediatePropagationStopped =
                            Ae, e && !this.isSimulated && e.stopImmediatePropagation(), this.stopPropagation();
                    }
                }, T.each({
                        altKey: !0,
                        bubbles: !0,
                        cancelable: !0,
                        changedTouches: !0,
                        ctrlKey: !0,
                        detail: !0,
                        eventPhase: !0,
                        metaKey: !0,
                        pageX: !0,
                        pageY: !0,
                        shiftKey: !0,
                        view: !0,
                        char: !0,
                        code: !0,
                        charCode: !0,
                        key: !0,
                        keyCode: !0,
                        button: !0,
                        buttons: !0,
                        clientX: !0,
                        clientY: !0,
                        offsetX: !0,
                        offsetY: !0,
                        pointerId: !0,
                        pointerType: !0,
                        screenX: !0,
                        screenY: !0,
                        targetTouches: !0,
                        toElement: !0,
                        touches: !0,
                        which: function(e) {
                            const t = e.button;
                            return null == e.which && xe.test(e.type)
                                ? null != e.charCode
                                ? e.charCode
                                : e.keyCode
                                : !e.which && void 0 !== t && Se.test(e.type)
                                ? 1 & t
                                ? 1
                                : 2 & t
                                ? 3
                                : 4 & t
                                ? 2
                                : 0
                                : e.which;
                        }
                    },
                    T.event.addProp), T.each({ focus: "focusin", blur: "focusout" },
                    (function(e, t) {
                        T.event.special[e] = {
                            setup: function() { return ke(this, e, Ie), !1 },
                            trigger: function() { return ke(this, e), !0 },
                            delegateType: t
                        };
                    })), T.each(
                    {
                        mouseenter: "mouseover",
                        mouseleave: "mouseout",
                        pointerenter: "pointerover",
                        pointerleave: "pointerout"
                    },
                    (function(e, t) {
                        T.event.special[e] = {
                            delegateType: t,
                            bindType: t,
                            handle: function(e) {
                                var n;
                                const i = this;
                                const r = e.relatedTarget;
                                const o = e.handleObj;
                                return r && (r === i || T.contains(i, r)) ||
                                    (e.type = o.origType, n = o.handler.apply(this, arguments), e.type = t), n;
                            }
                        };
                    })), T.fn.extend({
                    on: function(e, t, n, i) { return Oe(this, e, t, n, i) },
                    one: function(e, t, n, i) { return Oe(this, e, t, n, i, 1) },
                    off: function(e, t, n) {
                        var i, r;
                        if (e && e.preventDefault && e.handleObj)
                            return i = e.handleObj, T(e.delegateTarget)
                                .off(i.namespace ? i.origType + "." + i.namespace : i.origType,
                                    i.selector,
                                    i.handler), this;
                        if ("object" == typeof e) {
                            for (r in e) this.off(r, t, e[r]);
                            return this;
                        }
                        return!1 !== t && "function" != typeof t || (n = t, t = void 0), !1 === n && (n = Ne),
                            this.each((function() { T.event.remove(this, e, n, t) }));
                    }
                });
                var Le = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([a-z][^\/\0>\x20\t\r\n\f]*)[^>]*)\/>/gi,
                    je = /<script|<style|<link/i,
                    Pe = /checked\s*(?:[^=]|=\s*.checked.)/i,
                    He = /^\s*<!(?:\[CDATA\[|--)|(?:\]\]|--)>\s*$/g;

                function Re(e, t) {
                    return I(e, "table") &&
                        I(11 !== t.nodeType ? t : t.firstChild, "tr") &&
                        T(e).children("tbody")[0] ||
                        e;
                }

                function Me(e) { return e.type = (null !== e.getAttribute("type")) + "/" + e.type, e }

                function qe(e) {
                    return"true/" === (e.type || "").slice(0, 5)
                        ? e.type = e.type.slice(5)
                        : e.removeAttribute("type"), e;
                }

                function Fe(e, t) {
                    var n, i, r, o, s, a, l, u;
                    if (1 === t.nodeType) {
                        if (J.hasData(e) && (o = J.access(e), s = J.set(t, o), u = o.events))
                            for (r in delete s.handle, s.events =
                                {}, u) for (n = 0, i = u[r].length; n < i; n++) T.event.add(t, r, u[r][n]);
                        Z.hasData(e) && (a = Z.access(e), l = T.extend({}, a), Z.set(t, l));
                    }
                }

                function We(e, t) {
                    const n = t.nodeName.toLowerCase();
                    "input" === n && ge.test(e.type)
                        ? t.checked = e.checked
                        : "input" !== n && "textarea" !== n || (t.defaultValue = e.defaultValue);
                }

                function Be(e, t, n, i) {
                    t = u.apply([], t);
                    var r, o, s, a, l, c, f = 0;
                    const d = e.length;
                    const h = d - 1;
                    var p = t[0],
                        g = y(p);
                    if (g || d > 1 && "string" == typeof p && !v.checkClone && Pe.test(p))
                        return e.each((function(r) {
                            const o = e.eq(r);
                            g && (t[0] = p.call(this, r, o.html())), Be(o, t, n, i);
                        }));
                    if (d &&
                    (o = (r = Ce(t, e[0].ownerDocument, !1, e, i)).firstChild, 1 === r.childNodes.length && (r = o),
                        o || i)) {
                        for (a = (s = T.map(be(r, "script"), Me)).length; f < d; f++)
                            l = r, f !== h && (l = T.clone(l, !0, !0), a && T.merge(s, be(l, "script"))), n.call(e[f],
                                l,
                                f);
                        if (a)
                            for (c = s[s.length - 1].ownerDocument, T.map(s, qe), f = 0; f < a; f++)
                                l = s[f], ve.test(l.type || "") &&
                                    !J.access(l, "globalEval") &&
                                    T.contains(c, l) &&
                                    (l.src && "module" !== (l.type || "").toLowerCase()
                                        ? T._evalUrl &&
                                        !l.noModule &&
                                        T._evalUrl(l.src, { nonce: l.nonce || l.getAttribute("nonce") })
                                        : E(l.textContent.replace(He, ""), l, c));
                    }
                    return e;
                }

                function Ue(e, t, n) {
                    for (var i, r = t ? T.filter(t, e) : e, o = 0; null != (i = r[o]); o++)
                        n || 1 !== i.nodeType || T.cleanData(be(i)), i.parentNode &&
                            (n && ae(i) && _e(be(i, "script")), i.parentNode.removeChild(i));
                    return e;
                }

                T.extend({
                    htmlPrefilter: function(e) { return e.replace(Le, "<$1></$2>") },
                    clone: function(e, t, n) {
                        var i, r, o, s;
                        const a = e.cloneNode(!0);
                        const l = ae(e);
                        if (!(v.noCloneChecked || 1 !== e.nodeType && 11 !== e.nodeType || T.isXMLDoc(e)))
                            for (s = be(a), i = 0, r = (o = be(e)).length; i < r; i++) We(o[i], s[i]);
                        if (t)
                            if (n) for (o = o || be(e), s = s || be(a), i = 0, r = o.length; i < r; i++) Fe(o[i], s[i]);
                            else Fe(e, a);
                        return(s = be(a, "script")).length > 0 && _e(s, !l && be(e, "script")), a;
                    },
                    cleanData: function(e) {
                        for (var t, n, i, r = T.event.special, o = 0; void 0 !== (n = e[o]); o++)
                            if (X(n)) {
                                if (t = n[J.expando]) {
                                    if (t.events)
                                        for (i in t.events) r[i] ? T.event.remove(n, i) : T.removeEvent(n, i, t.handle);
                                    n[J.expando] = void 0;
                                }
                                n[Z.expando] && (n[Z.expando] = void 0);
                            }
                    }
                }), T.fn.extend({
                    detach: function(e) { return Ue(this, e, !0) },
                    remove: function(e) { return Ue(this, e) },
                    text: function(e) {
                        return K(this,
                            (function(e) {
                                return void 0 === e
                                    ? T.text(this)
                                    : this.empty().each((function() {
                                        1 !== this.nodeType && 11 !== this.nodeType && 9 !== this.nodeType ||
                                            (this.textContent = e);
                                    }));
                            }),
                            null,
                            e,
                            arguments.length);
                    },
                    append: function() {
                        return Be(this,
                            arguments,
                            (function(e) {
                                1 !== this.nodeType && 11 !== this.nodeType && 9 !== this.nodeType ||
                                    Re(this, e).appendChild(e);
                            }));
                    },
                    prepend: function() {
                        return Be(this,
                            arguments,
                            (function(e) {
                                if (1 === this.nodeType || 11 === this.nodeType || 9 === this.nodeType) {
                                    const t = Re(this, e);
                                    t.insertBefore(e, t.firstChild);
                                }
                            }));
                    },
                    before: function() {
                        return Be(this,
                            arguments,
                            (function(e) { this.parentNode && this.parentNode.insertBefore(e, this) }));
                    },
                    after: function() {
                        return Be(this,
                            arguments,
                            (function(e) { this.parentNode && this.parentNode.insertBefore(e, this.nextSibling) }));
                    },
                    empty: function() {
                        for (var e, t = 0; null != (e = this[t]); t++)
                            1 === e.nodeType && (T.cleanData(be(e, !1)), e.textContent = "");
                        return this;
                    },
                    clone: function(e, t) {
                        return e = null != e && e, t =
                            null == t ? e : t, this.map((function() { return T.clone(this, e, t) }));
                    },
                    html: function(e) {
                        return K(this,
                            (function(e) {
                                var t = this[0] || {}, n = 0;
                                const i = this.length;
                                if (void 0 === e && 1 === t.nodeType) return t.innerHTML;
                                if ("string" == typeof e &&
                                    !je.test(e) &&
                                    !ye[(me.exec(e) || ["", ""])[1].toLowerCase()]) {
                                    e = T.htmlPrefilter(e);
                                    try {
                                        for (; n < i; n++)
                                            1 === (t = this[n] || {}).nodeType &&
                                                (T.cleanData(be(t, !1)), t.innerHTML = e);
                                        t = 0;
                                    } catch (e) {
                                    }
                                }
                                t && this.empty().append(e);
                            }),
                            null,
                            e,
                            arguments.length);
                    },
                    replaceWith: function() {
                        var e = [];
                        return Be(this,
                            arguments,
                            (function(t) {
                                const n = this.parentNode;
                                T.inArray(this, e) < 0 && (T.cleanData(be(this)), n && n.replaceChild(t, this));
                            }),
                            e);
                    }
                }), T.each(
                    {
                        appendTo: "append",
                        prependTo: "prepend",
                        insertBefore: "before",
                        insertAfter: "after",
                        replaceAll: "replaceWith"
                    },
                    (function(e, t) {
                        T.fn[e] = function(e) {
                            for (var n, i = [], r = T(e), o = r.length - 1, s = 0; s <= o; s++)
                                n = s === o ? this : this.clone(!0), T(r[s])[t](n), c.apply(i, n.get());
                            return this.pushStack(i);
                        };
                    }));
                var Ve = new RegExp(`^(${ie})(?!px)[a-z%]+$`, "i"),
                    Ke = function(e) {
                        var t = e.ownerDocument.defaultView;
                        return t && t.opener || (t = n), t.getComputedStyle(e);
                    },
                    $e = new RegExp(oe.join("|"), "i");

                function Qe(e, t, n) {
                    var i, r, o, s;
                    const a = e.style;
                    return(n = n || Ke(e)) &&
                    ("" !== (s = n.getPropertyValue(t) || n[t]) || ae(e) || (s = T.style(e, t)), !
                        v.pixelBoxStyles() &&
                        Ve.test(s) &&
                        $e.test(t) &&
                        (i = a.width, r = a.minWidth, o = a.maxWidth, a.minWidth = a.maxWidth = a.width = s, s =
                            n.width, a.width = i, a.minWidth = r, a.maxWidth = o)), void 0 !== s ? s + "" : s;
                }

                function Ye(e, t) {
                    return{
                        get: function() {
                            if (!e()) return(this.get = t).apply(this, arguments);
                            delete this.get;
                        }
                    };
                }

                !function() {
                    function e() {
                        if (c) {
                            u.style.cssText =
                                    "border:0; left:-11111px;margin-top:1px;padding:0;position:absolute;width:60px;",
                                c.style.cssText =
                                    "border:1px;box-sizing:border-box;display:block;margin:auto;overflow:scroll;padding:1px;position:relative;top:1%; width:60%;",
                                se.appendChild(u).appendChild(c);
                            var e = n.getComputedStyle(c);
                            i = "1%" !== e.top, l = 12 === t(e.marginLeft), c.style.right = "60%", a =
                                36 === t(e.right), r = 36 === t(e.width), c.style.position = "absolute", o =
                                12 === t(c.offsetWidth / 3), se.removeChild(u), c = null;
                        }
                    }

                    function t(e) { return Math.round(parseFloat(e)) }

                    var i, r, o, a, l, u = s.createElement("div"), c = s.createElement("div");
                    c.style &&
                    (c.style.backgroundClip = "content-box", c.cloneNode(!0).style.backgroundClip =
                        "", v.clearCloneStyle = "content-box" === c.style.backgroundClip, T.extend(v,
                        {
                            boxSizingReliable: function() { return e(), r },
                            pixelBoxStyles: function() { return e(), a },
                            pixelPosition: function() { return e(), i },
                            reliableMarginLeft: function() { return e(), l },
                            scrollboxSize: function() { return e(), o }
                        }));
                }();
                var ze = ["Webkit", "Moz", "ms"], Xe = s.createElement("div").style, Ge = {};

                function Je(e) {
                    const t = T.cssProps[e] || Ge[e];
                    return t ||
                    (e in Xe
                        ? e
                        : Ge[e] = function(e) {
                            for (var t = e[0].toUpperCase() + e.slice(1), n = ze.length; n--;)
                                if ((e = ze[n] + t) in Xe) return e;
                        }(e) ||
                        e);
                }

                var Ze = /^(none|table(?!-c[ea]).+)/,
                    et = /^--/,
                    tt = { position: "absolute", visibility: "hidden", display: "block" },
                    nt = { letterSpacing: "0", fontWeight: "400" };

                function it(e, t, n) {
                    const i = re.exec(t);
                    return i ? Math.max(0, i[2] - (n || 0)) + (i[3] || "px") : t;
                }

                function rt(e, t, n, i, r, o) {
                    var s = "width" === t ? 1 : 0, a = 0, l = 0;
                    if (n === (i ? "border" : "content")) return 0;
                    for (; s < 4; s += 2)
                        "margin" === n && (l += T.css(e, n + oe[s], !0, r)), i
                            ? ("content" === n && (l -= T.css(e, `padding${oe[s]}`, !0, r)), "margin" !== n &&
                                (l -= T.css(e, `border${oe[s]}Width`, !0, r)))
                            : (l += T.css(e, `padding${oe[s]}`, !0, r), "padding" !== n
                                ? l += T.css(e, `border${oe[s]}Width`, !0, r)
                                : a += T.css(e, `border${oe[s]}Width`, !0, r));
                    return!i &&
                        o >= 0 &&
                        (l += Math.max(0, Math.ceil(e[`offset${t[0].toUpperCase()}${t.slice(1)}`] - o - l - a - .5)) ||
                            0), l;
                }

                function ot(e, t, n) {
                    const i = Ke(e);
                    var r = (!v.boxSizingReliable() || n) && "border-box" === T.css(e, "boxSizing", !1, i),
                        o = r,
                        s = Qe(e, t, i);
                    const a = `offset${t[0].toUpperCase()}${t.slice(1)}`;
                    if (Ve.test(s)) {
                        if (!n) return s;
                        s = "auto";
                    }
                    return(!v.boxSizingReliable() && r ||
                            "auto" === s ||
                            !parseFloat(s) && "inline" === T.css(e, "display", !1, i)) &&
                        e.getClientRects().length &&
                        (r = "border-box" === T.css(e, "boxSizing", !1, i), (o = a in e) && (s = e[a])), (s =
                            parseFloat(s) || 0) +
                        rt(e, t, n || (r ? "border" : "content"), o, i, s) +
                        "px";
                }

                function st(e, t, n, i, r) { return new st.prototype.init(e, t, n, i, r) }

                T.extend({
                        cssHooks: {
                            opacity: {
                                get: function(e, t) {
                                    if (t) {
                                        const n = Qe(e, "opacity");
                                        return"" === n ? "1" : n;
                                    }
                                }
                            }
                        },
                        cssNumber: {
                            animationIterationCount: !0,
                            columnCount: !0,
                            fillOpacity: !0,
                            flexGrow: !0,
                            flexShrink: !0,
                            fontWeight: !0,
                            gridArea: !0,
                            gridColumn: !0,
                            gridColumnEnd: !0,
                            gridColumnStart: !0,
                            gridRow: !0,
                            gridRowEnd: !0,
                            gridRowStart: !0,
                            lineHeight: !0,
                            opacity: !0,
                            order: !0,
                            orphans: !0,
                            widows: !0,
                            zIndex: !0,
                            zoom: !0
                        },
                        cssProps: {},
                        style: function(e, t, n, i) {
                            if (e && 3 !== e.nodeType && 8 !== e.nodeType && e.style) {
                                let r;
                                let o;
                                let s;
                                const a = z(t);
                                const l = et.test(t);
                                const u = e.style;
                                if (l || (t = Je(a)), s =
                                    T.cssHooks[t] || T.cssHooks[a], void 0 === n)
                                    return s && "get" in s && void 0 !== (r = s.get(e, !1, i)) ? r : u[t];
                                "string" === (o = typeof n) &&
                                        (r = re.exec(n)) &&
                                        r[1] &&
                                        (n = fe(e, t, r), o = "number"),
                                    null != n &&
                                        n == n &&
                                        ("number" !== o || l || (n += r && r[3] || (T.cssNumber[a] ? "" : "px")),
                                            v.clearCloneStyle ||
                                                "" !== n ||
                                                0 !== t.indexOf("background") ||
                                                (u[t] = "inherit"), s &&
                                                "set" in s &&
                                                void 0 === (n = s.set(e, n, i)) ||
                                                (l ? u.setProperty(t, n) : u[t] = n));
                            }
                        },
                        css: function(e, t, n, i) {
                            var r, o, s;
                            const a = z(t);
                            return et.test(t) || (t = Je(a)),
                                (s = T.cssHooks[t] || T.cssHooks[a]) && "get" in s && (r = s.get(e, !0, n)),
                                void 0 === r && (r = Qe(e, t, i)), "normal" === r && t in nt && (r = nt[t]), "" === n ||
                                    n
                                    ? (o = parseFloat(r), !0 === n || isFinite(o) ? o || 0 : r)
                                    : r;
                        }
                    }), T.each(["height", "width"],
                        (function(e, t) {
                            T.cssHooks[t] = {
                                get: function(e, n, i) {
                                    if (n)
                                        return!Ze.test(T.css(e, "display")) ||
                                            e.getClientRects().length && e.getBoundingClientRect().width
                                            ? ot(e, t, i)
                                            : ce(e, tt, (function() { return ot(e, t, i) }));
                                },
                                set: function(e, n, i) {
                                    var r;
                                    const o = Ke(e);
                                    const s = !v.scrollboxSize() && "absolute" === o.position;
                                    const a = (s || i) && "border-box" === T.css(e, "boxSizing", !1, o);
                                    var l = i ? rt(e, t, i, a, o) : 0;
                                    return a &&
                                        s &&
                                        (l -= Math.ceil(e[`offset${t[0].toUpperCase()}${t.slice(1)}`] -
                                            parseFloat(o[t]) -
                                            rt(e, t, "border", !1, o) -
                                            .5)), l &&
                                        (r = re.exec(n)) &&
                                        "px" !== (r[3] || "px") &&
                                        (e.style[t] = n, n = T.css(e, t)), it(0, n, l);
                                }
                            };
                        })), T.cssHooks.marginLeft =
                        Ye(v.reliableMarginLeft,
                            (function(e, t) {
                                if (t)
                                    return(parseFloat(Qe(e, "marginLeft")) ||
                                            e.getBoundingClientRect().left -
                                            ce(e,
                                                { marginLeft: 0 },
                                                (function() { return e.getBoundingClientRect().left }))) +
                                        "px";
                            })), T.each({ margin: "", padding: "", border: "Width" },
                        (function(e, t) {
                            T.cssHooks[e + t] = {
                                expand: function(n) {
                                    for (var i = 0, r = {}, o = "string" == typeof n ? n.split(" ") : [n]; i < 4; i++)
                                        r[e + oe[i] + t] = o[i] || o[i - 2] || o[0];
                                    return r;
                                }
                            }, "margin" !== e && (T.cssHooks[e + t].set = it);
                        })), T.fn.extend({
                        css: function(e, t) {
                            return K(this,
                                (function(e, t, n) {
                                    var i, r;
                                    var s = 0;
                                    if (Array.isArray(t)) {
                                        const o = {};
                                        for (i = Ke(e), r = t.length; s < r; s++) o[t[s]] = T.css(e, t[s], !1, i);
                                        return o;
                                    }
                                    return void 0 !== n ? T.style(e, t, n) : T.css(e, t);
                                }),
                                e,
                                t,
                                arguments.length > 1);
                        }
                    }), T.Tween = st, st.prototype = {
                        constructor: st,
                        init: function(e, t, n, i, r, o) {
                            this.elem = e, this.prop = n, this.easing = r || T.easing._default, this.options =
                                t, this.start = this.now = this.cur(), this.end = i, this.unit =
                                o || (T.cssNumber[n] ? "" : "px");
                        },
                        cur: function() {
                            const e = st.propHooks[this.prop];
                            return e && e.get ? e.get(this) : st.propHooks._default.get(this);
                        },
                        run: function(e) {
                            var t;
                            const n = st.propHooks[this.prop];
                            return this.options.duration
                                ? this.pos = t =
                                T.easing[this.easing](e, this.options.duration * e, 0, 1, this.options.duration)
                                : this.pos = t = e, this.now =
                                (this.end - this.start) * t + this.start, this.options.step &&
                                this.options.step.call(this.elem, this.now, this), n && n.set
                                ? n.set(this)
                                : st.propHooks._default.set(this), this;
                        }
                    }, st.prototype.init.prototype = st.prototype, st.propHooks = {
                        _default: {
                            get: function(e) {
                                var t;
                                return 1 !== e.elem.nodeType || null != e.elem[e.prop] && null == e.elem.style[e.prop]
                                    ? e.elem[e.prop]
                                    : (t = T.css(e.elem, e.prop, "")) && "auto" !== t
                                    ? t
                                    : 0;
                            },
                            set: function(e) {
                                T.fx.step[e.prop]
                                    ? T.fx.step[e.prop](e)
                                    : 1 !== e.elem.nodeType || !T.cssHooks[e.prop] && null == e.elem.style[Je(e.prop)]
                                    ? e.elem[e.prop] = e.now
                                    : T.style(e.elem, e.prop, e.now + e.unit);
                            }
                        }
                    }, st.propHooks.scrollTop = st.propHooks.scrollLeft =
                        { set: function(e) { e.elem.nodeType && e.elem.parentNode && (e.elem[e.prop] = e.now) } },
                    T.easing = {
                        linear: function(e) { return e },
                        swing: function(e) { return .5 - Math.cos(e * Math.PI) / 2 },
                        _default: "swing"
                    }, T.fx = st.prototype.init, T.fx.step = {};
                var at, lt, ut = /^(?:toggle|show|hide)$/, ct = /queueHooks$/;

                function ft() {
                    lt &&
                    (!1 === s.hidden && n.requestAnimationFrame
                        ? n.requestAnimationFrame(ft)
                        : n.setTimeout(ft, T.fx.interval), T.fx.tick());
                }

                function dt() { return n.setTimeout((function() { at = void 0 })), at = Date.now() }

                function ht(e, t) {
                    var n, i = 0;
                    const r = { height: e };
                    for (t = t ? 1 : 0; i < 4; i += 2 - t) r[`margin${n = oe[i]}`] = r[`padding${n}`] = e;
                    return t && (r.opacity = r.width = e), r;
                }

                function pt(e, t, n) {
                    for (var i, r = (gt.tweeners[t] || []).concat(gt.tweeners["*"]), o = 0, s = r.length; o < s; o++)
                        if (i = r[o].call(n, t, e)) return i;
                }

                function gt(e, t, n) {
                    var i, r, o = 0;
                    const s = gt.prefilters.length;
                    var a = T.Deferred().always((function() { delete l.elem })),
                        l = function() {
                            if (r) return!1;
                            for (var t = at || dt(),
                                n = Math.max(0, u.startTime + u.duration - t),
                                i = 1 - (n / u.duration || 0),
                                o = 0,
                                s = u.tweens.length;
                                o < s;
                                o++) u.tweens[o].run(i);
                            return a.notifyWith(e, [u, i, n]), i < 1 && s
                                ? n
                                : (s || a.notifyWith(e, [u, 1, 0]), a.resolveWith(e, [u]), !1);
                        },
                        u = a.promise({
                            elem: e,
                            props: T.extend({}, t),
                            opts: T.extend(!0, { specialEasing: {}, easing: T.easing._default }, n),
                            originalProperties: t,
                            originalOptions: n,
                            startTime: at || dt(),
                            duration: n.duration,
                            tweens: [],
                            createTween: function(t, n) {
                                const i = T.Tween(e, u.opts, t, n, u.opts.specialEasing[t] || u.opts.easing);
                                return u.tweens.push(i), i;
                            },
                            stop: function(t) {
                                var n = 0;
                                const i = t ? u.tweens.length : 0;
                                if (r) return this;
                                for (r = !0; n < i; n++) u.tweens[n].run(1);
                                return t
                                    ? (a.notifyWith(e, [u, 1, 0]), a.resolveWith(e, [u, t]))
                                    : a.rejectWith(e, [u, t]), this;
                            }
                        });
                    const c = u.props;
                    for (!function(e, t) {
                            var n, i, r, o, s;
                            for (n in e)
                                if (r = t[i = z(n)], o =
                                        e[n], Array.isArray(o) && (r = o[1], o = e[n] = o[0]),
                                    n !== i && (e[i] = o, delete e[n]), (s = T.cssHooks[i]) && "expand" in s)
                                    for (n in o = s.expand(o), delete e[i], o) n in e || (e[n] = o[n], t[n] = r);
                                else t[i] = r;
                        }(c, u.opts.specialEasing);
                        o < s;
                        o++)
                        if (i = gt.prefilters[o].call(u, e, c, u.opts))
                            return y(i.stop) && (T._queueHooks(u.elem, u.opts.queue).stop = i.stop.bind(i)), i;
                    return T.map(c, pt, u), y(u.opts.start) && u.opts.start.call(e, u), u.progress(u.opts.progress)
                        .done(u.opts.done, u.opts.complete).fail(u.opts.fail)
                        .always(u.opts.always), T.fx.timer(T.extend(l, { elem: e, anim: u, queue: u.opts.queue })), u;
                }

                T.Animation = T.extend(gt,
                    {
                        tweeners: {
                            "*": [
                                function(e, t) {
                                    const n = this.createTween(e, t);
                                    return fe(n.elem, e, re.exec(t), n), n;
                                }
                            ]
                        },
                        tweener: function(e, t) {
                            y(e) ? (t = e, e = ["*"]) : e = e.match(M);
                            for (var n, i = 0, r = e.length; i < r; i++)
                                n = e[i], gt.tweeners[n] = gt.tweeners[n] || [], gt.tweeners[n].unshift(t);
                        },
                        prefilters: [
                            function(e, t, n) {
                                var i,
                                    r,
                                    o,
                                    s,
                                    a,
                                    l,
                                    u,
                                    c,
                                    f = "width" in t || "height" in t,
                                    d = this,
                                    h = {},
                                    p = e.style,
                                    g = e.nodeType && ue(e),
                                    m = J.get(e, "fxshow");
                                for (i in n.queue ||
                                (null == (s = T._queueHooks(e, "fx")).unqueued &&
                                    (s.unqueued = 0, a = s.empty.fire, s.empty.fire = function() {
                                        s.unqueued || a();
                                    }),
                                    s.unqueued++, d.always((function() {
                                        d.always((function() {
                                            s.unqueued--, T.queue(e, "fx").length || s.empty.fire();
                                        }));
                                    }))), t)
                                    if (r = t[i], ut.test(r)) {
                                        if (delete t[i], o = o || "toggle" === r, r === (g ? "hide" : "show")) {
                                            if ("show" !== r || !m || void 0 === m[i]) continue;
                                            g = !0;
                                        }
                                        h[i] = m && m[i] || T.style(e, i);
                                    }
                                if ((l = !T.isEmptyObject(t)) || !T.isEmptyObject(h))
                                    for (i in f &&
                                            1 === e.nodeType &&
                                            (n.overflow =
                                                    [p.overflow, p.overflowX, p.overflowY],
                                                null == (u = m && m.display) && (u = J.get(e, "display")),
                                                "none" === (c = T.css(e, "display")) &&
                                                (u
                                                    ? c = u
                                                    : (pe([e], !0), u = e.style.display || u, c =
                                                        T.css(e, "display"), pe([e]))), (
                                                        "inline" === c || "inline-block" === c && null != u) &&
                                                    "none" === T.css(e, "float") &&
                                                    (l ||
                                                    (d.done((function() { p.display = u })), null == u &&
                                                        (c = p.display, u = "none" === c ? "" : c)), p.display =
                                                        "inline-block")),
                                        n.overflow &&
                                        (p.overflow = "hidden", d.always((function() {
                                            p.overflow = n.overflow[0], p.overflowX = n.overflow[1], p.overflowY =
                                                n.overflow[2];
                                        }))), l = !1, h)
                                        l ||
                                        (m
                                                ? "hidden" in m && (g = m.hidden)
                                                : m = J.access(e, "fxshow", { display: u }), o && (m.hidden = !g),
                                            g && pe([e], !0), d.done((function() {
                                                for (i in g || pe([e]), J.remove(e, "fxshow"), h) T.style(e, i, h[i]);
                                            }))), l = pt(g ? m[i] : 0, i, d), i in m ||
                                            (m[i] = l.start, g && (l.end = l.start, l.start = 0));
                            }
                        ],
                        prefilter: function(e, t) { t ? gt.prefilters.unshift(e) : gt.prefilters.push(e) }
                    }), T.speed = function(e, t, n) {
                    var i = e && "object" == typeof e
                        ? T.extend({}, e)
                        : { complete: n || !n && t || y(e) && e, duration: e, easing: n && t || t && !y(t) && t };
                    return T.fx.off
                            ? i.duration = 0
                            : "number" != typeof i.duration &&
                            (i.duration in T.fx.speeds
                                ? i.duration = T.fx.speeds[i.duration]
                                : i.duration = T.fx.speeds._default), null != i.queue && !0 !== i.queue ||
                            (i.queue = "fx"),
                        i.old = i.complete, i.complete = function() {
                            y(i.old) && i.old.call(this), i.queue && T.dequeue(this, i.queue);
                        }, i;
                }, T.fn.extend({
                    fadeTo: function(e, t, n, i) {
                        return this.filter(ue).css("opacity", 0).show().end().animate({ opacity: t }, e, n, i);
                    },
                    animate: function(e, t, n, i) {
                        var r = T.isEmptyObject(e), o = T.speed(t, n, i);
                        const s = function() {
                            const t = gt(this, T.extend({}, e), o);
                            (r || J.get(this, "finish")) && t.stop(!0);
                        };
                        return s.finish = s, r || !1 === o.queue ? this.each(s) : this.queue(o.queue, s);
                    },
                    stop: function(e, t, n) {
                        var i = function(e) {
                            const t = e.stop;
                            delete e.stop, t(n);
                        };
                        return"string" != typeof e && (n = t, t = e, e = void 0),
                            t && !1 !== e && this.queue(e || "fx", []), this.each((function() {
                                var t = !0, r = null != e && e + "queueHooks";
                                const o = T.timers;
                                const s = J.get(this);
                                if (r) s[r] && s[r].stop && i(s[r]);
                                else for (r in s) s[r] && s[r].stop && ct.test(r) && i(s[r]);
                                for (r = o.length; r--;)
                                    o[r].elem !== this ||
                                        null != e && o[r].queue !== e ||
                                        (o[r].anim.stop(n), t = !1, o.splice(r, 1));
                                !t && n || T.dequeue(this, e);
                            }));
                    },
                    finish: function(e) {
                        return!1 !== e && (e = e || "fx"), this.each((function() {
                            var t;
                            const n = J.get(this);
                            const i = n[e + "queue"];
                            const r = n[e + "queueHooks"];
                            const o = T.timers;
                            const s = i ? i.length : 0;
                            for (n.finish = !0, T.queue(this, e, []), r && r.stop && r.stop.call(this, !0), t =
                                    o.length;
                                t--;
                            ) o[t].elem === this && o[t].queue === e && (o[t].anim.stop(!0), o.splice(t, 1));
                            for (t = 0; t < s; t++) i[t] && i[t].finish && i[t].finish.call(this);
                            delete n.finish;
                        }));
                    }
                }), T.each(["toggle", "show", "hide"],
                    (function(e, t) {
                        var n = T.fn[t];
                        T.fn[t] = function(e, i, r) {
                            return null == e || "boolean" == typeof e
                                ? n.apply(this, arguments)
                                : this.animate(ht(t, !0), e, i, r);
                        };
                    })), T.each(
                    {
                        slideDown: ht("show"),
                        slideUp: ht("hide"),
                        slideToggle: ht("toggle"),
                        fadeIn: { opacity: "show" },
                        fadeOut: { opacity: "hide" },
                        fadeToggle: { opacity: "toggle" }
                    },
                    (function(e, t) { T.fn[e] = function(e, n, i) { return this.animate(t, e, n, i) } })), T.timers =
                    [], T.fx.tick = function() {
                    var e, t = 0;
                    const n = T.timers;
                    for (at = Date.now(); t < n.length; t++) (e = n[t])() || n[t] !== e || n.splice(t--, 1);
                    n.length || T.fx.stop(), at = void 0;
                }, T.fx.timer = function(e) { T.timers.push(e), T.fx.start() }, T.fx.interval = 13, T.fx.start =
                    function() { lt || (lt = !0, ft()) }, T.fx.stop = function() { lt = null }, T.fx.speeds =
                    { slow: 600, fast: 200, _default: 400 }, T.fn.delay = function(e, t) {
                    return e = T.fx && T.fx.speeds[e] || e, t = t || "fx", this.queue(t,
                        (function(t, i) {
                            var r = n.setTimeout(t, e);
                            i.stop = function() { n.clearTimeout(r) };
                        }));
                }, function() {
                    var e = s.createElement("input");
                    const t = s.createElement("select").appendChild(s.createElement("option"));
                    e.type = "checkbox", v.checkOn = "" !== e.value, v.optSelected =
                        t.selected, (e = s.createElement("input")).value = "t", e.type = "radio", v.radioValue =
                        "t" === e.value;
                }();
                var mt, vt = T.expr.attrHandle;
                T.fn.extend({
                        attr: function(e, t) { return K(this, T.attr, e, t, arguments.length > 1) },
                        removeAttr: function(e) { return this.each((function() { T.removeAttr(this, e) })) }
                    }), T.extend({
                        attr: function(e, t, n) {
                            var i, r;
                            const o = e.nodeType;
                            if (3 !== o && 8 !== o && 2 !== o)
                                return void 0 === e.getAttribute
                                    ? T.prop(e, t, n)
                                    : (1 === o && T.isXMLDoc(e) ||
                                        (r = T.attrHooks[t.toLowerCase()] ||
                                            (T.expr.match.bool.test(t) ? mt : void 0)),
                                        void 0 !== n
                                            ? null === n
                                            ? void T.removeAttr(e, t)
                                            : r && "set" in r && void 0 !== (i = r.set(e, n, t))
                                            ? i
                                            : (e.setAttribute(t, n + ""), n)
                                            : r && "get" in r && null !== (i = r.get(e, t))
                                            ? i
                                            : null == (i = T.find.attr(e, t))
                                            ? void 0
                                            : i);
                        },
                        attrHooks: {
                            type: {
                                set: function(e, t) {
                                    if (!v.radioValue && "radio" === t && I(e, "input")) {
                                        const n = e.value;
                                        return e.setAttribute("type", t), n && (e.value = n), t;
                                    }
                                }
                            }
                        },
                        removeAttr: function(e, t) {
                            var n, i = 0;
                            const r = t && t.match(M);
                            if (r && 1 === e.nodeType) for (; n = r[i++];) e.removeAttribute(n);
                        }
                    }), mt =
                        { set: function(e, t, n) { return!1 === t ? T.removeAttr(e, n) : e.setAttribute(n, n), n } },
                    T.each(T.expr.match.bool.source.match(/\w+/g),
                        (function(e, t) {
                            var n = vt[t] || T.find.attr;
                            vt[t] = function(e, t, i) {
                                var r, o;
                                const s = t.toLowerCase();
                                return i || (o = vt[s], vt[s] = r, r = null != n(e, t, i) ? s : null, vt[s] = o), r;
                            };
                        }));
                var yt = /^(?:input|select|textarea|button)$/i, bt = /^(?:a|area)$/i;

                function _t(e) { return(e.match(M) || []).join(" ") }

                function Et(e) { return e.getAttribute && e.getAttribute("class") || "" }

                function wt(e) { return Array.isArray(e) ? e : "string" == typeof e && e.match(M) || [] }

                T.fn.extend({
                    prop: function(e, t) { return K(this, T.prop, e, t, arguments.length > 1) },
                    removeProp: function(e) { return this.each((function() { delete this[T.propFix[e] || e] })) }
                }), T.extend({
                    prop: function(e, t, n) {
                        var i, r;
                        const o = e.nodeType;
                        if (3 !== o && 8 !== o && 2 !== o)
                            return 1 === o && T.isXMLDoc(e) || (t = T.propFix[t] || t, r = T.propHooks[t]), void 0 !== n
                                ? r && "set" in r && void 0 !== (i = r.set(e, n, t))
                                ? i
                                : e[t] = n
                                : r && "get" in r && null !== (i = r.get(e, t))
                                ? i
                                : e[t];
                    },
                    propHooks: {
                        tabIndex: {
                            get: function(e) {
                                const t = T.find.attr(e, "tabindex");
                                return t
                                    ? parseInt(t, 10)
                                    : yt.test(e.nodeName) || bt.test(e.nodeName) && e.href
                                    ? 0
                                    : -1;
                            }
                        }
                    },
                    propFix: { for: "htmlFor", class: "className" }
                }), v.optSelected ||
                (T.propHooks.selected = {
                    get: function(e) {
                        const t = e.parentNode;
                        return t && t.parentNode && t.parentNode.selectedIndex, null;
                    },
                    set: function(e) {
                        const t = e.parentNode;
                        t && (t.selectedIndex, t.parentNode && t.parentNode.selectedIndex);
                    }
                }), T.each([
                        "tabIndex", "readOnly", "maxLength", "cellSpacing", "cellPadding", "rowSpan", "colSpan",
                        "useMap",
                        "frameBorder", "contentEditable"
                    ],
                    (function() { T.propFix[this.toLowerCase()] = this })), T.fn.extend({
                    addClass: function(e) {
                        var t, n, i, r, o, s, a, l = 0;
                        if (y(e)) return this.each((function(t) { T(this).addClass(e.call(this, t, Et(this))) }));
                        if ((t = wt(e)).length)
                            for (; n = this[l++];)
                                if (r = Et(n), i = 1 === n.nodeType && " " + _t(r) + " ") {
                                    for (s = 0; o = t[s++];) i.indexOf(` ${o} `) < 0 && (i += o + " ");
                                    r !== (a = _t(i)) && n.setAttribute("class", a);
                                }
                        return this;
                    },
                    removeClass: function(e) {
                        var t, n, i, r, o, s, a, l = 0;
                        if (y(e)) return this.each((function(t) { T(this).removeClass(e.call(this, t, Et(this))) }));
                        if (!arguments.length) return this.attr("class", "");
                        if ((t = wt(e)).length)
                            for (; n = this[l++];)
                                if (r = Et(n), i = 1 === n.nodeType && " " + _t(r) + " ") {
                                    for (s = 0; o = t[s++];)
                                        for (; i.indexOf(` ${o} `) > -1;) i = i.replace(` ${o} `, " ");
                                    r !== (a = _t(i)) && n.setAttribute("class", a);
                                }
                        return this;
                    },
                    toggleClass: function(e, t) {
                        var n = typeof e, i = "string" === n || Array.isArray(e);
                        return"boolean" == typeof t && i
                            ? t
                            ? this.addClass(e)
                            : this.removeClass(e)
                            : y(e)
                            ? this.each((function(n) { T(this).toggleClass(e.call(this, n, Et(this), t), t) }))
                            : this.each((function() {
                                var t, r, o, s;
                                if (i)
                                    for (r = 0, o = T(this), s = wt(e); t = s[r++];)
                                        o.hasClass(t) ? o.removeClass(t) : o.addClass(t);
                                else
                                    void 0 !== e && "boolean" !== n ||
                                    ((t = Et(this)) && J.set(this, "__className__", t), this.setAttribute &&
                                        this.setAttribute("class",
                                            t || !1 === e ? "" : J.get(this, "__className__") || ""));
                            }));
                    },
                    hasClass: function(e) {
                        var t, n, i = 0;
                        for (t = ` ${e} `; n = this[i++];)
                            if (1 === n.nodeType && (` ${_t(Et(n))} `).indexOf(t) > -1) return!0;
                        return!1;
                    }
                });
                var Tt = /\r/g;
                T.fn.extend({
                    val: function(e) {
                        var t, n, i;
                        const r = this[0];
                        return arguments.length
                            ? (i = y(e), this.each((function(n) {
                                var r;
                                1 === this.nodeType &&
                                (null == (r = i ? e.call(this, n, T(this).val()) : e)
                                    ? r = ""
                                    : "number" == typeof r
                                    ? r += ""
                                    : Array.isArray(r) &&
                                    (r = T.map(r, (function(e) { return null == e ? "" : e + "" }))), (t =
                                        T.valHooks[this.type] || T.valHooks[this.nodeName.toLowerCase()]) &&
                                    "set" in t &&
                                    void 0 !== t.set(this, r, "value") ||
                                    (this.value = r));
                            })))
                            : r
                            ? (t = T.valHooks[r.type] || T.valHooks[r.nodeName.toLowerCase()]) &&
                            "get" in t &&
                            void 0 !== (n = t.get(r, "value"))
                            ? n
                            : "string" == typeof(n = r.value)
                            ? n.replace(Tt, "")
                            : null == n
                            ? ""
                            : n
                            : void 0;
                    }
                }), T.extend({
                    valHooks: {
                        option: {
                            get: function(e) {
                                const t = T.find.attr(e, "value");
                                return null != t ? t : _t(T.text(e));
                            }
                        },
                        select: {
                            get: function(e) {
                                var t, n, i;
                                const r = e.options;
                                const o = e.selectedIndex;
                                const s = "select-one" === e.type;
                                const a = s ? null : [];
                                const l = s ? o + 1 : r.length;
                                for (i = o < 0 ? l : s ? o : 0; i < l; i++)
                                    if (((n = r[i]).selected || i === o) &&
                                        !n.disabled &&
                                        (!n.parentNode.disabled || !I(n.parentNode, "optgroup"))) {
                                        if (t = T(n).val(), s) return t;
                                        a.push(t);
                                    }
                                return a;
                            },
                            set: function(e, t) {
                                for (var n, i, r = e.options, o = T.makeArray(t), s = r.length; s--;)
                                    ((i = r[s]).selected = T.inArray(T.valHooks.option.get(i), o) > -1) && (n = !0);
                                return n || (e.selectedIndex = -1), o;
                            }
                        }
                    }
                }), T.each(["radio", "checkbox"],
                    (function() {
                        T.valHooks[this] =
                        {
                            set: function(e, t) {
                                if (Array.isArray(t)) return e.checked = T.inArray(T(e).val(), t) > -1;
                            }
                        }, v.checkOn ||
                        (T.valHooks[this].get = function(e) {
                            return null === e.getAttribute("value") ? "on" : e.value;
                        });
                    })), v.focusin = "onfocusin" in n;
                var Ct = /^(?:focusinfocus|focusoutblur)$/, xt = function(e) { e.stopPropagation() };
                T.extend(T.event,
                    {
                        trigger: function(e, t, i, r) {
                            var o,
                                a,
                                l,
                                u,
                                c,
                                f,
                                d,
                                h,
                                g = [i || s],
                                m = p.call(e, "type") ? e.type : e,
                                v = p.call(e, "namespace") ? e.namespace.split(".") : [];
                            if (a = h = l = i = i || s, 3 !== i.nodeType &&
                                8 !== i.nodeType &&
                                !Ct.test(m + T.event.triggered) &&
                                (m.indexOf(".") > -1 && (v = m.split("."), m = v.shift(), v.sort()), c =
                                        m.indexOf(":") < 0 && "on" + m,
                                    (e = e[T.expando] ? e : new T.Event(m, "object" == typeof e && e)).isTrigger =
                                        r ? 2 : 3, e.namespace = v.join("."), e.rnamespace =
                                        e.namespace ? new RegExp(`(^|\\.)${v.join("\\.(?:.*\\.|)")}(\\.|$)`) : null,
                                    e.result = void 0, e.target || (e.target = i), t =
                                        null == t ? [e] : T.makeArray(t, [e]), d =
                                        T.event.special[m] || {}, r || !d.trigger || !1 !== d.trigger.apply(i, t))) {
                                if (!r && !d.noBubble && !b(i)) {
                                    for (u = d.delegateType || m, Ct.test(u + m) || (a = a.parentNode);
                                        a;
                                        a = a.parentNode) g.push(a), l = a;
                                    l === (i.ownerDocument || s) && g.push(l.defaultView || l.parentWindow || n);
                                }
                                for (o = 0; (a = g[o++]) && !e.isPropagationStopped();)
                                    h = a, e.type =
                                        o > 1 ? u : d.bindType || m, (f =
                                            (J.get(a, "events") || {})[e.type] && J.get(a, "handle")) &&
                                        f.apply(a, t), (f = c && a[c]) &&
                                        f.apply &&
                                        X(a) &&
                                        (e.result = f.apply(a, t), !1 === e.result && e.preventDefault());
                                return e.type =
                                    m, r ||
                                    e.isDefaultPrevented() ||
                                    d._default && !1 !== d._default.apply(g.pop(), t) ||
                                    !X(i) ||
                                    c &&
                                    y(i[m]) &&
                                    !b(i) &&
                                    ((l = i[c]) && (i[c] = null), T.event.triggered =
                                            m, e.isPropagationStopped() && h.addEventListener(m, xt), i[m](),
                                        e.isPropagationStopped() && h.removeEventListener(m, xt), T.event.triggered =
                                            void 0, l && (i[c] = l)), e.result;
                            }
                        },
                        simulate: function(e, t, n) {
                            const i = T.extend(new T.Event, n, { type: e, isSimulated: !0 });
                            T.event.trigger(i, null, t);
                        }
                    }), T.fn.extend({
                    trigger: function(e, t) { return this.each((function() { T.event.trigger(e, t, this) })) },
                    triggerHandler: function(e, t) {
                        const n = this[0];
                        if (n) return T.event.trigger(e, t, n, !0);
                    }
                }), v.focusin ||
                    T.each({ focus: "focusin", blur: "focusout" },
                        (function(e, t) {
                            var n = function(e) { T.event.simulate(t, e.target, T.event.fix(e)) };
                            T.event.special[t] = {
                                setup: function() {
                                    const i = this.ownerDocument || this;
                                    const r = J.access(i, t);
                                    r || i.addEventListener(e, n, !0), J.access(i, t, (r || 0) + 1);
                                },
                                teardown: function() {
                                    const i = this.ownerDocument || this;
                                    const r = J.access(i, t) - 1;
                                    r ? J.access(i, t, r) : (i.removeEventListener(e, n, !0), J.remove(i, t));
                                }
                            };
                        }));
                var St = n.location, Dt = Date.now(), At = /\?/;
                T.parseXML = function(e) {
                    var t;
                    if (!e || "string" != typeof e) return null;
                    try {
                        t = (new n.DOMParser).parseFromString(e, "text/xml");
                    } catch (e) {
                        t = void 0;
                    }
                    return t && !t.getElementsByTagName("parsererror").length || T.error(`Invalid XML: ${e}`), t;
                };
                var Nt = /\[\]$/,
                    It = /\r?\n/g,
                    Ot = /^(?:submit|button|image|reset|file)$/i,
                    kt = /^(?:input|select|textarea|keygen)/i;

                function Lt(e, t, n, i) {
                    var r;
                    if (Array.isArray(t))
                        T.each(t,
                            (function(t, r) {
                                n || Nt.test(e)
                                    ? i(e, r)
                                    : Lt(e + "[" + ("object" == typeof r && null != r ? t : "") + "]", r, n, i);
                            }));
                    else if (n || "object" !== w(t)) i(e, t);
                    else for (r in t) Lt(e + "[" + r + "]", t[r], n, i);
                }

                T.param = function(e, t) {
                    var n,
                        i = [],
                        r = function(e, t) {
                            const n = y(t) ? t() : t;
                            i[i.length] = encodeURIComponent(e) + "=" + encodeURIComponent(null == n ? "" : n);
                        };
                    if (null == e) return"";
                    if (Array.isArray(e) || e.jquery && !T.isPlainObject(e))
                        T.each(e, (function() { r(this.name, this.value) }));
                    else for (n in e) Lt(n, e[n], t, r);
                    return i.join("&");
                }, T.fn.extend({
                    serialize: function() { return T.param(this.serializeArray()) },
                    serializeArray: function() {
                        return this.map((function() {
                            const e = T.prop(this, "elements");
                            return e ? T.makeArray(e) : this;
                        })).filter((function() {
                            const e = this.type;
                            return this.name &&
                                !T(this).is(":disabled") &&
                                kt.test(this.nodeName) &&
                                !Ot.test(e) &&
                                (this.checked || !ge.test(e));
                        })).map((function(e, t) {
                            const n = T(this).val();
                            return null == n
                                ? null
                                : Array.isArray(n)
                                ? T.map(n, (function(e) { return{ name: t.name, value: e.replace(It, "\r\n") } }))
                                : { name: t.name, value: n.replace(It, "\r\n") };
                        })).get();
                    }
                });
                var jt = /%20/g,
                    Pt = /#.*$/,
                    Ht = /([?&])_=[^&]*/,
                    Rt = /^(.*?):[ \t]*([^\r\n]*)$/gm,
                    Mt = /^(?:GET|HEAD)$/,
                    qt = /^\/\//,
                    Ft = {},
                    Wt = {},
                    Bt = "*/".concat("*"),
                    Ut = s.createElement("a");

                function Vt(e) {
                    return function(t, n) {
                        "string" != typeof t && (n = t, t = "*");
                        var i, r = 0;
                        const o = t.toLowerCase().match(M) || [];
                        if (y(n))
                            for (; i = o[r++];)
                                "+" === i[0]
                                    ? (i = i.slice(1) || "*", (e[i] = e[i] || []).unshift(n))
                                    : (e[i] = e[i] || []).push(n);
                    };
                }

                function Kt(e, t, n, i) {
                    var r = {}, o = e === Wt;

                    function s(a) {
                        var l;
                        return r[a] = !0, T.each(e[a] || [],
                            (function(e, a) {
                                const u = a(t, n, i);
                                return"string" != typeof u || o || r[u]
                                    ? o
                                    ? !(l = u)
                                    : void 0
                                    : (t.dataTypes.unshift(u), s(u), !1);
                            })), l;
                    }

                    return s(t.dataTypes[0]) || !r["*"] && s("*");
                }

                function $t(e, t) {
                    var n, i;
                    const r = T.ajaxSettings.flatOptions || {};
                    for (n in t) void 0 !== t[n] && ((r[n] ? e : i || (i = {}))[n] = t[n]);
                    return i && T.extend(!0, e, i), e;
                }

                Ut.href = St.href, T.extend({
                    active: 0,
                    lastModified: {},
                    etag: {},
                    ajaxSettings: {
                        url: St.href,
                        type: "GET",
                        isLocal: /^(?:about|app|app-storage|.+-extension|file|res|widget):$/.test(St.protocol),
                        global: !0,
                        processData: !0,
                        async: !0,
                        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                        accepts: {
                            "*": Bt,
                            text: "text/plain",
                            html: "text/html",
                            xml: "application/xml, text/xml",
                            json: "application/json, text/javascript"
                        },
                        contents: { xml: /\bxml\b/, html: /\bhtml/, json: /\bjson\b/ },
                        responseFields: { xml: "responseXML", text: "responseText", json: "responseJSON" },
                        converters: {
                            "* text": String,
                            "text html": !0,
                            "text json": JSON.parse,
                            "text xml": T.parseXML
                        },
                        flatOptions: { url: !0, context: !0 }
                    },
                    ajaxSetup: function(e, t) { return t ? $t($t(e, T.ajaxSettings), t) : $t(T.ajaxSettings, e) },
                    ajaxPrefilter: Vt(Ft),
                    ajaxTransport: Vt(Wt),
                    ajax: function(e, t) {
                        "object" == typeof e && (t = e, e = void 0), t = t || {};
                        var i,
                            r,
                            o,
                            a,
                            l,
                            u,
                            c,
                            f,
                            d,
                            h,
                            p = T.ajaxSetup({}, t),
                            g = p.context || p,
                            m = p.context && (g.nodeType || g.jquery) ? T(g) : T.event,
                            v = T.Deferred(),
                            y = T.Callbacks("once memory"),
                            b = p.statusCode || {},
                            _ = {},
                            E = {},
                            w = "canceled",
                            C = {
                                readyState: 0,
                                getResponseHeader: function(e) {
                                    var t;
                                    if (c) {
                                        if (!a)
                                            for (a = {}; t = Rt.exec(o);)
                                                a[t[1].toLowerCase() + " "] =
                                                    (a[t[1].toLowerCase() + " "] || []).concat(t[2]);
                                        t = a[e.toLowerCase() + " "];
                                    }
                                    return null == t ? null : t.join(", ");
                                },
                                getAllResponseHeaders: function() { return c ? o : null },
                                setRequestHeader: function(e, t) {
                                    return null == c && (e = E[e.toLowerCase()] = E[e.toLowerCase()] || e, _[e] = t),
                                        this;
                                },
                                overrideMimeType: function(e) { return null == c && (p.mimeType = e), this },
                                statusCode: function(e) {
                                    var t;
                                    if (e)
                                        if (c) C.always(e[C.status]);
                                        else for (t in e) b[t] = [b[t], e[t]];
                                    return this;
                                },
                                abort: function(e) {
                                    const t = e || w;
                                    return i && i.abort(t), x(0, t), this;
                                }
                            };
                        if (v.promise(C), p.url =
                            ((e || p.url || St.href) + "").replace(qt, St.protocol + "//"), p.type =
                            t.method || t.type || p.method || p.type, p.dataTypes =
                            (p.dataType || "*").toLowerCase().match(M) || [""], null == p.crossDomain) {
                            u = s.createElement("a");
                            try {
                                u.href = p.url, u.href = u.href, p.crossDomain =
                                    Ut.protocol + "//" + Ut.host != u.protocol + "//" + u.host;
                            } catch (e) {
                                p.crossDomain = !0;
                            }
                        }
                        if (p.data &&
                            p.processData &&
                            "string" != typeof p.data &&
                            (p.data = T.param(p.data, p.traditional)), Kt(Ft, p, t, C), c) return C;
                        for (d in(f = T.event && p.global) && 0 == T.active++ && T.event.trigger("ajaxStart"), p.type =
                                p.type.toUpperCase(), p.hasContent = !Mt.test(p.type), r =
                                p.url.replace(Pt, ""),
                            p.hasContent
                                ? p.data &&
                                p.processData &&
                                0 === (p.contentType || "").indexOf("application/x-www-form-urlencoded") &&
                                (p.data = p.data.replace(jt, "+"))
                                : (h = p.url.slice(r.length), p.data &&
                                    (p.processData || "string" == typeof p.data) &&
                                    (r += (At.test(r) ? "&" : "?") + p.data, delete p.data), !1 === p.cache &&
                                    (r = r.replace(Ht, "$1"), h = (At.test(r) ? "&" : "?") + "_=" + Dt++ + h), p.url =
                                    r + h), p.ifModified &&
                            (T.lastModified[r] && C.setRequestHeader("If-Modified-Since", T.lastModified[r]),
                                T.etag[r] && C.setRequestHeader("If-None-Match", T.etag[r])),
                            (p.data && p.hasContent && !1 !== p.contentType || t.contentType) &&
                                C.setRequestHeader("Content-Type", p.contentType), C.setRequestHeader("Accept",
                                p.dataTypes[0] && p.accepts[p.dataTypes[0]]
                                ? p.accepts[p.dataTypes[0]] + ("*" !== p.dataTypes[0] ? `, ${Bt}; q=0.01` : "")
                                : p.accepts["*"]), p.headers) C.setRequestHeader(d, p.headers[d]);
                        if (p.beforeSend && (!1 === p.beforeSend.call(g, C, p) || c)) return C.abort();
                        if (w = "abort", y.add(p.complete), C.done(p.success), C.fail(p.error), i = Kt(Wt, p, t, C)) {
                            if (C.readyState = 1, f && m.trigger("ajaxSend", [C, p]), c) return C;
                            p.async &&
                                p.timeout > 0 &&
                                (l = n.setTimeout((function() { C.abort("timeout") }), p.timeout));
                            try {
                                c = !1, i.send(_, x);
                            } catch (e) {
                                if (c) throw e;
                                x(-1, e);
                            }
                        } else x(-1, "No Transport");

                        function x(e, t, s, a) {
                            var u, d, h, _, E, w = t;
                            c ||
                            (c = !0, l && n.clearTimeout(l), i = void 0, o = a || "", C.readyState = e > 0 ? 4 : 0, u =
                                    e >= 200 && e < 300 || 304 === e, s &&
                                (_ = function(e, t, n) {
                                    for (var i, r, o, s, a = e.contents, l = e.dataTypes; "*" === l[0];)
                                        l.shift(), void 0 === i &&
                                            (i = e.mimeType || t.getResponseHeader("Content-Type"));
                                    if (i)
                                        for (r in a)
                                            if (a[r] && a[r].test(i)) {
                                                l.unshift(r);
                                                break;
                                            }
                                    if (l[0] in n) o = l[0];
                                    else {
                                        for (r in n) {
                                            if (!l[0] || e.converters[r + " " + l[0]]) {
                                                o = r;
                                                break;
                                            }
                                            s || (s = r);
                                        }
                                        o = o || s;
                                    }
                                    if (o) return o !== l[0] && l.unshift(o), n[o];
                                }(p, C, s)), _ = function(e, t, n, i) {
                                    var r, o, s, a, l;
                                    const u = {};
                                    const c = e.dataTypes.slice();
                                    if (c[1]) for (s in e.converters) u[s.toLowerCase()] = e.converters[s];
                                    for (o = c.shift(); o;)
                                        if (e.responseFields[o] && (n[e.responseFields[o]] = t), !l &&
                                            i &&
                                            e.dataFilter &&
                                            (t = e.dataFilter(t, e.dataType)), l = o, o = c.shift())
                                            if ("*" === o) o = l;
                                            else if ("*" !== l && l !== o) {
                                                if (!(s = u[l + " " + o] || u[`* ${o}`]))
                                                    for (r in u)
                                                        if ((a = r.split(" "))[1] === o &&
                                                            (s = u[l + " " + a[0]] || u[`* ${a[0]}`])) {
                                                            !0 === s
                                                                ? s = u[r]
                                                                : !0 !== u[r] && (o = a[0], c.unshift(a[1]));
                                                            break;
                                                        }
                                                if (!0 !== s)
                                                    if (s && e.throws) t = s(t);
                                                    else
                                                        try {
                                                            t = s(t);
                                                        } catch (e) {
                                                            return{
                                                                state: "parsererror",
                                                                error: s ? e : `No conversion from ${l} to ${o}`
                                                            };
                                                        }
                                            }
                                    return{ state: "success", data: t };
                                }(p, _, C, u), u
                                    ? (p.ifModified &&
                                    ((E = C.getResponseHeader("Last-Modified")) && (T.lastModified[r] = E), (E =
                                            C.getResponseHeader("etag")) &&
                                        (T.etag[r] = E)), 204 === e || "HEAD" === p.type
                                        ? w = "nocontent"
                                        : 304 === e
                                        ? w = "notmodified"
                                        : (w = _.state, d = _.data, u = !(h = _.error)))
                                    : (h = w, !e && w || (w = "error", e < 0 && (e = 0))), C.status = e, C.statusText =
                                    (t || w) + "", u ? v.resolveWith(g, [d, w, C]) : v.rejectWith(g, [C, w, h]),
                                C.statusCode(b), b =
                                    void 0, f && m.trigger(u ? "ajaxSuccess" : "ajaxError", [C, p, u ? d : h]),
                                y.fireWith(g, [C, w]), f &&
                                    (m.trigger("ajaxComplete", [C, p]), --T.active || T.event.trigger("ajaxStop")));
                        }

                        return C;
                    },
                    getJSON: function(e, t, n) { return T.get(e, t, n, "json") },
                    getScript: function(e, t) { return T.get(e, void 0, t, "script") }
                }), T.each(["get", "post"],
                    (function(e, t) {
                        T[t] = function(e, n, i, r) {
                            return y(n) && (r = r || i, i = n, n = void 0), T.ajax(
                                T.extend({ url: e, type: t, dataType: r, data: n, success: i },
                                    T.isPlainObject(e) && e));
                        };
                    })), T._evalUrl = function(e, t) {
                    return T.ajax({
                        url: e,
                        type: "GET",
                        dataType: "script",
                        cache: !0,
                        async: !1,
                        global: !1,
                        converters: { "text script": function() {} },
                        dataFilter: function(e) { T.globalEval(e, t) }
                    });
                }, T.fn.extend({
                    wrapAll: function(e) {
                        var t;
                        return this[0] &&
                        (y(e) && (e = e.call(this[0])), t =
                                T(e, this[0].ownerDocument).eq(0).clone(!0), this[0].parentNode &&
                                t.insertBefore(this[0]),
                            t.map((function() {
                                for (var e = this; e.firstElementChild;) e = e.firstElementChild;
                                return e;
                            })).append(this)), this;
                    },
                    wrapInner: function(e) {
                        return y(e)
                            ? this.each((function(t) { T(this).wrapInner(e.call(this, t)) }))
                            : this.each((function() {
                                const t = T(this);
                                const n = t.contents();
                                n.length ? n.wrapAll(e) : t.append(e);
                            }));
                    },
                    wrap: function(e) {
                        var t = y(e);
                        return this.each((function(n) { T(this).wrapAll(t ? e.call(this, n) : e) }));
                    },
                    unwrap: function(e) {
                        return this.parent(e).not("body").each((function() {
                            T(this).replaceWith(this.childNodes);
                        })), this;
                    }
                }), T.expr.pseudos.hidden = function(e) { return!T.expr.pseudos.visible(e) }, T.expr.pseudos.visible =
                    function(e) {
                        return!!(e.offsetWidth || e.offsetHeight || e.getClientRects().length);
                    }, T.ajaxSettings.xhr = function() {
                    try {
                        return new n.XMLHttpRequest;
                    } catch (e) {
                    }
                };
                var Qt = { 0: 200, 1223: 204 }, Yt = T.ajaxSettings.xhr();
                v.cors = !!Yt && "withCredentials" in Yt, v.ajax = Yt = !!Yt, T.ajaxTransport((function(e) {
                        var t, i;
                        if (v.cors || Yt && !e.crossDomain)
                            return{
                                send: function(r, o) {
                                    var s, a = e.xhr();
                                    if (a.open(e.type, e.url, e.async, e.username, e.password), e.xhrFields)
                                        for (s in e.xhrFields) a[s] = e.xhrFields[s];
                                    for (s in e.mimeType && a.overrideMimeType && a.overrideMimeType(e.mimeType),
                                        e.crossDomain ||
                                            r["X-Requested-With"] ||
                                            (r["X-Requested-With"] = "XMLHttpRequest"), r) a.setRequestHeader(s, r[s]);
                                    t = function(e) {
                                        return function() {
                                            t &&
                                            (t = i = a.onload = a.onerror = a.onabort = a.ontimeout =
                                                a.onreadystatechange = null, "abort" === e
                                                ? a.abort()
                                                : "error" === e
                                                ? "number" != typeof a.status
                                                ? o(0, "error")
                                                : o(a.status, a.statusText)
                                                : o(Qt[a.status] || a.status,
                                                    a.statusText,
                                                    "text" !== (a.responseType || "text") ||
                                                    "string" != typeof a.responseText
                                                    ? { binary: a.response }
                                                    : { text: a.responseText },
                                                    a.getAllResponseHeaders()));
                                        };
                                    }, a.onload = t(), i = a.onerror = a.ontimeout =
                                        t("error"), void 0 !== a.onabort
                                        ? a.onabort = i
                                        : a.onreadystatechange = function() {
                                            4 === a.readyState && n.setTimeout((function() { t && i() }));
                                        }, t = t("abort");
                                    try {
                                        a.send(e.hasContent && e.data || null);
                                    } catch (e) {
                                        if (t) throw e;
                                    }
                                },
                                abort: function() { t && t() }
                            };
                    })), T.ajaxPrefilter((function(e) { e.crossDomain && (e.contents.script = !1) })), T.ajaxSetup({
                        accepts: {
                            script:
                                "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript"
                        },
                        contents: { script: /\b(?:java|ecma)script\b/ },
                        converters: { "text script": function(e) { return T.globalEval(e), e } }
                    }), T.ajaxPrefilter("script",
                        (function(e) { void 0 === e.cache && (e.cache = !1), e.crossDomain && (e.type = "GET") })),
                    T.ajaxTransport("script",
                        (function(e) {
                            var t, n;
                            if (e.crossDomain || e.scriptAttrs)
                                return{
                                    send: function(i, r) {
                                        t = T("<script>").attr(e.scriptAttrs || {})
                                            .prop({ charset: e.scriptCharset, src: e.url })
                                            .on("load error",
                                                n = function(e) {
                                                    t.remove(), n =
                                                        null, e && r("error" === e.type ? 404 : 200, e.type);
                                                }), s.head.appendChild(t[0]);
                                    },
                                    abort: function() { n && n() }
                                };
                        }));
                var zt, Xt = [], Gt = /(=)\?(?=&|$)|\?\?/;
                T.ajaxSetup({
                        jsonp: "callback",
                        jsonpCallback: function() {
                            const e = Xt.pop() || T.expando + "_" + Dt++;
                            return this[e] = !0, e;
                        }
                    }), T.ajaxPrefilter("json jsonp",
                        (function(e, t, i) {
                            var r, o, s;
                            const a = !1 !== e.jsonp &&
                            (Gt.test(e.url)
                                ? "url"
                                : "string" == typeof e.data &&
                                0 === (e.contentType || "").indexOf("application/x-www-form-urlencoded") &&
                                Gt.test(e.data) &&
                                "data");
                            if (a || "jsonp" === e.dataTypes[0])
                                return r = e.jsonpCallback =
                                        y(e.jsonpCallback) ? e.jsonpCallback() : e.jsonpCallback,
                                    a
                                        ? e[a] = e[a].replace(Gt, `$1${r}`)
                                        : !1 !== e.jsonp && (e.url += (At.test(e.url) ? "&" : "?") + e.jsonp + "=" + r),
                                    e.converters["script json"] =
                                        function() {
                                            return s || T.error(r + " was not called"), s[0];
                                        }, e.dataTypes[0] =
                                        "json", o = n[r], n[r] = function() { s = arguments }, i.always((function() {
                                        void 0 === o ? T(n).removeProp(r) : n[r] = o, e[r] &&
                                            (e.jsonpCallback = t.jsonpCallback, Xt.push(r)), s && y(o) && o(s[0]), s =
                                            o =
                                            void 0;
                                    })), "script";
                        })), v.createHTMLDocument = ((zt = s.implementation.createHTMLDocument("").body).innerHTML =
                        "<form></form><form></form>", 2 === zt.childNodes.length), T.parseHTML = function(e, t, n) {
                        return"string" != typeof e
                            ? []
                            : ("boolean" == typeof t && (n = t, t = !1), t ||
                            (v.createHTMLDocument
                                ? ((i = (t = s.implementation.createHTMLDocument("")).createElement("base")).href =
                                    s.location.href, t.head.appendChild(i))
                                : t = s), o = !n && [], (r = O.exec(e))
                                ? [t.createElement(r[1])]
                                : (r = Ce([e], t, o), o && o.length && T(o).remove(), T.merge([], r.childNodes)));
                        var i, r, o;
                    }, T.fn.load = function(e, t, n) {
                        var i, r, o, s = this;
                        const a = e.indexOf(" ");
                        return a > -1 && (i = _t(e.slice(a)), e = e.slice(0, a)),
                            y(t) ? (n = t, t = void 0) : t && "object" == typeof t && (r = "POST"), s.length > 0 &&
                                T.ajax({ url: e, type: r || "GET", dataType: "html", data: t })
                                .done((function(e) {
                                    o = arguments, s.html(i ? T("<div>").append(T.parseHTML(e)).find(i) : e);
                                })).always(n &&
                                    function(e, t) {
                                        s.each((function() { n.apply(this, o || [e.responseText, t, e]) }));
                                    }), this;
                    }, T.each(["ajaxStart", "ajaxStop", "ajaxComplete", "ajaxError", "ajaxSuccess", "ajaxSend"],
                        (function(e, t) { T.fn[t] = function(e) { return this.on(t, e) } })), T.expr.pseudos.animated =
                        function(e) {
                            return T.grep(T.timers, (function(t) { return e === t.elem })).length;
                        }, T.offset = {
                        setOffset: function(e, t, n) {
                            var i, r, o, s, a, l;
                            const u = T.css(e, "position");
                            const c = T(e);
                            const f = {};
                            "static" === u && (e.style.position = "relative"), a = c.offset(), o = T.css(e, "top"), l =
                                    T.css(e, "left"), ("absolute" === u || "fixed" === u) &&
                                    (o + l).indexOf("auto") > -1
                                    ? (s = (i = c.position()).top, r = i.left)
                                    : (s = parseFloat(o) || 0, r = parseFloat(l) || 0),
                                y(t) && (t = t.call(e, n, T.extend({}, a))), null != t.top &&
                                    (f.top = t.top - a.top + s),
                                null != t.left && (f.left = t.left - a.left + r), "using" in t
                                    ? t.using.call(e, f)
                                    : c.css(f);
                        }
                    }, T.fn.extend({
                        offset: function(e) {
                            if (arguments.length)
                                return void 0 === e
                                    ? this
                                    : this.each((function(t) { T.offset.setOffset(this, e, t) }));
                            var t, n;
                            const i = this[0];
                            return i
                                ? i.getClientRects().length
                                ? (t = i.getBoundingClientRect(), n =
                                    i.ownerDocument.defaultView, {
                                    top: t.top + n.pageYOffset,
                                    left: t.left + n.pageXOffset
                                })
                                : { top: 0, left: 0 }
                                : void 0;
                        },
                        position: function() {
                            if (this[0]) {
                                let t;
                                const i = this[0];
                                let r = { top: 0, left: 0 };
                                if ("fixed" === T.css(i, "position")) t = i.getBoundingClientRect();
                                else {
                                    let e;
                                    let n;
                                    for (t = this.offset(), n = i.ownerDocument, e =
                                            i.offsetParent || n.documentElement;
                                        e &&
                                            (e === n.body || e === n.documentElement) &&
                                            "static" === T.css(e, "position");
                                    ) e = e.parentNode;
                                    e &&
                                        e !== i &&
                                        1 === e.nodeType &&
                                        ((r = T(e).offset()).top += T.css(e, "borderTopWidth", !0), r.left += T.css(e,
                                            "borderLeftWidth",
                                            !0));
                                }
                                return{
                                    top: t.top - r.top - T.css(i, "marginTop", !0),
                                    left: t.left - r.left - T.css(i, "marginLeft", !0)
                                };
                            }
                        },
                        offsetParent: function() {
                            return this.map((function() {
                                for (var e = this.offsetParent; e && "static" === T.css(e, "position");)
                                    e = e.offsetParent;
                                return e || se;
                            }));
                        }
                    }), T.each({ scrollLeft: "pageXOffset", scrollTop: "pageYOffset" },
                        (function(e, t) {
                            var n = "pageYOffset" === t;
                            T.fn[e] = function(i) {
                                return K(this,
                                    (function(e, i, r) {
                                        var o;
                                        if (b(e) ? o = e : 9 === e.nodeType && (o = e.defaultView), void 0 === r)
                                            return o ? o[t] : e[i];
                                        o ? o.scrollTo(n ? o.pageXOffset : r, n ? r : o.pageYOffset) : e[i] = r;
                                    }),
                                    e,
                                    i,
                                    arguments.length);
                            };
                        })), T.each(["top", "left"],
                        (function(e, t) {
                            T.cssHooks[t] = Ye(v.pixelPosition,
                                (function(e, n) {
                                    if (n) return n = Qe(e, t), Ve.test(n) ? T(e).position()[t] + "px" : n;
                                }));
                        })), T.each({ Height: "height", Width: "width" },
                        (function(e, t) {
                            T.each({ padding: `inner${e}`, content: t, "": `outer${e}` },
                                (function(n, i) {
                                    T.fn[i] = function(r, o) {
                                        const s = arguments.length && (n || "boolean" != typeof r);
                                        var a = n || (!0 === r || !0 === o ? "margin" : "border");
                                        return K(this,
                                            (function(t, n, r) {
                                                var o;
                                                return b(t)
                                                    ? 0 === i.indexOf("outer")
                                                    ? t[`inner${e}`]
                                                    : t.document.documentElement[`client${e}`]
                                                    : 9 === t.nodeType
                                                    ? (o = t.documentElement, Math.max(t.body[`scroll${e}`],
                                                        o[`scroll${e}`],
                                                        t.body[`offset${e}`],
                                                        o[`offset${e}`],
                                                        o[`client${e}`]))
                                                    : void 0 === r
                                                    ? T.css(t, n, a)
                                                    : T.style(t, n, r, a);
                                            }),
                                            t,
                                            s ? r : void 0,
                                            s);
                                    };
                                }));
                        })), T.each(
                        "blur focus focusin focusout resize scroll click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup contextmenu"
                        .split(" "),
                        (function(e, t) {
                            T.fn[t] = function(e, n) {
                                return arguments.length > 0 ? this.on(t, null, e, n) : this.trigger(t);
                            };
                        })), T.fn.extend({ hover: function(e, t) { return this.mouseenter(e).mouseleave(t || e) } }),
                    T.fn.extend({
                        bind: function(e, t, n) { return this.on(e, null, t, n) },
                        unbind: function(e, t) { return this.off(e, null, t) },
                        delegate: function(e, t, n, i) { return this.on(t, e, n, i) },
                        undelegate: function(e, t, n) {
                            return 1 === arguments.length ? this.off(e, "**") : this.off(t, e || "**", n);
                        }
                    }), T.proxy = function(e, t) {
                        var n, i, r;
                        if ("string" == typeof t && (n = e[t], t = e, e = n), y(e))
                            return i = l.call(arguments, 2), (r = function() {
                                return e.apply(t || this, i.concat(l.call(arguments)));
                            }).guid = e.guid = e.guid || T.guid++, r;
                    }, T.holdReady = function(e) { e ? T.readyWait++ : T.ready(!0) }, T.isArray =
                        Array.isArray, T.parseJSON = JSON.parse, T.nodeName = I, T.isFunction = y, T.isWindow =
                        b, T.camelCase = z, T.type = w, T.now = Date.now, T.isNumeric = function(e) {
                        const t = T.type(e);
                        return("number" === t || "string" === t) && !isNaN(e - parseFloat(e));
                    }, void 0 === (i = function() { return T }.apply(t, [])) || (e.exports = i);
                var Jt = n.jQuery, Zt = n.$;
                return T.noConflict = function(e) {
                    return n.$ === T && (n.$ = Zt), e && n.jQuery === T && (n.jQuery = Jt), T;
                }, r || (n.jQuery = n.$ = T), T;
            }));
    }, function(e, t, n) {
        "use strict";
        n.r(t), function(e) {
            for (
/**!
 * @fileOverview Kickass library to create and place poppers near their reference elements.
 * @version 1.15.0
 * @license
 * Copyright (c) 2016 Federico Zivolo and contributors
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
                var n = "undefined" != typeof window && "undefined" != typeof document,
                i = ["Edge", "Trident", "Firefox"],
                r = 0,
                o = 0;
                o < i.length;
                o += 1)
                if (n && navigator.userAgent.indexOf(i[o]) >= 0) {
                    r = 1;
                    break;
                }
            var s = n && window.Promise
                ? function(e) {
                    var t = !1;
                    return function() { t || (t = !0, window.Promise.resolve().then((function() { t = !1, e() }))) };
                }
                : function(e) {
                    var t = !1;
                    return function() { t || (t = !0, setTimeout((function() { t = !1, e() }), r)) };
                };

            function a(e) { return e && "[object Function]" === {}.toString.call(e) }

            function l(e, t) {
                if (1 !== e.nodeType) return[];
                const n = e.ownerDocument.defaultView.getComputedStyle(e, null);
                return t ? n[t] : n;
            }

            function u(e) { return"HTML" === e.nodeName ? e : e.parentNode || e.host }

            function c(e) {
                if (!e) return document.body;
                switch (e.nodeName) {
                case"HTML":
                case"BODY":
                    return e.ownerDocument.body;
                case"#document":
                    return e.body;
                }
                const t = l(e);
                const n = t.overflow;
                const i = t.overflowX;
                const r = t.overflowY;
                return/(auto|scroll|overlay)/.test(n + r + i) ? e : c(u(e));
            }

            var f = n && !(!window.MSInputMethodContext || !document.documentMode),
                d = n && /MSIE 10/.test(navigator.userAgent);

            function h(e) { return 11 === e ? f : 10 === e ? d : f || d }

            function p(e) {
                if (!e) return document.documentElement;
                for (var t = h(10) ? document.body : null, n = e.offsetParent || null; n === t && e.nextElementSibling;)
                    n = (e = e.nextElementSibling).offsetParent;
                const i = n && n.nodeName;
                return i && "BODY" !== i && "HTML" !== i
                    ? -1 !== ["TH", "TD", "TABLE"].indexOf(n.nodeName) && "static" === l(n, "position")
                    ? p(n)
                    : n
                    : e
                    ? e.ownerDocument.documentElement
                    : document.documentElement;
            }

            function g(e) { return null !== e.parentNode ? g(e.parentNode) : e }

            function m(e, t) {
                if (!(e && e.nodeType && t && t.nodeType)) return document.documentElement;
                const n = e.compareDocumentPosition(t) & Node.DOCUMENT_POSITION_FOLLOWING;
                const i = n ? e : t;
                const r = n ? t : e;
                const o = document.createRange();
                o.setStart(i, 0), o.setEnd(r, 0);
                var s, a;
                const l = o.commonAncestorContainer;
                if (e !== l && t !== l || i.contains(r))
                    return"BODY" === (a = (s = l).nodeName) || "HTML" !== a && p(s.firstElementChild) !== s ? p(l) : l;
                const u = g(e);
                return u.host ? m(u.host, t) : m(e, g(t).host);
            }

            function v(e) {
                const t = "top" === (arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : "top")
                    ? "scrollTop"
                    : "scrollLeft";
                const n = e.nodeName;
                if ("BODY" === n || "HTML" === n) {
                    const i = e.ownerDocument.documentElement;
                    return(e.ownerDocument.scrollingElement || i)[t];
                }
                return e[t];
            }

            function y(e, t) {
                const n = "x" === t ? "Left" : "Top";
                const i = "Left" === n ? "Right" : "Bottom";
                return parseFloat(e[`border${n}Width`], 10) + parseFloat(e[`border${i}Width`], 10);
            }

            function b(e, t, n, i) {
                return Math.max(t[`offset${e}`],
                    t[`scroll${e}`],
                    n[`client${e}`],
                    n[`offset${e}`],
                    n[`scroll${e}`],
                    h(10)
                    ? parseInt(n[`offset${e}`]) +
                    parseInt(i[`margin${"Height" === e ? "Top" : "Left"}`]) +
                    parseInt(i[`margin${"Height" === e ? "Bottom" : "Right"}`])
                    : 0);
            }

            function _(e) {
                const t = e.body;
                const n = e.documentElement;
                const i = h(10) && getComputedStyle(n);
                return{ height: b("Height", t, n, i), width: b("Width", t, n, i) };
            }

            var E = function(e, t) { if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function") },
                w = function() {
                    function e(e, t) {
                        for (let n = 0; n < t.length; n++) {
                            const i = t[n];
                            i.enumerable = i.enumerable || !1, i.configurable =
                                !0, "value" in i && (i.writable = !0), Object.defineProperty(e, i.key, i);
                        }
                    }

                    return function(t, n, i) { return n && e(t.prototype, n), i && e(t, i), t };
                }(),
                T = function(e, t, n) {
                    return t in e
                        ? Object.defineProperty(e, t, { value: n, enumerable: !0, configurable: !0, writable: !0 })
                        : e[t] = n, e;
                },
                C = Object.assign ||
                    function(e) {
                        for (let t = 1; t < arguments.length; t++) {
                            const n = arguments[t];
                            for (let i in n) Object.prototype.hasOwnProperty.call(n, i) && (e[i] = n[i]);
                        }
                        return e;
                    };

            function x(e) { return C({}, e, { right: e.left + e.width, bottom: e.top + e.height }) }

            function S(e) {
                var t = {};
                try {
                    if (h(10)) {
                        t = e.getBoundingClientRect();
                        const n = v(e, "top");
                        const i = v(e, "left");
                        t.top += n, t.left += i, t.bottom += n, t.right += i;
                    } else t = e.getBoundingClientRect();
                } catch (e) {
                }
                const r = { left: t.left, top: t.top, width: t.right - t.left, height: t.bottom - t.top };
                const o = "HTML" === e.nodeName ? _(e.ownerDocument) : {};
                const s = o.width || e.clientWidth || r.right - r.left;
                const a = o.height || e.clientHeight || r.bottom - r.top;
                var u = e.offsetWidth - s, c = e.offsetHeight - a;
                if (u || c) {
                    const f = l(e);
                    u -= y(f, "x"), c -= y(f, "y"), r.width -= u, r.height -= c;
                }
                return x(r);
            }

            function D(e, t) {
                const n = arguments.length > 2 && void 0 !== arguments[2] && arguments[2];
                const i = h(10);
                const r = "HTML" === t.nodeName;
                const o = S(e);
                const s = S(t);
                const a = c(e);
                const u = l(t);
                const f = parseFloat(u.borderTopWidth, 10);
                const d = parseFloat(u.borderLeftWidth, 10);
                n && r && (s.top = Math.max(s.top, 0), s.left = Math.max(s.left, 0));
                var p = x({ top: o.top - s.top - f, left: o.left - s.left - d, width: o.width, height: o.height });
                if (p.marginTop = 0, p.marginLeft = 0, !i && r) {
                    const g = parseFloat(u.marginTop, 10);
                    const m = parseFloat(u.marginLeft, 10);
                    p.top -= f - g, p.bottom -= f - g, p.left -= d - m, p.right -= d - m, p.marginTop =
                        g, p.marginLeft = m;
                }
                return(i && !n ? t.contains(a) : t === a && "BODY" !== a.nodeName) &&
                (p = function(e, t) {
                    const n = arguments.length > 2 && void 0 !== arguments[2] && arguments[2];
                    const i = v(t, "top");
                    const r = v(t, "left");
                    const o = n ? -1 : 1;
                    return e.top += i * o, e.bottom += i * o, e.left += r * o, e.right += r * o, e;
                }(p, t)), p;
            }

            function A(e) {
                if (!e || !e.parentElement || h()) return document.documentElement;
                for (var t = e.parentElement; t && "none" === l(t, "transform");) t = t.parentElement;
                return t || document.documentElement;
            }

            function N(e, t, n, i) {
                const r = arguments.length > 4 && void 0 !== arguments[4] && arguments[4];
                var o = { top: 0, left: 0 };
                const s = r ? A(e) : m(e, t);
                if ("viewport" === i)
                    o = function(e) {
                        const t = arguments.length > 1 && void 0 !== arguments[1] && arguments[1];
                        const n = e.ownerDocument.documentElement;
                        const i = D(e, n);
                        const r = Math.max(n.clientWidth, window.innerWidth || 0);
                        const o = Math.max(n.clientHeight, window.innerHeight || 0);
                        const s = t ? 0 : v(n);
                        const a = t ? 0 : v(n, "left");
                        return x({
                            top: s - i.top + i.marginTop,
                            left: a - i.left + i.marginLeft,
                            width: r,
                            height: o
                        });
                    }(s, r);
                else {
                    let a = void 0;
                    "scrollParent" === i
                        ? "BODY" === (a = c(u(t))).nodeName && (a = e.ownerDocument.documentElement)
                        : a = "window" === i ? e.ownerDocument.documentElement : i;
                    const f = D(a, s, r);
                    if ("HTML" !== a.nodeName ||
                        function e(t) {
                            const n = t.nodeName;
                            if ("BODY" === n || "HTML" === n) return!1;
                            if ("fixed" === l(t, "position")) return!0;
                            const i = u(t);
                            return!!i && e(i);
                        }(s)) o = f;
                    else {
                        const d = _(e.ownerDocument);
                        const h = d.height;
                        const p = d.width;
                        o.top += f.top - f.marginTop, o.bottom = h + f.top, o.left += f.left - f.marginLeft, o.right =
                            p + f.left;
                    }
                }
                const g = "number" == typeof(n = n || 0);
                return o.left += g ? n : n.left || 0, o.top += g ? n : n.top || 0, o.right -= g ? n : n.right || 0,
                    o.bottom -= g ? n : n.bottom || 0, o;
            }

            function I(e, t, n, i, r) {
                const o = arguments.length > 5 && void 0 !== arguments[5] ? arguments[5] : 0;
                if (-1 === e.indexOf("auto")) return e;
                const s = N(n, i, o, r);
                var a = {
                    top: { width: s.width, height: t.top - s.top },
                    right: { width: s.right - t.right, height: s.height },
                    bottom: { width: s.width, height: s.bottom - t.bottom },
                    left: { width: t.left - s.left, height: s.height }
                };
                const l = Object.keys(a).map((function(e) {
                    return C({ key: e }, a[e], { area: (t = a[e], t.width * t.height) });
                    var t;
                })).sort((function(e, t) { return t.area - e.area }));
                const u = l.filter((function(e) {
                    const t = e.width;
                    const i = e.height;
                    return t >= n.clientWidth && i >= n.clientHeight;
                }));
                const c = u.length > 0 ? u[0].key : l[0].key;
                const f = e.split("-")[1];
                return c + (f ? `-${f}` : "");
            }

            function O(e, t, n) {
                const i = arguments.length > 3 && void 0 !== arguments[3] ? arguments[3] : null;
                return D(n, i ? A(t) : m(t, n), i);
            }

            function k(e) {
                const t = e.ownerDocument.defaultView.getComputedStyle(e);
                const n = parseFloat(t.marginTop || 0) + parseFloat(t.marginBottom || 0);
                const i = parseFloat(t.marginLeft || 0) + parseFloat(t.marginRight || 0);
                return{ width: e.offsetWidth + i, height: e.offsetHeight + n };
            }

            function L(e) {
                var t = { left: "right", right: "left", bottom: "top", top: "bottom" };
                return e.replace(/left|right|bottom|top/g, (function(e) { return t[e] }));
            }

            function j(e, t, n) {
                n = n.split("-")[0];
                const i = k(e);
                const r = { width: i.width, height: i.height };
                const o = -1 !== ["right", "left"].indexOf(n);
                const s = o ? "top" : "left";
                const a = o ? "left" : "top";
                const l = o ? "height" : "width";
                const u = o ? "width" : "height";
                return r[s] = t[s] + t[l] / 2 - i[l] / 2, r[a] = n === a ? t[a] - i[u] : t[L(a)], r;
            }

            function P(e, t) { return Array.prototype.find ? e.find(t) : e.filter(t)[0] }

            function H(e, t, n) {
                return(void 0 === n
                    ? e
                    : e.slice(0,
                        function(e, t, n) {
                            if (Array.prototype.findIndex) return e.findIndex((function(e) { return e[t] === n }));
                            const i = P(e, (function(e) { return e[t] === n }));
                            return e.indexOf(i);
                        }(e, "name", n))).forEach((function(e) {
                    e.function && console.warn("`modifier.function` is deprecated, use `modifier.fn`!");
                    const n = e.function || e.fn;
                    e.enabled &&
                        a(n) &&
                        (t.offsets.popper = x(t.offsets.popper), t.offsets.reference = x(t.offsets.reference), t =
                            n(t, e));
                })), t;
            }

            function R() {
                if (!this.state.isDestroyed) {
                    let e = { instance: this, styles: {}, arrowStyles: {}, attributes: {}, flipped: !1, offsets: {} };
                    e.offsets.reference =
                        O(this.state, this.popper, this.reference, this.options.positionFixed), e.placement =
                        I(this.options.placement,
                            e.offsets.reference,
                            this.popper,
                            this.reference,
                            this.options.modifiers.flip.boundariesElement,
                            this.options.modifiers.flip.padding), e.originalPlacement = e.placement, e.positionFixed =
                        this.options.positionFixed, e.offsets.popper =
                        j(this.popper, e.offsets.reference, e.placement), e.offsets.popper.position =
                        this.options.positionFixed ? "fixed" : "absolute", e =
                        H(this.modifiers, e), this.state.isCreated
                        ? this.options.onUpdate(e)
                        : (this.state.isCreated = !0, this.options.onCreate(e));
                }
            }

            function M(e, t) {
                return e.some((function(e) {
                    const n = e.name;
                    return e.enabled && n === t;
                }));
            }

            function q(e) {
                for (var t = [!1, "ms", "Webkit", "Moz", "O"], n = e.charAt(0).toUpperCase() + e.slice(1), i = 0;
                    i < t.length;
                    i++) {
                    const r = t[i];
                    const o = r ? `${r}${n}` : e;
                    if (void 0 !== document.body.style[o]) return o;
                }
                return null;
            }

            function F() {
                return this.state.isDestroyed =
                        !0, M(this.modifiers, "applyStyle") &&
                    (this.popper.removeAttribute("x-placement"), this.popper.style.position =
                        "", this.popper.style.top = "", this.popper.style.left = "", this.popper.style.right =
                        "", this.popper.style.bottom = "", this.popper.style.willChange =
                        "", this.popper.style[q("transform")] = ""), this.disableEventListeners(),
                    this.options.removeOnDestroy && this.popper.parentNode.removeChild(this.popper), this;
            }

            function W(e) {
                const t = e.ownerDocument;
                return t ? t.defaultView : window;
            }

            function B(e, t, n, i) {
                n.updateBound = i, W(e).addEventListener("resize", n.updateBound, { passive: !0 });
                const r = c(e);
                return function e(t, n, i, r) {
                    const o = "BODY" === t.nodeName;
                    const s = o ? t.ownerDocument.defaultView : t;
                    s.addEventListener(n, i, { passive: !0 }), o || e(c(s.parentNode), n, i, r), r.push(s);
                }(r, "scroll", n.updateBound, n.scrollParents), n.scrollElement = r, n.eventsEnabled = !0, n;
            }

            function U() {
                this.state.eventsEnabled ||
                    (this.state = B(this.reference, this.options, this.state, this.scheduleUpdate));
            }

            function V() {
                var e, t;
                this.state.eventsEnabled &&
                (cancelAnimationFrame(this.scheduleUpdate), this.state = (e = this.reference, t =
                    this.state, W(e).removeEventListener("resize", t.updateBound), t.scrollParents.forEach(
                    (function(e) { e.removeEventListener("scroll", t.updateBound) })), t.updateBound =
                    null, t.scrollParents = [], t.scrollElement = null, t.eventsEnabled = !1, t));
            }

            function K(e) { return"" !== e && !isNaN(parseFloat(e)) && isFinite(e) }

            function $(e, t) {
                Object.keys(t).forEach((function(n) {
                    var i = "";
                    -1 !== ["width", "height", "top", "right", "bottom", "left"].indexOf(n) && K(t[n]) && (i = "px"),
                        e.style[n] = t[n] + i;
                }));
            }

            var Q = n && /Firefox/i.test(navigator.userAgent);

            function Y(e, t, n) {
                var i = P(e, (function(e) { return e.name === t }));
                const r = !!i && e.some((function(e) { return e.name === n && e.enabled && e.order < i.order }));
                if (!r) {
                    const o = `\`${t}\``;
                    const s = `\`${n}\``;
                    console.warn(s +
                        " modifier is required by " +
                        o +
                        " modifier in order to work, be sure to include it before " +
                        o +
                        "!");
                }
                return r;
            }

            var z = [
                    "auto-start", "auto", "auto-end", "top-start", "top", "top-end", "right-start", "right",
                    "right-end", "bottom-end", "bottom", "bottom-start", "left-end", "left", "left-start"
                ],
                X = z.slice(3);

            function G(e) {
                const t = arguments.length > 1 && void 0 !== arguments[1] && arguments[1];
                const n = X.indexOf(e);
                const i = X.slice(n + 1).concat(X.slice(0, n));
                return t ? i.reverse() : i;
            }

            var J = { FLIP: "flip", CLOCKWISE: "clockwise", COUNTERCLOCKWISE: "counterclockwise" };

            function Z(e, t, n, i) {
                var r = [0, 0], o = -1 !== ["right", "left"].indexOf(i);
                const s = e.split(/(\+|\-)/).map((function(e) { return e.trim() }));
                const a = s.indexOf(P(s, (function(e) { return-1 !== e.search(/,|\s/) })));
                s[a] &&
                    -1 === s[a].indexOf(",") &&
                    console.warn("Offsets separated by white space(s) are deprecated, use a comma (,) instead.");
                const l = /\s*,\s*|\s+/;
                var u = -1 !== a
                    ? [s.slice(0, a).concat([s[a].split(l)[0]]), [s[a].split(l)[1]].concat(s.slice(a + 1))]
                    : [s];
                return(u = u.map((function(e, i) {
                    var r = (1 === i ? !o : o) ? "height" : "width", s = !1;
                    return e.reduce((function(e, t) {
                            return"" === e[e.length - 1] && -1 !== ["+", "-"].indexOf(t)
                                ? (e[e.length - 1] = t, s = !0, e)
                                : s
                                ? (e[e.length - 1] += t, s = !1, e)
                                : e.concat(t);
                        }),
                        []).map((function(e) {
                        return function(e, t, n, i) {
                            const r = e.match(/((?:\-|\+)?\d*\.?\d*)(.*)/);
                            const o = +r[1];
                            const s = r[2];
                            if (!o) return e;
                            if (0 === s.indexOf("%")) {
                                let a = void 0;
                                switch (s) {
                                case"%p":
                                    a = n;
                                    break;
                                case"%":
                                case"%r":
                                default:
                                    a = i;
                                }
                                return x(a)[t] / 100 * o;
                            }
                            if ("vh" === s || "vw" === s) {
                                return("vh" === s
                                        ? Math.max(document.documentElement.clientHeight, window.innerHeight || 0)
                                        : Math.max(document.documentElement.clientWidth, window.innerWidth || 0)) /
                                    100 *
                                    o;
                            }
                            return o;
                        }(e, r, t, n);
                    }));
                }))).forEach((function(e, t) {
                    e.forEach((function(n, i) { K(n) && (r[t] += n * ("-" === e[i - 1] ? -1 : 1)) }));
                })), r;
            }

            var ee = {
                    placement: "bottom",
                    positionFixed: !1,
                    eventsEnabled: !0,
                    removeOnDestroy: !1,
                    onCreate: function() {},
                    onUpdate: function() {},
                    modifiers: {
                        shift: {
                            order: 100,
                            enabled: !0,
                            fn: function(e) {
                                const t = e.placement;
                                const n = t.split("-")[0];
                                const i = t.split("-")[1];
                                if (i) {
                                    const r = e.offsets;
                                    const o = r.reference;
                                    const s = r.popper;
                                    const a = -1 !== ["bottom", "top"].indexOf(n);
                                    const l = a ? "left" : "top";
                                    const u = a ? "width" : "height";
                                    const c = { start: T({}, l, o[l]), end: T({}, l, o[l] + o[u] - s[u]) };
                                    e.offsets.popper = C({}, s, c[i]);
                                }
                                return e;
                            }
                        },
                        offset: {
                            order: 200,
                            enabled: !0,
                            fn: function(e, t) {
                                const n = t.offset;
                                const i = e.placement;
                                const r = e.offsets;
                                const o = r.popper;
                                const s = r.reference;
                                const a = i.split("-")[0];
                                var l = void 0;
                                return l = K(+n) ? [+n, 0] : Z(n, o, s, a), "left" === a
                                    ? (o.top += l[0], o.left -= l[1])
                                    : "right" === a
                                    ? (o.top += l[0], o.left += l[1])
                                    : "top" === a
                                    ? (o.left += l[0], o.top -= l[1])
                                    : "bottom" === a && (o.left += l[0], o.top += l[1]), e.popper = o, e;
                            },
                            offset: 0
                        },
                        preventOverflow: {
                            order: 300,
                            enabled: !0,
                            fn: function(e, t) {
                                var n = t.boundariesElement || p(e.instance.popper);
                                e.instance.reference === n && (n = p(n));
                                const i = q("transform");
                                const r = e.instance.popper.style;
                                const o = r.top;
                                const s = r.left;
                                const a = r[i];
                                r.top = "", r.left = "", r[i] = "";
                                var l = N(e.instance.popper, e.instance.reference, t.padding, n, e.positionFixed);
                                r.top = o, r.left = s, r[i] = a, t.boundaries = l;
                                const u = t.priority;
                                var c = e.offsets.popper,
                                    f = {
                                        primary: function(e) {
                                            var n = c[e];
                                            return c[e] < l[e] &&
                                                !t.escapeWithReference &&
                                                (n = Math.max(c[e], l[e])), T(
                                                {},
                                                e,
                                                n);
                                        },
                                        secondary: function(e) {
                                            const n = "right" === e ? "left" : "top";
                                            var i = c[n];
                                            return c[e] > l[e] &&
                                                !t.escapeWithReference &&
                                                (i = Math.min(c[n], l[e] - ("right" === e ? c.width : c.height))), T({},
                                                n,
                                                i);
                                        }
                                    };
                                return u.forEach((function(e) {
                                    const t = -1 !== ["left", "top"].indexOf(e) ? "primary" : "secondary";
                                    c = C({}, c, f[t](e));
                                })), e.offsets.popper = c, e;
                            },
                            priority: ["left", "right", "top", "bottom"],
                            padding: 5,
                            boundariesElement: "scrollParent"
                        },
                        keepTogether: {
                            order: 400,
                            enabled: !0,
                            fn: function(e) {
                                const t = e.offsets;
                                const n = t.popper;
                                const i = t.reference;
                                const r = e.placement.split("-")[0];
                                const o = Math.floor;
                                const s = -1 !== ["top", "bottom"].indexOf(r);
                                const a = s ? "right" : "bottom";
                                const l = s ? "left" : "top";
                                const u = s ? "width" : "height";
                                return n[a] < o(i[l]) && (e.offsets.popper[l] = o(i[l]) - n[u]), n[l] > o(i[a]) &&
                                    (e.offsets.popper[l] = o(i[a])), e;
                            }
                        },
                        arrow: {
                            order: 500,
                            enabled: !0,
                            fn: function(e, t) {
                                var n;
                                if (!Y(e.instance.modifiers, "arrow", "keepTogether")) return e;
                                var i = t.element;
                                if ("string" == typeof i) {
                                    if (!(i = e.instance.popper.querySelector(i))) return e;
                                } else if (!e.instance.popper.contains(i))
                                    return console.warn(
                                        "WARNING: `arrow.element` must be child of its popper element!"), e;
                                var r = e.placement.split("-")[0],
                                    o = e.offsets,
                                    s = o.popper,
                                    a = o.reference,
                                    u = -1 !== ["left", "right"].indexOf(r),
                                    c = u ? "height" : "width",
                                    f = u ? "Top" : "Left",
                                    d = f.toLowerCase(),
                                    h = u ? "left" : "top",
                                    p = u ? "bottom" : "right",
                                    g = k(i)[c];
                                a[p] - g < s[d] && (e.offsets.popper[d] -= s[d] - (a[p] - g)), a[d] + g > s[p] &&
                                    (e.offsets.popper[d] += a[d] + g - s[p]), e.offsets.popper = x(e.offsets.popper);
                                var m = a[d] + a[c] / 2 - g / 2,
                                    v = l(e.instance.popper),
                                    y = parseFloat(v[`margin${f}`], 10),
                                    b = parseFloat(v[`border${f}Width`], 10),
                                    _ = m - e.offsets.popper[d] - y - b;
                                return _ = Math.max(Math.min(s[c] - g, _), 0), e.arrowElement = i, e.offsets.arrow =
                                    (T(n = {}, d, Math.round(_)), T(n, h, ""), n), e;
                            },
                            element: "[x-arrow]"
                        },
                        flip: {
                            order: 600,
                            enabled: !0,
                            fn: function(e, t) {
                                if (M(e.instance.modifiers, "inner")) return e;
                                if (e.flipped && e.placement === e.originalPlacement) return e;
                                var n = N(e.instance.popper,
                                        e.instance.reference,
                                        t.padding,
                                        t.boundariesElement,
                                        e.positionFixed),
                                    i = e.placement.split("-")[0],
                                    r = L(i),
                                    o = e.placement.split("-")[1] || "",
                                    s = [];
                                switch (t.behavior) {
                                case J.FLIP:
                                    s = [i, r];
                                    break;
                                case J.CLOCKWISE:
                                    s = G(i);
                                    break;
                                case J.COUNTERCLOCKWISE:
                                    s = G(i, !0);
                                    break;
                                default:
                                    s = t.behavior;
                                }
                                return s.forEach((function(a, l) {
                                    if (i !== a || s.length === l + 1) return e;
                                    i = e.placement.split("-")[0], r = L(i);
                                    var u = e.offsets.popper,
                                        c = e.offsets.reference,
                                        f = Math.floor,
                                        d =
                                            "left" === i && f(u.right) > f(c.left) ||
                                                "right" === i && f(u.left) < f(c.right) ||
                                                "top" === i && f(u.bottom) > f(c.top) ||
                                                "bottom" === i && f(u.top) < f(c.bottom),
                                        h = f(u.left) < f(n.left),
                                        p = f(u.right) > f(n.right),
                                        g = f(u.top) < f(n.top),
                                        m = f(u.bottom) > f(n.bottom),
                                        v = "left" === i && h ||
                                            "right" === i && p ||
                                            "top" === i && g ||
                                            "bottom" === i && m,
                                        y = -1 !== ["top", "bottom"].indexOf(i),
                                        b = !!t.flipVariations &&
                                        (y && "start" === o && h ||
                                            y && "end" === o && p ||
                                            !y && "start" === o && g ||
                                            !y && "end" === o && m),
                                        _ = !!t.flipVariationsByContent &&
                                        (y && "start" === o && p ||
                                            y && "end" === o && h ||
                                            !y && "start" === o && m ||
                                            !y && "end" === o && g),
                                        E = b || _;
                                    (d || v || E) &&
                                    (e.flipped =
                                            !0, (d || v) && (i = s[l + 1]), E &&
                                        (o = function(e) {
                                            return"end" === e ? "start" : "start" === e ? "end" : e;
                                        }(o)), e.placement = i + (o ? `-${o}` : ""), e.offsets.popper =
                                            C({},
                                                e.offsets.popper,
                                                j(e.instance.popper, e.offsets.reference, e.placement)),
                                        e = H(e.instance.modifiers, e, "flip"));
                                })), e;
                            },
                            behavior: "flip",
                            padding: 5,
                            boundariesElement: "viewport",
                            flipVariations: !1,
                            flipVariationsByContent: !1
                        },
                        inner: {
                            order: 700,
                            enabled: !1,
                            fn: function(e) {
                                const t = e.placement;
                                const n = t.split("-")[0];
                                const i = e.offsets;
                                const r = i.popper;
                                const o = i.reference;
                                const s = -1 !== ["left", "right"].indexOf(n);
                                const a = -1 === ["top", "left"].indexOf(n);
                                return r[s ? "left" : "top"] = o[n] - (a ? r[s ? "width" : "height"] : 0), e.placement =
                                    L(t), e.offsets.popper = x(r), e;
                            }
                        },
                        hide: {
                            order: 800,
                            enabled: !0,
                            fn: function(e) {
                                if (!Y(e.instance.modifiers, "hide", "preventOverflow")) return e;
                                const t = e.offsets.reference;
                                const n = P(e.instance.modifiers, (function(e) { return"preventOverflow" === e.name }))
                                    .boundaries;
                                if (t.bottom < n.top || t.left > n.right || t.top > n.bottom || t.right < n.left) {
                                    if (!0 === e.hide) return e;
                                    e.hide = !0, e.attributes["x-out-of-boundaries"] = "";
                                } else {
                                    if (!1 === e.hide) return e;
                                    e.hide = !1, e.attributes["x-out-of-boundaries"] = !1;
                                }
                                return e;
                            }
                        },
                        computeStyle: {
                            order: 850,
                            enabled: !0,
                            fn: function(e, t) {
                                const n = t.x;
                                const i = t.y;
                                const r = e.offsets.popper;
                                const o = P(e.instance.modifiers, (function(e) { return"applyStyle" === e.name }))
                                    .gpuAcceleration;
                                void 0 !== o &&
                                    console.warn(
                                        "WARNING: `gpuAcceleration` option moved to `computeStyle` modifier and will not be supported in future versions of Popper.js!");
                                const s = void 0 !== o ? o : t.gpuAcceleration;
                                const a = p(e.instance.popper);
                                const l = S(a);
                                const u = { position: r.position };
                                const c = function(e, t) {
                                    const n = e.offsets;
                                    const i = n.popper;
                                    const r = n.reference;
                                    const o = Math.round;
                                    const s = Math.floor;
                                    const a = function(e) { return e };
                                    const l = o(r.width);
                                    const u = o(i.width);
                                    const c = -1 !== ["left", "right"].indexOf(e.placement);
                                    const f = -1 !== e.placement.indexOf("-");
                                    const d = t ? c || f || l % 2 == u % 2 ? o : s : a;
                                    const h = t ? o : a;
                                    return{
                                        left: d(l % 2 == 1 && u % 2 == 1 && !f && t ? i.left - 1 : i.left),
                                        top: h(i.top),
                                        bottom: h(i.bottom),
                                        right: d(i.right)
                                    };
                                }(e, window.devicePixelRatio < 2 || !Q);
                                const f = "bottom" === n ? "top" : "bottom";
                                const d = "right" === i ? "left" : "right";
                                const h = q("transform");
                                var g = void 0, m = void 0;
                                if (m =
                                    "bottom" === f
                                    ? "HTML" === a.nodeName
                                    ? -a.clientHeight + c.bottom
                                    : -l.height + c.bottom
                                    : c.top, g =
                                    "right" === d
                                    ? "HTML" === a.nodeName
                                    ? -a.clientWidth + c.right
                                    : -l.width + c.right
                                    : c.left, s && h)
                                    u[h] = `translate3d(${g}px, ${m}px, 0)`, u[f] = 0, u[d] = 0, u.willChange =
                                        "transform";
                                else {
                                    const v = "bottom" === f ? -1 : 1;
                                    const y = "right" === d ? -1 : 1;
                                    u[f] = m * v, u[d] = g * y, u.willChange = f + ", " + d;
                                }
                                const b = { "x-placement": e.placement };
                                return e.attributes = C({}, b, e.attributes), e.styles =
                                    C({}, u, e.styles), e.arrowStyles = C({}, e.offsets.arrow, e.arrowStyles), e;
                            },
                            gpuAcceleration: !0,
                            x: "bottom",
                            y: "right"
                        },
                        applyStyle: {
                            order: 900,
                            enabled: !0,
                            fn: function(e) {
                                var t, n;
                                return $(e.instance.popper, e.styles), t = e.instance.popper, n =
                                    e.attributes, Object.keys(n).forEach((function(e) {
                                    !1 !== n[e] ? t.setAttribute(e, n[e]) : t.removeAttribute(e);
                                })), e.arrowElement &&
                                    Object.keys(e.arrowStyles).length &&
                                    $(e.arrowElement, e.arrowStyles), e;
                            },
                            onLoad: function(e, t, n, i, r) {
                                const o = O(r, t, e, n.positionFixed);
                                const s = I(n.placement,
                                    o,
                                    t,
                                    e,
                                    n.modifiers.flip.boundariesElement,
                                    n.modifiers.flip.padding);
                                return t.setAttribute("x-placement", s), $(t,
                                    { position: n.positionFixed ? "fixed" : "absolute" }), n;
                            },
                            gpuAcceleration: void 0
                        }
                    }
                },
                te = function() {
                    function e(t, n) {
                        var i = this, r = arguments.length > 2 && void 0 !== arguments[2] ? arguments[2] : {};
                        E(this, e), this.scheduleUpdate =
                            function() { return requestAnimationFrame(i.update) }, this.update =
                            s(this.update.bind(this)), this.options = C({}, e.Defaults, r), this.state =
                            { isDestroyed: !1, isCreated: !1, scrollParents: [] }, this.reference =
                            t && t.jquery ? t[0] : t, this.popper = n && n.jquery ? n[0] : n, this.options.modifiers =
                            {}, Object.keys(C({}, e.Defaults.modifiers, r.modifiers)).forEach((function(t) {
                            i.options.modifiers[t] = C({},
                                e.Defaults.modifiers[t] || {},
                                r.modifiers ? r.modifiers[t] : {});
                        })), this.modifiers =
                            Object.keys(this.options.modifiers)
                            .map((function(e) { return C({ name: e }, i.options.modifiers[e]) }))
                            .sort((function(e, t) { return e.order - t.order })), this.modifiers.forEach((function(e) {
                            e.enabled && a(e.onLoad) && e.onLoad(i.reference, i.popper, i.options, e, i.state);
                        })), this.update();
                        const o = this.options.eventsEnabled;
                        o && this.enableEventListeners(), this.state.eventsEnabled = o;
                    }

                    return w(e,
                        [
                            { key: "update", value: function() { return R.call(this) } },
                            { key: "destroy", value: function() { return F.call(this) } },
                            { key: "enableEventListeners", value: function() { return U.call(this) } },
                            { key: "disableEventListeners", value: function() { return V.call(this) } }
                        ]), e;
                }();
            te.Utils = ("undefined" != typeof window ? window : e).PopperUtils, te.placements = z, te.Defaults =
                ee, t.default = te;
        }.call(this, n(4));
    }, function(e, t) {
        var n;
        n = function() { return this }();
        try {
            n = n || new Function("return this")();
        } catch (e) {
            "object" == typeof window && (n = window);
        }
        e.exports = n;
    }
]);