import { User } from "./user";
import { Post } from "./post";

export class Vote {
    upDown: string;
    userId: string;
    user: User;
    postId: string;
    post: Post;

    constructor(data: any) {
        if (data) {
            this.upDown = data.upDown;
            this.userId = data.userId;
            this.user = data.user;
            this.postId = data.postId;
            this.post = data.post;

        }
    }

}