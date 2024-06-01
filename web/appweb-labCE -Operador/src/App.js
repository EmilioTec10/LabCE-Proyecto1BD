import React, { useState, useContext} from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import './App.css';
import Home from './pages/home';
import Login_operador from './pages/login_operador';
import Prestamo_estudiante from './pages/prestamo_estudiante';
import Reserva_laboratorio from './pages/reserva_laboratorio';
import Prestamo_profesor from './pages/prestamo_profesor';
import Prestamos_aprobados from './pages/prestamos_aprobados';
import Devolucion_activo from './pages/devolucion_activo';
import Marcar_Horas from './pages/marcar_horas';
import Reportes from './pages/reportes';
import Registro from './pages/registro';
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
            <Route path="/registro" element={<Registro setLoggedIn={setLoggedIn} />} />
            <Route path="/prestamo_estudiante" element={<Prestamo_estudiante />} />
            <Route path="/marcar_horas" element={<Marcar_Horas />} />
            <Route path="/reserva_laboratorio" element={<Reserva_laboratorio />} />
            <Route path="/prestamo_profesor" element={<Prestamo_profesor />} />
            <Route path="/prestamos_aprobados" element={<Prestamos_aprobados />} />
            <Route path="/devolucion_activo" element={<Devolucion_activo />} />
            <Route path="/reportes" element={<Reportes />} />
          </Routes>
        </ThemeContext.Provider>
      </BrowserRouter>
    </div>
  );
}

export default App;
