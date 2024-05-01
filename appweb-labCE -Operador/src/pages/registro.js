import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { ThemeContext } from '../App';

const Registro = ({ setLoggedIn, setEmail }) => {
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
      if (email === "1@gmail.com" && password === "1") {
       // setLoggedIn(true);
        //setEmail(email);
        navigate('/gestion_laboratorios');
      } else {
        setEmailError('Invalid email or password');
        setPassword('');
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

export default Registro;
