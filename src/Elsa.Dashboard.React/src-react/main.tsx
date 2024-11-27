import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import ElsaStudioRoot from './pages/elsa-studio-root'

createRoot(document.getElementById('dashboard-root')!).render(
  <StrictMode>
    <ElsaStudioRoot />
  </StrictMode>,
)

