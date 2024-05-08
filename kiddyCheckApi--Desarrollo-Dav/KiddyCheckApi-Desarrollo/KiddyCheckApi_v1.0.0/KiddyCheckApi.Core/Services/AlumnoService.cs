using KiddyCheckApi.Core.Helpers;
using KiddyCheckApi.Core.Mappers;
using KiddyCheckApi.Core.Requests;
using KiddyCheckApi.Core.Response;
using KiddyCheckApi.Core.ViewModels;
using KiddyCheckApi.DataAccess;
using KiddyCheckApi.DataAccess.Entities;
using KiddyCheckApi.DataAccess.IRepositories;
using KiddyCheckApi.DataAccess.Response.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.Core.Services
{
    public class AlumnoService
    {
        #region Variables
        private readonly IPersonaRepository _repository;
        private ILogger<AlumnoService> _logger;
        private IConfiguration _configuration;
        private KiddyCheckDbContext _context;

        #endregion

        #region Constructor
        public AlumnoService(IPersonaRepository repository, ILogger<AlumnoService> logger,IConfiguration configuration,
            KiddyCheckDbContext context)
        {
            _repository = repository;
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }
        #endregion

        #region Metodos
        public async Task<GenericResponse> EnviarCorreo(int id, int idMaestro)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var persona = await _repository.GetById(id, _logger);

                if (persona != null)
                {
                    if (persona.IdTipoPersona == 4)
                    {
                        var datos = await _repository.DatosCorreo(id, idMaestro, _logger);

                        if (datos != null)
                        {
                            // Obtener credenciales de correo
                            var host = _configuration["EmailSettings:Host"];
                            var port = _configuration["EmailSettings:Port"];
                            var usuario = _configuration["EmailSettings:UserName"];
                            var contraseña = _configuration["EmailSettings:Password"];
                            string nombrePlantilla = "plantilla_correo.html";

                            var asistencia = await Asistencia(persona);

                            if (asistencia.Success)
                            {
                                // Enviar correo
                                MailHelper.EnviarEmail(host, port, usuario, contraseña, datos, nombrePlantilla);

                                response.Message = "Correo enviado al tutor correctamente";
                                response.Success = true;
                            }
                            else
                            {
                                response.Message = asistencia.Message;
                                response.Success = false;
                            }

                        }
                        else
                        {
                            response.Message = "Error alumno no encotrado";
                            response.Success = false;
                        }
                    }
                    else
                    {
                        response.Message = "La persona no es de tipo alumno";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Message = "El alumno no existe.";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> Asistencia(Personas persona)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var result = await _repository.Asistencia(persona);

                if (result.Success)
                {
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = result.Message;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        public async Task<int> AsistenciaDia()
        {
            try
            {

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var dia = DateTime.Now.Day;
                var mes = DateTime.Now.Month;
                var año = DateTime.Now.Year;
                var result = await _context.AlumnoAsistencia.Where(x => x.FechaAlt.Day == dia && x.FechaAlt.Month == mes && x.FechaAlt.Year == año).ToListAsync();
          


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        public async Task<int> AsistenciaSemana()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var today = DateTime.Today;
                var firstDayOfWeek = GetFirstDayOfWeek(today);
                var lastDayOfWeek = firstDayOfWeek.AddDays(7); // Sumamos 6 días para obtener el sábado

                var result = await _context.AlumnoAsistencia
                    .Where(x => x.FechaAlt >= firstDayOfWeek && x.FechaAlt <= lastDayOfWeek)
                    .ToListAsync();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return result.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        private DateTime GetFirstDayOfWeek(DateTime date)
        {
            // Calculamos el primer día de la semana restando el valor numérico del día de la semana actual
            // a la fecha actual. Esto nos da el domingo de la semana actual.
            var firstDayOfWeek = date.AddDays(-(int)date.DayOfWeek);

            return firstDayOfWeek;
        }


        public async Task<List<DatosAsistenciasVM>> ExportarAsistencias(ExportarAsistenciaRequest request, int idMaestro)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var datos = new List<DatosAsistenciasVM>();

                var result = await _repository.ExportarAsistencias(request.IdGrado, request.IdGrupo, idMaestro, _logger);

                if (result.Success)
                {
                     datos = AppMapper.Map<List<DatosAsistenciasDTO>, List<DatosAsistenciasVM>>(result.Data);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
