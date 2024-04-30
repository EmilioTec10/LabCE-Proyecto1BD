import React, { useContext, useState } from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { Light, Dark } from '../styles/themes';
import logo from '../assets/react.svg';
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import DataTable from 'react-data-table-component';
import Paper from '@mui/material/Paper';

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
    to: '/gestion_laboratorios',
  },
];

const secondarylinksArray = [
  {
    label: 'Salir',
    icon: <MdLogout />,
    to: '/',
  },
];

const Gestion_laboratorios = () => {
  const [passwordError, setPasswordError] = useState('');
  const { setTheme, theme } = useContext(ThemeContext);
  const themeStyle = theme === 'dark' ? Light : Dark;

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

  const data = [
    {
      nombre: 'F2-07',
      capacidad: '30',
      computadores: '30',
      facilidades: 'xxx',
      horario: 'xxx',
      activos: 'xxx',
    },
    {
      nombre: 'F2-08',
      capacidad: '30',
      computadores: '30',
      facilidades: 'xxx',
      horario: 'xxx',
      activos: 'xxx',
    },
    {
      nombre: 'F2-09',
      capacidad: '30',
      computadores: '30',
      facilidades: 'xxx',
      horario: 'xxx',
      activos: 'xxx',
    },
    {
      nombre: 'F2-10',
      capacidad: '30',
      computadores: '30',
      facilidades: 'xxx',
      horario: 'xxx',
      activos: 'xxx',
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
                <div className="content">
                  <button onClick={CambiarTheme}>Cambiar Tema</button>
                </div>
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
              style={{ marginLeft: '20px' }}
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
  margin-top: 20px;
  margin-bottom: 20px;
`;

export default Gestion_laboratorios;
