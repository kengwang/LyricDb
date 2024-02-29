export const HumanizerRole = function (role: number) {
    switch (role) {
        case 0:
            return "未认证";
        case 1:
            return "用户";
        case 2:
            return "审核员";
        case 3:
            return "管理员";
        default:
            return "Unknown";
    }
}

export const KnownMusicPlatforms = [
    {
        id: 'ncm',
        name: '网易云音乐'
    },
    {
        id: 'qmc',
        name: 'QQ音乐'
    },
    {
        id: 'apl',
        name: 'Apple Music'
    },
    {
        id: 'spo',
        name: 'Spotify'
    }
]