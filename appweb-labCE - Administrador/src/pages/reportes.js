import "../App.css";
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import "bootstrap/dist/css/bootstrap.min.css";
import React, { useContext, useState } from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { Light, Dark } from '../styles/themes';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import logo from '../assets/react.svg';
import { Button } from "reactstrap";
import pdfMake from "pdfmake/build/pdfmake";
import pdfFonts from "pdfmake/build/vfs_fonts";
import axios from "axios";

import {
  Table,
  Container,
  Modal,
  ModalHeader,
  ModalBody,
  FormGroup,
  ModalFooter,
} from "reactstrap";

pdfMake.vfs = pdfFonts.pdfMake.vfs;

const data = [
  {  
    nombreyapellidos: "Emmanuel esquivel Chavarria",  
    turnos: [
      { dia: "02/05/2024", hora: "8:00-12:00" },
      { dia: "04/05/2024", hora: "10:00-14:00" },
      { dia: "06/05/2024", hora: "14:00-18:00" }
    ]
  },
  { 
    nombreyapellidos: "Carlos",  
    turnos: [
      { dia: "03/05/2024", hora: "9:00-13:00" },
      { dia: "05/05/2024", hora: "11:00-15:00" },
      { dia: "07/05/2024", hora: "13:00-17:00" }
    ]
  },
  // Agrega más objetos de operadores aquí con sus respectivos turnos
];

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

const DataTableContainer = styled.div`
  width: 80%;
  margin-left: auto;
  margin-right: auto;
`;

const NameColumn = styled.td`
  padding-left: 30px;
  vertical-align: top;
`;

class Reportes extends React.Component {
  
  state = {
    data: [],
    modalActualizar: false,
    modalInsertar: false,
    form: {
      id: "",
      cedula: "",
      nombreyapellidos: "",
      edad: "",
      fechanacimiento: "",
      correo: "",
    },
  };

  componentDidMount() {
    // Función para obtener los reportes de la API
    const fetchData = async () => {
      try {
        const response = await axios.get('http://localhost:5129/api/ControladorAdmin/generar-reporte');
        this.setState({ data: response.data }); // Actualiza el estado con los datos obtenidos de la API
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData(); // Llama a la función para obtener los datos al cargar el componente
  }

  generatePDF = () => {
    const docDefinition = {
      content: [
        { text: 'Reporte de Operadores', style: 'header' },
        {
          ul: this.state.data.map((operador) => ({
            text: `${operador.nombreyapellidos}: ${operador.turnos.map(turno => `${turno.dia} - ${turno.hora}`).join(', ')}`
          }))
        }
      ],
      styles: {
        header: {
          fontSize: 18,
          bold: true,
          margin: [0, 0, 0, 10]
        }
      }
    };
  
    pdfMake.createPdf(docDefinition).open();
  };

  render() {
    const themeStyle = this.props.theme === 'dark' ? Light : Dark;

    return (
      <ThemeProvider theme={themeStyle}>
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
          {secondarylinksArray.map(({ icon, label, to }) => (
            <div className="LinkContainer" key={label}>
              <NavLink to={to} className="Links" activeClassName="active">
                <div className="Linkicon">{icon}</div>
                <span>{label}</span>
              </NavLink>
            </div>
          ))}
        </Sidebar>
        <Content> 
          <Container>
            <br />
            <br />
            <h1>Reportes</h1>
            <Table>
              <thead>
                <tr>
                  <th>Operador</th>
                  <th>Fecha</th>
                  <th>Entrada</th>
                  <th>Salida</th>
                </tr>
              </thead>
              <tbody>
                {data && data.map((operador, index) => (
                  <React.Fragment key={index}>
                    <tr>
                      <NameColumn>{operador.nombre}</NameColumn>
                    </tr>
                    {operador.turnos && operador.turnos.map((turno, i) => (
                      <tr key={i}>
                        <td style={{ paddingLeft: '30px' }}></td>
                        <td>{turno.fecha}</td>
                        <td>{turno.hora.split('-')[0]}</td>
                        <td>{turno.hora.split('-')[1]}</td>
                      </tr>
                    ))}
                    {index < data.length - 1 && <tr><td colSpan="4" style={{ height: '20px' }}></td></tr>}
                  </React.Fragment>
                ))}
              </tbody>
            </Table>
            <Button color="primary" onClick={this.generatePDF}>Generar PDF</Button>
          </Container>
        </Content> 
      </ThemeProvider>
    );
  }
}
export default Reportes;
