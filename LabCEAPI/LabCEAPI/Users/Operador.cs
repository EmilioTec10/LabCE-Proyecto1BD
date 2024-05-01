using LabCEAPI.Laboratorios;
using LabCEAPI.NewFolder;
using LabCEAPI.Prestamos;
using LabCEAPI.Reservaciones;
using System;
using System.Runtime.CompilerServices;

namespace LabCEAPI.Users
{
    public class Operador
    {
        public static LinkedList<Operador> operadores = new LinkedList<Operador>();
        public string cedula { get; set; }

        public string carne { get; set; }

        public string nombre { get; set; }

        public string apellidos {  get; set; }

        public DateTime fecha_de_nacimiento { get; set; }

        private int edad;

        public string email {  get; set; }

        public string contraseña { get; set; }

        HorasLaboradas actual_hora { get; set; }

        LinkedList<HorasLaboradas> HorasLaboradas { get; set; }

        public Operador (string cedula, string carne, string nombre, string apellidos, DateTime fecha_de_nacimiento, string email)
        {
            this.cedula = cedula;
            this.carne = carne;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.fecha_de_nacimiento = fecha_de_nacimiento;
            this.email = email;
        }

        public Operador()
        {

        }


        //Metodo que registra un nuevo operador y lo guarda en la base de datos
        public void registrar_operador(
            int cedula,
            int carne,
            string nombre,
            string apellidos,
            DateOnly fecha_de_nacimiento,
            int edad,
            string email,
            string contraseña)
        {

        }

        //Metodo para ingresar como operador en la aplicacion
        public void ingresar_operador(string email, string contraseña)
        {
            actual_hora = new HorasLaboradas(DateTime.Now, DateOnly.FromDateTime(DateTime.Now));
        }

        public void salir_operador()
        {
            actual_hora.hora_salida = DateTime.Now;
            actual_hora.horas_trabajadas = (actual_hora.hora_salida.Hour - actual_hora.hora_ingreso.Hour) + ((actual_hora.hora_salida.Minute - actual_hora.hora_ingreso.Minute)/60);
            HorasLaboradas.AddFirst(actual_hora);
        }


        //Metodo para ver todos los laboratorios que estan disponibles en este momento
        public LinkedList<Laboratorio> ver_labs_disponibles()
        {
            return Laboratorio.labs;
        }

        //Metodo para reservar un laboratorio en una fecha determinada
        public void reservar_laboratorio(Laboratorio lab, DateTime dia, DateTime hora_inicio, DateTime hora_fin)
        {
           // ReservarLab reservarLab = new ReservarLab(lab, this, dia, hora_inicio, hora_fin);
           // return reservarLab;
        }

        //Metodo para ver los activos que actualmente estan disponibles
        public LinkedList<Activo> ver_activos_disponibles()
        {
            return Activo.activos_disponibles;
        }

        //Metodo que solicita a un profesor el prestamo de un activo a un estudiante
        public PrestamoActivo solicitar_activo_estudiante(Activo activo, Profesor profesor)
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
            DateTime ahora = DateTime.Now;
            PrestamoActivo prestamoActivo = new PrestamoActivo(activo, profesor, fechaActual, ahora);
            return prestamoActivo;
        }

        //Metodo que presta el activo al estudiante
        public void prestar_activo_estudiante(Activo activo, string contraseña_operador)
        {
            Activo.activos_disponibles.Remove(activo);
            
        }

        //Metodo que presta un activo a un profesor
        public void prestar_activo_profesor(Activo activo, string contraseña_profesor)
        {
            Activo.activos_disponibles.Remove(activo);
            Activo.activos_prestados.AddFirst(activo);
        }

        //Metodo que deja ver los activos prestados
        public LinkedList<Activo> ver_activos_prestados()
        {
            return Activo.activos_prestados;
        }

        //Metodo para registrar la devolucion de un activo por parte de un estudiante
        public RetornoActivo devolucion_activo_estudiante(Activo activo, string contraseña_operador)
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
            DateTime ahora = DateTime.Now;
            RetornoActivo retornoActivo = new RetornoActivo(activo, fechaActual, ahora);
            return retornoActivo;
        }

        //Metodo para registrar la devolucion de un activo por parte de un profesor
        public RetornoActivo devolucion_activo_profesor(Activo activo, string contraseña_profesor)
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
            DateTime ahora = DateTime.Now;
            RetornoActivo retornoActivo = new RetornoActivo(activo, fechaActual, ahora);
            return retornoActivo;
        }

        //Metodo para reportar una averia de un activo
        public Activo reportar_averia_activo(Activo activo, string detalle)
        {
            activo.estado = "Dañado";
            activo.dellate_dañado = detalle;
            return activo;
        }

        //Metodo para ver las horas laboradas del operador
        public ReporteOperador ver_horas_laboradas(DateTime dia, Operador operador, string detalles, LinkedList<HorasLaboradas> horasLaboradas)
        {
            ReporteOperador reporteOperador = new ReporteOperador(dia, operador, detalles, horasLaboradas);
            return reporteOperador;
        }
    }
}
