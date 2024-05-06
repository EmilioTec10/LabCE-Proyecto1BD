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

const data = [
  { placa: 886, tipo: "Proyector", marca: "sony", fechacompra: "28/10/04", aprobador: "si" },
  { placa: 879, tipo: "Proyector", marca: "sony", fechacompra: "23/08/97", aprobador: "si"},
  { placa: 687, tipo: "Proyector", marca: "sony", fechacompra: "23/06/97", aprobador: "si"},
  { placa: 368, tipo: "Proyector", marca: "sony", fechacompra: "23/08/97", aprobador: "si"},
  { placa: 809, tipo: "Proyector", marca: "sony", fechacompra: "23/08/97", aprobador: "si"},
  { placa: 557, tipo: "Proyector", marca: "sony", fechacompra: "23/08/97", aprobador: "si"},
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

class Gestion_activos extends React.Component {
  state = {
    data: data,
    modalActualizar: false,
    modalInsertar: false,
    form: {
      placa: "",
      tipo: "",
      marca: "",
      fechacompra: "",
      aprobador: "",
    },
  };

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
      if (dato.placa == registro.placa) {
        arreglo[contador].placa = dato.placa;
        arreglo[contador].tipo = dato.tipo;
        arreglo[contador].marca = dato.marca;
        arreglo[contador].fechacompra = dato.fechacompra;
        arreglo[contador].aprobador = dato.aprobador;
      }
      contador++;
    });
    this.setState({ data: arreglo, modalActualizar: false });
  };

  eliminar = (dato) => {
    var opcion = window.confirm("Estás Seguro que deseas Eliminar el elemento " + dato.cedula);
    if (opcion === true) {
      var arreglo = this.state.data.filter(registro => registro.cedula !== dato.cedula);
      this.setState({ data: arreglo, modalActualizar: false });
    }
  };

  insertar = () => {
    var valorNuevo = { ...this.state.form };
    valorNuevo.cedula = parseInt(valorNuevo.cedula); // Aseguramos que la cédula sea un número
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
            <h1 style={{ position: 'relative', left: '50%', transform: 'translateX(-50%)', top: '-1px' }}>Gestion Activos</h1>
          </Container>
          <Container>
            <Table>
              <thead>
                <tr>
                  <th>Placa</th>
                  <th>Tipo</th>
                  <th>Marca</th>
                  <th>Fecha de Compra</th>
                  <th>Aprobador</th>
                  <th>Acciones</th>
                </tr>
              </thead>
  
              <tbody>
                {this.state.data.map((dato) => (
                  <tr key={dato.placa}>
                    <td>{dato.placa}</td>
                    <td>{dato.tipo}</td>
                    <td>{dato.marca}</td>
                    <td>{dato.fechacompra}</td>
                    <td>{dato.aprobador}</td>
                    <td>
                      <Button
                        color="primary"
                        onClick={() => this.mostrarModalActualizar(dato)}
                      >
                        Editar
                      </Button>{" "}
                      
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
                Placa:
                </label>
              
                <input
                  className="form-control"
                  name="placa"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.placa}
                />
              </FormGroup>
              
              <FormGroup>
                <label>
                  Tipo: 
                </label>
                <input
                  className="form-control"
                  name="tipo"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.tipo}
                />
              </FormGroup>
              
              <FormGroup>
                <label>
                  Marca: 
                </label>
                <input
                  className="form-control"
                  name="marca"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.marca}
                />
              </FormGroup>
              <FormGroup>
                <label>
                  Fecha de Compra: 
                </label>
                <input
                  className="form-control"
                  name="fechacompra"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.fechacompra}
                />
              </FormGroup>
              <FormGroup>
                <label>
                  Aprobador: 
                </label>
                <input
                  className="form-control"
                  name="aprobador"
                  type="text"
                  onChange={this.handleChange}
                  value={this.state.form.aprobador}
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
          </Modal>
            
        </Content> 
        
      </ThemeProvider>
    );
  }
}
export default Gestion_activos;
