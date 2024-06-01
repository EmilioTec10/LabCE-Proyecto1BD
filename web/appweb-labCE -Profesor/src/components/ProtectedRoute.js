import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { ThemeContext } from '../App';

const ProtectedRoute = ({ children }) => {
  const { loggedIn } = useContext(ThemeContext);

  if (!loggedIn) {
    // Redirige a la página de inicio de sesión si no está autenticado
    return <Navigate to="/" />;
  }

  return children;
};

export default ProtectedRoute;
