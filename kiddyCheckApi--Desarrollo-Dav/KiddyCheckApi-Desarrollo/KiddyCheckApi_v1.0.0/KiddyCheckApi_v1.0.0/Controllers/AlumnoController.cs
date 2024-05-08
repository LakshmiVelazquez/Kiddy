using KiddyCheckApi.Core.Helpers;
using KiddyCheckApi.Core.Requests;
using KiddyCheckApi.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using System.Reflection;

namespace KiddyCheckApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    public class AlumnoController : Controller
    {
        private ILogger<AlumnoController> _logger;
        private readonly AlumnoService _service;
        public AlumnoController(
            ILogger<AlumnoController> logger, AlumnoService service)
        {
            _logger= logger;
            _service= service;
        }

        [HttpPost("generateqr")]
        public async Task<IActionResult> generateqr(RequestQr requestQr)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(requestQr);
                string qRCodeHelper = QRCodeHelper.GenerateQRCode(jsonString);
                return Ok(new { data = qRCodeHelper, error = false });
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("EnviarCorreo")]
        public async Task<IActionResult> EnviarCorreo(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var user = HttpContext.User.Claims.Where(x => x.Type == "Id").FirstOrDefault().Value;

                var response = await _service.EnviarCorreo(id, int.Parse(user));

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("error aqui" + MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        [HttpPost("Exportar")]
        public async Task<IActionResult> ExportarAsistencias(ExportarAsistenciaRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var user = HttpContext.User.Claims.Where(x => x.Type == "Id").FirstOrDefault().Value;

                var response = await _service.ExportarAsistencias(request, int.Parse(user));

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("error aqui" + MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        [HttpGet("AsistenciasHoy")]
        public async Task<IActionResult> AsistenciaHoy()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                //var user = HttpContext.User.Claims.Where(x => x.Type == "Id").FirstOrDefault().Value;

                var response = await _service.AsistenciaDia();

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("error aqui" + MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        [HttpGet("AsistenciasSemanal")]
        public async Task<IActionResult> AsistenciasSemanal()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                //var user = HttpContext.User.Claims.Where(x => x.Type == "Id").FirstOrDefault().Value;

                var response = await _service.AsistenciaSemana();

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("error aqui" + MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
    }

   

}
