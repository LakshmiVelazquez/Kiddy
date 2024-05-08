using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.DataAccess.Response.DTO
{
    public class DatosAsistenciasDTO
    {
        public string NombreAlumno { get; set; }
        public string Grado { get; set; }
        public string Grupo { get; set; }
        public string FechaAsistencia { get; set; }
    }
}
