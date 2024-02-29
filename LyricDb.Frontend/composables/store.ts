import {CurrentUser, type UserInfo} from "~/models/user";
import type {Ref} from "vue";

export const useUserInfo = () => {
    return useState<CurrentUser>('userInfo', () =>{
        return {
            id: '',
            name: '登录',
            email: '',
            avatar: '',
            role: 0,
            isLogin: false
        }
    })
}

// Code from https://juejin.cn/post/7255243298955247674 by @煎饼狗子丨

const enduring: { [key: string]: () => Ref<any> } = {
    useUserInfo
}

/**把所有指定数据保存到本地存储
 * @param key 要保存的数据名。不填的话就是保存全部（一般不填，统一在页面关闭时保存。如果是特别重要的数据，就时不时单独保存一下即可。）
 */
export const setLocal = (key?: string) => {
    if (key) {
        const useKey = 'use' + key.slice(0, 1).toUpperCase() + key.slice(1).toLowerCase();
        const func = enduring[useKey]
        if (!func) {
            console.log('error while finding ', useKey, ' function');
            return
        }
        localStorage.setItem(key, JSON.stringify(func().value))
    } else {
        for (const key in enduring) {
            if (Object.prototype.hasOwnProperty.call(enduring, key)) {
                const element = enduring[key];
                const setKey = key.toLowerCase().substring(3)
                try {
                    localStorage.setItem(setKey, JSON.stringify(element().value))
                } catch (error) {
                    console.log(`error while storing ${setKey} data`, error);
                }
            }
        }
    }
}
export const getLocal = () => {
    for (const key in enduring) {
        if (Object.prototype.hasOwnProperty.call(enduring, key)) {
            const element = enduring[key];
            const setKey = key.toLowerCase().substring(3)
            try {
                const localData = localStorage.getItem(setKey) || ''
                if (localData) {
                    element().value = JSON.parse(localData)
                }
            } catch (error) {
                console.log(`error while getting data from localstorage`, error);
            }
        }
    }
}