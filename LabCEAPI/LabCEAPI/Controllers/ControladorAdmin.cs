using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LabCEAPI.Laboratorios;
using LabCEAPI.Reportes_de_Horas;
using LabCEAPI.Users;
using static LabCEAPI.Controllers.ControladorProfesor;
using LabCEAPI.Reservaciones;
using LabCEAPI.Prestamos;

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

        // Devuelve la lista de todos los activos
        [HttpGet("ver-activos")]
        public IActionResult VerActivos()
        {
            LinkedList<Activo> activos = admin.ver_activos();
            return Ok(activos);
        }

        [HttpPost("crear-activo")]
        public IActionResult CrearActivo([FromBody] ActivoData activoData)
        {

            Activo activo = new Activo(activoData.tipo, activoData.marca, activoData.estado, activoData.placa, activoData.lab, activoData.purchase_date, activoData.necesita_aprobador);

            if (activo == null)
            {
                return BadRequest("El activo proporcionado es nulo.");
            }

            bool insercionExitosa = admin.crear_activo(activo);

            if (insercionExitosa)
            {
                return Ok("El activo se creó exitosamente.");
            }
            else
            {
                return StatusCode(500, "Error al crear el activo.");
            }
        }

        [HttpPut("modificar-activo")]
        public IActionResult ModificarActivo([FromBody] ActivoData activoData)
        {
            Activo activo = new Activo(activoData.tipo, activoData.marca, activoData.estado, activoData.placa, activoData.lab, activoData.purchase_date, activoData.necesita_aprobador);

            if (activo == null)
            {
                return BadRequest("El activo proporcionado es nulo.");
            }

            bool modificacionExitosa = admin.modificar_activo(activo);

            if (modificacionExitosa)
            {
                return Ok("El activo se modificó exitosamente.");
            }
            else
            {
                return StatusCode(500, "Error al modificar el activo.");
            }
        }

        // Devuelve la lista de operadores que se han registrado pero no se a revisado por un administrador
        [HttpGet("ver-profesores-registrados")]
        public IActionResult VerProfesoresRegistrados()
        {
            LinkedList<Profesor> profesoresRegistrados = admin.ver_profesores_registrados();
            return Ok(profesoresRegistrados);
        }

        [HttpPut("modificar-profesor")]
        public IActionResult ModificarProfesor([FromBody] ProfesorData profesorData)
        {
            Profesor profesor = new Profesor(profesorData.Cedula, profesorData.Nombre, profesorData.Apellidos, new DateOnly(profesorData.FechaDeNacimiento.Year, profesorData.FechaDeNacimiento.Month, profesorData.FechaDeNacimiento.Day), profesorData.Email);
            bool modificacion_profesor = admin.modificar_profesor(profesor);
            if (modificacion_profesor)
            {
                return Ok("El profesor se modificó exitosamente.");
            }
            else
            {
                return StatusCode(500, "Error al modificar el profesor.");
            }
        }

        [HttpDelete("eliminar-profesor")]
        public IActionResult EliminarProfesor(string email)
        {
    
            bool eliminado = admin.eliminar_profesor(email);
            if (eliminado)
            {
                return Ok("Profesor eliminado exitosamente");
            } else
            {
                return BadRequest("No se pudo eliminar el profesor de la base de datos");
            }
            
        }

        // Devuelve la lista de operadores que se han registrado pero no se a revisado por un administrador
        [HttpGet("ver-operadores-registrados")]
        public IActionResult VerOperadoresRegistrados()
        {
            LinkedList<Operador> operadoresRegistrados = admin.ver_todos_los_operadores_registrados();
            return Ok(operadoresRegistrados);
        }

        // Acepta el operador registrado en el sistema
        [HttpPost("aceptar-operador")]
        public IActionResult AceptarOperador(string email_op)
        {
            // Aquí deberías obtener el operador de la base de datos o de alguna otra fuente de datos
   
            bool aceptado = admin.aceptar_operador(email_op);

            if (aceptado)
            {
                return Ok("Operador aceptado exitosamente");
            }
            else
            {
                return BadRequest("No se pudo aceptar el operador");
            }
            
        }
        
        //Rechaza el operador registrado en el sistema
        [HttpPost("rechazar-operador")]
        public IActionResult RechazarOperador(string email_op)
        {

            bool rechazado = admin.rechazar_operador(email_op);

            if (rechazado)
            {
                return Ok("Operador rechazado exitosamente");
            }
            else
            {
                return BadRequest("No se pudo rechazado el operador");
            }
        }


        [HttpPost("generar-nueva-contrasena")]
        public IActionResult GenerarNuevaContraseña(string email_admin)
        {
            bool generada = admin.generar_nueva_contraseña(email_admin);

            if (generada)
            {
                return Ok("Nueva contraseña generada exitosamente y enviada por correo electrónico");
            }
            else
            {
                return BadRequest("No se pudo generar la contraseña nueva");
            }
            
        }

        [HttpGet("generar-reporte")]
        public IActionResult GenerarReporte()
        {
            LinkedList<ReporteOperador> reporte_operadores =  admin.generar_reporte();
            return Ok(reporte_operadores);
        }

        public class LoginData
        {
            public string Email { get; set; }
            public string Contraseña { get; set; }
        }

        public class ProfesorData
        {
            public string Cedula { get; set; }
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
    
