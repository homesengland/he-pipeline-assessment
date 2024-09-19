import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';

import { createVuetify } from 'vuetify';
import 'vuetify/styles'; // Required Vuetify styles
import '@mdi/font/css/materialdesignicons.css'; // Material Design Icons
import * as components from 'vuetify/components'; // Import all Vuetify components
import * as directives from 'vuetify/directives'; // Import Vuetify directives

const vuetify = createVuetify({
  components, // Register all Vuetify components
  directives, // Register Vuetify directives
});

const app = createApp(App);

app.use(createPinia());
app.use(vuetify);

app.mount('#app');
