// import './assets/main.css'
import { createApp } from 'vue'
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
import { createPinia } from 'pinia'
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'
import App from './App.vue'
// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import router from './routes' 
import  VueCookies  from 'vue-cookies';


const vuetify = createVuetify({
  components,
  directives
})
const app = createApp(App)
const pinia = createPinia()
pinia.use(piniaPluginPersistedstate)
app.use(Toast)
app.use(router)
app.use(pinia)
app.use(vuetify)
app.use(VueCookies);

app.mount('#app')