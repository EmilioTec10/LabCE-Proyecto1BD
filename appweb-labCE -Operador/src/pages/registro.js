import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { ThemeContext } from '../App';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

const Registro = ({ setLoggedIn, setEmail }) => {
  const [email, setEmailInput] = useState('');
  const [password, setPassword] = useState('');
  const [emailError, setEmailError] = useState('');
  const [passwordError, setPasswordError] = useState('');
  const [cedula, setCedula] = useState('');
  const [carnet, setCarnet] = useState('');
  const [nombre, setNombre] = useState('');
  const [fechaNacimiento, setFechaNacimiento] = useState('');
  const [correo, setCorreo] = useState('');
  const [cedulaError, setCedulaError] = useState('');
  const [carnetError, setCarnetError] = useState('');
  const [nombreError, setNombreError] = useState('');
  const [fechaNacimientoError, setFechaNacimientoError] = useState('');
  const [correoError, setCorreoError] = useState('');

  const navigate = useNavigate();
  const { theme } = useContext(ThemeContext);

  const onButtonClick = async () => {
    setEmailError('');
    setPasswordError('');
    setCedulaError('');
    setCarnetError('');
    setNombreError('');
    setFechaNacimientoError('');
    setCorreoError('');

    let isValid = true;

    // Verificar si los campos están vacíos y mostrar mensajes de error correspondientes
    if (!email) {
      setEmailError('Please enter your email');
      isValid = false;
    } else if (!/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(email)) {
      setEmailError('Please enter a valid email');
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

    if (!fechaNacimiento) {
      setFechaNacimientoError('Please select your date of birth');
      isValid = false;
    }

    if (!correo) {
      setCorreoError('Please enter your email address');
      isValid = false;
    }

    if (isValid) {
      // Extraer los valores de la fecha de nacimiento
      const dateValues = extractDateValues();

      // Aquí puedes hacer lo que necesites con los valores de la fecha de nacimiento

      // Redireccionar al usuario a la página de inicio de sesión
      navigate('/login');
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
      <div style={{ position: 'relative', left: '-135px' }}>Inserte su cédula: </div>
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
      <div style={{ position: 'relative', left: '-135px' }}>Inserte su carnet: </div>
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
      <div style={{ position: 'relative', left: '-93px' }}>Inserte su nombre y apellidos: </div>
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
      <div style={{ position: 'relative', left: '-125px' }}>Fecha de nacimiento : </div>
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
      <div style={{ position: 'relative', left: '-135px' }}>Inserte su correo: </div>
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
      <div style={{ position: 'relative', left: '-120px' }}>Inserte su contraseña: </div>
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
      <div className="inputContainer">
        <input className="inputButton" type="button" onClick={onButtonClick} value="Registrarse" />
      </div>
    </div>
  );
};

export default Registro;
