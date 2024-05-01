using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using LabCEAPI.Users;

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
            profesor.registrar_profesor(profesor_data.Cedula, profesor_data.Nombre, profesor_data.Apellidos, profesor_data.Edad, profesor_data.Email, profesor_data.Contraseña);
            return Ok("Profesor registrado exitosamente");
        }

        // Endpoint para que un profesor ingrese a la aplicación
        [HttpPost("ingresar")]
        public IActionResult IngresarProfesor(string email, string contraseña)
        {
            try
            {
                bool accesoPermitido = profesor.ingresar_profesor(email, contraseña);

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

        [HttpPost("generar-nueva-contrasena")]
        public IActionResult GenerarNuevaContrasena(string email)
        {
            profesor.generar_nueva_contraseña(email);
            return Ok("Nueva contraseña generada exitosamente y enviada por correo electrónico");
        }

        [HttpGet("ver-activos-prestados")]
        public IActionResult VerActivosPrestados()
        {
            LinkedList<Activo> activos_prestados = profesor.ver_activos_prestados();
            return Ok(activos_prestados);
        }

        [HttpPost("solicitar-prestamo")]
        public IActionResult SolicitarPrestamoActivo(string placa)
        {
            profesor.solicitar_prestamo_activo(placa);
            return Ok("Solicitud de préstamo de activo realizada correctamente");
        }

        [HttpPost("aceptar-solicitud-prestamo")]
        public IActionResult AceptarSolicitudPrestamo([FromBody] PrestamoData prestamo_data)
        {
            PrestamoActivo prestamo = new PrestamoActivo(new Activo(prestamo_data.Activo.Nombre, prestamo_data.Activo.Marca, prestamo_data.Activo.estado, prestamo_data.Activo.placa), new Profesor(), DateOnly.FromDateTime(DateTime.Now), DateTime.Now);
            profesor.aceptar_solicitud_prestamo(prestamo);
            return Ok("Solicitud de préstamo de activo aceptada correctamente");
        }

        [HttpPost("rechazar-solicitud-prestamo")]
        public IActionResult RechazarSolicitudPrestamo([FromBody] PrestamoData prestamo_data)
        {
            PrestamoActivo prestamo = new PrestamoActivo(new Activo(prestamo_data.Activo.Nombre, prestamo_data.Activo.Marca, prestamo_data.Activo.estado, prestamo_data.Activo.placa), new Profesor(), DateOnly.FromDateTime(DateTime.Now), DateTime.Now);
            profesor.rechazar_solicitud_prestamo(prestamo);
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
            Laboratorio lab = new Laboratorio(reserva_data.Nombre);
            ReservarLab reserva = profesor.reservar_laboratorio(lab, reserva_data.Dia, reserva_data.HoraInicio, reserva_data.HoraFin);
            return Ok("Laboratorio reservado correctamente para el día " + reserva_data.Dia.ToString() + " de " + reserva_data.HoraInicio.ToString() + " hasta " + reserva_data.HoraFin.ToString());
        }


        public class ProfesorDataRequest
        {
            public int Cedula { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
           // public DateOnly FechaDeNacimiento { get; set; }
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
            public ActivoData Activo { get; set; }
        }

        public class ActivoData
        {
            public string Nombre { get; set; }
            public string Marca { get; set; }
            public string placa { get; set; }
            public string estado { get; set; }
        }

        public class ReservaLabData
        {
            public string Nombre { get; set; } // Nombre del laboratorio
            public DateOnly Dia { get; set; } // Fecha de la reserva
            public DateTime HoraInicio { get; set; } 
            public DateTime HoraFin { get; set; } 
        }

    }
}
