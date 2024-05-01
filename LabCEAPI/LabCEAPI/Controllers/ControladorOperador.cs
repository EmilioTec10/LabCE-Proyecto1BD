using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using LabCEAPI.Laboratorios;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using LabCEAPI.Users;
using static LabCEAPI.Controllers.ControladorAdmin;
using static LabCEAPI.Controllers.ControladorProfesor;
using LabCEAPI.NewFolder;

namespace LabCEAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OperadorController : ControllerBase
    {
        Operador operador = new Operador();

        [HttpPost("registrar")]
        public IActionResult RegistrarOperador([FromBody] OperadorDataRequest operadorData)
        {
            operador.registrar_operador(operadorData.Cedula, operadorData.Carne, operadorData.Nombre, operadorData.Apellidos, operadorData.FechaDeNacimiento, operadorData.Edad, operadorData.Email, operadorData.Contraseña);
            return Ok("Operador registrado exitosamente");
        }

        [HttpPost("ingresar")]
        public IActionResult IngresarOperador([FromBody] LoginDataOperador loginData)
        {
            operador.ingresar_operador(loginData.Email, loginData.Contraseña);
            return Ok("Operador ingresado exitosamente");
        }

        [HttpPost("salir")]
        public IActionResult SalirOperador()
        {
            operador.salir_operador();
            return Ok("Operador ha salido exitosamente");
        }

        [HttpGet("ver-labs-disponibles")]
        public IActionResult VerLabsDisponibles()
        {
            LinkedList<Laboratorio> labsDisponibles = operador.ver_labs_disponibles();
            return Ok(labsDisponibles);
        }

        [HttpPost("reservar-laboratorio")]
        public IActionResult ReservarLaboratorio([FromBody] ReservaLabData reservaLabData)
        {
            Laboratorio lab = new Laboratorio(reservaLabData.Nombre);
            //ReservarLab reservaLab = operador.reservar_laboratorio(lab, reservaLabData.Dia, reservaLabData.HoraInicio, reservaLabData.HoraFin);
            return Ok("Laboratorio reservado correctamente para el día " + reservaLabData.Dia.ToString() + " de " + reservaLabData.HoraInicio.ToString() + " hasta las " + reservaLabData.HoraFin.ToString());
        }

        [HttpGet("ver-activos-disponibles")]
        public IActionResult VerActivosDisponibles()
        {
            LinkedList<Activo> activosDisponibles = operador.ver_activos_disponibles();
            return Ok(activosDisponibles);
        }

        
        [HttpPost("solicitar-activo-estudiante")]
        public IActionResult SolicitarActivoEstudiante([FromBody] SolicitarActivoData solicitarActivoData)
        {
            // Aquí deberías obtener el profesor de la base de datos o de alguna otra fuente de datos
            Profesor profesor = new Profesor();
            Activo activo = new Activo(solicitarActivoData.Nombre, solicitarActivoData.Marca, solicitarActivoData.Modelo, solicitarActivoData.estado);
            PrestamoActivo prestamoActivo = operador.solicitar_activo_estudiante(activo, profesor);
            return Ok("Solicitud de activo para estudiante realizada correctamente");
        }
        

        [HttpPost("prestar-activo-estudiante")]
        public IActionResult PrestarActivoEstudiante([FromBody] ActivoDataOperador activoData)
        {
            Activo activo = new Activo(activoData.Nombre, activoData.Marca, activoData.Modelo, activoData.estado);
            operador.prestar_activo_estudiante(activo, activoData.ContraseñaOperador);
            return Ok("Activo prestado a estudiante correctamente");
        }

        [HttpPost("prestar-activo-profesor")]
        public IActionResult PrestarActivoProfesor([FromBody] ActivoDataOperador activoData)
        {
            Activo activo = new Activo(activoData.Nombre, activoData.Marca, activoData.Modelo, activoData.estado);
            operador.prestar_activo_profesor(activo, activoData.ContraseñaProfesor);
            return Ok("Activo prestado a profesor correctamente");
        }

        [HttpGet("ver-activos-prestados")]
        public IActionResult VerActivosPrestados()
        {
            LinkedList<Activo> activos_prestados = Activo.activos_prestados;
            return Ok(activos_prestados);
        }
        [HttpPost("devolucion-activo-estudiante")]
        public IActionResult DevolucionActivoEstudiante([FromBody] DevolucionActivoData devolucionActivoData)
        {
            Activo activo = new Activo(devolucionActivoData.Activo.Nombre, devolucionActivoData.Activo.Marca, devolucionActivoData.Activo.estado, devolucionActivoData.Activo.placa);
            operador.devolucion_activo_estudiante(activo, devolucionActivoData.ContraseñaOperador);
            return Ok("Devolución de activo por parte del estudiante registrada correctamente");
        }

        [HttpPost("devolucion-activo-profesor")]
        public IActionResult DevolucionActivoProfesor([FromBody] DevolucionActivoData devolucionActivoData)
        {
            Activo activo = new Activo(devolucionActivoData.Activo.Nombre, devolucionActivoData.Activo.Marca, devolucionActivoData.Activo.estado, devolucionActivoData.Activo.placa);
            operador.devolucion_activo_profesor(activo, devolucionActivoData.ContraseñaOperador);
            return Ok("Devolución de activo por parte del profesor registrada correctamente");
        }

        [HttpPost("reportar-averia")]
        public IActionResult ReportarAveria([FromBody] AveriaActivoData averiaActivoData)
        {
            Activo activo = new Activo(averiaActivoData.Activo.Nombre, averiaActivoData.Activo.Marca, averiaActivoData.Activo.estado, averiaActivoData.Activo.placa);
            operador.reportar_averia_activo(activo, averiaActivoData.Detalle);
            return Ok("Avería de activo reportada correctamente");
        }
        [HttpPost("ver-horas-laboradas")]
        public IActionResult VerHorasLaboradas([FromBody] VerHorasLaboradasData verHorasLaboradasData)
        {
            // Aquí deberías obtener las horas laboradas del operador de la base de datos o de alguna otra fuente de datos
            return Ok(verHorasLaboradasData);
        }

        public class OperadorDataRequest
        {
            public int Cedula { get; set; }
            public int Carne { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public DateOnly FechaDeNacimiento { get; set; }
            public int Edad { get; set; }
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        public class LoginDataOperador
        {
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        public class ActivoDataOperador
        {
            public string Nombre { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string estado { get; set; }
            public string ContraseñaProfesor { get; set; }
            public string ContraseñaOperador { get; set; }
        }

        public class SolicitarActivoData
        {

            public string Nombre { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Placa {  get; set; }

            public string estado { get; set; }
        }

        public class PrestamoActivoData
        {
            public ActivoData Activo { get; set; }
            public string ContraseñaOperador { get; set; }
        }

        public class ReportarAveriaData
        {
            // Agrega las propiedades necesarias para representar los datos de una avería
        }

        public class DevolucionActivoData
        {
            public ActivoData Activo { get; set; }
            public string ContraseñaOperador { get; set; }
        }

        public class AveriaActivoData
        {
            public ActivoData Activo { get; set; }
            public string Detalle { get; set; }
        }

        public class VerHorasLaboradasData
        {
            public DateOnly fecha { get; set; }
            public DateTime hora_ingreso { get; set; }
            public DateTime hora_salida { get; set; }
            public float horas_trabajadas { get; set; }
        }

    } 
}
