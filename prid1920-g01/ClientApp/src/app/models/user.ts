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

  role: Role;

  token: string;

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


      this.role = data.role || Role.Member;

      this.token = data.token;
    }

  }

  public get roleAsString(): string {

    return Role[this.role];

  }

}