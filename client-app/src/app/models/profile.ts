import { User } from "./user";

export interface Profile {
  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
  followersCount: number;
  followingCount: number;
  following: boolean;
}

export class Profile implements Profile {
  constructor(user: User) {
    this.username = user.username;
    this.displayName = user.displayName;
    this.image = user.image;
  }

  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  followersCount = 0;
  followingCount = 0;
  following = false;
}

export interface Photo {
  id: string;
  url: string;
  isMain: boolean;
}
