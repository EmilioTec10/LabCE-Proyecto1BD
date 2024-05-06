import React, { useContext, useState, useEffect } from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { Light, Dark } from '../styles/themes';
import logo from '../assets/react.svg';
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import DataTable from 'react-data-table-component';
import Paper from '@mui/material/Paper';
import axios from 'axios'; // Importar Axios

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
  const [passwordError, setPasswordError] = useState('');
  const { setTheme, theme } = useContext(ThemeContext);
  const [data, setData] = useState([]);
  const themeStyle = theme === 'dark' ? Light : Dark;

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await axios.get('http://localhost:5129/api/Operador/prestamos-pendientes-profesores'); // Hacer la solicitud GET con Axios
      setData(response.data); // Actualizar el estado de 'data' con los datos recibidos
    } catch (error) {
      console.error(error);
    }
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
      name: 'Fecha y Hora de Solicitud',
      selector: row => row.fecha_hora_solicitud,
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
          <DataTableContainer>
            <DataTable
              columns={columnas}
              data={data}
              fixedHeader
              noHeader
              dense
              style={{ marginTop: '20px' }} // Bajar la tabla en el eje Y
              customStyles={{
                table: {
                  style: {
                    marginBottom: '400px', // Aumentar el tamaño de la tabla
                  },
                },
              }}
            />
          </DataTableContainer>
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

export default Prestamo_profesor;
