
namespace KiddyCheckApi.Core.ViewModels
{
    public class CalificacionAlumnoVM
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public int NumBimestreGrado {  get; set; }
        public string Grado { get; set; }
        public string Grupo { get; set; }
        public string NombreMateria { get; set; }
        public string Bimestre { get; set; }
        public int Calificacion { get; set; }
        public double Promedio { get; set; }
        public string Probabilidad {  get; set; }
    }
}
