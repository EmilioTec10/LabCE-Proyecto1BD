import React, { useState, useContext} from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';
import Home from './pages/home';
import Login_administrador from './pages/login_administrador';
import Login_profesor from './pages/login_profesor';
import Login_operador from './pages/login_operador';
import Menu_gestion_profesores from './pages/menu_gestion_profesores';
import Gestion_activos from './pages/gestion_activos';
import Gestion_profesores from './pages/gestion_profesores';
import Gestion_laboratorios from './pages/gestion_laboratorios';
import Aprobacion_operadores from './pages/aprobacion_operadores';
import Reportes from './pages/reportes';
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
            <Route path="/" element={<Home setLoggedIn={setLoggedIn} />} />
            <Route path="/login_operador" element={<Login_operador setLoggedIn={setLoggedIn} />} />
            <Route path="/login_administrador" element={<Login_administrador setLoggedIn={setLoggedIn} setEmail={setEmail} />} />
            <Route path="/login_profesor" element={<Login_profesor setLoggedIn={setLoggedIn} />} />
            <Route path="/menu_gestion_profesores" element={<Menu_gestion_profesores setLoggedIn={setLoggedIn} />} />
            <Route path="/gestion_activos" element={<Gestion_activos />} />
            <Route path="/gestion_profesores" element={<Gestion_profesores />} />
            <Route path="/gestion_laboratorios" element={<Gestion_laboratorios />} />
            <Route path="/aprobacion_operadores" element={<Aprobacion_operadores />} />
            <Route path="/reportes" element={<Reportes />} />
          </Routes>
        </ThemeContext.Provider>
      </BrowserRouter>
    </div>
  );
}

export default App;
