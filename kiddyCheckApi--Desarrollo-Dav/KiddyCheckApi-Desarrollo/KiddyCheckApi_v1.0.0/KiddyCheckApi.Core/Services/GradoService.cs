

using KiddyCheckApi.Core.Response;
using KiddyCheckApi.DataAccess.Entities;
using KiddyCheckApi.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace KiddyCheckApi.Core.Services
{
    public class GradoService
    {
        #region <--- Variables --->
        private readonly IBaseRepository<Grados> _Gradorepository;
        private ILogger<GradoService> _logger;
        #endregion

        #region <--- Constructor --->
        public GradoService(IBaseRepository<Grados> repository, ILogger<GradoService> logger)
        {
            _Gradorepository = repository;
            _logger = logger;
        }
        #endregion

        #region
        public async Task<GenericResponse> AddGrado(Grados gradoAdd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();
                
                var gradosave = await _Gradorepository.Add(gradoAdd, _logger);
               
               if(gradosave != null)
                {

                    response.Success = true;
                    response.Message = "Se agrego el grado con exito";
                    response.CreatedId = gradosave.Id.ToString();

                   
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se agrego el grado";

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

        public async Task<List<Grados>> GetAllGrados()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

             
                var GetGrados = await _Gradorepository.GetAll(_logger);

                var grados = new List<Grados>();

                foreach( var gradoAdd in GetGrados)
                {
                    grados.Add(gradoAdd);
                }

                return grados;


            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        public async Task<Grados> GetByIdGrado(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var grado = await _Gradorepository.GetById(id, _logger);

                if( grado != null )
                {
                    return grado;
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }


        public async Task<GenericResponse> UpdateGrado(Grados grado)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();


                var updateGrado = await _Gradorepository.Update(grado, _logger);

             
                if(updateGrado != null)
                {
                    response.Success = true;
                    response.UpdatedId = updateGrado.Id.ToString();
                    response.Message = "Se modifico el grado";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se pudo modificar el grado";
                }

                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> DeleteGrado(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();


                var deleteGrado = await _Gradorepository.Delete(id, _logger);


                if (deleteGrado > 0)
                {
                    response.Success = true;
                    response.DeletedId = deleteGrado.ToString();
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
