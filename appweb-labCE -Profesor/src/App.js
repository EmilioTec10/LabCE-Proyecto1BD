import React, { useState, useContext} from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';

import Login_profesor from './pages/login_profesor';
import Cambio_contrasenna from './pages/cambio_contrasenna';
import Gestion_reserva_laboratorios from './pages/gestion_reserva_laboratorios';
import Aprobacion_prestamo from './pages/aprobacion_prestamo';
import { createContext } from 'react';

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

  const handleAdminLogin = () => {
    setLoggedIn(true);
    setEmail('');
  };

  return (
    <div className="App">
      <BrowserRouter>
        {/* Proveedor del contexto */}
        <ThemeContext.Provider value={{ loggedIn, setLoggedIn, email, setEmail, theme, setTheme }}>
          {/* Rutas y componentes */}
          <Routes>
            
            <Route path="/" element={<Login_profesor setLoggedIn={setLoggedIn} />} />
            <Route path="/cambio_contrasenna" element={<Cambio_contrasenna  />} />
            <Route path="/gestion_reserva_laboratorios" element={<Gestion_reserva_laboratorios />} />
            <Route path="/aprobacion_prestamo" element={<Aprobacion_prestamo />} />
          </Routes>
        </ThemeContext.Provider>
      </BrowserRouter>
    </div>
  );
}

export default App;
