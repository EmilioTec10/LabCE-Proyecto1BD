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

import {
  Table,
  Container,
  Modal,
  ModalHeader,
  ModalBody,
  FormGroup,
  ModalFooter,
} from "reactstrap";

const data = [
  { cedula: 504560886, nombreyapellidos: "Emmanuel esquivel Chavarria", edad: "19", fechanacimiento: "28/10/04", correo: "prueba@gmail.com" },
  { cedula: 103450879, nombreyapellidos: "Carlos", edad: "20", fechanacimiento: "23/08/97", correo: "ema@gmail.com"},
  { cedula: 103410687, nombreyapellidos: "Juan", edad: "80", fechanacimiento: "23/06/97", correo: "ema@gmail.com"},
  { cedula: 119200368, nombreyapellidos: "Pepe", edad: "45", fechanacimiento: "23/08/97", correo: "ema@gmail.com" },
  { cedula: 123467809, nombreyapellidos: "Emilio", edad: "21", fechanacimiento: "23/08/97", correo: "ema@gmail.com"},
  { cedula: 345676557, nombreyapellidos: "Naruto", edad: "89", fechanacimiento: "23/08/97", correo: "ema@gmail.com"},
];

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

class Devolucion_activo extends React.Component {
  
  state = {
    data: data,
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

  guardarEmail = (email, dato) => {
    // Aquí implementa la lógica para guardar el email relacionado a este row
    console.log('Email guardado:', email);
    // Eliminar el row después de guardar el email
    this.eliminar_luego_de_aceptar(dato);
  };

  eliminar_luego_de_aceptar = (dato) => {
    var opcion = window.confirm("Estás Seguro que deseas aceptar al operador" + dato.cedula);
    if (opcion === true) {
      var arreglo = this.state.data.filter(registro => registro.cedula !== dato.cedula);
      this.setState({ data: arreglo, modalActualizar: false });
    }
  };

  eliminar = (dato) => {
    var opcion = window.confirm("Estás Seguro que deseas rechazar al operador " + dato.cedula);
    if (opcion === true) {
      // Guardar el email antes de eliminar el row
      this.guardarEmail(dato.correo, dato);
      // Eliminar el row después de guardar el email
      var arreglo = this.state.data.filter(registro => registro.cedula !== dato.cedula);
      this.setState({ data: arreglo, modalActualizar: false });
    }
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
            <h1>Aprobacion de Operadores</h1>
            <Table>
              <thead>
                <tr>
                  <th>Cedula</th>
                  <th>Nombre y Apellidos</th>
                  <th>Edad</th>
                  <th>Fecha de Nacimiento</th>
                  <th>Correo</th>
                  <th>Acciones</th>
                </tr>
              </thead>
  
              <tbody>
                {this.state.data.map((dato) => (
                  <tr key={dato.cedula}>
                    <td>{dato.cedula}</td>
                    <td>{dato.nombreyapellidos}</td>
                    <td>{dato.edad}</td>
                    <td>{dato.fechanacimiento}</td>
                    <td>{dato.correo}</td>
                    <td>
                      <Button
                        color="primary"
                        onClick={() => this.guardarEmail(dato.correo, dato)}
                      >
                        Aprobar
                      </Button>{" "}
                      <Button color="danger" onClick={()=> this.eliminar(dato)}>Denegar</Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
        </Content> 
      </ThemeProvider>
    );
  }
}
export default Devolucion_activo;
