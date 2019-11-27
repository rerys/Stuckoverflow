import { User } from "./user";
import { Post } from "./post";

export class Vote {
    upDown: string;
    user: User;
    post: Post;

    constructor(data: any) {
        if (data) {
            this.upDown = data.upDown;
            this.user = data.user;
            this.post = data.post;

        }
    }

}