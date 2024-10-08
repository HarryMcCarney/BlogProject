import { disposeSafe, defaultOf, toIterator, getEnumerator, structuralHash, equals as equals_1, compare } from "./Util.js";
import { class_type } from "./Reflection.js";
import { toArray, empty, singleton, append, enumerateWhile, delay } from "./Seq.js";
import { setItem, initialize, copyTo, fill, item as item_1 } from "./Array.js";
import { max } from "./Double.js";
export class Comparer$1 {
    constructor(comparison) {
        this.comparison = comparison;
    }
    Compare(x, y) {
        const _ = this;
        return _.comparison(x, y) | 0;
    }
}
export function Comparer$1_$reflection(gen0) {
    return class_type("System.Collections.Generic.Comparer`1", [gen0], Comparer$1);
}
export function Comparer$1_$ctor_47C913C(comparison) {
    return new Comparer$1(comparison);
}
export function Comparer$1_get_Default() {
    return Comparer$1_$ctor_47C913C(compare);
}
export function Comparer$1_Create_47C913C(comparison) {
    return Comparer$1_$ctor_47C913C(comparison);
}
export function Comparer$1__Compare_5BDDA0(_, x, y) {
    return _.comparison(x, y);
}
export class EqualityComparer$1 {
    constructor(equals, getHashCode) {
        this.equals = equals;
        this.getHashCode = getHashCode;
    }
    Equals(x, y) {
        const _ = this;
        return _.equals(x, y);
    }
    GetHashCode(x) {
        const _ = this;
        return _.getHashCode(x) | 0;
    }
}
export function EqualityComparer$1_$reflection(gen0) {
    return class_type("System.Collections.Generic.EqualityComparer`1", [gen0], EqualityComparer$1);
}
export function EqualityComparer$1_$ctor_Z6EE254AB(equals, getHashCode) {
    return new EqualityComparer$1(equals, getHashCode);
}
export function EqualityComparer$1_get_Default() {
    return EqualityComparer$1_$ctor_Z6EE254AB(equals_1, structuralHash);
}
export function EqualityComparer$1_Create_Z6EE254AB(equals, getHashCode) {
    return EqualityComparer$1_$ctor_Z6EE254AB(equals, getHashCode);
}
export function EqualityComparer$1__Equals_5BDDA0(_, x, y) {
    return _.equals(x, y);
}
export function EqualityComparer$1__GetHashCode_2B595(_, x) {
    return _.getHashCode(x);
}
export class Stack$1 {
    constructor(initialContents, initialCount) {
        this.contents = initialContents;
        this.count = (initialCount | 0);
    }
    GetEnumerator() {
        const _ = this;
        return getEnumerator(delay(() => {
            let index = _.count - 1;
            return enumerateWhile(() => (index >= 0), delay(() => append(singleton(item_1(index, _.contents)), delay(() => {
                index = ((index - 1) | 0);
                return empty();
            }))));
        }));
    }
    [Symbol.iterator]() {
        return toIterator(getEnumerator(this));
    }
    "System.Collections.IEnumerable.GetEnumerator"() {
        const this$ = this;
        return getEnumerator(this$);
    }
}
export function Stack$1_$reflection(gen0) {
    return class_type("System.Collections.Generic.Stack`1", [gen0], Stack$1);
}
function Stack$1_$ctor_Z3B4C077E(initialContents, initialCount) {
    return new Stack$1(initialContents, initialCount);
}
export function Stack$1_$ctor_Z524259A4(initialCapacity) {
    return Stack$1_$ctor_Z3B4C077E(fill(new Array(initialCapacity), 0, initialCapacity, null), 0);
}
export function Stack$1_$ctor() {
    return Stack$1_$ctor_Z524259A4(4);
}
export function Stack$1_$ctor_BB573A(xs) {
    const arr = Array.from(xs);
    return Stack$1_$ctor_Z3B4C077E(arr, arr.length);
}
export function Stack$1__Ensure_Z524259A4(_, newSize) {
    const oldSize = _.contents.length | 0;
    if (newSize > oldSize) {
        const old = _.contents;
        _.contents = fill(new Array(max(newSize, oldSize * 2)), 0, max(newSize, oldSize * 2), null);
        copyTo(old, 0, _.contents, 0, _.count);
    }
}
export function Stack$1__get_Count(_) {
    return _.count;
}
export function Stack$1__Pop(_) {
    _.count = ((_.count - 1) | 0);
    return item_1(_.count, _.contents);
}
export function Stack$1__Peek(_) {
    return item_1(_.count - 1, _.contents);
}
export function Stack$1__Contains_2B595(_, x) {
    let found = false;
    let i = 0;
    while ((i < _.count) && !found) {
        if (equals_1(x, item_1(i, _.contents))) {
            found = true;
        }
        else {
            i = ((i + 1) | 0);
        }
    }
    return found;
}
export function Stack$1__TryPeek_1F3DB691(this$, result) {
    if (this$.count > 0) {
        result.contents = Stack$1__Peek(this$);
        return true;
    }
    else {
        return false;
    }
}
export function Stack$1__TryPop_1F3DB691(this$, result) {
    if (this$.count > 0) {
        result.contents = Stack$1__Pop(this$);
        return true;
    }
    else {
        return false;
    }
}
export function Stack$1__Push_2B595(this$, x) {
    Stack$1__Ensure_Z524259A4(this$, this$.count + 1);
    this$.contents[this$.count] = x;
    this$.count = ((this$.count + 1) | 0);
}
export function Stack$1__Clear(_) {
    _.count = 0;
    fill(_.contents, 0, _.contents.length, defaultOf());
}
export function Stack$1__TrimExcess(this$) {
    if ((this$.count / this$.contents.length) > 0.9) {
        Stack$1__Ensure_Z524259A4(this$, this$.count);
    }
}
export function Stack$1__ToArray(_) {
    return initialize(_.count, (i) => item_1((_.count - 1) - i, _.contents));
}
export class Queue$1 {
    constructor(initialContents, initialCount) {
        this.contents = initialContents;
        this.count = (initialCount | 0);
        this.head = 0;
        this.tail = (((initialCount === this.contents.length) ? 0 : initialCount) | 0);
    }
    GetEnumerator() {
        const _ = this;
        return getEnumerator(Queue$1__toSeq(_));
    }
    [Symbol.iterator]() {
        return toIterator(getEnumerator(this));
    }
    "System.Collections.IEnumerable.GetEnumerator"() {
        const this$ = this;
        return getEnumerator(this$);
    }
}
export function Queue$1_$reflection(gen0) {
    return class_type("System.Collections.Generic.Queue`1", [gen0], Queue$1);
}
function Queue$1_$ctor_Z3B4C077E(initialContents, initialCount) {
    return new Queue$1(initialContents, initialCount);
}
export function Queue$1_$ctor_Z524259A4(initialCapacity) {
    if (initialCapacity < 0) {
        throw new Error("capacity is less than 0");
    }
    return Queue$1_$ctor_Z3B4C077E(fill(new Array(initialCapacity), 0, initialCapacity, null), 0);
}
export function Queue$1_$ctor() {
    return Queue$1_$ctor_Z524259A4(4);
}
export function Queue$1_$ctor_BB573A(xs) {
    const arr = Array.from(xs);
    return Queue$1_$ctor_Z3B4C077E(arr, arr.length);
}
export function Queue$1__get_Count(_) {
    return _.count;
}
export function Queue$1__Enqueue_2B595(_, value) {
    if (_.count === Queue$1__size(_)) {
        Queue$1__ensure_Z524259A4(_, _.count + 1);
    }
    _.contents[_.tail] = value;
    _.tail = (((_.tail + 1) % Queue$1__size(_)) | 0);
    _.count = ((_.count + 1) | 0);
}
export function Queue$1__Dequeue(_) {
    if (_.count === 0) {
        throw new Error("Queue is empty");
    }
    const value = item_1(_.head, _.contents);
    _.head = (((_.head + 1) % Queue$1__size(_)) | 0);
    _.count = ((_.count - 1) | 0);
    return value;
}
export function Queue$1__Peek(_) {
    if (_.count === 0) {
        throw new Error("Queue is empty");
    }
    return item_1(_.head, _.contents);
}
export function Queue$1__TryDequeue_1F3DB691(this$, result) {
    if (this$.count === 0) {
        return false;
    }
    else {
        result.contents = Queue$1__Dequeue(this$);
        return true;
    }
}
export function Queue$1__TryPeek_1F3DB691(this$, result) {
    if (this$.count === 0) {
        return false;
    }
    else {
        result.contents = Queue$1__Peek(this$);
        return true;
    }
}
export function Queue$1__Contains_2B595(_, x) {
    let found = false;
    let i = 0;
    while ((i < _.count) && !found) {
        if (equals_1(x, item_1(Queue$1__toIndex_Z524259A4(_, i), _.contents))) {
            found = true;
        }
        else {
            i = ((i + 1) | 0);
        }
    }
    return found;
}
export function Queue$1__Clear(_) {
    _.count = 0;
    _.head = 0;
    _.tail = 0;
    fill(_.contents, 0, Queue$1__size(_), defaultOf());
}
export function Queue$1__TrimExcess(_) {
    if ((_.count / _.contents.length) > 0.9) {
        Queue$1__ensure_Z524259A4(_, _.count);
    }
}
export function Queue$1__ToArray(_) {
    return toArray(Queue$1__toSeq(_));
}
export function Queue$1__CopyTo_Z3B4C077E(_, target, start) {
    let i = start;
    const enumerator = getEnumerator(Queue$1__toSeq(_));
    try {
        while (enumerator["System.Collections.IEnumerator.MoveNext"]()) {
            const item = enumerator["System.Collections.Generic.IEnumerator`1.get_Current"]();
            setItem(target, i, item);
            i = ((i + 1) | 0);
        }
    }
    finally {
        disposeSafe(enumerator);
    }
}
export function Queue$1__size(this$) {
    return this$.contents.length;
}
export function Queue$1__toIndex_Z524259A4(this$, i) {
    return (this$.head + i) % Queue$1__size(this$);
}
export function Queue$1__ensure_Z524259A4(this$, requiredSize) {
    const newBuffer = fill(new Array(requiredSize), 0, requiredSize, null);
    if (this$.head < this$.tail) {
        copyTo(this$.contents, this$.head, newBuffer, 0, this$.count);
    }
    else {
        copyTo(this$.contents, this$.head, newBuffer, 0, Queue$1__size(this$) - this$.head);
        copyTo(this$.contents, 0, newBuffer, Queue$1__size(this$) - this$.head, this$.tail);
    }
    this$.head = 0;
    this$.contents = newBuffer;
    this$.tail = (((this$.count === Queue$1__size(this$)) ? 0 : this$.count) | 0);
}
export function Queue$1__toSeq(this$) {
    return delay(() => {
        let i = 0;
        return enumerateWhile(() => (i < this$.count), delay(() => append(singleton(item_1(Queue$1__toIndex_Z524259A4(this$, i), this$.contents)), delay(() => {
            i = ((i + 1) | 0);
            return empty();
        }))));
    });
}
