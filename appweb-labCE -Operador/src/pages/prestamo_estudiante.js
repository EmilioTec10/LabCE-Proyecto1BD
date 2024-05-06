import React, { useContext, useState, useEffect } from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { Light, Dark } from '../styles/themes';
import { Table, Button } from 'reactstrap';
import logo from '../assets/react.svg';
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import axios from 'axios';
import DataTable from 'react-data-table-component';
import Paper from '@mui/material/Paper';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import TextField from '@mui/material/TextField';
import DialogActions from '@mui/material/DialogActions';
import { email_output } from './login_operador';

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

const Prestamo_estudiante = () => {
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
      const response = await axios.get('http://localhost:5129/api/Operador/prestamos-pendientes-estudiantes');
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

  const devolverActivo = (dato, contraseñaEst) => {
    axios.post('http://localhost:5129/api/Operador/devolucion-activo-estudiante', {
      placa: dato.placa,
      email_est: dato.email_prof,
      email_op: email_output,
      contraseña_op: contraseñaEst
    })
    .then(response => {
      console.log(response.data);
      alert("Devolución de activo por parte del estudiante registrada correctamente");
      setData(prevData => prevData.filter(item => item.placa !== dato.placa));
    })
    .catch(error => {
      console.log(dato);
      console.log(email_output);
      console.log(contraseñaEst);
      console.error(error);
      alert("No se pudo realizar la devolución del activo. Por favor, verifique las credenciales del estudiante.");
    });
  };

  const CambiarTheme = () => {
    setTheme((theme) => (theme === 'light' ? 'dark' : 'light'));
  };

  const columnas = [
    {
      name: 'Placa',
      selector: row => row.placa,
      sortable: true
    },
    {
      name: 'Tipo',
      selector: row => row.tipo,
      sortable: true
    },
    {
      name: 'Marca',
      selector: row => row.marca,
      sortable: true
    },
    {
      name: 'Fecha de Compra',
      selector: row => row.fechaCompra,
      sortable: true
    },
    {
      name: 'Requiere Aprobador',
      selector: row => row.requiereAprobador,
      sortable: true,
      cell: row => <input type="checkbox" checked={row.requiereAprobador} disabled />
    },
    {
      name: 'Acciones',
      cell: row => <Button color="primary" onClick={() => handleDevolverActivo(row)}>Devolver Activo</Button>,
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
            <h1 style={{ position: 'relative', right: '-360px' }}>Prestamos de estudiantes</h1>
          </Container>
         {/* Tabla de prestamos de profesores */}
         <Table>
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Apellidos</th>
                  <th>Email Estudiante</th>
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
      <Dialog open={passwordDialogOpen} onClose={() => setPasswordDialogOpen(false)}>
        <DialogTitle>Confirmar devolución de activo</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Por favor, ingrese la contraseña del operador para confirmar la acción.
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            label="Contraseña del operador"
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
  margin-left: 300px;
  flex-grow: 1;
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
`;

export default Prestamo_estudiante;
