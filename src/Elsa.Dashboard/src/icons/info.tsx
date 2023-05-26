import { h } from '@stencil/core';

const TrashCanIcon = (options?) => {
  return (
    <svg
      class={`elsa-h-5 elsa-w-5 ${options?.color ? `elsa-text-${options.color}-500` : ''} ${options?.hoverColor ? `hover:elsa-text-${options.hoverColor}-500` : ''}`}
      width="24" height="24" viewBox="0 0 24 24"
      stroke-width="2" stroke="currentColor" fill="transparent" stroke-linecap="round"
      stroke-linejoin="round">
      </svg>
  )


}

export default TrashCanIcon;
