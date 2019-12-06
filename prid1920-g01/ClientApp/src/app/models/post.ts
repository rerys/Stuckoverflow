import { User } from "./user";
import { Tag } from "./tag";
import { List } from "lodash";
import { Vote } from "./vote";
import { Comment } from "./comment";

export class Post {
    id: string;
    title: string;
    body: string;
    timestamp: string;
    parentId: string;
    parent: Post;
    responses: Post[];
    accepted: Post;
    userId: string;
    user: User;
    tags: Tag[];
    votes: Vote[];
    comments: Comment[];

    constructor(data: any) {
        this.id = data.id;
        this.title = data.title;
        this.body = data.body;
        this.timestamp = data.timestamp;
        this.parentId = data.parentId;
        this.parent = data.parent;
        this.responses = data.responses;
        this.accepted = data.accepted;
        this.userId = data.userId;
        this.user = data.user;
        this.tags = data.tags;
        this.votes = data.votes;
        this.comments = data.comments;
    }

}