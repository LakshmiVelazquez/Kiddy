using KiddyCheckApi.Core.Helpers;
using KiddyCheckApi.Core.Mappers;
using KiddyCheckApi.Core.Requests;
using KiddyCheckApi.Core.ViewModels;
using KiddyCheckApi.DataAccess.Entities;
using KiddyCheckApi.Core.Response;
using KiddyCheckApi.DataAccess.IRepositories;
using KiddyCheckApi.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace KiddyCheckApi.Core.Services
{
    public class PersonasService
    {
        #region<--Variables-->
        private readonly IPersonaRepository _personaRepository;
        private readonly IBaseRepository<Grados> _Gradorepository;
        private readonly IBaseRepository<Grupos> _GruposRepository;
        private ILogger<PersonasService> _logger;
      
        private readonly JwtSettings _jwtSettings;
        private IConfiguration _configuration;
        #endregion

        #region <-- Constructor -->
        public PersonasService(IPersonaRepository personaRepository,
            IBaseRepository<Grados> Gradorepository,
            IBaseRepository<Grupos> repository,
            ILogger<PersonasService> logger, JwtSettings jwtSettings,
            IConfiguration configuration)
        {
            _personaRepository = personaRepository;
            _logger = logger;
            _jwtSettings = jwtSettings;
            _configuration = configuration;
            _Gradorepository = Gradorepository;
            _GruposRepository = repository;
        }
        #endregion

        #region <-- Metodos -->

        public async Task<GenericResponse> AgregarPersona(PersonaRequest request, int usrAlta)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();



                var persona = AppMapper.Map<PersonaRequest, Personas>(request);

                if (request.IdTipoPersona == 1 || request.IdTipoPersona == 2)
                {
                    CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    persona.PasswordHash = passwordHash;
                    persona.PasswordSalt = passwordSalt;
                }
                persona.FechaAlt = DateTime.Now;
                persona.UserAlt = usrAlta;
                persona.Activo = true;

                var result = await _personaRepository.AgregarPersona(persona, _logger);

                if (result.Success)
                {
                    response.Message = "Persona agregado correctamente";
                    response.Success = true;
                    response.CreatedId = result.CreatedId;
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

        public async Task<GenericResponse> ActualizarPersona(PersonaRequest request, int usrMod)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var persona = await _personaRepository.GetById(request.Id, _logger);

                if (persona == null)
                {
                    response.Message = "El usuario no existe";
                    response.Success = false;

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
                }

                //var userDb = await _personaRepository.ExistsByNombreUsuario(request.NombreUsuario, _logger);

                //if (userDb != null && userDb.Id != request.Id)
                //{
                //    response.Message = "El nombre de usuario ya existe";
                //    response.Success = false;

                //    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                //    return response;
                //}

                if (request.Password == null || request.Password == " ")
                {
                    persona.PasswordHash = persona.PasswordHash;
                    persona.PasswordSalt = persona.PasswordSalt;
                }


                else if (!VerifyPasswordHash(request.Password, persona.PasswordHash, persona.PasswordSalt))
                {
                    CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    persona.PasswordHash = passwordHash;
                    persona.PasswordSalt = passwordSalt;
                }

                persona.Nombre = request.Nombre;
                persona.Correo = request.Correo;
                persona.ApellidoPaterno = request.ApellidoPaterno;
                persona.ApellidoMaterno = request.ApellidoMaterno;
                persona.Direccion = request.Direccion;
                persona.telefono = request.telefono;
                persona.Grado = request.Grado;
                persona.Grupo = request.Grupo;
                persona.IdTutor= request.IdTutor;
                persona.UserUpd = usrMod;
                persona.FechaUpd = DateTime.Now;
                persona.IdDocente = request.IdDocente;



                var result = await _personaRepository.ActualizarPersona(persona, _logger);

                if (result.Success)
                {
                    response.Message = "Persona actualizado correctamente";
                    response.Success = true;
                    response.UpdatedId = result.UpdatedId;
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


        //Obtener todos las personas
        public async Task<ListResponse<Personas>> ObtenerPersonas()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _personaRepository.ObtenerPersonas(_logger);

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    var list = AppMapper.Map<List<Personas>, List<Personas>>(response.Data);

                    return new ListResponse<Personas>
                    {
                        Data = list,
                        Total = list.Count
                    };
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return new ListResponse<Personas>();
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> EliminarPersona(int id, int usrMod)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var result = await _personaRepository.EliminarPersona(id, usrMod, _logger);

                if (result.Success)
                {
                    response.Message = result.Message;
                    response.Success = true;
                    response.UpdatedId = result.UpdatedId;
                }
                else
                {
                    response.Message = result.Message;
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

        //Obtener persona  por tipo
        public async Task<ListResponse<PersonaVM>> ObtenerPersonaPorTipo(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new ListResponse<PersonaVM>();

                var maestros = await _personaRepository.ObtenerPersonaTipo(id, _logger);

                if (maestros != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    var listVM = AppMapper.Map<List<Personas>, List<PersonaVM>>(maestros.Data);

                    foreach (var item in listVM)
                    {
                        if (item.Grado != null && item.Grado != 0)
                        {
                            var id_grado = item.Grado.Value;
                            var nombreGrado = await _Gradorepository.GetById(id_grado, _logger);
                            if(nombreGrado != null)
                            {
                                item.nombreGrado = nombreGrado.Grado;
                            }
                          
                        }
                        if (item.Grupo != null && item.Grupo != 0)
                        {
                            var idGrupo = item.Grupo.Value;
                            var nombreGrupo = await _GruposRepository.GetById(idGrupo, _logger);
                            item.nombreGrupo = nombreGrupo.Nombre;
                        }
                        if (item.IdTutor != null && item.Grupo != 0)
                        {
                            var idTutor = item.IdTutor.Value;
                            var tutor = await _personaRepository.ObtenerTutor(idTutor, _logger);
                            item.nombreTutor = tutor.Nombre + " " + tutor.ApellidoPaterno + " " + tutor.ApellidoMaterno;
                        }
                        if (item.IdDocente != null && item.IdDocente != 0)
                        {
                            var idDocente = item.IdDocente.Value;
                            var docente = await _personaRepository.obtenerDocente(idDocente, _logger);
                            item.nombreDocente = docente.Nombre + " " + docente.ApellidoPaterno + " " + docente.ApellidoMaterno;
                        }

                    }

                    if (listVM != null)
                    {
                        response.Data = listVM;
                    }
                    else
                    {
                        response.Data = new List<PersonaVM>();
                    }
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        #endregion
    }
}
