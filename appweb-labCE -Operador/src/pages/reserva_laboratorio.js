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
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import TimePicker from 'react-time-picker';
import 'react-time-picker/dist/TimePicker.css';



import {
  Table,
  Container,
  Modal,
  ModalHeader,
  ModalBody,
  FormGroup,
  ModalFooter,
  Button,
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

class Reserva_laboratorio extends React.Component {
  constructor(props) {
    super(props);
  
    this.state = {
      data: [], // Inicialmente, los datos estarán vacíos
      modalActualizar: false,
      modalInsertar: false,
      form: {
        laboratorio: "",
        capacidad: "",
        facilidades: "",
        activos: "",
      },
    };
  }

  componentDidMount() {
    // Realizar la solicitud al endpoint de la API para obtener los datos
    fetch('http://localhost:5129/api/Operador/ver-labs-disponibles')
      .then(response => response.json())
      .then(data => {
        // Actualizar el estado con los datos recibidos
        this.setState({ data: data });
      })
      .catch(error => {
        console.error('Error al obtener los datos del laboratorio:', error);
      });
  }

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
    var arreglo = [...this.state.data]; // Copia del array de datos
    var indice = arreglo.findIndex(item => item.laboratorio === dato.laboratorio); // Busca el índice del dato a editar
    if (indice !== -1) {
      arreglo[indice] = dato; // Reemplaza el dato en el índice encontrado
      this.setState({ data: arreglo, modalActualizar: false }); // Actualiza el estado con el nuevo array de datos
    }
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

  //-----------codigo de reserva-----------//


  

  

  

  
  
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
            <h1>Laboratorios</h1>
          </Container>
          <Container>
            
            <Table>
              <thead>
                <tr>
                  <th>Laboratorio</th>
                  <th>Capacidad</th>
                  <th>Facilidades</th>
                  <th>Activos</th>
                  <th>Acciones</th>
                </tr>
              </thead>
  
              <tbody>
                {this.state.data.map((dato) => (
                  <tr key={dato.laboratorio}>
                    <td>{dato.nombre}</td>
                    <td>{dato.capacidad}</td>
                    <td>{dato.facilidades}</td>
                    <td>{dato.activos}</td>
                    <td>
                      <Button
                        color="primary"
                        onClick={() => this.mostrarModalActualizar(dato)}
                      >
                        Reservar
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
          <Modal isOpen={this.state.modalActualizar}>
            <ModalHeader>
            <div><h3>Selecciona la Fecha</h3></div>
            </ModalHeader>
  
            <ModalBody>
              
              
            </ModalBody>
  
            <ModalFooter>
              <Button
                color="primary"
                onClick={() => this.editar(this.state.form)}
              >
                Reservar
              </Button>
              <Button
                color="danger"
                onClick={() => this.cerrarModalActualizar()}
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
export default Reserva_laboratorio;
