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
        [HttpPost("registrar")]
        public IActionResult RegistrarProfesor(
            [FromBody] ProfesorData data)
        {
            // Aquí deberías implementar la lógica para registrar un profesor en la base de datos
            // utilizando los datos recibidos en la solicitud
            // data.Cedula, data.Nombre, data.Apellidos, data.FechaDeNacimiento, data.Edad, data.Email, data.Contraseña
            // Puedes llamar al método registrar_profesor de la clase Profesor
            // y luego retornar un resultado adecuado
            return Ok("Profesor registrado exitosamente");
        }

        [HttpPost("ingresar")]
        public IActionResult IngresarProfesor(
            [FromBody] LoginData data)
        {
            // Aquí deberías implementar la lógica para que un profesor inicie sesión en la aplicación
            // utilizando los datos recibidos en la solicitud
            // data.Email, data.Contraseña
            // Puedes llamar al método ingresar_profesor de la clase Profesor
            // y luego retornar un resultado adecuado
            return Ok("Profesor ingresado exitosamente");
        }

        [HttpPost("generar-nueva-contrasena")]
        public IActionResult GenerarNuevaContrasena()
        {
            Profesor profesor = new Profesor();
            profesor.generar_nueva_contraseña();
            return Ok("Nueva contraseña generada exitosamente y enviada por correo electrónico");
        }

        [HttpGet("ver-activos-prestados")]
        public IActionResult VerActivosPrestados()
        {
            // Aquí deberías implementar la lógica para mostrar todos los activos
            // que actualmente están prestados
            // Puedes llamar al método ver_activos_prestados de la clase Profesor
            // y luego retornar un resultado adecuado
            return Ok("Lista de activos prestados");
        }

        // Otros métodos similares para las demás operaciones de la API
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
}
