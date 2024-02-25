import type {UserInfoResponse} from "~/utils/LyricDbApi";
import type {UserInfo} from "~/models/user";

export function mapUserInfoResponseToUserInfo(userRes: UserInfoResponse | undefined): UserInfo {
    if (!userRes)
        return {
            id: '0',
            name: '未知用户',
            avatar: '',
            role: 0,
        }
    return {
        id: userRes.id!,
        name: userRes.userName!,
        avatar: "https://cravatar.cn/avatar/" + userRes.avatar,
        role: userRes.role!,
    }
}

export function roleToColorMapper(role:number) {
    switch (role) {
        case 0:
            return 'gray'
        case 1:
            return 'green'
        case 2:
            return 'lime'
        case 3:
            return 'blue'
    }
}