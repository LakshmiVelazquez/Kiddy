using KiddyCheckApi.DataAccess.Entities;
using KiddyCheckApi.DataAccess.Response;
using KiddyCheckApi.DataAccess.Response.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.DataAccess.IRepositories
{
    public interface IPersonaRepository : IBaseRepository<Personas>
    {
        Task<Personas> ExistsByNombreUsuario(string nombreUsuario, ILogger logger);
        Task<GenericResponse<Personas>> AgregarPersona(Personas persona, ILogger logger);
        Task<GenericResponse<Personas>> ActualizarPersona(Personas persona, ILogger logger);
        Task<GenericResponse<Personas>> EliminarPersona(int id, int userMod, ILogger logger);
        Task<GenericResponse<List<Personas>>> ObtenerPersonas(ILogger Logger);
        Task<GenericResponse<List<Personas>>> ObtenerPersonaTipo(int tipo, ILogger logger);
        Task<DatosCorreoDTO> DatosCorreo(int id, int idMaestro, ILogger Logger);
        Task<Personas> ObtenerTutor(int tutor, ILogger logger);
        Task<GenericResponse<Personas>> Asistencia(Personas persona);
        Task<GenericResponse<List<DatosAsistenciasDTO>>> ExportarAsistencias(int idGrado, int idGrupo, int idMaestro, ILogger logger);
        Task<Personas> obtenerDocente(int IdDocente, ILogger logger);
    }
}
