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
      label: 'Marcar Horas',
      icon: <AiOutlineHome />,
      to: '/marcar_horas',
  },
  {
    label: 'Reservar Laboratorio',
    icon: <AiOutlineHome />,
    to: '/reserva_laboratorio',
  },
  {
    label: 'Prestamos Profes',
    icon: <MdOutlineAnalytics />,
    to: '/prestamo_profesor',
  },
  {
    label: 'Prestamos Estudiantes',
    icon: <AiOutlineApartment />,
    to: '/prestamo_estudiante',
  },
  {
    label: 'Prestar Activo',
    icon: <MdOutlineAnalytics />,
    to: '/devolucion_activo',
  },
  {
    label: 'Prestamos Aprobados',
    icon: <MdOutlineAnalytics />,
    to: '/prestamos_aprobados',
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
    width: 310px;
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

  class Prestamos_aprobados extends React.Component {

    state = {
        data: [],
        modalActualizar: false,
        modalInsertar: false,
        passwordDialogOpen: false,
        emailDialogOpen: false,
        emailEstudiante: '',
        password: '',
        selectedData: null,
        selectedOperadorEmail: '',
        form: {
          nombre: "",
          apellidos: "",
          email_est: "",
          estado: "",
          placa: "",
          fecha_solicitud: "",
          hora_solicitud: "",
        },
    };

    componentDidMount() {
        axios.get('http://localhost:5129/api/Operador/ver-prestamos-aprobados') // Reemplaza 'ruta_de_tu_api' con la URL de tu API
          .then(response => {
            this.setState({ data: response.data });
          })
          .catch(error => {
            console.error('Error al obtener datos de la API:', error);
          });
    };

    handlePrestarEstudiante(dato, email) {
        // Aquí puedes realizar cualquier lógica adicional antes de abrir el diálogo
        this.setState({ selectedData: dato, selectedOperadorEmail: email, passwordDialogOpen: true });
    };

    handleConfirmPrestamoEstudiante = () => {
        // Aquí puedes implementar la lógica para confirmar el préstamo al profesor con el correo electrónico y la contraseña ingresados
        this.setState({ passwordDialogOpen: false });
        // Lógica adicional después de confirmar el préstamo al profesor
        this.prestar_estudiante(this.state.selectedData, this.state.selectedOperadorEmail, this.state.password);
    };

    prestar_estudiante = (dato, email_prof, contra_prof) => {
        console.log(email_prof);
        console.log(contra_prof);
        console.log(dato.placa);
        console.log(dato.email_est);
        // Realizar la solicitud HTTP
        axios.post('http://localhost:5129/api/Operador/prestar-activo-estudiante', {
          placa: dato.placa,
          email_op: email_prof,
          contraseña_op: contra_prof,
          email_est: dato.email_est,
        })
        .then(response => {
          // Manejar la respuesta de la API
          console.log(response.data); // Imprimir la respuesta en la consola
          this.setState(prevState => ({
            data: prevState.data.filter(item => item.placa !== dato.placa)
          }));
          alert("Prestamo de activo por parte del estudiante registrada correctamente");
        })
        .catch(error => {
          // Manejar los errores de la solicitud
          console.error(error); // Imprimir el error en la consola
          alert("No se pudo realizar el prestamo del activo. Por favor, verifique las credenciales del operador.");
        });
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
                <h1>Prestamos Aprobados</h1>
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
                    {this.state.data.map((dato) => (
                      <tr key={dato.id}>
                        <td>{dato.nombre}</td>
                        <td>{dato.apellidos}</td>
                        <td>{dato.email_est}</td>
                        <td>{dato.estado}</td>
                        <td>{dato.placa}</td>
                        <td>{dato.fecha_hora_solicitud.split('T')[0]}</td>
                        <td>{dato.fecha_hora_solicitud.split('T')[1].split('.')[0]}</td>
                        <td>
                          <Button
                            color="success"
                            onClick={() => this.handlePrestarEstudiante(dato)}
                          >
                            Prestar Activo a estudiante
                          </Button>{" "}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </Table>
              </Container>
            </Content> 
            <Dialog open={this.state.passwordDialogOpen} onClose={() => this.setState({ passwordDialogOpen: false })}>
          <DialogTitle>Confirmar préstamo al estudiante</DialogTitle>
          <DialogContent>
            <DialogContentText>
              Por favor, ingresa el correo electrónico y la contraseña del operador a cargo.
            </DialogContentText>
            <TextField
              autoFocus
              margin="dense"
              label="Correo electrónico del operador"
              type="email"
              fullWidth
              value={this.state.selectedOperadorEmail}
              onChange={(e) => this.setState({ selectedOperadorEmail: e.target.value })}
            />
            <TextField
              margin="dense"
              label="Contraseña del operador"
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
            <Button onClick={this.handleConfirmPrestamoEstudiante} color="primary">
              Confirmar
            </Button>
          </DialogActions>
        </Dialog>
        </ThemeProvider>);
    }

  } export default Prestamos_aprobados;