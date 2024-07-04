import { Record, Union } from "../fable_modules/fable-library-js.4.19.3/Types.js";
import { record_type, class_type, array_type, string_type, union_type } from "../fable_modules/fable-library-js.4.19.3/Reflection.js";

export class Category extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Draft", "Note", "Essay"];
    }
}

export function Category_$reflection() {
    return union_type("blog.Model.Category", [], Category, () => [[], [], []]);
}

export class Post extends Record {
    constructor(FileName, Title, Summary, Content, Tags, Category, Updated, Created) {
        super();
        this.FileName = FileName;
        this.Title = Title;
        this.Summary = Summary;
        this.Content = Content;
        this.Tags = Tags;
        this.Category = Category;
        this.Updated = Updated;
        this.Created = Created;
    }
}

export function Post_$reflection() {
    return record_type("blog.Model.Post", [], Post, () => [["FileName", string_type], ["Title", string_type], ["Summary", string_type], ["Content", string_type], ["Tags", array_type(string_type)], ["Category", Category_$reflection()], ["Updated", class_type("System.DateTime")], ["Created", class_type("System.DateTime")]]);
}

export class JsonContainer extends Record {
    constructor(Posts) {
        super();
        this.Posts = Posts;
    }
}

export function JsonContainer_$reflection() {
    return record_type("blog.Model.JsonContainer", [], JsonContainer, () => [["Posts", class_type("System.Collections.Generic.IEnumerable`1", [Post_$reflection()])]]);
}

