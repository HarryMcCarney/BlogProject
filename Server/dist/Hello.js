import { printf, toConsole } from "./fable_modules/fable-library.4.2.1/String.js";

export function greet(name) {
    toConsole(printf("Hello, %s!"))(name);
}

greet("World");

