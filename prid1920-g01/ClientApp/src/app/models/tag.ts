import { List } from "lodash";
import { Post } from "./post";

export class Tag{
    id: string;
    name: string;
    posts: List<Post>;

    constructor(data: any){
        this.id = data.id;
        this.name = data.name;
        this.posts = data.posts;
    }
}