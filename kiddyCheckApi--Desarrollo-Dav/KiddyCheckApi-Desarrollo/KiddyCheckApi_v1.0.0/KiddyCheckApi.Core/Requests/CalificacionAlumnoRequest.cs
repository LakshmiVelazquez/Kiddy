namespace KiddyCheckApi.Core.Requests
{
    public class CalificasionAlumnoRequest
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdMateria { get; set; }
        public string Bimestre { get; set; }
        public int Calificacion { get; set; }
    }
}
