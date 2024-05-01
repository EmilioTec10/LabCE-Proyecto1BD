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
  const [horaInicio, setHoraInicio] = useState('07:00'); // Hora inicial
  const [horaFin, setHoraFin] = useState('07:30'); // Hora final
  const [errorHoraInicio, setErrorHoraInicio] = useState(false); // Estado para controlar el mensaje de error de horaInicio
  const [errorHoraFin, setErrorHoraFin] = useState(false); // Estado para controlar el mensaje de error de horaFin

  const onDateChange = fecha => {
    setFecha(fecha);
  };

  const onHoraInicioChange = horaInicio => {
    setHoraInicio(horaInicio);
  };

  const onHoraFinChange = horaFin => {
    setHoraFin(horaFin);
  };

  const mostrarFechaHora = () => {
    const horaInicioSplit = horaInicio.split(':');
    const horaInicioSeleccionada = parseInt(horaInicioSplit[0], 10);
    const minutosInicioSeleccionados = parseInt(horaInicioSplit[1], 10);

    const horaFinSplit = horaFin.split(':');
    const horaFinSeleccionada = parseInt(horaFinSplit[0], 10);
    const minutosFinSeleccionados = parseInt(horaFinSplit[1], 10);

    // Verificar si la hora de inicio cumple con las condiciones (de 7:00 a 20:30)
    if ((horaInicioSeleccionada >= 7 && horaInicioSeleccionada <= 20) &&
        (minutosInicioSeleccionados === 0 || minutosInicioSeleccionados === 30)) {
      setErrorHoraInicio(false);
    } else {
      setErrorHoraInicio(true);
    }

    // Verificar si la hora de fin cumple con las condiciones (de 7:30 a 21:00)
    if ((horaFinSeleccionada >= 7 && horaFinSeleccionada <= 21) &&
        (minutosFinSeleccionados === 0 || minutosFinSeleccionados === 30)) {
      setErrorHoraFin(false);
    } else {
      setErrorHoraFin(true);
    }

    // Si ambas horas cumplen con las condiciones, mostrar la fecha y hora seleccionadas
    if (!errorHoraInicio && !errorHoraFin) {
      const fechaHoraInicio = new Date(fecha);
      fechaHoraInicio.setHours(horaInicioSeleccionada);
      fechaHoraInicio.setMinutes(minutosInicioSeleccionados);

      const fechaHoraFin = new Date(fecha);
      fechaHoraFin.setHours(horaFinSeleccionada);
      fechaHoraFin.setMinutes(minutosFinSeleccionados);

      alert(`Fecha y hora de inicio: ${fechaHoraInicio}\nFecha y hora de fin: ${fechaHoraFin}`);
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
        <label className="label-time-picker">Seleccione la hora de inicio:</label>
        <div className="time-picker-container">
          <TimePicker
            onChange={onHoraInicioChange}
            value={horaInicio}
            clearIcon={null}
            clockIcon={null}
            format="HH:mm"
            clockClassName="custom-clock"
            disableClock={true}
            hourPlaceholder="hh"
            minutePlaceholder="mm"
            className="time-picker"
            minTime="07:00"
            maxTime="20:30"
            clockClassName="custom-clock"
            clockHourClassName="custom-clock-hour"
            clockMinuteClassName="custom-clock-minute"
            clockSecondClassName="custom-clock-second"
          />
          {errorHoraInicio && <p className="error-message">Por favor, ingrese una hora válida de inicio (de 07:00 a 20:30, y solo minutos 00 o 30).</p>}
        </div>
        <br />
        <label className="label-time-picker">Seleccione la hora de fin:</label>
        <div className="time-picker-container">
          <TimePicker
            onChange={onHoraFinChange}
            value={horaFin}
            clearIcon={null}
            clockIcon={null}
            format="HH:mm"
            clockClassName="custom-clock"
            disableClock={true}
            hourPlaceholder="hh"
            minutePlaceholder="mm"
            className="time-picker"
            minTime="07:30"
            maxTime="21:00"
            clockClassName="custom-clock"
            clockHourClassName="custom-clock-hour"
            clockMinuteClassName="custom-clock-minute"
            clockSecondClassName="custom-clock-second"
          />
          {errorHoraFin && <p className="error-message">Por favor, ingrese una hora válida de fin (de 07:30 a 21:00, y solo minutos 00 o 30).</p>}
        </div>
        <br />
        <input type="button" value="Mostrar Fecha y Hora" className="btn btn-primary" onClick={mostrarFechaHora}/>
      </div>
    </div>
  );
};

export default App;
