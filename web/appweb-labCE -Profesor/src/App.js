import React, { useState, createContext } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';

import Login_profesor from './pages/login_profesor';
import Cambio_contrasenna from './pages/cambio_contrasenna';
import Gestion_reserva_laboratorios from './pages/gestion_reserva_laboratorios';
import Aprobacion_prestamo from './pages/aprobacion_prestamo';
import ProtectedRoute from './components/ProtectedRoute'; // Asegúrate de importar correctamente

// Definimos y exportamos ThemeContext aquí
export const ThemeContext = createContext({
  loggedIn: false,
  setLoggedIn: () => {},
  email: '',
  setEmail: () => {},
  theme: 'light',
  setTheme: () => {},
});

function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const [email, setEmail] = useState('');
  const [theme, setTheme] = useState('light');

  return (
    <div className="App">
      <BrowserRouter>
        {/* Proveedor del contexto */}
        <ThemeContext.Provider value={{ loggedIn, setLoggedIn, email, setEmail, theme, setTheme }}>
          {/* Rutas y componentes */}
          <Routes>
            <Route path="/" element={<Login_profesor />} />
            <Route path="/cambio_contrasenna" element={<ProtectedRoute><Cambio_contrasenna /></ProtectedRoute>} />
            <Route path="/gestion_reserva_laboratorios" element={<ProtectedRoute><Gestion_reserva_laboratorios /></ProtectedRoute>} />
            <Route path="/aprobacion_prestamo" element={<ProtectedRoute><Aprobacion_prestamo /></ProtectedRoute>} />
          </Routes>
        </ThemeContext.Provider>
      </BrowserRouter>
    </div>
  );
}

export default App;
