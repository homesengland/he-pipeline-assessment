import { h } from '@stencil/core';

const MinimiseIcon = (options?) => {
  return (
    <svg
      class={`elsa-h-5 elsa-w-5 ${options?.color ? `elsa-text-${options.color}-500` : ''} ${options?.hoverColor ? `hover:elsa-text-${options.hoverColor}-500` : ''}`}
      width="24px"
      height="24px"
      viewBox="0 0 24 24"
      fill="none">

      <path d="M19.9604 11.4802C19.9604 13.8094 19.0227 15.9176 17.5019 17.4512C16.9332 18.0247 16.2834 18.5173 15.5716 18.9102C14.3594 19.5793 12.9658 19.9604 11.4802 19.9604C6.79672 19.9604 3 16.1637 3 11.4802C3 6.79672 6.79672 3 11.4802 3C16.1637 3 19.9604 6.79672 19.9604 11.4802Z" stroke="#333333" stroke-width="2" />
      <path d="M18.1553 18.1553L21.8871 21.8871" stroke="#333333" stroke-width="2" stroke-linecap="round" />
      <path d="M8 11.5492H15.0983" stroke="#333333" stroke-width="2" stroke-linecap="round" />
    </svg>
  )


}

export default MinimiseIcon;
