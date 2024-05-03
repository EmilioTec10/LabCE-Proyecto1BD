import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios'; // Importa Axios para hacer solicitudes HTTP
import { ThemeContext } from '../App';

const Login_profesor = () => {
  const [email, setEmailInput, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [emailError, setEmailError] = useState('');
  const [passwordError, setPasswordError] = useState('');
  const navigate = useNavigate();

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
    
    // Validación de las credenciales del chef
    if (isValid) {
      
      try {
        const response = await axios.post('http://localhost:5129/api/ControladorProfesor/ingresar', {
          email: email,
          contraseña: password,
        });

        if (response.status === 200) {
          navigate('/aprobacion_prestamo');
        } else {
          setEmailError('Invalid email or password');
          setPassword('');
        }
      } catch (error) {
        console.error('Error al iniciar sesión:', error);
        setEmailError('Error al iniciar sesión');
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
          onChange={(ev) => setEmailInput(ev.target.value)}
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

export default Login_profesor;
