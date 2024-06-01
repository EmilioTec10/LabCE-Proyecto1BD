
import React, { useContext } from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { Light, Dark } from '../styles/themes';
import { NavLink } from 'react-router-dom';
import { ThemeContext } from '../App';
import logo from '../assets/react.svg';
import { AiOutlineHome, AiOutlineApartment } from 'react-icons/ai';
import { MdOutlineAnalytics, MdLogout } from 'react-icons/md';
import axios from 'axios';



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

const EmailInput = styled.input`

  height: 40px;
`;

const DataTableContainer = styled.div`
  width: 80%;
  margin-left: auto;
  margin-right: auto;
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

class Reserva_laboratorio extends React.Component {
  constructor(props) {
    super(props);
  
    this.state = {
      data: [], // Inicialmente, los datos estarán vacíos
      modalActualizar: false,
      modalInsertar: false,
      labSchedule: {},
      selectedLab: "",
      form: {
        dia: "",
        horaInicio: "",
        horaFin: "",
        descripcion: "",
        palmada: false,
        email_est: "",
        email_prof: "" 
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

  reservarLaboratorio = (nombre) => {
    // Recopilar los datos del formulario de reserva
    const { dia, horaInicio, horaFin, descripcion, palmada, email_est, email_prof } = this.state.form;
    console.log(this.state.form);

    const fechaFormulario = this.state.form.dia;

    // Forma la fecha en el formato deseado "yyyy-mm-dd"
    const fechaAPI = `${fechaFormulario}T00:00:00.000Z`;

    const horaInicioFormulario = this.state.form.horaInicio;
    const horaFinFormulario = this.state.form.horaFin;

    // Forma la hora de inicio y fin en el formato deseado "yyyy-mm-ddThh:mm:ss.msZ"
    const horaInicioAPI = `${fechaFormulario}T${horaInicioFormulario}:00.000Z`;
    const horaFinAPI = `${fechaFormulario}T${horaFinFormulario}:00.000Z`;

    // Realizar la solicitud HTTP POST a la API
    axios.post('http://localhost:5129/api/Operador/reservar-laboratorio', {
      Nombre: nombre,
      Dia: fechaAPI,
      HoraInicio: horaInicioAPI,
      HoraFin: horaFinAPI,
      Descripcion: descripcion,
      Palmada: false,
      email_est: email_est,
      email_prof: ""
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
  };

  
  mostrarModalActualizar = (dato) => {
    this.setState({
      modalActualizar: true,
      selectedLab: dato.nombre,
    });
  };


  cerrarModalActualizar = () => {
    this.setState({ modalActualizar: false });
  };

  mostrarModalInsertar = (lab) => {
    this.mostrarHorario(lab);
    this.setState({
      modalInsertar: true,
      selectedLab: lab,
    });
  };

  cerrarModalInsertar = () => {
    this.setState({ modalInsertar: false });
  };

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
            const day = new Date(reservation.dia).toLocaleDateString('en-US', { weekday: 'long' });
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
                      <Button color="danger" onClick={() => this.mostrarModalInsertar(dato.nombre)}>Horario</Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
          <Modal isOpen={this.state.modalActualizar} size="lg">
          <ModalHeader>
            <div><h3>Reservar Laboratorio</h3></div>
          </ModalHeader>
          <ModalBody>
            {/* Agrega los campos necesarios para el formulario de reserva */}
            <FormGroup>
              <label>Fecha:</label>
              <input className="form-control" name="dia" type="date" onChange={this.handleChange} value={this.state.form.dia} />
            </FormGroup>
            <FormGroup>
              <label>Hora de inicio:</label>
              <input className="form-control" name="horaInicio" type="time" onChange={this.handleChange} value={this.state.form.horaInicio} />
            </FormGroup>
            <FormGroup>
              <label>Hora de fin:</label>
              <input className="form-control" name="horaFin" type="time" onChange={this.handleChange} value={this.state.form.horaFin} />
            </FormGroup>
            <FormGroup>
              <label>Descripción:</label>
              <textarea className="form-control" name="descripcion" onChange={this.handleChange} value={this.state.form.descripcion} />
            </FormGroup>
            <FormGroup>
              <label>Email Estudiante:</label>
              <EmailInput className="form-control" type="text" name="email_est" onChange={this.handleChange}value={this.state.form.email_est}/>
            </FormGroup>
            <FormGroup check>
              <label check>
                <input type="checkbox" name="palmada" onChange={this.handleChange} checked={this.state.form.palmada} />{' '}
                Palmada
              </label>
            </FormGroup>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" onClick={() => this.reservarLaboratorio(this.state.selectedLab)}>Confirmar</Button>
            <Button className="btn btn-danger" onClick={() => this.cerrarModalActualizar()}>Cerrar</Button>
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
export default Reserva_laboratorio;
