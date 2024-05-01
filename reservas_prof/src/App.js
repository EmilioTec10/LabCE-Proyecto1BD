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
  const [errorHora, setErrorHora] = useState(false); // Estado para controlar el mensaje de error

  const onDateChange = fecha => {
    setFecha(fecha);
  };

  const onTimeChange = hora => {
    setHora(hora);
  };

  const mostrarFechaHora = () => {
    const horaSplit = hora.split(':');
    const horaSeleccionada = parseInt(horaSplit[0], 10);
    const minutosSeleccionados = parseInt(horaSplit[1], 10);

    // Verificar si la hora cumple con las condiciones (de 7:00 a 20:30, y solo minutos 00 o 30)
    if ((horaSeleccionada >= 7 && horaSeleccionada <= 20) &&
        (minutosSeleccionados === 0 || minutosSeleccionados === 30)) {
      const fechaHora = new Date(fecha);
      fechaHora.setHours(horaSeleccionada);
      fechaHora.setMinutes(minutosSeleccionados);
      alert(fechaHora);
      setErrorHora(false); // Restablecer el estado de error
    } else {
      setErrorHora(true); // Mostrar mensaje de error
    }
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
            clockClassName="custom-clock"
            disableClock={true} // Deshabilita el reloj analógico
            hourPlaceholder="hh"
            minutePlaceholder="mm"
            className="time-picker"
            minTime="07:00" // Hora mínima
            maxTime="20:30" // Hora máxima
            clockClassName="custom-clock"
            clockHourClassName="custom-clock-hour"
            clockMinuteClassName="custom-clock-minute"
            clockSecondClassName="custom-clock-second"
          />
          {errorHora && <p className="error-message">Por favor, ingrese una hora válida (de 07:00 a 20:30, y solo minutos 00 o 30).</p>}
        </div>
        <br />
        <input type="button" value="Mostrar Fecha y Hora" className="btn btn-primary" onClick={mostrarFechaHora}/>
      </div>
    </div>
  );
};

export default App;
