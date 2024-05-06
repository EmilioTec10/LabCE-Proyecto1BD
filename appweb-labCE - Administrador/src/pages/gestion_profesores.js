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
import axios from "axios";

import {
  Table,
  Button,
  Container,
  Modal,
  ModalHeader,
  ModalBody,
  FormGroup,
  ModalFooter,
} from "reactstrap";

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

class Gestion_profesores extends React.Component {
  
  state = {
    data: [],
    modalActualizar: false,
    modalInsertar: false,
    form: {
      cedula: "",
      nombre: "",
      apellidos: "",
      edad: "",
      fecha_de_nacimiento: "",
      email: "",
    },
  };

  componentDidMount() {
    axios.get('http://localhost:5129/api/ControladorAdmin/ver-profesores-registrados')
     .then(response => {
        console.log(response.data);
        this.setState({ data: response.data, isLoading: false });
      })
     .catch(error => {
        console.error(error);
        this.setState({ isLoading: false });
      });
  }

  mostrarModalActualizar = (dato) => {
    this.setState({
      form: dato,
      modalActualizar: true,
    });
  };

  cerrarModalActualizar = () => {
    this.setState({ modalActualizar: false });
  };

  mostrarModalInsertar = () => {
    this.setState({
      modalInsertar: true,
    });
  };

  cerrarModalInsertar = () => {
    this.setState({ modalInsertar: false });
  };

  editar = (dato) => {
    var contador = 0;
    var arreglo = this.state.data;
    arreglo.map((registro) => {
      if (dato.email == registro.email) {
        arreglo[contador].cedula = dato.cedula;
        arreglo[contador].nombre = dato.nombre;
        arreglo[contador].apellidos = dato.apellidos;
        arreglo[contador].edad = this.calcularEdad(dato.fecha_de_nacimiento);
        arreglo[contador].fechanacimiento = dato.fecha_de_nacimiento;
        arreglo[contador].correo = dato.email;
  
        axios.put('http://localhost:5129/api/ControladorAdmin/modificar-profesor', {
          Cedula: dato.cedula,
          Nombre: dato.nombre,
          Apellidos: dato.apellidos,
          FechaDeNacimiento: dato.fecha_de_nacimiento,
          Email: dato.email
        }, {
          headers: {
            'Content-Type': 'application/json'
          }
        })
      .then(response => {
          if (response.status === 200) {
            console.log(response.data); // "El profesor se modificó exitosamente."
            this.setState({ data: arreglo, modalActualizar: false });
          }
        })
      .catch(error => {
          console.error(error);
        });
      }
      contador++;
    });
  };
  
  calcularEdad(fechaNacimiento) {
    const hoy = new Date();
    const nacimiento = new Date(fechaNacimiento);
    let edad = hoy.getFullYear() - nacimiento.getFullYear();
    const mes = hoy.getMonth() - nacimiento.getMonth();
    if (mes < 0 || (mes === 0 && hoy.getDate() < nacimiento.getDate())) {
        edad--;
    }
    return edad;
}

eliminar = (dato) => {
  var opcion = window.confirm("Estás seguro que deseas eliminar al profesor " + dato.nombre + " " + dato.apellidos);
  if (opcion === true) {
    // Realizar la solicitud DELETE a la API
    axios.delete('http://localhost:5129/api/ControladorAdmin/eliminar-profesor', {
      params: {
        email: dato.email
      }
    })
    .then(response => {
      console.log(response.data); // Puedes imprimir la respuesta en la consola
      // Si la respuesta indica éxito, puedes actualizar el estado o realizar otras acciones
      var arreglo = this.state.data.filter(registro => registro.cedula !== dato.cedula);
      this.setState({ data: arreglo, modalActualizar: false });
    })
    .catch(error => {
      // Manejas el error en caso de que ocurra
      console.error(error);
    });
  }
};


  insertar = () => {
    var valorNuevo = { ...this.state.form };
    console.log(valorNuevo);

    axios.post('http://localhost:5129/api/ControladorAdmin/registrar-profesor', {
      Cedula: valorNuevo.cedula,
      Nombre: valorNuevo.nombre,
      Apellidos: valorNuevo.apellidos,
      FechaDeNacimiento: valorNuevo.fecha_de_nacimiento,
      Email: valorNuevo.email
    })
        .then(response => {
            // Manejas la respuesta de la API
            console.log(response.data); // Puedes imprimir la respuesta en la consola
            // Si la respuesta indica éxito, puedes actualizar el estado o realizar otras acciones
            this.setState({ modalInsertar: false });
        })
        .catch(error => {
            // Manejas el error en caso de que ocurra
            console.error(error);
        });

    var lista = this.state.data;
    lista.push(valorNuevo);
    this.setState({ modalInsertar: false, data: lista });
  };
  

  handleChange = (e) => {
    this.setState({
      form: {
        ...this.state.form,
        [e.target.name]: e.target.value,
      },
    });
  };

  CambiarTheme = () => {
    const { setTheme, theme } = useContext(ThemeContext);
    setTheme((theme) => (theme === 'light' ? 'dark' : 'light'));
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
          <div className="Themecontent">
            <div className="Togglecontent">
              <div className="grid theme-container">
                
              </div>
            </div>
          </div>
        </Sidebar>
        <Content> 
        <Container>
            <div className="title-container">
              <h1 className="title">Profesores</h1>
            </div>
            </Container>
          <Container>
            <br />
            <Button color="success" onClick={()=>this.mostrarModalInsertar()}>Crear</Button>
            <br />
            <br />
            <Table>
              <thead>
                <tr>
                  <th>Cedula</th>
                  <th>Nombre</th>
                  <th>Apellidos</th>
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
                    <td>{dato.nombre}</td>
                    <td>{dato.apellidos}</td>
                    <td>{this.calcularEdad(dato.fecha_de_nacimiento)}</td>
                    <td>{dato.fecha_de_nacimiento}</td>
                    <td>{dato.email}</td>
                    <td>
                      <Button
                        color="primary"
                        onClick={() => this.mostrarModalActualizar(dato)}
                      >
                        Editar
                      </Button>{" "}
                      <Button color="danger" onClick={()=> this.eliminar(dato)}>Eliminar</Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
          <Modal isOpen={this.state.modalActualizar}>
            <ModalHeader>
            <div><h3>Editar Registro</h3></div>
            </ModalHeader>
  
            <ModalBody>
              <FormGroup>
                <label>
                Cedula:
                </label>
              
                <input
                  className="form-control"
                  name="cedula"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.cedula}
                />
              </FormGroup>
              
              <FormGroup>
                <label>
                  Nombre: 
                </label>
                <input
                  className="form-control"
                  name="nombre"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.nombre}
                />
              </FormGroup>

              <FormGroup>
                <label>
                  Apellidos: 
                </label>
                <input
                  className="form-control"
                  name="apellidos"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.apellidos}
                />
              </FormGroup>
              
              <FormGroup>
                <label>
                  Edad: 
                </label>
                <input
                  className="form-control"
                  name="edad"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.edad}
                />
              </FormGroup>
              <FormGroup>
                <label>
                  Fecha de Nacimiento: 
                </label>
                <input
                  className="form-control"
                  name="fecha_de_nacimiento"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.fecha_de_nacimiento}
                />
              </FormGroup>
            
            </ModalBody>
  
            <ModalFooter>
              <Button
                color="primary"
                onClick={() => this.editar(this.state.form)}
              >
                Editar
              </Button>
              <Button
                color="danger"
                onClick={() => this.cerrarModalActualizar()}
              >
                Cancelar
              </Button>
            </ModalFooter>
          </Modal>
          <Modal isOpen={this.state.modalInsertar}>
            <ModalHeader>
            <div><h3>Insertar Profesor</h3></div>
            </ModalHeader>
  
            <ModalBody>
              <FormGroup>
                <label>
                  Cedula: 
                </label>
                
                <input
                  className="form-control"
                  name="cedula"
                  type="text"
                  onChange={this.handleChange}
                />
              </FormGroup>
              
              <FormGroup>
                <label>
                  Nombre: 
                </label>
                <input
                  className="form-control"
                  name="nombre"
                  type="text"
                  onChange={this.handleChange}
                />
              </FormGroup>

              <FormGroup>
                <label>
                  Apellidos: 
                </label>
                <input
                  className="form-control"
                  name="apellidos"
                  type="text"
                  onChange={this.handleChange}
                />
              </FormGroup>
              
              <FormGroup>
                <label>
                  Edad: 
                </label>
                <input
                  className="form-control"
                  name="edad"
                  type="text"
                  onChange={this.handleChange}
                />
              </FormGroup>
              <FormGroup>
                <label>
                  Fecha de Nacimiento: 
                </label>
                <input
                  className="form-control"
                  name="fecha_de_nacimiento"
                  type="text"
                  onChange={this.handleChange} 
                />
              </FormGroup>
              <FormGroup>
                <label>
                  Correo: 
                </label>
                <input
                  className="form-control"
                  name="email"
                  type="text"
                  onChange={this.handleChange}
                />
              </FormGroup>
            </ModalBody>
  
            <ModalFooter>
              <Button
                color="primary"
                onClick={() => this.insertar()}
              >
                Insertar
              </Button>
              <Button
                className="btn btn-danger"
                onClick={() => this.cerrarModalInsertar()}
              >
                Cancelar
              </Button>
            </ModalFooter>
            
          </Modal>
            
        </Content> 
        
      </ThemeProvider>
    );
  }
}
export default Gestion_profesores;
