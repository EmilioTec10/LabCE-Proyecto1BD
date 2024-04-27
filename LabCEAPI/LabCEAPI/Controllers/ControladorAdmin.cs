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
        public IActionResult IngresarAdmin([FromBody] LoginData loginData)
        {
            admin.ingresar_admin(loginData.Email, loginData.Contraseña);
            return Ok("Admin ingresado exitosamente");
        }

        [HttpPost("registrar-profesor")]
        public IActionResult RegistrarProfesor([FromBody] ProfesorData profesorData)
        {
            admin.registrar_profesor(profesorData.Cedula, profesorData.Nombre, profesorData.Apellidos, profesorData.FechaDeNacimiento, profesorData.Edad, profesorData.Email);
            return Ok("Profesor registrado exitosamente");
        }

        [HttpPut("modificar-profesor")]
        public IActionResult ModificarProfesor([FromBody] ModificarProfesorData modificarProfesorData)
        {
            admin.modificar_profesor(modificarProfesorData.profesor, modificarProfesorData.NewEmail, modificarProfesorData.NewContraseña);
            return Ok("Profesor modificado exitosamente");
        }

        [HttpDelete("eliminar-profesor")]
        public IActionResult EliminarProfesor([FromBody] ProfesorData profesorData)
        {
            Profesor profesor = new Profesor();
            profesor.email = profesorData.Email;
            admin.eliminar_profesor(profesor);
            return Ok("Profesor eliminado exitosamente");
        }
        [HttpGet("ver-operadores-registrados")]
        public IActionResult VerOperadoresRegistrados()
        {
            LinkedList<Operador> operadoresRegistrados = admin.ver_todos_los_operadores_registrados();
            return Ok(operadoresRegistrados);
        }

        [HttpPost("aceptar-operador")]
        public IActionResult AceptarOperador([FromBody] OperadorData operadorData)
        {
            // Aquí deberías obtener el operador de la base de datos o de alguna otra fuente de datos
            Operador operador = new Operador();
            admin.aceptar_operador(operador);
            return Ok("Operador aceptado exitosamente");
        }

        [HttpPost("rechazar-operador")]
        public IActionResult RechazarOperador([FromBody] OperadorData operadorData)
        {
            // Aquí deberías obtener el operador de la base de datos o de alguna otra fuente de datos
            Operador operador = new Operador();
            admin.rechazar_operador(operador);
            return Ok("Operador rechazado exitosamente");
        }

        [HttpPost("generar-nueva-contrasena")]
        public IActionResult GenerarNuevaContraseña()
        {
            admin.generar_nueva_contraseña();
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
            public DateOnly FechaDeNacimiento { get; set; }
            public int Edad { get; set; }
            public string Email { get; set; }
        }

        public class ModificarProfesorData
        {
            public Profesor profesor { get; set; }
            public string NewEmail { get; set; }
            public string NewContraseña { get; set; }
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
    
