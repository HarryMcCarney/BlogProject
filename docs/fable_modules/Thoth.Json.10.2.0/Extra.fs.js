import { empty as empty_1 } from "../fable-library-js.4.19.3/Map.js";
import { comparePrimitives } from "../fable-library-js.4.19.3/Util.js";
import { ExtraCoders } from "./Types.fs.js";

export const empty = new ExtraCoders("", empty_1({
    Compare: comparePrimitives,
}));

