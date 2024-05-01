import React from "react";
import "../App.css";
import "bootstrap/dist/css/bootstrap.min.css";
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
  { cedula: 504560886, nombreyapellidos: "Emmanuel esquivel Chavarria", edad: "19", fechanacimiento: "28/10/04", correo: "ema@gmail.com" },
  { cedula: 103450879, nombreyapellidos: "Carlos", edad: "20", fechanacimiento: "23/08/97", correo: "ema@gmail.com"},
  { cedula: 103410687, nombreyapellidos: "Juan", edad: "80", fechanacimiento: "23/06/97", correo: "ema@gmail.com"},
  { cedula: 119200368, nombreyapellidos: "Pepe", edad: "45", fechanacimiento: "23/08/97", correo: "ema@gmail.com" },
  { cedula: 123467809, nombreyapellidos: "Emilio", edad: "21", fechanacimiento: "23/08/97", correo: "ema@gmail.com"},
  { cedula: 345676557, nombreyapellidos: "Naruto", edad: "89", fechanacimiento: "23/08/97", correo: "ema@gmail.com"},
];

class Gestion_profesores extends React.Component {
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
      if (dato.cedula == registro.cedula) {
        arreglo[contador].cedula = dato.cedula;
        arreglo[contador].nombreyapellidos = dato.nombreyapellidos;
        arreglo[contador].edad = dato.edad;
        arreglo[contador].fechanacimiento = dato.fechanacimiento;
        arreglo[contador].correo = dato.correo;
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

  render() {
    
    return (
      <>
        <Container>
        <br />
          <Button color="success" onClick={()=>this.mostrarModalInsertar()}>Crear</Button>
          <br />
          <br />
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
                Nombre y Apellidos: 
              </label>
              <input
                className="form-control"
                name="nombreyapellidos"
                type="text"
                onChange={this.handleChange}
                value={this.state.form.nombreyapellidos}
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
                name="fechanacimiento"
                type="text"
                onChange={this.handleChange}
                value={this.state.form.fechanacimiento}
              />
            </FormGroup>
            <FormGroup>
              <label>
                Correo: 
              </label>
              <input
                className="form-control"
                name="correo"
                type="text"
                onChange={this.handleChange}
                value={this.state.form.correo}
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
                Nombre y Apellidos: 
              </label>
              <input
                className="form-control"
                name="nombreyapellidos"
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
                name="fechadenacimiento"
                type="text"
                
              />
            </FormGroup>
            <FormGroup>
              <label>
                Correo: 
              </label>
              <input
                className="form-control"
                name="correo"
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
      </>
    );
  }
}
export default Gestion_profesores;