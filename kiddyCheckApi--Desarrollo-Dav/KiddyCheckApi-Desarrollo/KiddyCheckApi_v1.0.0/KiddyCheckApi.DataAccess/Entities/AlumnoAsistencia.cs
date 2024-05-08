using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.DataAccess.Entities
{
    public class AlumnoAsistencia
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdGrado { get; set; }
        public int IdGrupo { get; set; }
        public DateTime FechaAlt { get; set; }
    }
}
