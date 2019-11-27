import { User } from "./user";
import { Tag } from "./tag";
import { List } from "lodash";
import { Vote } from "./vote";
import { Comment } from "./comment";

export class Post{
    id: string;
    title: string;
    body: string;
    timestamp: string;
    user: User;
    tags: List<Tag>;
    votes: List<Vote>;
    comments: List<Comment>;

    constructor(data: any){
        this.id = data.id;
        this.title = data.title;
        this.body = data.body;
        this.timestamp = data.timestamp;
        this.user = data.user;
        this.tags = data.tags;
        this.votes = data.votes;
        this.comments = data.comments;
    }

}