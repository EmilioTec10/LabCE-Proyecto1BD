using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using LabCEAPI.Users;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LabCEAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorProfesor : ControllerBase
    {
        Profesor profesor = new Profesor();

        [HttpPost("registrar")]
        public IActionResult RegistrarProfesor(
            [FromBody] ProfesorDataRequest profesor_data)
        {
            bool registrado =  profesor.registrar_profesor(profesor_data.Cedula, profesor_data.Nombre, profesor_data.Apellidos, profesor_data.Edad, profesor_data.Email, profesor_data.Contraseña, profesor_data.FechaDeNacimiento);

            if (registrado)
            {
                return Ok("PrIngreofesor registrado exitosamente");
            } 
            else 
            { 
                return BadRequest("No se pudo registrar el profesor en la base de datos"); 
            }
            
        }

        // Endpoint para que un profesor ingrese a la aplicación
        [HttpPost("ingresar")]
        public IActionResult IngresarProfesor([FromBody] LoginDataRequest login_data)
        {
            try
            {
                bool accesoPermitido = profesor.ingresar_profesor(login_data.Email, login_data.Contraseña);

                if (accesoPermitido)
                {
                    profesor.email = login_data.Email;
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

        [HttpPost("generar-nueva-contrasena")]
        public IActionResult GenerarNuevaContrasena(string email)
        {
            bool generada =  profesor.generar_nueva_contraseña(email);
            if (generada)
            {
                return Ok("Nueva contraseña generada exitosamente y enviada por correo electrónico");
            }
            else
            {
                return BadRequest("No se pudo generar la nueva contraseña");
            }
            
        }

        [HttpGet("ver-activos-prestados")]
        public IActionResult VerActivosPrestados()
        {
            LinkedList<Activo> activos_prestados = profesor.ver_activos_prestados();
            return Ok(activos_prestados);
        }

        [HttpPost("solicitar-prestamo")]
        public IActionResult SolicitarPrestamoActivo(string placa, string email_prof)
        {
            profesor.solicitar_prestamo_activo(placa, email_prof);
            return Ok("Solicitud de préstamo de activo realizada correctamente");
        }

        [HttpGet("ver-prestamos-pendientes/{email:string}")]
        public IActionResult VerPrestamosPendientes(string email)
        {
            LinkedList<PrestamoActivo> prestamos_pendientes = profesor.ver_prestamos_pendientes(email);
            return Ok(prestamos_pendientes);
        }

        [HttpPost("aceptar-solicitud-prestamo")]
        public IActionResult AceptarSolicitudPrestamo([FromBody] PrestamoData prestamo_data)
        {

            profesor.aceptar_solicitud_prestamo(prestamo_data.ID_activo, prestamo_data.email_estudiante, prestamo_data.email_prof);
            return Ok("Solicitud de préstamo de activo aceptada correctamente");
        }

        [HttpPost("rechazar-solicitud-prestamo")]
        public IActionResult RechazarSolicitudPrestamo([FromBody] PrestamoData prestamo_data)
        { 
            profesor.rechazar_solicitud_prestamo(prestamo_data.ID_activo, prestamo_data.email_estudiante, prestamo_data.email_prof);
            return Ok("Solicitud de préstamo de activo rechazada correctamente");
        }

        [HttpGet("ver-laboratorios-disponibles")]
        public IActionResult VerLaboratoriosDisponibles()
        {
            LinkedList<Laboratorio> laboratoriosDisponibles = profesor.ver_labs_disponibles();
            return Ok(laboratoriosDisponibles);
        }

        [HttpPost("reservar-laboratorio")]
        public IActionResult ReservarLaboratorio([FromBody] ReservaLabData reserva_data)
        {
            // Crear una instancia de Laboratorio utilizando el nombre proporcionado en reserva_data
            Laboratorio lab = new Laboratorio(reserva_data.Nombre);

            // Invocar el método reservar_laboratorio de la clase Profesor para realizar la reserva
            bool reservado = profesor.reservar_laboratorio(lab, reserva_data.Dia, reserva_data.HoraInicio, reserva_data.HoraFin, reserva_data.Descripcion, reserva_data.Palmada, reserva_data.email_prof);

            // Verificar si la reserva se realizó correctamente
            if (reservado)
            {
                // Si se realizó correctamente, devolver un mensaje de éxito
                return Ok("Laboratorio reservado correctamente para el día " + reserva_data.Dia.ToString() + " de " + reserva_data.HoraInicio.ToString() + " hasta " + reserva_data.HoraFin.ToString());
            }
            else
            {
                // Si no se pudo realizar la reserva, devolver un mensaje de error
                return BadRequest("No se pudo reservar el laboratorio. Por favor, inténtelo de nuevo.");
            }
        }


        public class ProfesorDataRequest
        {
            public int Cedula { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public DateTime FechaDeNacimiento { get; set; }
            public int Edad { get; set; }
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        public class LoginDataRequest
        {
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        public class PrestamoData
        {
            public string ID_activo { get; set; }
            public string email_estudiante {  get; set; }
            public string email_prof {  get; set; }
        }

        public class ActivoData
        {
            public string tipo { get; set; }
            public string marca { get; set; }
            public string placa { get; set; }
            public string estado { get; set; }
            public string lab {  get; set; }
            public DateTime purchase_date { get; set; }
            public bool necesita_aprobador {  get; set; }
        }

        public class ReservaLabData
        {
            public string Nombre { get; set; } // Nombre del laboratorio
            public DateTime Dia { get; set; } // Fecha de la reserva
            public DateTime HoraInicio { get; set; }
            public DateTime HoraFin { get; set; }
            public string Descripcion {  get; set; }
            public bool Palmada { get; set; }
            public string email_est {  get; set; }
            public string email_prof {  get; set; }
        }

    }
}
