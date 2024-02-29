import {Api} from '~/utils/LyricDbApi'

export const ApiCore = new Api({
    baseURL: '/api'
})
export const useApi =  () => {
    return ApiCore
}
