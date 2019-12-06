import { User } from "./user";
import { Post } from "./post";

export class Comment {
    id: string;
    body: string;
    timestamp: string;
    userId: string;
    user: User;
    postId: string;
    post: Post;

    constructor(data: any) {
        this.id = data.id;
        this.body = data.body;
        this.timestamp = data.timestamp;
        this.user = data.user;
        this.post = data.post;
    }
}