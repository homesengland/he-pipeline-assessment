import { h } from '@stencil/core';

const SortIcon = (options?) => {
  return (
    <svg
      class={`elsa-h-5 elsa-w-5 ${options?.color ? `elsa-text-${options.color}-500` : ''} ${options?.hoverColor ? `hover:elsa-text-${options.hoverColor}-500` : ''}`}
      width="24px"
      height="24px"
      viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
      <path d="M15 20V5h3l-5.1-5L8 5h3v15zM2 0v15h-3l4.9 5L9 15H6V0z" /></svg>
  )
}

export default SortIcon;
