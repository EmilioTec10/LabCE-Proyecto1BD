import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { ThemeContext } from '../App';
import axios from 'axios';

export let email_output = ''; // Declara la variable email fuera del componente

const Login_operador = ({ setLoggedIn, setEmail }) => {
  const [email, setEmailInput] = useState(''); // Define setEmailInput aquí
  const [password, setPassword] = useState('');
  const [emailError, setEmailError] = useState('');
  const [passwordError, setPasswordError] = useState('');
  const navigate = useNavigate();
  const { theme } = useContext(ThemeContext);

  const onButtonClick = async () => {
    setEmailError('');
    setPasswordError('');

    let isValid = true;

    // Validación del correo electrónico
    if (!email) {
      setEmailError('Please enter your email');
      isValid = false;
    } else if (!/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(email)) {
      setEmailError('Please enter a valid email');
      isValid = false;
    }

    // Validación de la contraseña
    if (!password) {
      setPasswordError('Please enter a password');
      isValid = false;
    } else if (password.length < 1) {
      setPasswordError('The password must be 8 characters or longer');
      isValid = false;
    }
    
    
    if (isValid) {
      try {
        const response = await axios.post('http://localhost:5129/api/Operador/ingresar', {
          email: email,
          contraseña: password,
        });
    
        if (response.status === 200) {
          navigate('/reserva_laboratorio');
          email_output = email; // Asigna el valor de emailInput a la variable email
        } else {
          // Si la solicitud fue exitosa pero la respuesta no es 200,
          // significa que la API devolvió un error diferente de 200
          setEmailError('Error al iniciar sesión');
        }
      } catch (error) {
        // Manejar el error de solicitud a la API
        if (error.response) {
          // Si la respuesta de error contiene un código de estado 401,
          // indica que las credenciales son inválidas
          if (error.response.status === 401) {
            setEmailError(error.response.data);
            setPassword('');
          } else {
            setEmailError('Error al iniciar sesión');
          }
        } else {
          // Si el error no tiene una respuesta, es un error de red o un error interno del cliente
          console.error('Error al iniciar sesión:', error);
          setEmailError('Error al iniciar sesión');
        }
      }
    }
    
  };

  return (
    <div className="mainContainer">
      <div className="titleContainer">
        <div>Inserte sus credenciales </div>
      </div>
      <br />
      <div className="inputContainer">
        <input
          value={email}
          placeholder="Enter your email here"
          onChange={(ev) => setEmailInput(ev.target.value)} // Utiliza setEmailInput para actualizar el estado del email
          className="inputBox"
        />
        <label className="errorLabel">{emailError}</label>
      </div>
      <br />
      <div className="inputContainer">
        <input
          value={password}
          placeholder="Enter your password here"
          onChange={(ev) => setPassword(ev.target.value)}
          className="inputBox"
        />
        <label className="errorLabel">{passwordError}</label>
      </div>
      <br />
      <div className="inputContainer">
        <input className="inputButton" type="button" onClick={onButtonClick} value="Log in" />
      </div>
    </div>
  );
};

export default Login_operador;
