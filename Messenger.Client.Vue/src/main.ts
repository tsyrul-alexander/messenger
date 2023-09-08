import './assets/main.css'
import "primevue/resources/themes/lara-light-indigo/theme.css";

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import InputText from 'primevue/inputText';
import DataView from 'primevue/dataView';
import Dialog from 'primevue/dialog';
import StyleClass from 'primevue/styleclass';

const app = createApp(App)
app.use(PrimeVue);
app.use(router)
app.directive('styleclass', StyleClass);

app.component('Button', Button);
app.component('InputText', InputText);
app.component('Dialog', Dialog);
app.component('DataView', DataView);

app.mount('#app')
