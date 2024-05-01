import React, { useState } from "react";
import logo from "./logo.svg";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import "./index.css";

const App = () => {
  const [fecha, setFecha] = useState(new Date());

  const onChange = fecha => {
    setFecha(fecha);
  };

  const mostrarFecha = fecha => {
    alert(fecha);
  };

  const hoy = new Date();
  const tresSemanasDespues = new Date();
  tresSemanasDespues.setDate(hoy.getDate() + 21);

  const esDiaHabil = date => {
    const diaSemana = date.getDay();
    return diaSemana >= 1 && diaSemana <= 6; // Lunes a sábado
  };

  return (
    <>
      <div className="contenedor">
        <div className="center">
          <DatePicker
            selected={fecha}
            onChange={onChange}
            minDate={hoy}
            maxDate={tresSemanasDespues}
            filterDate={esDiaHabil}
          />
          <br /><br />
          <input type="button" value="Mostrar Fecha" className="btn btn-primary" onClick={() => mostrarFecha(fecha)}/>
        </div>
      </div>
    </>
  );
};

export default App;