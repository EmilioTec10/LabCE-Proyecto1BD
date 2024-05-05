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
        console.log(this.state.data);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData(); // Llama a la función para obtener los datos al cargar el componente
  }

  generatePDF = () => {
    const { data } = this.state;
  
    // Crear un array de datos para la tabla del PDF
    const tableData = [
      [{ text: 'Operador', style: 'tableHeader' },
       { text: 'Fecha', style: 'tableHeader' },
       { text: 'Hora de Entrada', style: 'tableHeader' },
       { text: 'Hora de Salida', style: 'tableHeader' },
       { text: 'Horas Trabajadas', style: 'tableHeader' }],
      ...data.flatMap(operador => {
        const totalHorasTrabajadas = operador.horasLaboradas.reduce((total, hora) => total + hora.horas_trabajadas, 0);
        const operadorRow = [{ text: `${operador.apellido1} ${operador.apellido2} ${operador.nombre}` }, '', '', '', totalHorasTrabajadas];
        const horasRows = operador.horasLaboradas.map(hora => ['', new Date(hora.fecha).toLocaleDateString(), new Date(hora.hora_ingreso).toLocaleTimeString(), new Date(hora.hora_salida).toLocaleTimeString(), hora.horas_trabajadas]);
        return [operadorRow, ...horasRows];
      })
    ];
  
    // Definir la estructura del documento PDF
    const docDefinition = {
      content: [
        {
          text: 'LabCE\nEscuela De Ingenieria en Computadores\nHoras Laboradas por Operador',
          style: 'header',
          alignment: 'left',
        },
        {
          text: new Date().toLocaleDateString(),
          alignment: 'right',
          margin: [0, 0, 0, 10],
        },
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*', '*', '*'],
            body: tableData,
          },
          layout: 'lightHorizontalLines',
        },
      ],
      styles: {
        header: {
          fontSize: 18,
          bold: true,
          margin: [0, 0, 0, 10],
        },
        tableHeader: {
          bold: true,
          fontSize: 13,
          color: 'black',
        },
      },
    };
  
    // Generar y abrir el PDF
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
        </Sidebar>
        <Content> 
          <Container>
            <br />
            <br />
            <h1>Reportes</h1>
            <Table>
              <thead>
              <tr>
                <th colSpan="5">
                  <div style={{ display: "flex", justifyContent: "space-between" }}>
                    <div>
                      <p>LabCE</p>
                      <p>Escuela De Ingenieria en Computadores</p>
                      <p>Horas Laboradas por Operador</p>
                    </div>
                    <div>
                      <p>{new Date().toLocaleDateString()}</p>
                    </div>
                  </div>
                </th>
              </tr>
                <tr>
                  <th>Operador</th>
                  <th>Fecha</th>
                  <th>Hora de Entrada</th>
                  <th>Hora de Salida</th>
                  <th>Horas Trabajadas</th>
                </tr>
              </thead>
              <tbody>
                {this.state.data.map((operador, index) => {
                  // Calcular la suma total de horas trabajadas para el operador actual
                  const totalHorasTrabajadas = operador.horasLaboradas.reduce(
                    (total, hora) => total + hora.horas_trabajadas,
                    0
                  );

                  return (
                    <React.Fragment key={index}>
                      <tr>
                        <NameColumn>{operador.apellido1} {operador.apellido2} {operador.nombre}</NameColumn>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>{totalHorasTrabajadas}</td> {/* Mostrar la suma total aquí */}
                      </tr>
                      {operador.horasLaboradas.map((hora, i) => (
                        <tr key={i}>
                          <td></td>
                          <td>{new Date(hora.fecha).toLocaleDateString()}</td>
                          <td>{new Date(hora.hora_ingreso).toLocaleTimeString()}</td>
                          <td>{new Date(hora.hora_salida).toLocaleTimeString()}</td>
                          <td>{hora.horas_trabajadas}</td>
                        </tr>
                      ))}
                    </React.Fragment>
                  );
                })}
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
