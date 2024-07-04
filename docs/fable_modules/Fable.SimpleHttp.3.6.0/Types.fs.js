import { Record, Union } from "../fable-library-js.4.19.3/Types.js";
import { obj_type, record_type, int32_type, option_type, bool_type, list_type, class_type, string_type, union_type } from "../fable-library-js.4.19.3/Reflection.js";

export class HttpMethod extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["GET", "POST", "PUT", "PATCH", "DELETE", "HEAD", "OPTIONS"];
    }
}

export function HttpMethod_$reflection() {
    return union_type("Fable.SimpleHttp.HttpMethod", [], HttpMethod, () => [[], [], [], [], [], [], []]);
}

export class Header extends Union {
    constructor(Item1, Item2) {
        super();
        this.tag = 0;
        this.fields = [Item1, Item2];
    }
    cases() {
        return ["Header"];
    }
}

export function Header_$reflection() {
    return union_type("Fable.SimpleHttp.Header", [], Header, () => [[["Item1", string_type], ["Item2", string_type]]]);
}

export class BodyContent extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Empty", "Text", "Binary", "Form"];
    }
}

export function BodyContent_$reflection() {
    return union_type("Fable.SimpleHttp.BodyContent", [], BodyContent, () => [[], [["Item", string_type]], [["Item", class_type("Browser.Types.Blob", undefined)]], [["Item", class_type("Browser.Types.FormData", undefined)]]]);
}

export class ResponseTypes extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Text", "Blob", "ArrayBuffer"];
    }
}

export function ResponseTypes_$reflection() {
    return union_type("Fable.SimpleHttp.ResponseTypes", [], ResponseTypes, () => [[], [], []]);
}

export class HttpRequest extends Record {
    constructor(url, method, headers, withCredentials, overridenMimeType, overridenResponseType, timeout, content) {
        super();
        this.url = url;
        this.method = method;
        this.headers = headers;
        this.withCredentials = withCredentials;
        this.overridenMimeType = overridenMimeType;
        this.overridenResponseType = overridenResponseType;
        this.timeout = timeout;
        this.content = content;
    }
}

export function HttpRequest_$reflection() {
    return record_type("Fable.SimpleHttp.HttpRequest", [], HttpRequest, () => [["url", string_type], ["method", HttpMethod_$reflection()], ["headers", list_type(Header_$reflection())], ["withCredentials", bool_type], ["overridenMimeType", option_type(string_type)], ["overridenResponseType", option_type(ResponseTypes_$reflection())], ["timeout", option_type(int32_type)], ["content", BodyContent_$reflection()]]);
}

export class ResponseContent extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Text", "Blob", "ArrayBuffer", "Unknown"];
    }
}

export function ResponseContent_$reflection() {
    return union_type("Fable.SimpleHttp.ResponseContent", [], ResponseContent, () => [[["Item", string_type]], [["Item", class_type("Browser.Types.Blob", undefined)]], [["Item", class_type("Fable.Core.JS.ArrayBuffer")]], [["Item", obj_type]]]);
}

export class HttpResponse extends Record {
    constructor(statusCode, responseText, responseType, responseUrl, responseHeaders, content) {
        super();
        this.statusCode = (statusCode | 0);
        this.responseText = responseText;
        this.responseType = responseType;
        this.responseUrl = responseUrl;
        this.responseHeaders = responseHeaders;
        this.content = content;
    }
}

export function HttpResponse_$reflection() {
    return record_type("Fable.SimpleHttp.HttpResponse", [], HttpResponse, () => [["statusCode", int32_type], ["responseText", string_type], ["responseType", string_type], ["responseUrl", string_type], ["responseHeaders", class_type("Microsoft.FSharp.Collections.FSharpMap`2", [string_type, string_type])], ["content", ResponseContent_$reflection()]]);
}

