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
        private int cedula { get; set; }

        private int carne { get; set; }

        private string nombre { get; set; }

        private string apellidos {  get; set; }

        private DateOnly fecha_de_nacimiento { get; set; }

        private int edad;

        private string email {  get; set; }

        private string contraseña {  get; set; }

        LinkedList<HorasLaboradas> HorasLaboradas { get; set; }



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
            
        }

        //Metodo para ver todos los laboratorios que estan disponibles en este momento
        public LinkedList<Laboratorio> ver_labs_disponibles()
        {
            return Laboratorio.labs;
        }

        //Metodo para reservar un laboratorio en una fecha determinada
        public ReservarLab reservar_laboratorio(Laboratorio lab, DateOnly dia, DateTime hora)
        {
            ReservarLab reservarLab = new ReservarLab(lab, this, dia, hora);
            return reservarLab;
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
            activo.dañado = true;
            activo.dellate_dañado = detalle;
            return activo;
        }

        public void registrar_entrada()
        {

        }

        public void registrar_salida()
        {

        }

        //Metodo para ver las horas laboradas del operador
        public ReporteOperador ver_horas_laboradas(DateTime dia, Operador operador, string detalles, LinkedList<HorasLaboradas> horasLaboradas)
        {
            ReporteOperador reporteOperador = new ReporteOperador(dia, operador, detalles, horasLaboradas);
            return reporteOperador;
        }
    }
}
