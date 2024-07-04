import { fromString, seq, array as array_1, object, string, fail, succeed, andThen } from "./fable_modules/Thoth.Json.10.2.0/Decode.fs.js";
import { stringHash, uncurry2, uncurry3 } from "./fable_modules/fable-library-js.4.19.3/Util.js";
import { JsonContainer, Post, Category } from "./Model/Model.js";
import { tryParse, minValue } from "./fable_modules/fable-library-js.4.19.3/Date.js";
import { FSharpRef } from "./fable_modules/fable-library-js.4.19.3/Types.js";
import { singleton } from "./fable_modules/fable-library-js.4.19.3/AsyncBuilder.js";
import { Http_request, Http_method, Http_overrideMimeType, Http_send } from "./fable_modules/Fable.SimpleHttp.3.6.0/Http.fs.js";
import { HttpMethod } from "./fable_modules/Fable.SimpleHttp.3.6.0/Types.fs.js";
import { toText, printf, toConsole } from "./fable_modules/fable-library-js.4.19.3/String.js";
import { startImmediate } from "./fable_modules/fable-library-js.4.19.3/Async.js";
import { contains as contains_1, filter, iterate, map, delay } from "./fable_modules/fable-library-js.4.19.3/Seq.js";
import { rangeDouble } from "./fable_modules/fable-library-js.4.19.3/Range.js";
import { contains } from "./fable_modules/fable-library-js.4.19.3/Array.js";

export const categoryDecoder = (path_2) => ((value_1) => andThen(uncurry3((_arg) => {
    switch (_arg) {
        case "Draft":
            return (arg10$0040) => ((arg20$0040) => succeed(new Category(0, []), arg10$0040, arg20$0040));
        case "Note":
            return (arg10$0040_1) => ((arg20$0040_1) => succeed(new Category(1, []), arg10$0040_1, arg20$0040_1));
        case "Article":
            return (arg10$0040_2) => ((arg20$0040_2) => succeed(new Category(2, []), arg10$0040_2, arg20$0040_2));
        default:
            return (path_1) => ((arg20$0040_3) => fail(`Unknown category: ${_arg}`, path_1, arg20$0040_3));
    }
}), string, path_2, value_1));

export const dateTimeDecoder = (path_2) => ((value_1) => andThen(uncurry3((dateString) => {
    let matchValue;
    let outArg = minValue();
    matchValue = [tryParse(dateString, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return (arg10$0040) => ((arg20$0040) => succeed(matchValue[1], arg10$0040, arg20$0040));
    }
    else {
        return (path_1) => ((arg20$0040_1) => fail(`Invalid date: ${dateString}`, path_1, arg20$0040_1));
    }
}), string, path_2, value_1));

export const postDecoder = (path_6) => ((v) => object((get$) => {
    let objectArg, objectArg_1, objectArg_2, objectArg_3, objectArg_4, objectArg_5, objectArg_6, objectArg_7;
    return new Post((objectArg = get$.Required, objectArg.Field("FileName", string)), (objectArg_1 = get$.Required, objectArg_1.Field("Title", string)), (objectArg_2 = get$.Required, objectArg_2.Field("Summary", string)), (objectArg_3 = get$.Required, objectArg_3.Field("Content", string)), (objectArg_4 = get$.Required, objectArg_4.Field("Tags", (path_5, value_5) => array_1(string, path_5, value_5))), (objectArg_5 = get$.Required, objectArg_5.Field("Category", uncurry2(categoryDecoder))), (objectArg_6 = get$.Required, objectArg_6.Field("Updated", uncurry2(dateTimeDecoder))), (objectArg_7 = get$.Required, objectArg_7.Field("Created", uncurry2(dateTimeDecoder))));
}, path_6, v));

export const jsonContainerDecoder = (path_1) => ((v) => object((get$) => {
    let objectArg;
    return new JsonContainer((objectArg = get$.Required, objectArg.Field("Posts", (path, value) => seq(uncurry2(postDecoder), path, value))));
}, path_1, v));

export function fetchJson(url) {
    return singleton.Delay(() => singleton.Bind(Http_send(Http_overrideMimeType("application/json", Http_method(new HttpMethod(0, []), Http_request(url)))), (_arg) => {
        const matchValue = fromString(uncurry2(jsonContainerDecoder), _arg.responseText);
        if (matchValue.tag === 1) {
            toConsole(printf("Failed to decode JSON: %s"))(matchValue.fields[0]);
            return singleton.Return(undefined);
        }
        else {
            const data = matchValue.fields[0];
            toConsole(printf("%A"))(data);
            return singleton.Return(data);
        }
    }));
}

export function addTagFilters() {
    startImmediate(singleton.Delay(() => singleton.Bind(fetchJson("SearchIndex.json"), (_arg) => {
        const searchIndex = _arg;
        if (searchIndex == null) {
            throw new Error("cannot access search index");
            return singleton.Zero();
        }
        else {
            const si = searchIndex;
            const nodes = document.getElementsByClassName("tag");
            const tags = delay(() => map((i) => (nodes[i]), rangeDouble(0, 1, nodes.length - 1)));
            const postNodes = document.getElementsByClassName("post-card");
            const posts = delay(() => map((i_1) => (postNodes[i_1]), rangeDouble(0, 1, postNodes.length - 1)));
            iterate((t) => {
                t.addEventListener("click", (_arg_1) => {
                    if (t.classList.contains("is-primary")) {
                        const value = t.classList.remove("is-primary");
                        iterate((p) => {
                            p.classList.remove("is-hidden");
                        }, posts);
                    }
                    else {
                        iterate((x) => {
                            const value_1 = x.classList.remove("is-primary");
                        }, tags);
                        iterate((p_1) => {
                            p_1.classList.remove("is-hidden");
                        }, posts);
                        const value_2 = t.classList.add("is-primary");
                        const postsToHide = map((p_3) => p_3.FileName, filter((p_2) => !contains(t.id, p_2.Tags, {
                            Equals: (x_1, y) => (x_1 === y),
                            GetHashCode: stringHash,
                        }), si.Posts));
                        iterate((p_5) => {
                            p_5.classList.toggle("is-hidden");
                        }, filter((p_4) => contains_1(p_4.id, postsToHide, {
                            Equals: (x_2, y_1) => (x_2 === y_1),
                            GetHashCode: stringHash,
                        }), posts));
                    }
                });
            }, tags);
            return singleton.Zero();
        }
    })));
}

export function expandHamburger() {
    const burger = document.getElementById("navbar-burger");
    const menu = document.getElementById("navbarBasicExample");
    burger.addEventListener("click", (_arg) => {
        burger.classList.toggle("is-active");
        menu.classList.toggle("is-active");
    });
}

export function hideFsiOutput() {
    const outputs = document.getElementsByClassName("fsdocs-tip");
    iterate((e) => {
        e.classList.add("is-hidden");
    }, delay(() => map((i) => (outputs[i]), rangeDouble(0, 1, outputs.length - 1))));
}

export function addCardClickEvents() {
    const postNodes = document.getElementsByClassName("post-card");
    iterate((p) => {
        p.addEventListener("click", (_arg) => {
            let arg;
            window.location.href = ((arg = p.id, toText(printf("%s.html"))(arg)));
        });
    }, delay(() => map((i) => (postNodes[i]), rangeDouble(0, 1, postNodes.length - 1))));
}

export function execScripts() {
    startImmediate(singleton.Delay(() => {
        const value = expandHamburger();
        const value_1 = hideFsiOutput();
        const value_2 = addTagFilters();
        const value_3 = addCardClickEvents();
        return singleton.Zero();
    }));
}

window.execScripts = (() => {
    execScripts();
});

