import React, { useContext, useState, useEffect} from 'react';
import { Table } from 'reactstrap';
import styled, { ThemeProvider } from 'styled-components';
import { Light, Dark } from '../styles/themes';
import logo from '../assets/react.svg';
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import DataTable from 'react-data-table-component';
import Paper from '@mui/material/Paper';
import axios from 'axios';

const linksArray = [
  {
    label: 'Gestion Profesores',
    icon: <AiOutlineHome />,
    to: '/gestion_profesores',
  },
  {
    label: 'Gestion Laboratorios',
    icon: <MdOutlineAnalytics />,
    to: '/gestion_laboratorios',
  },
  {
    label: 'Gestion Activos',
    icon: <AiOutlineApartment />,
    to: '/gestion_activos',
  },
  {
    label: 'Aprobar Operadores',
    icon: <MdOutlineAnalytics />,
    to: '/aprobacion_operadores',
  },
  {
    label: 'Cambio Contraseña',
    icon: <MdOutlineAnalytics />,
    to: '/cambio_contrasenna',
  },
  {
    label: 'Reportes',
    icon: <MdOutlineAnalytics />,
    to: '/reportes',
  },
];

const Title = styled.h1`
  text-align: center; /* Centra el texto horizontalmente */
`;

const CenteredTable = styled.div`
  display: flex;
  justify-content: center; /* Centra el contenido horizontalmente */
`;

const secondarylinksArray = [
  {
    label: 'Salir',
    icon: <MdLogout />,
    to: '/',
  },
];

const styles = {
  titleContainer: {
    textAlign: 'center',
    marginTop: '20px', // Ajusta el margen superior según sea necesario
  },
  title: {
    marginBottom: '20px', // Ajusta el margen inferior según sea necesario
  },
};

const Gestion_laboratorios = () => {
  const [passwordError, setPasswordError] = useState('');
  const { setTheme, theme } = useContext(ThemeContext);
  const themeStyle = theme === 'dark' ? Light : Dark;
  const [laboratorios, setLaboratorios] = useState([]);

  useEffect(() => {
    // Realizar la llamada a la API para obtener los laboratorios disponibles
    axios.get('http://localhost:5129/api/ControladorAdmin/ver-laboratorios-disponibles')
      .then(response => {
        // Actualizar el estado de los laboratorios con la respuesta de la API
        setLaboratorios(response.data);
      })
      .catch(error => {
        console.error('Error fetching data:', error);
      });
  }, []);

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
      name: 'Capacidad',
      selector: row => row.capacidad,
      sortable: true
    },
    {
      name: 'Computadores',
      selector: row => row.computadores,
      sortable: true
    },
    {
      name: 'Facilidades',
      selector: row => row.facilidades,
      sortable: true
    },
    {
      name: 'Horario',
      selector: row => row.horario,
      sortable: true
    },
    {
      name: 'Activos',
      selector: row => row.activos,
      sortable: true
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
            <div className="title-container">
              <h1 className="title"style={{ position: 'relative', right: '-420px' }}>Laboratorios</h1>
            </div>
            </Container>
          <Container>
            <Table>
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Capacidad</th>
                  <th>Computadores</th>
                  <th>Facilidades</th>
                  <th>Horario</th>
                  <th>Activos</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {laboratorios.map((lab) => (
                  <tr key={lab.nombre}>
                    <td>{lab.nombre}</td>
                    <td>{lab.capacidad}</td>
                    <td>{lab.computadores}</td>
                    <td>{lab.facilidades}</td>
                    <td>{"Ver horario"}</td>
                    <td>{lab.activos}</td>
                    <td>
                      {/* Botones de editar y eliminar */}
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
          {/* Modales para insertar y actualizar */}
        </Content>
      </Container>
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
`;

export default Gestion_laboratorios;
