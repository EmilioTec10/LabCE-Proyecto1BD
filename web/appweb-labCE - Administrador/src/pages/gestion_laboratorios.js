import React, { useContext } from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { Light, Dark } from '../styles/themes';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import logo from '../assets/react.svg';
import { Table, Button, Container, Modal, ModalHeader, ModalBody, FormGroup, ModalFooter } from "reactstrap";
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import axios from 'axios';

const linksArray = [
  { label: 'Gestion Profesores', icon: <AiOutlineHome />, to: '/gestion_profesores' },
  { label: 'Gestion Laboratorios', icon: <MdOutlineAnalytics />, to: '/gestion_laboratorios' },
  { label: 'Gestion Activos', icon: <AiOutlineApartment />, to: '/gestion_activos' },
  { label: 'Aprobar Operadores', icon: <MdOutlineAnalytics />, to: '/aprobacion_operadores' },
  { label: 'Cambio Contraseña', icon: <MdOutlineAnalytics />, to: '/cambio_contrasenna' },
  { label: 'Reportes', icon: <MdOutlineAnalytics />, to: '/reportes' },
];

const secondarylinksArray = [
  { label: 'Salir', icon: <MdLogout />, to: '/' },
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
  margin-left: 300px;
  flex-grow: 1;
`;

const Divider = styled.div`
  height: 1px;
  width: 100%;
  background: ${(props) => props.theme.bg3};
  margin: 20px 0;
`;

const ScheduleContainer = styled.div`
  display: grid;
  grid-template-columns: auto repeat(6, minmax(100px, 1fr));
  grid-template-rows: auto repeat(24, minmax(30px, 1fr));
  gap: 1px;
`;

const TimeSlot = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid #ccc;
  background-color: ${(props) => (props.occupied ? '#ffcccc' : '#ccffcc')};
  position: relative; /* Para posicionar la descripción */
`;

const Description = styled.div`
  position: absolute;
  top: 0;
  left: 0;
  padding: 5px;
  background-color: rgba(255, 255, 255, 0.8);
`;

const daysOfWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
const startTime = 7;
const endTime = 21;

const generateTimeSlots = () => {
  const timeSlots = [];
  for (let hour = startTime; hour <= endTime; hour++) {
    for (let minute = 0; minute < 60; minute += 30) {
      const time = `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}`;
      timeSlots.push(time);
    }
  }
  return timeSlots;
};

var lab = "";
class Gestion_laboratorios extends React.Component {
  
  state = {
    data: [],
    modalActualizar: false,
    modalInsertar: false,
    labSchedule: {}, // Inicializar labSchedule como un objeto vacío
    selectedLab: "",
    form: {
      id: "",
      nombre: "",
      capacidad: "",
      computadores: "",
      facilidades: "",
      activos: "",
    },
  };

  componentDidMount() {
    // Obtener los laboratorios disponibles
    axios.get('http://localhost:5129/api/ControladorAdmin/ver-laboratorios-disponibles')
      .then(response => {
        console.log(response.data);
        this.setState({ data: response.data, isLoading: false });
      })
      .catch(error => {
        console.error(error);
        this.setState({ isLoading: false });
      });
   
  }

  mostrarHorario = (lab) => {
    console.log(lab);
     // Obtener las reservaciones para el laboratorio específico (F2-07 en este caso)
     axios.get('http://localhost:5129/api/ControladorAdmin/ver-reservaciones-lab',{
      params:{
        nombre_lab: lab
      }
    })
    .then(response => {
      const labReservations = response.data;
      let newLabSchedule = {}; // Cambio const por let

      labReservations.forEach(reservation => {
          const date = new Date(reservation.dia);
          date.setDate(date.getDate() + 1);
          const day = date.toLocaleDateString('en-US', { weekday: 'long' });
          const startTime = new Date(reservation.hora_inicio);
          const endTime = new Date(reservation.hora_fin);
          const description = reservation.descripcion;

          // Verificar si la fecha de la reservación es mayor que la fecha actual
          const currentDate = new Date();
          if (startTime >= currentDate) {
              // Verificar si el día ya existe en el objeto labSchedule, si no, inicializarlo
              if (!(day in newLabSchedule)) {
                  newLabSchedule[day] = {};
              }

              // Iterar sobre el rango de tiempo desde la hora de inicio hasta la hora de fin
              let currentTime = startTime;
              while (currentTime <= endTime) {
                  const currentTimeString = currentTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
                  newLabSchedule[day][currentTimeString] = description;
                  // Incrementar el tiempo en 30 minutos
                  currentTime.setMinutes(currentTime.getMinutes() + 30);
              }
          }
      });
      this.setState({ labSchedule: newLabSchedule });
      console.log(this.state.labSchedule);
  })
  .catch(error => {
      console.log(this.state.selectedLab);
      console.error(error);
  });
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

  mostrarModalInsertar = (lab) => {
    this.mostrarHorario(lab);
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
      if (dato.nombre === registro.nombre) {
        arreglo[contador].nombre = dato.nombre;
        arreglo[contador].capacidad = dato.capacidad;
        arreglo[contador].computadores = dato.computadores;
        arreglo[contador].facilidades = dato.facilidades;
        arreglo[contador].activos = dato.activos;
      }
      contador++;
    });
  
    // Realizar la solicitud HTTP POST para actualizar el laboratorio
    axios.post('http://localhost:5129/api/ControladorAdmin/actualizar-laboratorio', {
      nombre: dato.nombre,
      capacidad: dato.capacidad,
      computadores: dato.computadores,
      facilidades: dato.facilidades,
      activos: dato.activos
    })
    .then(response => {
      alert(response.data);
      // Aquí puedes manejar la respuesta si es necesario
    })
    .catch(error => {
      console.error(error);
      alert(error);
      // Aquí puedes manejar el error si es necesario
    });
  
    this.setState({ data: arreglo, modalActualizar: false });
  };

  eliminar = (dato) => {
    var opcion = window.confirm("Estás Seguro que deseas Eliminar el elemento " + dato.nombre);
    if (opcion === true) {
      var arreglo = this.state.data.filter(registro => registro.nombre !== dato.nombre);
      this.setState({ data: arreglo, modalActualizar: false });
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

  render() {
    const themeStyle = this.props.theme === 'dark' ? Light : Dark;
    const timeSlots = generateTimeSlots(); 
    
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
              <div className="grid theme-container"></div>
            </div>
          </div>
        </Sidebar>

        <Content> 
          <Container>
            <h1 style={{ position: 'relative', left: '50%', transform: 'translateX(-50%)', top: '-1px' }}>Gestion Laboratorios</h1>
          </Container>
          <Container>
            <Table>
              <thead>
                <tr>
                  <th>Nombre</th>
                  <th>Capacidad</th>
                  <th>Computadores</th>
                  <th>Facilidades</th>
                  <th>Activos</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {this.state.data.map((dato) => (
                  <tr key={dato.nombre}>
                    <td>{dato.nombre}</td>
                    <td>{dato.capacidad}</td>
                    <td>{dato.computadores}</td>
                    <td>{dato.facilidades}</td>
                    <td>{dato.activos}</td>
                    <td>
                      <Button color="primary" onClick={() => this.mostrarModalActualizar(dato)}>Editar</Button>{" "}
                      <Button color="danger" onClick={() => this.mostrarModalInsertar(dato.nombre)}>Horario</Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
          <Modal isOpen={this.state.modalActualizar}>
            <ModalHeader>
              <div><h3>Editar</h3></div>
            </ModalHeader>
            <ModalBody>
              <FormGroup>
                <label>Nombre:</label>
                <input className="form-control" name="nombre" type="text" onChange={this.handleChange} value={this.state.form.nombre} />
              </FormGroup>
              <FormGroup>
                <label>Capacidad:</label>
                <input className="form-control" name="capacidad" type="text" onChange={this.handleChange} value={this.state.form.capacidad} />
              </FormGroup>
              <FormGroup>
                <label>Computadores:</label>
                <input className="form-control" name="computadores" type="text" onChange={this.handleChange} value={this.state.form.computadores} />
              </FormGroup>
              <FormGroup>
                <label>Facilidades:</label>
                <input className="form-control" name="facilidades" type="text" onChange={this.handleChange} value={this.state.form.facilidades} />
              </FormGroup>
              <FormGroup>
                <label>Activos:</label>
                <input className="form-control" name="activos" type="text" onChange={this.handleChange} value={this.state.form.activos} />
              </FormGroup>
            </ModalBody>
            <ModalFooter>
              <Button color="primary" onClick={() => this.editar(this.state.form)}>Editar</Button>
              <Button color="danger" onClick={() => this.cerrarModalActualizar()}>Cancelar</Button>
            </ModalFooter>
          </Modal>
          <Modal isOpen={this.state.modalInsertar} size="lg">
            <ModalHeader>
              <div><h3>Horarios Disponibles</h3></div>
            </ModalHeader>
            <ScheduleContainer>
              <div></div>
              {daysOfWeek.map((day) => (
                <TimeSlot key={day}>{day}</TimeSlot>
              ))}
              {timeSlots.map((time) => (
                <React.Fragment key={time}>
                  <TimeSlot>{time}</TimeSlot>
                  {daysOfWeek.map((day, index) => (
                    <TimeSlot key={`${day}-${time}`} occupied={this.state.labSchedule[day] && this.state.labSchedule[day][time]}>
                      {this.state.labSchedule[day] && this.state.labSchedule[day][time] && (
                        <Description>{this.state.labSchedule[day][time]}</Description>
                      )}
                    </TimeSlot>
                  ))}
                </React.Fragment>
              ))}
            </ScheduleContainer>
            <ModalFooter>
              <Button className="btn btn-danger" onClick={() => this.cerrarModalInsertar()}>Cerrar</Button>
            </ModalFooter>
          </Modal>
        </Content> 
      </ThemeProvider>
    );
  }
}

export default Gestion_laboratorios;