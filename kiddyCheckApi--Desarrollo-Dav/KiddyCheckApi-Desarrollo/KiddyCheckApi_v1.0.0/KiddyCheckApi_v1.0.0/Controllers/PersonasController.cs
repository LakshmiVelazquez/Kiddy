using KiddyCheckApi.Core.Requests;
using KiddyCheckApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using KiddyCheckApi.Core.Helpers;
using System.Reflection;

namespace KiddyCheckApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonasController : Controller
    {
        #region <--- Variables --->
        private readonly PersonasService _personasService;
        private ILogger<PersonasController> _logger;
        #endregion

        #region <--- Constructor --->
        public PersonasController(PersonasService personasService,
            ILogger<PersonasController> logger
            )
        {
            _personasService = personasService;
            _logger = logger;

        }
        #endregion

        #region <--- Metodos --->
        [HttpPost("AgregarPersona")]
        public async Task<IActionResult> AgregarPersona(PersonaRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var idUserLogin = HttpContext.User.Claims.Where(x => x.Type == "Id").FirstOrDefault().Value;

               
                var response = await _personasService.AgregarPersona(request, int.Parse(idUserLogin));

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Actualizar usuario
        [HttpPost("ActualizarPersona")]
        public async Task<IActionResult> ActualizarUsuario(PersonaRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var idUserLogin = HttpContext.User.Claims.Where(x => x.Type == "Id").FirstOrDefault().Value;
                var response = await _personasService.ActualizarPersona(request, int.Parse(idUserLogin));

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }


        //Obtener usuarios
        [HttpGet("ObtenerPersonas")]
        public async Task<IActionResult> ObtenerPersonas()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _personasService.ObtenerPersonas();

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        //Eliminar persona
        [HttpDelete("EliminarPersona")]
        public async Task<IActionResult> EliminarPersona(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _personasService.EliminarPersona(id, 1);

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        //Obtener por tipo
        [HttpGet("ObtenerPersonaPorTipo")]
        public async Task<IActionResult> ObtenerPersonaPorTipo(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _personasService.ObtenerPersonaPorTipo(id);

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return NotFound();
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
