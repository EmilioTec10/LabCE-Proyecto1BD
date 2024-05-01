using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LabCEAPI.Laboratorios;
using LabCEAPI.Reportes_de_Horas;
using LabCEAPI.Users;
using static LabCEAPI.Controllers.ControladorProfesor;
using LabCEAPI.Reservaciones;

namespace LabCEAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorAdmin : ControllerBase
    {
        Admin admin = new Admin();

        [HttpPost("ingresar")]
        public IActionResult IngresarAdmin(string email, string contraseña)
        {
            try
            {
                bool accesoPermitido = admin.ingresar_admin(email, contraseña);

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

        [HttpPost("registrar-profesor")]
        public IActionResult RegistrarProfesor([FromBody] ProfesorData profesorData)
        {

            DateOnly fecha_de_nacimiento = new DateOnly(profesorData.FechaDeNacimiento.Year, profesorData.FechaDeNacimiento.Month, profesorData.FechaDeNacimiento.Day);
            admin.registrar_profesor(profesorData.Cedula, profesorData.Nombre, profesorData.Apellidos, fecha_de_nacimiento, profesorData.Edad, profesorData.Email);
            return Ok("Profesor registrado exitosamente");
        }

        [HttpPut("modificar-profesor")]
        public IActionResult ModificarProfesor([FromBody] Profesor profesor)
        {
            admin.modificar_profesor(profesor);
            return Ok("Profesor modificado exitosamente");
        }

        [HttpDelete("eliminar-profesor")]
        public IActionResult EliminarProfesor(string email)
        {
    
            admin.eliminar_profesor(email);
            return Ok("Profesor eliminado exitosamente");
        }
        [HttpGet("ver-operadores-registrados")]
        public IActionResult VerOperadoresRegistrados()
        {
            LinkedList<Operador> operadoresRegistrados = admin.ver_todos_los_operadores_registrados();
            return Ok(operadoresRegistrados);
        }

        [HttpPost("aceptar-operador")]
        public IActionResult AceptarOperador(string email_op)
        {
            // Aquí deberías obtener el operador de la base de datos o de alguna otra fuente de datos
   
            admin.aceptar_operador(email_op);
            return Ok("Operador aceptado exitosamente");
        }

        [HttpPost("rechazar-operador")]
        public IActionResult RechazarOperador(string email_op)
        {

            admin.rechazar_operador(email_op);
            return Ok("Operador rechazado exitosamente");
        }

        [HttpPost("generar-nueva-contrasena")]
        public IActionResult GenerarNuevaContraseña(string email_admin)
        {
            admin.generar_nueva_contraseña(email_admin);
            return Ok("Nueva contraseña generada exitosamente y enviada por correo electrónico");
        }

        [HttpPost("generar-reporte")]
        public IActionResult GenerarReporte()
        {
            admin.generar_reporte();
            return Ok("Reporte generado exitosamente");
        }

        public class LoginData
        {
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        public class ProfesorData
        {
            public int Cedula { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public DateTime FechaDeNacimiento { get; set; }
            public int Edad { get; set; }
            public string Email { get; set; }
        }


        public class OperadorData
        {
            private int Cedula { get; set; }

            private int Carne { get; set; }

            private string Nombre { get; set; }

            private string Apellidos { get; set; }

            private DateOnly Fecha_de_nacimiento { get; set; }

            private int Edad;

            private string Email { get; set; }

            private string Contraseña { get; set; }

        }
    }
}
    
