using KiddyCheckApi.Core.ViewModels;
using KiddyCheckApi.DataAccess.Entities;
using KiddyCheckApi.DataAccess.IRepositories;
using KiddyCheckApi.DataAccess;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using KiddyCheckApi.Core.Helpers;
using KiddyCheckApi.Core.Response;
using KiddyCheckApi.Core.Requests;

namespace KiddyCheckApi.Core.Services
{
 
    public class CalificacionAlumnoService
    {
        #region <--- Variables --->
        private readonly IBaseRepository<CalificacionAlumno> _calificacionAlumnoRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IBaseRepository<Grupos> _GruposRepository;
        private ILogger<GrupoService> _logger;
        private readonly IBaseRepository<Grados> _GradosRepository;
        public readonly KiddyCheckDbContext _context;
        #endregion

        #region <--- Constructor --->
        public CalificacionAlumnoService(IBaseRepository<Grupos> repository, ILogger<GrupoService> logger,
            IBaseRepository<Grados> gradoRepository, 
            KiddyCheckDbContext context, IBaseRepository<CalificacionAlumno> calificacionAlumnoRepository, IPersonaRepository personaRepository)
        {
            _GruposRepository = repository;
            _logger = logger;
            _GradosRepository = gradoRepository;
            _context = context;
            _calificacionAlumnoRepository = calificacionAlumnoRepository;
            _personaRepository = personaRepository;
        }
        #endregion

        public async Task<List<CalificacionAlumnoVM>> GetCalificacionAlumno()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var alumnos = await _personaRepository.ObtenerPersonaTipo(4, _logger);
                var materias = await _context.Materias.ToListAsync();
                var califiacionAlumno = await _calificacionAlumnoRepository.GetAll(_logger);
                var grados = await _GradosRepository.GetAll(_logger);
                var grupos = await _GruposRepository.GetAll(_logger);

                var calificacionVM = (from cal in califiacionAlumno
                                      join materia in materias on cal.IdMateria equals materia.Id into materiasJoin
                                      from materia in materiasJoin.DefaultIfEmpty()
                                      join alumno in alumnos.Data on cal.IdAlumno equals alumno.Id into alumnosJoin
                                      from alumno in alumnosJoin.DefaultIfEmpty()
                                      join grado in grados on alumno?.Grado equals grado.Id into gradosJoin
                                      from grado in gradosJoin.DefaultIfEmpty()
                                      join grupo in grupos on alumno?.Grupo equals grupo.Id into gruposJoin
                                      from grupo in gruposJoin.DefaultIfEmpty()
                                      select new CalificacionAlumnoVM
                                      {
                                          Id = cal.Id,
                                          Nombre = alumno?.Nombre,
                                          IdAlumno = cal.IdAlumno,
                                          ApellidoPaterno = alumno?.ApellidoPaterno,
                                          ApellidoMaterno = alumno?.ApellidoMaterno,
                                          NumBimestreGrado = grado != null ? Convert.ToInt16(grado.Bimestre) : (short)0,
                                          NombreMateria = materia?.Nombre,
                                          Grado = grado?.Grado,
                                          Grupo = grupo?.Nombre,
                                          Bimestre = cal.Bimestre,
                                          Calificacion = cal.Calificacion
                                      }).ToList();




                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return calificacionVM;


            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<List<object>> CalcularProbabilidades()
        {
            var listProbabilidad = new List<object>();
            var califiacionAlumno = await GetCalificacionAlumno();

            var AlumnoAgrupado = califiacionAlumno.GroupBy(cal => cal.Nombre);

            foreach (var alumno in AlumnoAgrupado)
            {
                int maxCal = alumno.Count() * 10;

                var calificacionAnterior = 0;

                foreach (var cal in alumno)
                {
                    cal.Calificacion = cal.Calificacion + calificacionAnterior;
                    calificacionAnterior = cal.Calificacion;
                }

                double promedio = (alumno.Last().Calificacion * 10) / maxCal;
                string probabilidad;

                if (promedio < 5)
                {
                    probabilidad = "baja";
                }
                else if (promedio >= 5 && promedio <= 7) // Cambiado el || a && para corregir la condición
                {
                    probabilidad = "media";
                }
                else
                {
                    probabilidad = "alta";
                }

                foreach (var cal in alumno)
                {
                    cal.Promedio = promedio;
                    cal.Probabilidad = probabilidad;
                }

                var ultimoCalificacion = alumno.Last();
                listProbabilidad.Add(ultimoCalificacion);
            }

            return listProbabilidad;
        }

        public async Task<List<object>> CalcularProbabilidadePorIdAlumno(int id)
        {
  
            var listProbabilidad = new List<object>();
            var califiacionAlumno = await GetCalificacionAlumno();

            var AlumnoAgrupado = califiacionAlumno.Where(x => x.IdAlumno == id).ToList().GroupBy(cal => cal.Nombre);

            foreach (var alumno in AlumnoAgrupado)
            {
                var calificacionAnterior = 0;
                int maxCal = alumno.Count() * 10;
                foreach (var cal in alumno)
                {
                    cal.Calificacion = cal.Calificacion + calificacionAnterior ;
                    calificacionAnterior = cal.Calificacion;
                }

                double promedio = (alumno.Last().Calificacion * 10) / maxCal;
                foreach (var cal in alumno)
                {
                    cal.Promedio = promedio;
                }

                string probabilidad = string.Empty;
                if (promedio < 5)
                {
                    if(promedio <= 0)
                    {
                        probabilidad = "Caso perdido";
                    }
                    else
                    {
                        probabilidad = "baja";
                    }
                   
                }
                else if (promedio >= 5 && promedio <= 7)
                {
                    probabilidad = "media";
                }
                else if (promedio > 7)
                {
                    probabilidad = "alta";
                }
 

                foreach (var cal in alumno)
                {
                    cal.Probabilidad = probabilidad;
                }

                var ultimoCalificacion = alumno.Last();
                listProbabilidad.Add(ultimoCalificacion);
            }

            return listProbabilidad;
        }

        public async Task<List<BimestreCalificacionVM>> GetByIdCal(int id, string bimestre)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var calificacionAlumno = await _context.CalificacionAlumno
                    .Where(x => x.IdAlumno == id && x.Bimestre == bimestre)
                    .ToListAsync();

                var materias = await _context.Materias.ToListAsync();

                var joinCalificacionPorBimestre = (from cal in calificacionAlumno
                                                   join mat in materias on cal.IdMateria equals mat.Id
                                                   select new BimestreCalificacionVM
                                                   {
                                                     IdCalificacion = cal.Id,
                                                     Materia = mat.Nombre,
                                                     Calificacion = cal.Calificacion
                                                   }).ToList();

                return joinCalificacionPorBimestre;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> AgregarCalificacion(CalificasionAlumnoRequest request, int idUsuario)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var entidad = new CalificacionAlumno
                {
                    Id = 0,
                    IdAlumno = request.IdAlumno,
                    IdMateria = request.IdMateria,
                    Bimestre = request.Bimestre,
                    Calificacion = request.Calificacion,
                    FechaAlt = HoraHelper.GetHora("mx"),
                    UserAlt = idUsuario
                };

                var result = await _calificacionAlumnoRepository.Add(entidad, _logger);

                if (result != null)
                {
                    response.Message = "Calificacion agregada correctamente";
                    response.CreatedId = result.Id.ToString();
                    response.Success = true;
                }
                else
                {
                    response.Message = "Error al agregar calificacion";
                    response.Success = false;
                }

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<GenericResponse> EliminarCalificasion(int id)
        {

            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();
                var entity = await _calificacionAlumnoRepository.GetById(id, _logger);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "la calificasoio no existe";
                    return response;
                }
                var result = await _calificacionAlumnoRepository.Delete(id, _logger);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "la calificasion fue eliminada correctamente";
                }
                else
                {
                    response.Success = false;
                    response.Message = "La calificasion no pudo ser eliminada";
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<GenericResponse> EditarCalificacion(BimestreCalificacionVM cal)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();
                var entity = await _calificacionAlumnoRepository.GetById(cal.IdCalificacion, _logger);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "la calificasoio no existe";
                    return response;
                }
                entity.Calificacion = cal.Calificacion;
                var result = await _calificacionAlumnoRepository.Update(entity, _logger);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "la calificasion fue editada correctamente";
                }
                else
                {
                    response.Success = false;
                    response.Message = "La calificasion no pudo ser editada";
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
