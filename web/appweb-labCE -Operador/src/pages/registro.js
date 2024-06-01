import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { ThemeContext } from '../App';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import axios from 'axios';

const Registro = ({ setLoggedIn, setEmail }) => {
  const [email, setEmailInput] = useState('');
  const [password, setPassword] = useState('');
  const [emailError, setEmailError] = useState('');
  const [passwordError, setPasswordError] = useState('');
  const [cedula, setCedula] = useState('');
  const [carnet, setCarnet] = useState('');
  const [nombre, setNombre] = useState('');
  const [apellidos, setApellidos] = useState('');
  const [fechaNacimiento, setFechaNacimiento] = useState('');
  const [correo, setCorreo] = useState('');
  const [cedulaError, setCedulaError] = useState('');
  const [carnetError, setCarnetError] = useState('');
  const [nombreError, setNombreError] = useState('');
  const [apellidosError, setApellidosError] = useState('');
  const [fechaNacimientoError, setFechaNacimientoError] = useState('');
  const [correoError, setCorreoError] = useState('');

  const navigate = useNavigate();
  const { theme } = useContext(ThemeContext);

  const calcularEdad = (fechaNacimiento) => {
    const hoy = new Date();
    const nacimiento = new Date(fechaNacimiento);
    let edad = hoy.getFullYear() - nacimiento.getFullYear();
    const mes = hoy.getMonth() - nacimiento.getMonth();
    if (mes < 0 || (mes === 0 && hoy.getDate() < nacimiento.getDate())) {
        edad--;
    }
    return edad;
  };

  const convertirFecha = (fecha) => {
    const partes = fecha.split('-');
    // Reorganiza las partes de la fecha para formar el nuevo formato
    return `${partes[2]}-${partes[1]}-${partes[0]}`;
  };

  const volver = async () => {
    navigate("/");
  } ;

  const onButtonClick = async () => {
    setPasswordError('');
    setCedulaError('');
    setCarnetError('');
    setNombreError('');
    setApellidosError('');
    setFechaNacimientoError('');
    setCorreoError('');

    var isValid = true;
    // Verificar si los campos están vacíos y mostrar mensajes de error correspondientes
    if (!correo) {
      setCorreoError('Please enter your email');
      isValid = false;
    } else if (!/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(correo)) {
      setCorreoError('Please enter a valid email');
      isValid = false;
    }

    if (!password) {
      setPasswordError('Please enter a password');
      isValid = false;
    } else if (password.length < 8) {
      setPasswordError('The password must be 8 characters or longer');
      isValid = false;
    }

    if (!cedula) {
      setCedulaError('Please enter your ID number');
      isValid = false;
    }

    if (!carnet) {
      setCarnetError('Please enter your student ID');
      isValid = false;
    }

    if (!nombre) {
      setNombreError('Please enter your name');
      isValid = false;
    }

    if (!apellidos) {
      setApellidosError("Please enter your last name");
      isValid = false;
    } 

    if (!fechaNacimiento) {
      setFechaNacimientoError('Please select your date of birth');
      isValid = false;
    }
    console.log(cedula);

    if (isValid) {
      axios.post('http://localhost:5129/api/Operador/registrar', {
        Cedula: cedula,
        Carne: carnet,
        Nombre: nombre,
        Apellidos: apellidos,
        FechaDeNacimiento: convertirFecha(fechaNacimiento),
        Edad: calcularEdad(fechaNacimiento),
        Email: correo,
        Contraseña: password
      })
      .then(function (response) {
        // Manejar la respuesta de la API
        if (response.data === 'Operador registrado exitosamente') {
          // Si la operación fue exitosa, redireccionar al usuario a la página de inicio de sesión\
          alert(response.data);
          navigate('/login_operador');
        } else {
          // Si hubo algún error, mostrar un mensaje de error
          alert("No se pudo registrar correctamente el operador");
        }
      })
      .catch(function (error) {
        // Manejar cualquier error que pueda ocurrir
        console.log(error);
        // Mostrar un mensaje de error genérico
        alert("Hubo un error al registrar el operador");
      });
    }
  };

  // Función para extraer los valores de la fecha de nacimiento
  const extractDateValues = () => {
    const dateParts = fechaNacimiento.split('-');
    const day = parseInt(dateParts[0], 10);
    const month = parseInt(dateParts[1], 10);
    const year = parseInt(dateParts[2], 10);
    return { day, month, year };
  };

  return (
    <div className="mainContainer">
      <div className="titleContainer">
        <div>Registro </div>
      </div>
      <br />
      <div style={{ position: 'relative', left: '-170px' }}>Cédula: </div>
      <div className="inputContainer">
        <input
          value={cedula}
          placeholder="Inserte su cedula"
          onChange={(ev) => setCedula(ev.target.value)}
          className="inputBox"
        />
        <label className="errorLabel">{cedulaError}</label>
      </div>
      <br />
      <div style={{ position: 'relative', left: '-170px' }}>Carnet: </div>
      <div className="inputContainer">
        <input
          value={carnet}
          placeholder="Inserte su carnet"
          onChange={(ev) => setCarnet(ev.target.value)}
          className="inputBox"
        />
        <label className="errorLabel">{carnetError}</label>
      </div>
      <br />
      <div style={{ position: 'relative', left: '-170px' }}>Nombre: </div>
      <div className="inputContainer">
        <input
          value={nombre}
          placeholder="Inserte su nombre"
          onChange={(ev) => setNombre(ev.target.value)}
          className="inputBox"
        />
        <label className="errorLabel">{nombreError}</label>
      </div>
      <br />
      <div style={{ position: 'relative', left: '-160px' }}>Apellidos: </div>
      <div className="inputContainer">
        <input
          value={apellidos}
          placeholder="Inserte sus apellidos"
          onChange={(ev) => setApellidos(ev.target.value)}
          className="inputBox"
        />
        <label className="errorLabel">{nombreError}</label>
      </div>
      <br />
      <div style={{ position: 'relative', left: '-120px' }}>Fecha de nacimiento : </div>
      <div className="inputContainer">
        <input
          value={fechaNacimiento}
          placeholder="dd-mm-yyyy"
          onChange={(ev) => setFechaNacimiento(ev.target.value)}
          className="inputBox"
          pattern="\d{2}-\d{2}-\d{4}" // Expresión regular para validar el formato
        />
        <label className="errorLabel">{fechaNacimientoError}</label>
      </div>
      <br />
      <div style={{ position: 'relative', left: '-170px' }}>Correo: </div>
      <div className="inputContainer">
        <input
          value={correo}
          placeholder="Inserte su correo"
          onChange={(ev) => setCorreo(ev.target.value)}
          className="inputBox"
        />
        <label className="errorLabel">{correoError}</label>
      </div>
      <br />
      <div style={{ position: 'relative', left: '-155px' }}>Contraseña: </div>
      <div className="inputContainer">
        <input
          value={password}
          placeholder="Inserte su contraseña"
          onChange={(ev) => setPassword(ev.target.value)}
          className="inputBox"
        />
        <label className="errorLabel">{passwordError}</label>
      </div>
      <br />
      <div className="inputCon" style={{ display: 'flex', justifyContent: 'space-between' }}>
        <input className="inputButton" type="button" onClick={onButtonClick} value="Registrarse" />
        <input className="inputButton" type="button" onClick={volver} value="Volver" />
      </div>
    </div>
  );
};

export default Registro;