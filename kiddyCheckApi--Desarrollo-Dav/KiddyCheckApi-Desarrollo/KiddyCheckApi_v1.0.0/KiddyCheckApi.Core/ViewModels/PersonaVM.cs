using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.Core.ViewModels
{
    public class PersonaVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Direccion { get; set; }
        public string? telefono { get; set; }
        public string? Correo { get; set; }
        public int? IdTutor { get; set; }
        public int? IdDocente { get; set; }
        public int? IdTipoPersona { get; set; }
        public int? Grado { get; set; }

        public int? Grupo { get; set; }
        public String nombreGrado { get; set; }
        public String nombreGrupo { get; set; }
        public String nombreDocente { get; set; }

        public String nombreTutor { get; set; }

        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno;
            }
        }
    }
}
