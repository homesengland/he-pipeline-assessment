import { jsx as _jsx } from "react/jsx-runtime";
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import ElsaStudioRoot from './pages/elsa-studio-root';
createRoot(document.getElementById('dashboard-root')).render(_jsx(StrictMode, { children: _jsx(ElsaStudioRoot, {}) }));
