import React, { useContext, useState, useEffect } from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { Table, Button } from 'reactstrap';
import { Light, Dark } from '../styles/themes';
import logo from '../assets/react.svg';
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import DataTable from 'react-data-table-component';
import Paper from '@mui/material/Paper';
import axios from 'axios'; // Importar Axios

import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import TextField from '@mui/material/TextField';
import DialogActions from '@mui/material/DialogActions';

const linksArray = [
  {
    label: 'Reserva Laboratorio',
    icon: <AiOutlineHome />,
    to: '/reserva_laboratorio',
  },
  {
    label: 'Prestamo Profesor',
    icon: <MdOutlineAnalytics />,
    to: '/prestamo_profesor',
  },
  {
    label: 'Prestamo Estudiante',
    icon: <AiOutlineApartment />,
    to: '/prestamo_estudiante',
  },
  {
    label: 'Devolucion Activo',
    icon: <MdOutlineAnalytics />,
    to: '/devolucion_activo',
  },
  {
    label: 'Reportes',
    icon: <MdOutlineAnalytics />,
    to: '/reportes',
  },
];

const secondarylinksArray = [
  {
    label: 'Salir',
    icon: <MdLogout />,
    to: '/',
  },
];

const Prestamo_profesor = () => {
  const { setTheme, theme } = useContext(ThemeContext);
  const [data, setData] = useState([]);
  const [selectedData, setSelectedData] = useState(null);
  const [password, setPassword] = useState('');
  const [passwordDialogOpen, setPasswordDialogOpen] = useState(false);
  const themeStyle = theme === 'dark' ? Light : Dark;

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await axios.get('http://localhost:5129/api/Operador/prestamos-pendientes-profesores');
      setData(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  
  const handleDevolverActivo = (dato) => {
    setSelectedData(dato);
    setPasswordDialogOpen(true);
  };

  const handleConfirmDevolucion = () => {
    setPasswordDialogOpen(false);
    devolverActivo(selectedData, password);
  };

  const devolverActivo = (dato, contraseñaProf) => {
    // Realizar la solicitud HTTP
    axios.post('http://localhost:5129/api/Operador/devolucion-activo-profesor', {
      placa: dato.placa,
      email_prof: dato.email_prof,
      contraseña_prof: contraseñaProf
    })
    .then(response => {
      // Manejar la respuesta de la API
      console.log(response.data); // Imprimir la respuesta en la consola
      alert("Devolución de activo por parte del profesor registrada correctamente");
      setData(prevData => prevData.filter(item => item.placa !== dato.placa));
    })
    .catch(error => {
      console.log(dato.email_prof);
      console.log(dato.placa);
      console.log(contraseñaProf);
      // Manejar los errores de la solicitud
      console.error(error); // Imprimir el error en la consola
      alert("No se pudo realizar la devolución del activo. Por favor, verifique las credenciales del profesor.");
    });
  };

  const CambiarTheme = () => {
    setTheme((theme) => (theme === 'light' ? 'dark' : 'light'));
  };

  const columnas = [
    {
      name: 'Nombre',
      selector: row => row.nombre,
      sortable: true
    },
    {
      name: 'Apellidos',
      selector: row => row.apellidos,
      sortable: true
    },
    {
      name: 'Email Profesor',
      selector: row => row.email_prof,
      sortable: true
    },
    {
      name: 'Estado',
      selector: row => row.estado,
      sortable: true
    },
    {
      name: 'Placa',
      selector: row => row.placa,
      sortable: true
    },
    {
      name: 'Fecha de Solicitud',
      selector: row => {
        const fechaHora = row.fecha_hora_solicitud.split('T');
        return fechaHora[0]; // Obtener solo la fecha
      },
      sortable: true
    },
    {
      name: 'Hora de Solicitud',
      selector: row => {
        const fechaHora = row.fecha_hora_solicitud.split('T');
        return fechaHora[1]; // Obtener solo la hora
      },
      sortable: true
    },
    {
      name: 'Acciones',
      cell: row => <button style={{backgroundColor: 'blue', color: 'white'}}>Devolver Activo</button>,
      ignoreRowClick: true,
      allowOverflow: true,
      button: true,
    },
  ];

  return (
    <ThemeProvider theme={themeStyle}>
      <Container>
        <Sidebar>
          <div className="Logocontent">
            <div className="imgcontent">
              <img src={logo} alt="logo" />
            </div>
            <h2>LabCE</h2>
          </div>
          {linksArray.map(({ icon, label, to }) => (
            <div className="LinkContainer" key={label}>
              <NavLink to={to} className="Links" activeClassName="active">
                <div className="Linkicon">{icon}</div>
                <span>{label}</span>
              </NavLink>
            </div>
          ))}
          <Divider />
          {secondarylinksArray.map(({ icon, label, to }) => (
            <div className="LinkContainer" key={label}>
              <NavLink to={to} className="Links" activeClassName="active">
                <div className="Linkicon">{icon}</div>
                <span>{label}</span>
              </NavLink>
            </div>
          ))}
          <Divider />
          <div className="Themecontent">
            <div className="Togglecontent">
              <div className="grid theme-container">
              </div>
            </div>
          </div>
        </Sidebar>
        <Content>
          <Container>
            <h1 style={{ position: 'relative', right: '-360px' }}>Prestamos de profesores</h1>
          </Container>
         {/* Tabla de prestamos de profesores */}
         <Table>
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Apellidos</th>
                  <th>Email Profesor</th>
                  <th>Estado</th>
                  <th>Placa</th>
                  <th>Fecha de Solicitud</th>
                  <th>Hora de Solicitud</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {data.map((row) => (
                  <tr key={row.id}>
                    <td>{row.nombre}</td>
                    <td>{row.apellidos}</td>
                    <td>{row.email_prof}</td>
                    <td>{row.estado}</td>
                    <td>{row.placa}</td>
                    <td>{row.fecha_hora_solicitud.split('T')[0]}</td>
                    <td>{row.fecha_hora_solicitud.split('T')[1].split('.')[0]}</td>
                    <td>
                    <Button
                        color="primary"
                        onClick={() => handleDevolverActivo(row)}
                      >
                        Devolver Activo
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
        </Content>
      </Container>
       {/* Diálogo para ingresar la contraseña */}
       <Dialog open={passwordDialogOpen} onClose={() => setPasswordDialogOpen(false)}>
        <DialogTitle>Confirmar devolución de activo</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Por favor, ingrese la contraseña del profesor para confirmar la acción.
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            label="Contraseña del profesor"
            type="password"
            fullWidth
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setPasswordDialogOpen(false)} color="primary">
            Cancelar
          </Button>
          <Button onClick={handleConfirmDevolucion} color="primary">
            Confirmar
          </Button>
        </DialogActions>
      </Dialog>
    </ThemeProvider>
  );
};

const Container = styled.div`
  display: flex;
`;

const StyledTable = styled.table`
  width: 100%;
  border-collapse: collapse;
  
  th, td {
    padding: 8px;
    text-align: left;
    border-bottom: 1px solid #ddd;
  }
  
  th {
    background-color: #f2f2f2;
  }
`;


const Sidebar = styled.div`
  color: ${(props) => props.theme.text};
  background: ${(props) => props.theme.bg};
  position: fixed;
  width: 300px;
  height: 100vh;
  z-index: 1000;

  .Logocontent {
    display: flex;
    justify-content: center;
    align-items: center;
    padding-bottom: 20px;
    .imgcontent {
      display: flex;
      img {
        max-width: 100%;
        height: auto;
      }
    }
  }

  .LinkContainer {
    margin: 8px 0;
    padding: 0 15%;
    :hover {
      background: ${(props) => props.theme.bg3};
    }
    .Links {
      display: flex;
      align-items: center;
      text-decoration: none;
      padding: 8px 0;
      color: ${(props) => props.theme.text};
      .Linkicon {
        padding: 8px 16px;
        display: flex;
        svg {
          font-size: 25px;
        }
      }
      &.active {
        .Linkicon {
          svg {
            color: ${(props) => props.theme.bg4};
          }
        }
      }
    }
  }

  .Themecontent {
    display: flex;
    align-items: center;
    justify-content: space-between;
    .titletheme {
      padding: 10px;
      font-weight: 700;
    }
    .Togglecontent {
      margin: auto 40px;
      width: 36px;
      height: 20px;
      border-radius: 10px;
    }
  }
`;

const Content = styled.div`
  margin-left: 300px; // Asegurar que el contenido comience después de la barra lateral
  flex-grow: 1; // Permitir que el contenido crezca para llenar el espacio restante
`;

const Divider = styled.div`
  height: 1px;
  width: 100%;
  background: ${(props) => props.theme.bg3};
  margin: 20px 0;
`;


const DataTableContainer = styled.div`
width: 80%;
margin-left: auto;
margin-right: auto;

.rdt_Table {
  border-collapse: collapse;
  width: 100%;
}

.rdt_TableHead {
  background-color: #f2f2f2;
}

.rdt_TableHeadRow {
  border-bottom: 2px solid #ddd;
}

.rdt_TableHeadRow th {
  padding: 10px;
  text-align: left;
}

.rdt_TableBodyRow {
  border-bottom: 1px solid #ddd;
}

.rdt_TableBodyRow td {
  padding: 10px;
}

.rdt_Pagination {
  margin-top: 20px;
}

.rdt_PaginationPrev, .rdt_PaginationNext {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 5px 10px;
  border-radius: 5px;
  cursor: pointer;
  margin-right: 10px;
}
`;

export default Prestamo_profesor;