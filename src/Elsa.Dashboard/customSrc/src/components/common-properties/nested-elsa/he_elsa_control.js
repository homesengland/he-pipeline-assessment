var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Host, Prop } from '@stencil/core';
let HeElsaControl = class HeElsaControl {
    render() {
        const content = this.content;
        if (typeof content === 'string')
            return h(Host, { innerHTML: content });
        if (content != null && content.tagName != null)
            return h(Host, { ref: el => this.el = el });
        return (h(Host, null, content));
    }
    componentDidLoad() {
        if (!this.el)
            return;
        const content = this.content;
        this.el.append(content);
    }
};
__decorate([
    Prop()
], HeElsaControl.prototype, "content", void 0);
HeElsaControl = __decorate([
    Component({
        tag: 'he-elsa-control',
        shadow: false,
    })
], HeElsaControl);
export { HeElsaControl };
