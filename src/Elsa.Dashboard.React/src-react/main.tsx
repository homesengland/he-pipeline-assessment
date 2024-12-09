import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import ElsaStudioRoot from './components/dashboard/pages/elsa-studio-root'
import { BrowserRouter } from 'react-router-dom'

createRoot(document.getElementById('dashboard-root')!).render(
  <BrowserRouter>
  <StrictMode>
    <ElsaStudioRoot />
  </StrictMode>
  </BrowserRouter>
)

