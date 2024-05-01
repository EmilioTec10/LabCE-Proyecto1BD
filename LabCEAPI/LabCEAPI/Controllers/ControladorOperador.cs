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
            try
            {
                bool accesoPermitido = operador.ingresar_operador(loginData.Email, loginData.Contraseña);

                if (accesoPermitido)
                {
                    return Ok("Inicio de sesión exitoso");
                }
                else
                {
                    return Unauthorized("Credenciales inválidas");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("marcar-entrada")]
        public IActionResult MarcarHoraEntrada(string email_op)
        {
            operador.marcar_hora_entrada(email_op);
            return Ok("Operador a marcado horas exitosamente");
        }

        [HttpPost("marcar-salida")]
        public IActionResult MarcarHorasSalida(string email_op)
        {
            operador.marcar_hora_salida(email_op);
            return Ok("Operador a marcado horas exitosamente");
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
            bool reservado = operador.reservar_laboratorio(reservaLabData.Nombre, reservaLabData.Dia, reservaLabData.HoraInicio, reservaLabData.HoraFin, reservaLabData.Descripcion, reservaLabData.Palmada, reservaLabData.email_est);
            // Verificar si la reserva se realizó correctamente
            if (reservado)
            {
                // Si se realizó correctamente, devolver un mensaje de éxito
                return Ok("Laboratorio reservado correctamente para el día " + reservaLabData.Dia.ToString() + " de " + reservaLabData.HoraInicio.ToString() + " hasta " + reservaLabData.HoraFin.ToString());
            }
            else
            {
                // Si no se pudo realizar la reserva, devolver un mensaje de error
                return BadRequest("No se pudo reservar el laboratorio. Por favor, inténtelo de nuevo.");
            }
        }

        [HttpGet("ver-activos-disponibles")]
        public IActionResult VerActivosDisponibles()
        {
            LinkedList<Activo> activosDisponibles = operador.ver_activos_disponibles();
            return Ok(activosDisponibles);
        }

        
        [HttpPost("solicitar-activo-estudiante")]
        public IActionResult SolicitarActivoEstudiante(string placa, string email_prof, string email_est)
        {
            // Aquí deberías obtener el profesor de la base de datos o de alguna otra fuente de datos
            operador.solicitar_activo_estudiante(placa, email_est, email_prof);
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
        public IActionResult PrestarActivoProfesor(string placa, string email_prof, string contra_prof)
        {
            bool prestado = operador.prestar_activo_profesor(placa, email_prof, contra_prof);

            if (prestado) {
                return Ok("Activo prestado a profesor correctamente");
            }
            else
            {
                return BadRequest("No se pudo prestar activo al profesor");
            }
            
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
        public IActionResult DevolucionActivoProfesor(string placa, string email_prof, string contraseña_prof)
        {
            bool devolucionExitosa = operador.devolucion_activo_profesor(placa, email_prof, contraseña_prof);

            if (devolucionExitosa)
            {
                return Ok("Devolución de activo por parte del profesor registrada correctamente");
            }
            else
            {
                return BadRequest("No se pudo realizar la devolución del activo. Por favor, verifique las credenciales del profesor.");
            }
        }


        [HttpPost("reportar-averia")]
        public IActionResult ReportarAveria(string placa, string descripcion, string email_prof, string email_est)
        {
            operador.reportar_averia_activo(placa, descripcion, email_prof, email_est);
            return Ok("Avería de activo reportada correctamente");
        }
        [HttpGet("ver-horas-laboradas")]
        public IActionResult VerHorasLaboradas(string email_op)
        {
            // Aquí deberías obtener las horas laboradas del operador de la base de datos o de alguna otra fuente de datos
            LinkedList<HorasLaboradas> horas_laboradas= operador.ver_horas_laboradas(email_op);
            return Ok(horas_laboradas);
        }

        public class OperadorDataRequest
        {
            public int Cedula { get; set; }
            public int Carne { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public DateTime FechaDeNacimiento { get; set; }
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
