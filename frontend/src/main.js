import { createApp } from 'vue'
import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import axios from 'axios'
import VueAxios from 'vue-axios'
import {ElMessage} from 'element-plus'
import './utils/dialog'

axios.defaults.baseURL='https://localhost:7163'
// axios.defaults.baseURL='http://localhost:9099'
const app=createApp(App)
app.use(store)
    .use(router)
    .use(ElementPlus)
    .use(VueAxios, axios)
    .mount('#app')
app.config.globalProperties.$message=ElMessage
