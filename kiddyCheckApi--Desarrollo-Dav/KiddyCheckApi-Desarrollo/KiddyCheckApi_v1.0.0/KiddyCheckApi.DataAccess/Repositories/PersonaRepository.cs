using KiddyCheckApi.DataAccess.Entities;
using KiddyCheckApi.DataAccess.IRepositories;
using KiddyCheckApi.DataAccess.Response;
using KiddyCheckApi.DataAccess.Response.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.DataAccess.Repositories
{
    public class PersonaRepository : BaseRepository<Personas>, IPersonaRepository
    {
        private readonly KiddyCheckDbContext _context;
        public PersonaRepository(KiddyCheckDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<GenericResponse<Personas>> AgregarPersona(Personas persona, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Personas>();

                var add = _context.Add(persona);

                var addResult = await _context.SaveChangesAsync();

                if (addResult > 0)
                {

                    await transaction.CommitAsync();
                    response.Success = true;
                    response.CreatedId = add.Entity.Id.ToString();
                    response.Data = add.Entity;

                }
                else
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = "No se pudo agregar la persona";
                }

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }


        //Obtener todos las personas
        public async Task<GenericResponse<List<Personas>>> ObtenerPersonas(ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<Personas>>();

                var personas = await EntitySet.AsNoTracking().Where(x => x.Activo == true).ToListAsync();

                response.Success = true;
                response.Data = personas;

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<Personas> ExistsByNombreUsuario(string correo, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var user = EntitySet.AsNoTracking().Where(x => x.Correo == correo && x.Activo == true).FirstOrDefault();

                if (user != null)
                {
                    logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                    return user;
                }

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return null;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Obtener tutor
        public async Task<Personas> ObtenerTutor( int tutorid,ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<Personas>>();

                var personas =  EntitySet.AsNoTracking().Where(x => x.Activo == true && x.Id== tutorid).FirstOrDefault();

               if(personas != null)
                {
                    logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                    
                    return personas;

                }
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");


                return null;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        //Obtener docente
        public async Task<Personas> obtenerDocente(int idDocente, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<Personas>>();

                var docemtes = EntitySet.AsNoTracking().Where(x => x.Activo == true && x.Id == idDocente).FirstOrDefault();

                if (docemtes != null)
                {
                    logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return docemtes;

                }
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");


                return null;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        //Obtener persona x tipo
        public async Task<GenericResponse<List<Personas>>> ObtenerPersonaTipo(int tipo, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<Personas>>();

                var personas = await EntitySet.AsNoTracking().Where(x => x.Activo == true && x.IdTipoPersona == tipo).ToListAsync();

                response.Success = true;
                response.Data = personas;

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        public async Task<GenericResponse<Personas>> ActualizarPersona(Personas persona, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Personas>();

                var usr = await base.GetById(persona.Id, logger);

                if (usr != null)
                {
                    usr.Nombre = persona.Nombre;
                    usr.ApellidoPaterno = persona.ApellidoPaterno;
                    usr.ApellidoMaterno = persona.ApellidoMaterno;
                    usr.PasswordHash = persona.PasswordHash;
                    usr.PasswordSalt = persona.PasswordSalt;
                    usr.Direccion = persona.Direccion;
                    usr.telefono = persona.telefono;
                    usr.Correo = persona.Correo;
                    usr.Grado = persona.Grado;
                    usr.Grupo = persona.Grupo; ;
                    usr.UserUpd = persona.UserUpd;
                    usr.FechaUpd = persona.FechaUpd;
                    usr.Activo = persona.Activo;

                    var update = _context.Update(usr);

                    var updateResult = await _context.SaveChangesAsync();

                    if (updateResult == 1)
                    {

                        await transaction.CommitAsync();
                        response.Success = true;
                        response.UpdatedId = usr.Id.ToString();
                        response.Data = usr;
                        response.Message = "Se modifico la persona correctamente";

                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se pudo modificar la persona";
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = "No se encontro la persona";
                }

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Eliminar Personas
        public async Task<GenericResponse<Personas>> EliminarPersona(int id, int userMod, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Personas>();

                var usr = await base.GetById(id, logger);

                if (usr != null)
                {
                    usr.Activo = false;
                    usr.FechaUpd = DateTime.UtcNow.AddHours(-6);
                    usr.UserUpd = userMod;

                    var update = _context.Update(usr);

                    var updateResult = await _context.SaveChangesAsync();

                    if (updateResult == 1)
                    {
                        await transaction.CommitAsync();
                        response.Success = true;
                        response.UpdatedId = usr.Id.ToString();
                        response.Data = usr;
                        response.Message = "Se elimino la persona correctamente";

                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se pudo eliminar la persona";
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = "No se encontro el usuario";
                }

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<DatosCorreoDTO> DatosCorreo(int id, int idMaestro, ILogger Logger)
        {
            try
            {
                var datos = await (from alumno in _context.Personas
                                   join tutor in _context.Personas on alumno.IdTutor equals tutor.Id
                                   where alumno.Id == id
                                   select new DatosCorreoDTO
                                   {
                                       NombreAlumno = alumno.Nombre + " " + alumno.ApellidoPaterno + " " + alumno.ApellidoMaterno,
                                       NombreTutor = tutor.Nombre + " " + tutor.ApellidoPaterno + " " + tutor.ApellidoMaterno,
                                       Correo = tutor.Correo,
                                       NombreMaestro = _context.Personas.Where(x=>x.Id == idMaestro).Select(x=> (x.Nombre + " " + x.ApellidoPaterno + " " + x.ApellidoMaterno)).FirstOrDefault()
                                   }).FirstOrDefaultAsync();

                return datos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GenericResponse<Personas>> Asistencia(Personas persona)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var response = new GenericResponse<Personas>();

                var dia = DateTime.Now.Day;
                var mes = DateTime.Now.Month;
                var año = DateTime.Now.Year;

                var result = await _context.AlumnoAsistencia.AnyAsync(x=>x.IdAlumno == persona.Id && x.FechaAlt.Day == dia && x.FechaAlt.Month == mes && x.FechaAlt.Year == año);

                if (result == false)
                {
                    var asistencia = new AlumnoAsistencia();
                    asistencia.FechaAlt = DateTime.Now;
                    asistencia.IdGrado = persona.Grado.Value;
                    asistencia.IdGrupo = persona.Grupo.Value;
                    asistencia.IdAlumno = persona.Id;

                    _context.Add(asistencia);

                    var res = await _context.SaveChangesAsync();
                    if (res == 1)
                    {
                        await transaction.CommitAsync();
                        response.Success = true;
                        response.Message = "Asistencia tomada correctamente.";
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se pudo tomar la asistencia, intentelo nuevamente.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "La asistencia ya ha sido tomada.";
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GenericResponse<List<DatosAsistenciasDTO>>> ExportarAsistencias(int idGrado, int idGrupo, int idMaestro, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<DatosAsistenciasDTO>>();

                var dia = DateTime.Now.Day;
                var mes = DateTime.Now.Month;
                var año = DateTime.Now.Year;

                var datos =await (from personas in _context.Personas
                            join grupo in _context.Grupos on personas.Grupo equals grupo.Id
                            join grado in _context.Grados on personas.Grado equals grado.Id
                            join asistencia in _context.AlumnoAsistencia on personas.Id equals asistencia.IdAlumno into asistenciasTemp
                            from asistencia in asistenciasTemp.DefaultIfEmpty()
                            where personas.IdDocente == idMaestro && personas.Grupo == idGrupo && personas.Grado == idGrado
                                 && (asistencia == null || (asistencia.FechaAlt.Day == dia && asistencia.FechaAlt.Month == mes && asistencia.FechaAlt.Year == año))
                            select new DatosAsistenciasDTO
                            {
                                NombreAlumno = personas.Nombre + " " + personas.ApellidoPaterno + " " + personas.ApellidoMaterno,
                                Grado = grado.Grado,
                                Grupo = grupo.Nombre,
                                FechaAsistencia = asistencia != null ?
                                                  asistencia.FechaAlt.ToString("dddd") + " " + asistencia.FechaAlt.Day + " de " + asistencia.FechaAlt.ToString("MMMM") + " del " + asistencia.FechaAlt.Year :
                                                  "No asistio."
                            }).ToListAsync();


                response.Success = true;
                response.Data = datos;

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
    }
}
