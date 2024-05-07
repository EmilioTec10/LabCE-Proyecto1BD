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
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Primitives;

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
            bool registrado = operador.registrar_operador(operadorData.Cedula, operadorData.Carne, operadorData.Nombre, operadorData.Apellidos, operadorData.FechaDeNacimiento, operadorData.Edad, operadorData.Email, operadorData.Contraseña);
            if (registrado)
            {
                return Ok("Operador registrado exitosamente");
            }
            else
            {
                return BadRequest("No se pudo registrar correctamente el operador");
            }
            
        }

        [HttpPost("ingresar")]
        public IActionResult IngresarOperador([FromBody] LoginDataOperador loginData)
        {  
            try
            {
                bool accesoPermitido = operador.ingresar_operador(loginData.Email, loginData.Contraseña);

                if (accesoPermitido)
                {
                    operador.email = loginData.Email;
                    return Ok("Inicio de sesión exitoso");
                }
                else
                {
                    return Unauthorized("Sus credenciales son inválidas o no ha sido aprobado por un administrador");
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
            bool marcado = operador.marcar_hora_entrada(email_op);
            if (marcado)
            {
                return Ok("Operador a marcado horas exitosamente");
            }
            else
            {
                return BadRequest("No se pudo marcar horas correctamente");
            }
            
        }

        [HttpPost("marcar-salida")]
        public IActionResult MarcarHorasSalida(string email_op)
        {
            bool marcado = operador.marcar_hora_salida(email_op);
            if (marcado)
            {
                return Ok("Operador a marcado horas exitosamente");
            }
            else
            {
                return BadRequest("No se pudo marcar horas correctamente");
            }
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
            bool solicitado = operador.solicitar_activo_estudiante(placa, email_est, email_prof);
            if (solicitado)
            {
                return Ok("Solicitud de activo para estudiante realizada correctamente");
            }
            else
            {
                return BadRequest("No se pudo solicitar el activo para el estudiante");
            }
        }

        [HttpGet("ver-prestamos-aprobados")]
        public IActionResult VerPrestamosAprobados()
        {
            try
            {
                LinkedList<PrestamoActivo> prestamosAprobados = operador.ver_prestamos_aprobados();
                return Ok(prestamosAprobados);
            }
            catch (Exception ex)
            {
                // Manejar el error de alguna manera, aquí simplemente lo devolvemos como respuesta
                return StatusCode(500, $"Error al obtener los prestamos aprobados: {ex.Message}");
            }
        }

        [HttpPost("prestar-activo-estudiante")]
        public IActionResult PrestarActivoEstudiante(string email, string contraseña)
        {
            try
            {
                bool prestamoExitoso = operador.prestar_activo_estudiante(email, contraseña);
                if (prestamoExitoso)
                {
                    return Ok("El activo fue prestado exitosamente.");
                }
                else
                {
                    return BadRequest("No se pudo realizar el préstamo del activo. Verifique sus credenciales.");
                }
            }
            catch (Exception ex)
            {
                // Manejar el error de alguna manera, aquí simplemente lo devolvemos como respuesta
                return StatusCode(500, $"Error al realizar el préstamo del activo: {ex.Message}");
            }
        }

        [HttpGet("prestamos-pendientes-estudiantes")]
        public IActionResult VerPrestamosPendientesEstudiantes()
        {
            try
            {
                LinkedList<PrestamoActivo> prestamosPendientes = operador.ver_prestamos_pendientes_estudiantes();
                return Ok(prestamosPendientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los préstamos pendientes: {ex.Message}");
            }
        }

        [HttpPost("prestar-activo-profesor")]
        public IActionResult PrestarActivoProfesor([FromBody] DevolucionProfesorData profesorData)
        {
            bool prestado = operador.prestar_activo_profesor(profesorData.placa, profesorData.email_prof, profesorData.contraseña_prof);

            if (prestado) {
                return Ok("Activo prestado a profesor correctamente");
            }
            else
            {
                return BadRequest("No se pudo prestar activo al profesor");
            }
            
        }

        [HttpGet("prestamos-pendientes-profesores")]
        public IActionResult VerPrestamosPendientesProfesores()
        {
            try
            {
                LinkedList<PrestamoActivo> prestamosPendientes = operador.ver_prestamos_pendientes_profesores();
                return Ok(prestamosPendientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los préstamos pendientes: {ex.Message}");
            }
        }

        [HttpGet("ver-activos-prestados")]
        public IActionResult VerActivosPrestados()
        {
            LinkedList<Activo> activos_prestados = operador.ver_activos_prestados();
            Console.WriteLine(activos_prestados);
            return Ok(activos_prestados);
        }

        [HttpPost("devolucion-activo-estudiante")]
        public IActionResult DevolucionActivoEstudiante([FromBody] DevolucionActivoData devolucionActivoData)
        {
            bool devolucionExitosa = operador.devolucion_activo_estudiante(devolucionActivoData.placa, devolucionActivoData.email_est, devolucionActivoData.email_op, devolucionActivoData.contraseña_op);
            if (devolucionExitosa)
            {
                return Ok("Devolución de activo por parte del estudiante registrada correctamente");
            }
            else
            {
                return BadRequest("No se pudo realizar la devolución del activo. Por favor, verifique las credenciales del operador.");
            }
            return Ok("Devolución de activo por parte del estudiante registrada correctamente");
        }


        [HttpPost("devolucion-activo-profesor")]
        public IActionResult DevolucionActivoProfesor([FromBody] DevolucionProfesorData profesorData)
        {
            bool devolucionExitosa = operador.devolucion_activo_profesor(profesorData.placa, profesorData.email_prof, profesorData.contraseña_prof);

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
        public IActionResult VerHorasLaboradas(string email)
        {
            // Aquí deberías obtener las horas laboradas del operador de la base de datos o de alguna otra fuente de datos
            LinkedList<HorasLaboradas> horas_laboradas= operador.ver_horas_laboradas(email);
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
            public string placa {  get; set; }
            public string email_est {  get; set; }
            public string email_op {  get; set; }
            public string contraseña_op {  get; set; }
        }

        public class DevolucionProfesorData
        {
            public string placa { get; set; }
            public string email_prof { get; set; }
            public string contraseña_prof { get; set; }
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
