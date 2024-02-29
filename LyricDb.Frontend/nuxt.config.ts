// https://nuxt.com/docs/api/configuration/nuxt-config
import vuetify, {transformAssetUrls} from 'vite-plugin-vuetify'
export default defineNuxtConfig({
    build: {
        transpile: ['vuetify'],
    },
    modules: [
        'nuxt-monaco-editor',
        (_options, nuxt) => {
            nuxt.hooks.hook('vite:extendConfig', (config) => {
                // @ts-expect-error
                config.plugins.push(vuetify({autoImport: true}))
            })
        },
    ],
    vite: {
        vue: {
            template: {
                transformAssetUrls,
            },
        },
    },
    nitro: {
        devProxy: {
            '/api': 'http://localhost:5140'
        }
    },
    devtools: {enabled: true},
    ssr: false,
    monacoEditor:{
        locale: 'zh-hans'
    }
})
