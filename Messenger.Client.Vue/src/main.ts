import './assets/main.css'
import "primevue/resources/themes/lara-light-indigo/theme.css";
import 'primevue/resources/primevue.min.css';
import 'primeflex/primeflex.css';

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import InputText from 'primevue/inputText';
import DataView from 'primevue/dataView';
import Dialog from 'primevue/dialog';
import Card from 'primevue/card';
import Splitter from 'primevue/splitter';
import SplitterPanel from 'primevue/splitterpanel';
import Fieldset from 'primevue/fieldset';
import ScrollPanel from 'primevue/scrollpanel';
import StyleClass from 'primevue/styleclass';
import { SecureService } from './services/secure';

const app = createApp(App)
app.config.globalProperties.$secureService = new SecureService();
app.use(PrimeVue);
app.use(router)
app.directive('styleclass', StyleClass);

app.component('Button', Button);
app.component('InputText', InputText);
app.component('Dialog', Dialog);
app.component('DataView', DataView);
app.component('Card', Card);
app.component('Splitter', Splitter);
app.component('SplitterPanel', SplitterPanel);
app.component('Fieldset', Fieldset);
app.component('ScrollPanel', ScrollPanel);

app.mount('#app')
