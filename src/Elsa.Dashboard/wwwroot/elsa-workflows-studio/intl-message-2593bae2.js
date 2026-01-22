import { i as instance } from './i18n-loader-aa6cec69.js';
import './index-842ad20c.js';
import { h } from './index-1542df5c.js';

const IntlMessage = (props) => (h("intl-message", Object.assign({}, Object.assign({ i18next: instance }, props))));
function GetIntlMessage(i18next) {
  return (props) => (h("intl-message", Object.assign({}, Object.assign({ i18next }, props))));
}

export { GetIntlMessage as G };
