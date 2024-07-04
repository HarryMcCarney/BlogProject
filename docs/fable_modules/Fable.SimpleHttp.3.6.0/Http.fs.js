import { fromContinuations } from "../fable-library-js.4.19.3/Async.js";
import { HttpResponse, ResponseContent, HttpRequest, BodyContent, HttpMethod, Header } from "./Types.fs.js";
import { tail, head, isEmpty, ofArray as ofArray_1, singleton, append, empty } from "../fable-library-js.4.19.3/List.js";
import { ofArray, empty as empty_1 } from "../fable-library-js.4.19.3/Map.js";
import { disposeSafe, getEnumerator, comparePrimitives } from "../fable-library-js.4.19.3/Util.js";
import { join, split, isNullOrEmpty } from "../fable-library-js.4.19.3/String.js";
import { choose } from "../fable-library-js.4.19.3/Array.js";
import { some } from "../fable-library-js.4.19.3/Option.js";
import { singleton as singleton_1 } from "../fable-library-js.4.19.3/AsyncBuilder.js";

/**
 * Download a Blob
 */
export function Blob_download(blob, fileName) {
    const element = document.createElement("a");
    element.target = "_blank";
    element.href = (window.URL.createObjectURL(blob));
    element.setAttribute("download", fileName);
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);
}

/**
 * Asynchronously reads the blob data content as string
 */
export function FileReader_readBlobAsText(blob) {
    return fromContinuations((tupledArg) => {
        const reader = new FileReader();
        reader.onload = ((_arg_2) => {
            if (reader.readyState === 2) {
                tupledArg[0](reader.result);
            }
        });
        reader.readAsText(blob);
    });
}

/**
 * Asynchronously reads the blob data content as string
 */
export function FileReader_readFileAsText(file) {
    return fromContinuations((tupledArg) => {
        const reader = new FileReader();
        reader.onload = ((_arg_2) => {
            if (reader.readyState === 2) {
                tupledArg[0](reader.result);
            }
        });
        reader.readAsText(file);
    });
}

/**
 * Appends a key-value pair to the form data
 */
export function FormData_append(key, value, form) {
    form.append(key, value);
    return form;
}

/**
 * Appends a file to the form data
 */
export function FormData_appendFile(key, file, form) {
    form.append(key, file);
    return form;
}

/**
 * Appends a named file to the form data
 */
export function FormData_appendNamedFile(key, fileName, file, form) {
    form.append(key, file, fileName);
    return form;
}

/**
 * Appends a blog to the form data
 */
export function FormData_appendBlob(key, blob, form) {
    form.append(key, blob);
    return form;
}

/**
 * Appends a blog to the form data
 */
export function FormData_appendNamedBlob(key, fileName, blob, form) {
    form.append(key, blob, fileName);
    return form;
}

export function Headers_contentType(value) {
    return new Header("Content-Type", value);
}

export function Headers_accept(value) {
    return new Header("Accept", value);
}

export function Headers_acceptCharset(value) {
    return new Header("Accept-Charset", value);
}

export function Headers_acceptEncoding(value) {
    return new Header("Accept-Encoding", value);
}

export function Headers_acceptLanguage(value) {
    return new Header("Accept-Language", value);
}

export function Headers_acceptDateTime(value) {
    return new Header("Accept-Datetime", value);
}

export function Headers_authorization(value) {
    return new Header("Authorization", value);
}

export function Headers_cacheControl(value) {
    return new Header("Cache-Control", value);
}

export function Headers_connection(value) {
    return new Header("Connection", value);
}

export function Headers_cookie(value) {
    return new Header("Cookie", value);
}

export function Headers_contentMD5(value) {
    return new Header("Content-MD5", value);
}

export function Headers_date(value) {
    return new Header("Date", value);
}

export function Headers_expect(value) {
    return new Header("Expect", value);
}

export function Headers_ifMatch(value) {
    return new Header("If-Match", value);
}

export function Headers_ifModifiedSince(value) {
    return new Header("If-Modified-Since", value);
}

export function Headers_ifNoneMatch(value) {
    return new Header("If-None-Match", value);
}

export function Headers_ifRange(value) {
    return new Header("If-Range", value);
}

export function Headers_IfUnmodifiedSince(value) {
    return new Header("If-Unmodified-Since", value);
}

export function Headers_maxForwards(value) {
    return new Header("Max-Forwards", value);
}

export function Headers_origin(value) {
    return new Header("Origin", value);
}

export function Headers_pragma(value) {
    return new Header("Pragma", value);
}

export function Headers_proxyAuthorization(value) {
    return new Header("Proxy-Authorization", value);
}

export function Headers_range(value) {
    return new Header("Range", value);
}

export function Headers_referer(value) {
    return new Header("Referer", value);
}

export function Headers_userAgent(value) {
    return new Header("User-Agent", value);
}

export function Headers_create(key, value) {
    return new Header(key, value);
}

const Http_defaultRequest = new HttpRequest("", new HttpMethod(0, []), empty(), false, undefined, undefined, undefined, new BodyContent(0, []));

const Http_emptyResponse = new HttpResponse(0, "", "", "", empty_1({
    Compare: comparePrimitives,
}), new ResponseContent(0, [""]));

function Http_splitAt(delimiter, input) {
    if (isNullOrEmpty(input)) {
        return new Array(0);
    }
    else {
        return split(input, [delimiter], undefined, 0);
    }
}

function Http_serializeMethod(_arg) {
    switch (_arg.tag) {
        case 1:
            return "POST";
        case 3:
            return "PATCH";
        case 2:
            return "PUT";
        case 4:
            return "DELETE";
        case 6:
            return "OPTIONS";
        case 5:
            return "HEAD";
        default:
            return "GET";
    }
}

/**
 * Starts the configuration of the request with the specified url
 */
export function Http_request(url) {
    return new HttpRequest(url, Http_defaultRequest.method, Http_defaultRequest.headers, Http_defaultRequest.withCredentials, Http_defaultRequest.overridenMimeType, Http_defaultRequest.overridenResponseType, Http_defaultRequest.timeout, Http_defaultRequest.content);
}

/**
 * Sets the Http method of the request
 */
export function Http_method(httpVerb, req) {
    return new HttpRequest(req.url, httpVerb, req.headers, req.withCredentials, req.overridenMimeType, req.overridenResponseType, req.timeout, req.content);
}

/**
 * Appends a header to the request configuration
 */
export function Http_header(singleHeader, req) {
    return new HttpRequest(req.url, req.method, append(req.headers, singleton(singleHeader)), req.withCredentials, req.overridenMimeType, req.overridenResponseType, req.timeout, req.content);
}

/**
 * Appends a list of headers to the request configuration
 */
export function Http_headers(values, req) {
    return new HttpRequest(req.url, req.method, append(req.headers, values), req.withCredentials, req.overridenMimeType, req.overridenResponseType, req.timeout, req.content);
}

/**
 * Enables cross-site credentials such as cookies
 */
export function Http_withCredentials(enabled, req) {
    return new HttpRequest(req.url, req.method, req.headers, enabled, req.overridenMimeType, req.overridenResponseType, req.timeout, req.content);
}

/**
 * Enables Http request timeout
 */
export function Http_withTimeout(timeoutInMilliseconds, req) {
    return new HttpRequest(req.url, req.method, req.headers, req.withCredentials, req.overridenMimeType, req.overridenResponseType, timeoutInMilliseconds, req.content);
}

/**
 * Specifies a MIME type other than the one provided by the server to be used instead when interpreting the data being transferred in a request. This may be used, for example, to force a stream to be treated and parsed as "text/xml", even if the server does not report it as such.
 */
export function Http_overrideMimeType(value, req) {
    return new HttpRequest(req.url, req.method, req.headers, req.withCredentials, value, req.overridenResponseType, req.timeout, req.content);
}

/**
 * Change the expected response type from the server
 */
export function Http_overrideResponseType(value, req) {
    return new HttpRequest(req.url, req.method, req.headers, req.withCredentials, req.overridenMimeType, value, req.timeout, req.content);
}

/**
 * Sets the body content of the request
 */
export function Http_content(bodyContent, req) {
    return new HttpRequest(req.url, req.method, req.headers, req.withCredentials, req.overridenMimeType, req.overridenResponseType, req.timeout, bodyContent);
}

/**
 * Sends the request to the server, this function does not throw
 */
export function Http_send(req) {
    return fromContinuations((tupledArg) => {
        const xhr = new XMLHttpRequest();
        xhr.open(Http_serializeMethod(req.method), req.url);
        xhr.onreadystatechange = (() => {
            let responseText, matchValue, statusCode, responseType, content, matchValue_1, responseHeaders;
            if (xhr.readyState === 4) {
                tupledArg[0]((responseText = ((matchValue = xhr.responseType, (matchValue === "") ? xhr.responseText : ((matchValue === "text") ? xhr.responseText : ""))), (statusCode = (xhr.status | 0), (responseType = xhr.responseType, (content = ((matchValue_1 = xhr.responseType, (matchValue_1 === "") ? (new ResponseContent(0, [xhr.responseText])) : ((matchValue_1 === "text") ? (new ResponseContent(0, [xhr.responseText])) : ((matchValue_1 === "arraybuffer") ? (new ResponseContent(2, [xhr.response])) : ((matchValue_1 === "blob") ? (new ResponseContent(1, [xhr.response])) : (new ResponseContent(3, [xhr.response]))))))), (responseHeaders = ofArray(choose((headerLine) => {
                    const matchValue_2 = ofArray_1(Http_splitAt(":", headerLine));
                    if (!isEmpty(matchValue_2)) {
                        return [head(matchValue_2).toLocaleLowerCase(), join(":", tail(matchValue_2)).trim()];
                    }
                    else {
                        return undefined;
                    }
                }, Http_splitAt("\r\n", xhr.getAllResponseHeaders())), {
                    Compare: comparePrimitives,
                }), new HttpResponse(statusCode, responseText, responseType, xhr.responseURL, responseHeaders, content)))))));
            }
        });
        const enumerator = getEnumerator(req.headers);
        try {
            while (enumerator["System.Collections.IEnumerator.MoveNext"]()) {
                const forLoopVar = enumerator["System.Collections.Generic.IEnumerator`1.get_Current"]();
                xhr.setRequestHeader(forLoopVar.fields[0], forLoopVar.fields[1]);
            }
        }
        finally {
            disposeSafe(enumerator);
        }
        xhr.withCredentials = req.withCredentials;
        const matchValue_3 = req.overridenMimeType;
        if (matchValue_3 == null) {
        }
        else {
            const mimeType = matchValue_3;
            xhr.overrideMimeType(mimeType);
        }
        const matchValue_4 = req.overridenResponseType;
        if (matchValue_4 == null) {
        }
        else {
            switch (matchValue_4.tag) {
                case 1: {
                    xhr.responseType = "blob";
                    break;
                }
                case 2: {
                    xhr.responseType = "arraybuffer";
                    break;
                }
                default:
                    xhr.responseType = "text";
            }
        }
        const matchValue_5 = req.timeout;
        if (matchValue_5 == null) {
        }
        else {
            const timeout = matchValue_5 | 0;
            xhr.timeout = (timeout | 0);
        }
        const matchValue_6 = req.content;
        switch (matchValue_6.tag) {
            case 1: {
                xhr.send(some(matchValue_6.fields[0]));
                break;
            }
            case 3: {
                xhr.send(some(matchValue_6.fields[0]));
                break;
            }
            case 2: {
                xhr.send(some(matchValue_6.fields[0]));
                break;
            }
            default:
                xhr.send();
        }
    });
}

/**
 * Safely sends a GET request and returns a tuple(status code * response text). This function does not throw.
 */
export function Http_get(url) {
    return singleton_1.Delay(() => singleton_1.Bind(Http_send(Http_method(new HttpMethod(0, []), Http_request(url))), (_arg) => {
        const response = _arg;
        return singleton_1.Return([response.statusCode, response.responseText]);
    }));
}

/**
 * Safely sends a PUT request and returns a tuple(status code * response text). This function does not throw.
 */
export function Http_put(url, data) {
    return singleton_1.Delay(() => singleton_1.Bind(Http_send(Http_content(new BodyContent(1, [data]), Http_method(new HttpMethod(2, []), Http_request(url)))), (_arg) => {
        const response = _arg;
        return singleton_1.Return([response.statusCode, response.responseText]);
    }));
}

/**
 * Safely sends a DELETE request and returns a tuple(status code * response text). This function does not throw.
 */
export function Http_delete(url) {
    return singleton_1.Delay(() => singleton_1.Bind(Http_send(Http_method(new HttpMethod(4, []), Http_request(url))), (_arg) => {
        const response = _arg;
        return singleton_1.Return([response.statusCode, response.responseText]);
    }));
}

/**
 * Safely sends a PATCH request and returns a tuple(status code * response text). This function does not throw.
 */
export function Http_patch(url, data) {
    return singleton_1.Delay(() => singleton_1.Bind(Http_send(Http_content(new BodyContent(1, [data]), Http_method(new HttpMethod(3, []), Http_request(url)))), (_arg) => {
        const response = _arg;
        return singleton_1.Return([response.statusCode, response.responseText]);
    }));
}

/**
 * Safely sends a POST request and returns a tuple(status code * response text). This function does not throw.
 */
export function Http_post(url, data) {
    return singleton_1.Delay(() => singleton_1.Bind(Http_send(Http_content(new BodyContent(1, [data]), Http_method(new HttpMethod(1, []), Http_request(url)))), (_arg) => {
        const response = _arg;
        return singleton_1.Return([response.statusCode, response.responseText]);
    }));
}

