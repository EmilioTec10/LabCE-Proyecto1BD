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

import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import TextField from '@mui/material/TextField';
import DialogActions from '@mui/material/DialogActions';

import {
  Table,
  Container,
  Modal,
  ModalHeader,
  ModalBody,
  FormGroup,
  ModalFooter,
} from "reactstrap";

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
    data: [],
    modalActualizar: false,
    modalInsertar: false,
    passwordDialogOpen: false,
    password: '',
    selectedData: null,
    selectedProfessorEmail: '',
    form: {
      id: "",
      cedula: "",
      nombreyapellidos: "",
      edad: "",
      fechanacimiento: "",
      correo: "",
    },
  };

  handlePrestarProfesor(dato, email) {
    // Aquí puedes realizar cualquier lógica adicional antes de abrir el diálogo
    this.setState({ selectedData: dato, selectedProfessorEmail: email, passwordDialogOpen: true });
  }

  handleConfirmPrestamoProfesor = () => {
    // Aquí puedes implementar la lógica para confirmar el préstamo al profesor con el correo electrónico y la contraseña ingresados
    this.setState({ passwordDialogOpen: false });
    // Lógica adicional después de confirmar el préstamo al profesor
    this.prestar_profesor(this.state.selectedData, this.state.selectedProfessorEmail, this.state.password);
  };
  
  componentDidMount() {
    axios.get('http://localhost:5129/api/Operador/ver-activos-disponibles') // Reemplaza 'ruta_de_tu_api' con la URL de tu API
      .then(response => {
        this.setState({ data: response.data });
      })
      .catch(error => {
        console.error('Error al obtener datos de la API:', error);
      });
  }

  guardarEmail = (email, dato) => {
    // Aquí implementa la lógica para guardar el email relacionado a este row
    console.log('Email guardado:', email);
    // Eliminar el row después de guardar el email
    this.devolver_activo(dato);
  };

  prestar_profesor = (dato, email_prof, contra_prof) => {
        console.log(email_prof);
        console.log(contra_prof);
        // Realizar la solicitud HTTP
        axios.post('http://localhost:5129/api/Operador/prestar-activo-profesor', {
          placa: dato.placa,
          email_prof: email_prof,
          contraseña_prof: contra_prof
        })
        .then(response => {
          // Manejar la respuesta de la API
          console.log(response.data); // Imprimir la respuesta en la consola
          this.setState(prevState => ({
            data: prevState.data.filter(item => item.placa !== dato.placa)
          }));
          alert("Prestamo de activo por parte del profesor registrada correctamente");
        })
        .catch(error => {
          console.log(dato.email_prof);
          console.log(dato.placa);
          console.log(contra_prof);
          // Manejar los errores de la solicitud
          console.error(error); // Imprimir el error en la consola
          alert("No se pudo realizar el prestamo del activo. Por favor, verifique las credenciales del profesor.");
        });
  };

  prestar_estudiante = (dato, contra_prof) => {
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
            <h1>Activos Disponibles</h1>
            <Table>
              <thead>
                <tr>
                  <th>Placa</th>
                  <th>Tipo</th>
                  <th>Marca</th>
                  <th>Fecha de compra</th>
                  <th>Lab</th>
                  <th>Acciones</th>
                </tr>
              </thead>
  
              <tbody>
                {this.state.data.map((dato) => (
                  <tr key={dato.cedula}>
                    <td>{dato.placa}</td>
                    <td>{dato.tipo}</td>
                    <td>{dato.marca}</td>
                    <td>{dato.purchase_date.slice(0, 10)}</td>
                    <td>{dato.lab}</td>
                    <td>
                      <Button
                        color="primary"
                        onClick={() => this.handlePrestarProfesor(dato)}
                      >
                        Prestar a profesor
                      </Button>{" "}
                      <Button
                        color="success"
                        onClick={() => this.guardarEmail(dato.correo, dato)}
                      >
                        Solicitar a estudiante
                      </Button>{" "}
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
        </Content> 
        <Dialog open={this.state.passwordDialogOpen} onClose={() => this.setState({ passwordDialogOpen: false })}>
  <DialogTitle>Confirmar préstamo al profesor</DialogTitle>
  <DialogContent>
    <DialogContentText>
      Por favor, ingresa el correo electrónico y la contraseña del profesor.
    </DialogContentText>
    <TextField
      autoFocus
      margin="dense"
      label="Correo electrónico del profesor"
      type="email"
      fullWidth
      value={this.state.selectedProfessorEmail}
      onChange={(e) => this.setState({ selectedProfessorEmail: e.target.value })}
    />
    <TextField
      margin="dense"
      label="Contraseña del profesor"
      type="password"
      fullWidth
      value={this.state.password}
      onChange={(e) => this.setState({ password: e.target.value })}
    />
  </DialogContent>
  <DialogActions>
    <Button onClick={() => this.setState({ passwordDialogOpen: false })} color="primary">
      Cancelar
    </Button>
    <Button onClick={this.handleConfirmPrestamoProfesor} color="primary">
      Confirmar
    </Button>
  </DialogActions>
</Dialog>
      </ThemeProvider>
    );
  }
}
export default Devolucion_activo;
