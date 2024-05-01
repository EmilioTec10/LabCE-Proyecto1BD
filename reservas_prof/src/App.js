import React, { useState } from "react";
import logo from "./logo.svg";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import TimePicker from 'react-time-picker';
import 'react-time-picker/dist/TimePicker.css';

const App = () => {
  const [fecha, setFecha] = useState(new Date());
  const [hora, setHora] = useState('12:00'); // Hora inicial, puedes cambiarla según tus necesidades

  const onDateChange = fecha => {
    setFecha(fecha);
  };

  const onTimeChange = hora => {
    setHora(hora);
  };

  const mostrarFechaHora = () => {
    const fechaHora = new Date(fecha);
    const horaSplit = hora.split(':');
    fechaHora.setHours(parseInt(horaSplit[0], 10));
    fechaHora.setMinutes(parseInt(horaSplit[1], 10));
    alert(fechaHora);
  };

  const hoy = new Date();
  const tresSemanasDespues = new Date();
  tresSemanasDespues.setDate(hoy.getDate() + 21);

  const esDiaHabil = date => {
    const diaSemana = date.getDay();
    return diaSemana >= 1 && diaSemana <= 6; // Lunes a sábado
  };

  return (
    <div className="contenedor">
      <div className="center">
        <label className="label-date-picker">Seleccione la fecha a reservar:</label>
        <div className="date-picker-container">
          <DatePicker
            selected={fecha}
            onChange={onDateChange}
            minDate={hoy}
            maxDate={tresSemanasDespues}
            filterDate={esDiaHabil}
            className="date-picker"
          />
        </div>
        <br />
        <label className="label-time-picker">Seleccione la hora:</label>
        <div className="time-picker-container">
          <TimePicker
            onChange={onTimeChange}
            value={hora}
            clearIcon={null} // Quita el botón de limpiar
            clockIcon={null} // Quita el botón de reloj
            format="HH:mm" // Formato de 24 horas
            className="time-picker"
          />
        </div>
        <br />
        <input type="button" value="Mostrar Fecha y Hora" className="btn btn-primary" onClick={mostrarFechaHora}/>
      </div>
    </div>
  );
};

export default App;
