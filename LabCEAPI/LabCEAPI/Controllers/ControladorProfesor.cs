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
            [FromBody] ProfesorData profesor_data)
        {
            profesor.registrar_profesor(profesor_data.Cedula, profesor_data.Nombre, profesor_data.Apellidos, profesor_data.FechaDeNacimiento, profesor_data.Edad, profesor_data.Email, profesor_data.Contraseña);
            return Ok("Profesor registrado exitosamente");
        }

        [HttpPost("ingresar")]
        public IActionResult IngresarProfesor(
            [FromBody] LoginData login_data)
        {
            profesor.ingresar_profesor(login_data.Email, login_data.Contraseña);
            return Ok("Profesor ingresado exitosamente");
        }

        [HttpPost("generar-nueva-contrasena")]
        public IActionResult GenerarNuevaContrasena()
        {
            profesor.generar_nueva_contraseña();
            return Ok("Nueva contraseña generada exitosamente y enviada por correo electrónico");
        }

        [HttpGet("ver-activos-prestados")]
        public IActionResult VerActivosPrestados()
        {
            Activo activo = new Activo("nose", "nissan", "123", true, "");
            Activo.activos_prestados.AddFirst(activo);
            Activo.activos_prestados.AddFirst(new Activo("sise", "toyota", "321", true, "noe"));
            LinkedList<Activo> activos_prestados = profesor.ver_activos_prestados();
            return Ok(activos_prestados);
        }

        [HttpPost("solicitar-prestamo")]
        public IActionResult SolicitarPrestamoActivo([FromBody] ActivoData activo_data)
        {
            Activo activo = new Activo(activo_data.Nombre, activo_data.Marca, activo_data.Modelo, activo_data.Disponible, activo_data.PrestadoA);
            profesor.solicitar_prestamo_activo(activo);
            return Ok("Solicitud de préstamo de activo realizada correctamente");
        }

        [HttpPost("aceptar-solicitud-prestamo")]
        public IActionResult AceptarSolicitudPrestamo([FromBody] PrestamoData prestamo_data)
        {
            PrestamoActivo prestamo = new PrestamoActivo(new Activo(prestamo_data.Activo.Nombre, prestamo_data.Activo.Marca, prestamo_data.Activo.Modelo, prestamo_data.Activo.Disponible, prestamo_data.Activo.PrestadoA), new Profesor(), DateOnly.FromDateTime(DateTime.Now), DateTime.Now);
            profesor.aceptar_solicitud_prestamo(prestamo);
            return Ok("Solicitud de préstamo de activo aceptada correctamente");
        }

        [HttpPost("rechazar-solicitud-prestamo")]
        public IActionResult RechazarSolicitudPrestamo([FromBody] PrestamoData prestamo_data)
        {
            PrestamoActivo prestamo = new PrestamoActivo(new Activo(prestamo_data.Activo.Nombre, prestamo_data.Activo.Marca, prestamo_data.Activo.Modelo, prestamo_data.Activo.Disponible, prestamo_data.Activo.PrestadoA), new Profesor(), DateOnly.FromDateTime(DateTime.Now), DateTime.Now);
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
            ReservarLab reserva = profesor.reservar_laboratorio(lab, reserva_data.Dia, reserva_data.Hora);
            return Ok("Laboratorio reservado correctamente para el día " + reserva_data.Dia.ToString() + " a las " + reserva_data.Hora.ToString());
        }


        public class ProfesorData
        {
            public int Cedula { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public DateOnly FechaDeNacimiento { get; set; }
            public int Edad { get; set; }
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        public class LoginData
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
            public string Modelo { get; set; }
            public bool Disponible { get; set; }
            public string PrestadoA { get; set; }
        }

        public class ReservaLabData
        {
            public string Nombre { get; set; } // Nombre del laboratorio
            public DateOnly Dia { get; set; } // Fecha de la reserva
            public DateTime Hora { get; set; } // Hora de la reserva
        }

    }
}
