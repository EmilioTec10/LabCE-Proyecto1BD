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

import { email } from './login_profesor';

const data = [
  { nombreyapellidos: "Emmanuel esquivel Chavarria",placa: "2231",  correo: "prueba@gmail.com" },
  { nombreyapellidos: "Carlos", placa: "2251", correo: "ema@gmail.com"},
  { nombreyapellidos: "Juan",  placa: "6231", correo: "ema@gmail.com"},
  { nombreyapellidos: "Pepe", placa: "2731",  correo: "ema@gmail.com" },
  { nombreyapellidos: "Emilio", placa: "2239",  correo: "ema@gmail.com"},
  { nombreyapellidos: "Naruto", placa: "2211",  correo: "ema@gmail.com"},
];

const linksArray = [
  {
    label: 'Aprobar Prestamos',
    icon: <AiOutlineHome />,
    to: '/aprobacion_prestamo',
  },
  {
    label: 'Reserva Laboratorios',
    icon: <MdOutlineAnalytics />,
    to: '/gestion_reserva_laboratorios',
  },
  {
    label: 'Cambio Contraseña',
    icon: <MdOutlineAnalytics />,
    to: '/cambio_contrasenna',
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

class Aprobacion_prestamo extends React.Component {
  
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
    // Realiza la solicitud HTTP para obtener los datos de la API
    axios.get('http://localhost:5129/api/ControladorProfesor/ver-prestamos-pendientes', {
      params: {
        email: email
      }
    })
    .then(response => {
      console.log(response.data);
      this.setState({ data: response.data });
    })
    .catch(error => {
      console.error(error);
    });
  }
  

  guardarEmail = (email, dato) => {
    // Aquí implementa la lógica para guardar el email relacionado a este row
    console.log('Email guardado:', email);
    // Eliminar el row después de guardar el email
    this.eliminar_luego_de_aceptar(dato);
  };

  eliminar_luego_de_aceptar = (dato) => {
    var opcion = window.confirm("Estás Seguro que deseas aceptar el prestamo al operador" + dato.nombreyapellidos);
    if (opcion === true) {
      var arreglo = this.state.data.filter(registro => registro.cedula !== dato.cedula);
      this.setState({ data: arreglo, modalActualizar: false });
    }
  };

  eliminar = (dato) => {
    var opcion = window.confirm("Estás Seguro que deseas rechazar el prestamo al operador " + dato.nombreyapellidos);
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
            <h1>Aprobacion de Prestamos</h1>
            <Table>
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Correo</th>
                  <th>Placa Activo</th>
                  <th>Acciones</th>
                </tr>
              </thead>
  
              <tbody>
              {this.state.data.map((dato) => (
                <tr key={dato.nombre}>
                  <td>{dato.nombre}</td>
                  <td>{dato.email_est}</td>
                  <td>{dato.placa}</td>
                  <td>
                    <Button
                      color="primary"
                      onClick={() => this.guardarEmail(dato.email_est, dato)}
                    >
                      Aprobar
                    </Button>{" "}
                    <Button color="danger" onClick={() => this.eliminar(dato)}>Denegar</Button>
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
export default Aprobacion_prestamo;
