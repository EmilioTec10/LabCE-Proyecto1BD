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
  const [informacion, setInformacion] = useState(""); // Estado para almacenar la información a mostrar
  const [mostrarTabla, setMostrarTabla] = useState(false); // Estado para controlar la visualización de la tabla

  const onDateChange = fecha => {
    setFecha(fecha);
  };

  const onHoraInicioChange = horaInicio => {
    setHoraInicio(horaInicio);
    // Al cambiar la hora de inicio, reiniciamos el mensaje de error
    setErrorHoraInicio(false);
  };

  const onHoraFinChange = horaFin => {
    setHoraFin(horaFin);
    // Al cambiar la hora de fin, reiniciamos el mensaje de error
    setErrorHoraFin(false);
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

      // Actualizar el estado para mostrar la información en el label
      setInformacion(`Fecha seleccionada: ${fecha.toLocaleDateString()} | Hora de inicio: ${horaInicio} | Hora de fin: ${horaFin}`);

      // Mostrar la tabla
      setMostrarTabla(true);
    }
  };

  const hoy = new Date();
  const tresSemanasDespues = new Date();
  tresSemanasDespues.setDate(hoy.getDate() + 21);

  const esDiaHabil = date => {
    const diaSemana = date.getDay();
    return diaSemana >= 1 && diaSemana <= 6; // Lunes a sábado
  };

  const seleccionarLaboratorio = (laboratorio) => {
    alert(`Información seleccionada:\nFecha: ${fecha.toLocaleDateString()} | Hora de inicio: ${horaInicio} | Hora de fin: ${horaFin} | Laboratorio: ${laboratorio}`);
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
        {(errorHoraInicio || errorHoraFin) ? (
          <p className="error-message">Por favor, corrija la hora seleccionada.</p>
        ) : (
          <input type="button" value="Mostrar Fecha y Hora" className="btn btn-primary" onClick={mostrarFechaHora}/>
        )}
        <br />
        {mostrarTabla && !errorHoraInicio && !errorHoraFin && (
          <table className="table">
            <thead>
              <tr>
                <th>Sin Laboratorio</th>
                <th>Capacidad</th>
                <th>Computadores</th>
                <th>Activos</th>
                <th>Facilidades</th>
                <th>Seleccionar</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>f2-07</td>
                <td>30</td>
                <td>15</td>
                <td>proyector</td>
                <td>proector hacia la pared</td>
                <td><button onClick={() => seleccionarLaboratorio('f2-07')}>Seleccionar</button></td>
              </tr>
              <tr>
                <td>f2-08</td>
                <td>14</td>
                <td>14</td>
                <td>proyy</td>
                <td>pantalla</td>
                <td><button onClick={() => seleccionarLaboratorio('f2-08')}>Seleccionar</button></td>
              </tr>
              {/* Agrega más filas aquí si es necesario */}
            </tbody>
          </table>
        )}
        <br />
        <label>{informacion}</label>
      </div>
    </div>
  );
};

export default App;
