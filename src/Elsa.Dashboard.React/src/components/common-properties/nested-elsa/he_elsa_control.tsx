import { Component, h, Host, Prop, VNode } from '@stencil/core';

@Component({
  tag: 'he-elsa-control',
  shadow: false,
})
export class HeElsaControl {
  @Prop() content: VNode | string | Element;
  el: HTMLElement;

  render() {
    const content: any = this.content;

    if (typeof content === 'string')
      return <Host innerHTML={content} />;

    if (content != null && content.tagName != null)
      return <Host ref={el => this.el = el} />;

    return (
      <Host>{content}</Host>
    );
  }

  componentDidLoad() {
    if (!this.el)
      return;

    const content = this.content as HTMLElement;
    this.el.append(content);
  }
}
