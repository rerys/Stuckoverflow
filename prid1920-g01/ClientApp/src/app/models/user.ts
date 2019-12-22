import { List } from "lodash";
import { Post } from "./post";
import { Vote } from "./vote";

export enum Role {
  Member = 0,
  Manager = 1,
  Admin = 2
}

export class User {

  id: string;
  pseudo: string;
  password: string;
  email: string;
  lastName: string;
  firstName: string;
  birthDate: string;
  reputation: string;
  role: Role;
  token: string;
  posts: List<Post>;
  votes: List<Vote>;
  comments: List<Comment>;
  nbPosts: string;

  constructor(data: any) {

    if (data) {

      this.id = data.id;
      this.pseudo = data.pseudo;
      this.password = data.password;
      this.email = data.email;
      this.lastName = data.lastName;
      this.firstName = data.firstName;
      this.birthDate = data.birthDate &&
        data.birthDate.length > 10 ? data.birthDate.substring(0, 10) : data.birthDate;
      this.reputation = data.reputation;
      this.role = data.role || Role.Member;
      this.token = data.token;
      this.posts = data.posts;
      this.votes = data.votes;
      this.comments = data.comments;
      this.nbPosts = data.nbPosts;
    }

  }

  public get roleAsString(): string {
    return Role[this.role];
  }

}