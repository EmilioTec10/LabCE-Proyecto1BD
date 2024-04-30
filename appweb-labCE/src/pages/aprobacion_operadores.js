import React, { useContext } from 'react';
import { ThemeContext } from '../App'; // Asegúrate de que la ruta sea correcta

import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';

function AprobacionOperadores() {
  // Inicializa el contexto como un objeto vacío si no está disponible
  const context = useContext(ThemeContext) || {};

  // Obtén los valores del contexto
  const { loggedIn, setLoggedIn, email, setEmail, theme, setTheme } = context;

  return (
    <div className='aprob tabla'>
      <TableContainer>
        <TableHead>
          <TableRow>
            <TableCell>Nombre</TableCell>
            
          </TableRow>
        </TableHead>
      </TableContainer>
    </div>
  );
}

export default AprobacionOperadores;
