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
    to: '/cambio_contrasenna',
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

const Gestion_activos = () => {
  const [passwordError, setPasswordError] = useState('');
  const { setTheme, theme } = useContext(ThemeContext);
  const themeStyle = theme === 'dark' ? Light : Dark;

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
    }
  ];

  const data = [
    {
      placa: '2234',
      tipo: 'Proyector',
      marca: 'xxx',
      fechaCompra: '12/2/22',
      requiereAprobador: true
    },
    {
      placa: '2235',
      tipo: 'Proyector',
      marca: 'xxx',
      fechaCompra: '12/2/22',
      requiereAprobador: true
    },
    {
      placa:'2236',
      tipo: 'Control de Proyector',
      marca: 'xxx',
      fechaCompra: '12/2/22',
      requiereAprobador: true
    },
    {
      placa: '2237',
      tipo: 'Control de Tele',
      marca: 'xxx',
      fechaCompra: '12/2/22',
      requiereAprobador: true
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

export default Gestion_activos;
