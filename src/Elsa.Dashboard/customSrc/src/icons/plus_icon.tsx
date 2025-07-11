import { h } from '@stencil/core';

const PlusIcon = (options?) => {
  return (
    <svg
      class={`-elsa-ml-1 elsa-mr-2 elsa-h-5 elsa-w-5 ${options?.color ? `elsa-text-${options.color}-500` : ''} ${options?.hoverColor ? `hover:elsa-text-${options.hoverColor}-500` : ''}`}
      width="24" height="24" viewBox="0 0 24 24"
      stroke-width="2" stroke="currentColor" fill="transparent" stroke-linecap="round"
      stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z" />
      <line x1="12" y1="5" x2="12" y2="19" />
      <line x1="5" y1="12" x2="19" y2="12" />
    </svg>
  )
}

export default PlusIcon;
