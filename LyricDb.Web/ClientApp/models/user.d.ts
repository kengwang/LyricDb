export interface UserInfo
{
    id: string;
    name: string;
    avatar: string;
    role: number;
}

export class CurrentUser extends UserInfo
{
    id: string;
    name: string;
    avatar: string;
    role: number;
    isLogin: boolean;
}