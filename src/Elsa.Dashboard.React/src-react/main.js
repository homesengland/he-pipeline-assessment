import { jsx as _jsx } from "react/jsx-runtime";
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import ElsaStudioRoot from './components/dashboard/pages/elsa-studio-root';
import { BrowserRouter } from 'react-router-dom';
createRoot(document.getElementById('dashboard-root')).render(_jsx(BrowserRouter, { children: _jsx(StrictMode, { children: _jsx(ElsaStudioRoot, {}) }) }));
