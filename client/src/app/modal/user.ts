export interface User {
  id: number;
  name: string;
  userName: string;
  gender: string;
  token: string;
  photoUrl: string;
  roles: string[];
}

export interface UserInfo {
  id: number;
  name: string;
  userName: string;
  photoUrl: string;
  exist: boolean;
}
