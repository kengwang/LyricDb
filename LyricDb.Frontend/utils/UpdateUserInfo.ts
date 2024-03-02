export function updateUserInfo() {
    useApi().user.getMyUserInfo()
        .then((res) => {
            let userInfo = useUserInfo()
            userInfo.value = {
                id: res.data.id!,
                name: res.data.userName!,
                avatar: "https://cravatar.cn/avatar/" + res.data.avatar,
                role: res.data.role!,
                isLogin: true as boolean
            }
        }).catch(() => {
        let userInfo = useUserInfo()
        userInfo.value.isLogin = false as boolean
        userInfo.value = {
            id: '',
            name: "登录",
            avatar: "https://cravatar.cn/avatar/0" as string,
            role: 0,
            isLogin: false as boolean
        }
    })

}