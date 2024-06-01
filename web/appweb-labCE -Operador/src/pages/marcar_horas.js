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
import { Button, Container } from "reactstrap";
import axios from "axios";

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
    width: 308px;
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
  margin: 0 auto; /* Centra el contenido horizontalmente */
  width: 70%; /* Establece un ancho máximo para el contenido */
`;


  const Divider = styled.div`
    height: 1px;
    width: 100%;
    background: ${(props) => props.theme.bg3};
    margin: 20px 0;
  `;

  const Layout = styled.div`
  display: flex;
`;

class Marcar_Horas extends React.Component {
    constructor(props) {
      super(props);
      this.state = {
        data: [],
        buttonText: localStorage.getItem('buttonText') || 'Entrada', // Obtener el estado del botón almacenado en localStorage
      };
    }
  
    componentDidMount() {
      // Obtener los datos del operador al montar el componente
      console.log(localStorage.getItem('email_output'));
      var email_output = localStorage.getItem('email_output');
      axios
        .get('http://localhost:5129/api/Operador/ver-datos-operador', {
          params: {
            email_op: email_output,
          },
        })
        .then((response) => {
          this.setState({ data: response.data });
          console.log(this.state.data);
        })
        .catch((error) => {
          console.error('Error al obtener datos de la API:', error);
        });
    }
  
    marcar_horas() {
      var email_output = localStorage.getItem('email_output');
      axios
        .post('http://localhost:5129/api/Operador/marcar-entrada', {
          email_op: email_output,
        })
        .then((response) => {
          alert('Entrada marcada correctamente');
          // Cambiar el estado del botón a "Salida" después de marcar la entrada
          this.setState({ buttonText: 'Salida' });
          // Guardar el estado del botón en localStorage
          localStorage.setItem('buttonText', 'Salida');
        })
        .catch((error) => {
          alert('Error al marcar horas de entrada:', error);
        });
    }
  
    marcar_salida() {
      var email_output = localStorage.getItem('email_output');
      axios
        .post('http://localhost:5129/api/Operador/marcar-salida', {
          email_op: email_output,
        })
        .then((response) => {
          alert('Salida marcada correctamente');
          // Cambiar el estado del botón a "Entrada" después de marcar la salida
          this.setState({ buttonText: 'Entrada' });
          // Guardar el estado del botón en localStorage
          localStorage.setItem('buttonText', 'Entrada');
        })
        .catch((error) => {
          alert('Error al marcar horas de salida:', error);
        });
    }

    render(){

        const themeStyle = this.props.theme === 'dark' ? Light : Dark;

        return (
        <ThemeProvider theme={themeStyle}>
            <Layout>
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
                    <h1 style={{ position: 'relative', right: '-140px' }}>Horas de operador</h1>
                    {/* Muestra los datos del operador */}
                    <div>
                        <br />
                        <p style={{ position: 'relative', right: '-140px' }}><strong>Nombre:</strong> {this.state.data.nombre}</p>
                        <br />
                        <p style={{ position: 'relative', right: '-140px' }}><strong>Apellidos:</strong> {this.state.data.apellidos}</p>
                        <br />
                        <p style={{ position: 'relative', right: '-140px' }}><strong>Email:</strong> {this.state.data.email}</p>
                        <br />
                        <p style={{ position: 'relative', right: '-140px' }}><strong>Carnet:</strong> {this.state.data.carne}</p>
                        <br />
                        <p style={{ position: 'relative', right: '-140px' }}><strong>Cedula:</strong> {this.state.data.cedula}</p>
                        <br />
                        <p style={{ position: 'relative', right: '-140px' }}><strong>Fecha de nacimiento:</strong> {this.state.data.fecha_de_nacimiento}</p>
                        <br />
                    </div>
                    {/* Agrega el botón "Marcar Horas" */}
                    <Button
                        color="success"
                        className="d-block mx-auto"
                        style={{ position: 'relative', right: '-140px' }}
                        onClick={() =>
                        this.state.buttonText === 'Entrada'
                            ? this.marcar_horas()
                            : this.marcar_salida()
                        }
                    >
                        {this.state.buttonText}
                    </Button>
                </Container>
                </Content>
            </Layout>
        </ThemeProvider>);
    }

  } export default Marcar_Horas;

