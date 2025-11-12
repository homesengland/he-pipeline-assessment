// Shared navigation helpers for workflow definitions and instances

export function navigateTo(path: string) {
  // Check if the dashboard has a custom navigation handler
  if ((window as any).__navigateTo) {
    (window as any).__navigateTo(path);
  } else {
    // Fallback to changing pathname and triggering popstate
    window.history.pushState(null, '', path);
    window.dispatchEvent(new PopStateEvent('popstate'));
  }
}

export function navigateToInstance(instanceId: string) {
  navigateTo(`/workflow-instances/${instanceId}`);
}
