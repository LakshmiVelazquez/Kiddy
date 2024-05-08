
using System.ComponentModel.DataAnnotations;

namespace KiddyCheckApi.DataAccess.Entities
{
    public class CalificacionAlumno
    {
        [Key]
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdMateria {  get; set; } 
        public string Bimestre {  get; set; }
        public int Calificacion {  get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
    }
}
