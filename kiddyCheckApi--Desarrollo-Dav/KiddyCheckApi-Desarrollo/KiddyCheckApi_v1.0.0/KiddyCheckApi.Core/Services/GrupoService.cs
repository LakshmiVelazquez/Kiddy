

using KiddyCheckApi.Core.Response;
using KiddyCheckApi.Core.ViewModels;
using KiddyCheckApi.DataAccess;
using KiddyCheckApi.DataAccess.Entities;
using KiddyCheckApi.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace KiddyCheckApi.Core.Services
{
    public class GrupoService
    {
        #region <--- Variables --->
        private readonly IBaseRepository<Grupos> _GruposRepository;
        private ILogger<GrupoService> _logger;
        private readonly IBaseRepository<Grados> _GradosRepository;
        public readonly KiddyCheckDbContext _context;
        #endregion

        #region <--- Constructor --->
        public GrupoService(IBaseRepository<Grupos> repository, ILogger<GrupoService> logger,
            IBaseRepository<Grados> gradoRepository,
            KiddyCheckDbContext context)
        {
            _GruposRepository = repository;
            _logger = logger;
            _GradosRepository = gradoRepository;
            _context = context;
        }
        #endregion

        #region
        public async Task<GenericResponse> AddGrupo(Grupos grupoAdd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var grupoSave = await _GruposRepository.Add(grupoAdd, _logger);

                if (grupoSave != null)
                {

                    response.Success = true;
                    response.Message = "Se agrego el grupo con exito";
                    response.CreatedId = grupoSave.Id.ToString();


                }
                else
                {
                    response.Success = false;
                    response.Message = "No se grupo el grado";

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

        public async Task<List<Grupos>> GetAllGrupos()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var GetGrupos = await _GruposRepository.GetAll(_logger);
                List<Grupos> listaGrupo = GetGrupos.Select(selector => new Grupos { Id = selector.Id, Nombre= selector.Nombre }).ToList();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return listaGrupo;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> UpdateGrupo(Grupos grupos)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();


                var updateGrupo = await _GruposRepository.Update(grupos, _logger);


                if (updateGrupo != null)
                {
                    response.Success = true;
                    response.UpdatedId = updateGrupo.Id.ToString();
                    response.Message = "Se modifico el grupo";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se pudo modificar el grupo";
                }

                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> DeleteGrupo(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();


                var deleteGrupo = await _GruposRepository.Delete(id, _logger);


                if (deleteGrupo > 0)
                {
                    response.Success = true;
                    response.DeletedId = deleteGrupo.ToString();
                    response.Message = "Se elmino el grado";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se pudo elminar el grado";
                }

                return response;


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
