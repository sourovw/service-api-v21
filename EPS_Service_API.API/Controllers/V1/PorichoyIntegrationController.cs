using EPS_Service_API.Repositories;
using EPS_Service_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.API.Repositories;
using EPS_Service_API.Utility;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using AutoMapper;
using Newtonsoft.Json;

namespace EPS_Service_API.API.Controllers.V1
{
    [ApiVersion("1.0")]
    public class PorichoyIntegrationController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private IEmailSender emailSender;
        private IEmailTemplateService emailTemplateService;
        private readonly IConfiguration _config;
        private readonly ILogger<PorichoyIntegrationController> _logger;
        private readonly IMapper _mapper;
        private IPorichoyLogTaker _IPorichoyLogTaker;
        private ICustomerProfileRepository _CustomerProfileRepository;

        public PorichoyIntegrationController
            (
            SecurityHelper securityHelper,
            IEmailSender emailSender,
            IEmailTemplateService emailTemplateService,
            IConfiguration config,
            ILogger<PorichoyIntegrationController> logger,
            IMapper mapper,
            IPorichoyLogTaker PorichoyLogTaker,
            ICustomerProfileRepository CustomerProfileRepository
            )
        {
            _securityHelper = securityHelper;
            this.emailSender = emailSender;
            this.emailTemplateService = emailTemplateService;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _IPorichoyLogTaker = PorichoyLogTaker;
            _CustomerProfileRepository = CustomerProfileRepository;

        }


        [HttpPost]
        [Route("~/api/v{version:apiVersion}/PorichoyIntegration/Porichoy_Basic_Autofill_Get")]
        public async Task<IActionResult> Porichoy_Basic_Autofill_Get([FromBody] Porichoy_Basic_Autofill PBAF)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            PBAF.ActionMethod = "Porichoy_Basic_Autofill_Get";
            var jsonBody = JsonConvert.SerializeObject(PBAF);
            dynamic jsonObject = JObject.Parse(jsonBody);
            PorichoyAPIHitLogModel PAHLM = new PorichoyAPIHitLogModel();
            PAHLM = _mapper.Map<PorichoyAPIHitLogModel>(jsonObject);

            string Trx_ID = DateTime.Now.ToString("yyyyMMddHHmmss");
            PAHLM.trx_Id = Trx_ID;

            int a= await _IPorichoyLogTaker.Porichoy_API_Hit(PAHLM);

            objPorichoy_AF _objResponseModel = new objPorichoy_AF();

            try
            {
                var result = await Porichoy_Basic_Autofill_Get_API(PBAF, Trx_ID);

                if (result == null)
                {
                    _objResponseModel.obj_Porichoy_Basic = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //  return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.obj_Porichoy_Basic = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //    return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Get Porichoy Basic Auto Fill API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.obj_Porichoy_Basic = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //  return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }


        [HttpPost]
        [Route("~/api/v{version:apiVersion}/PorichoyIntegration/Porichoy_Basic_Live_Get")]
        public async Task<IActionResult> Porichoy_Basic_Live_Get([FromBody] PorichoyIntegrationLive_Basic PILB)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            PILB.ActionMethod = "Porichoy_Basic_Live_Get";
            var jsonBody = JsonConvert.SerializeObject(PILB);
            dynamic jsonObject = JObject.Parse(jsonBody);
            PorichoyAPIHitLogModel PAHLM = new PorichoyAPIHitLogModel();
            PAHLM = _mapper.Map<PorichoyAPIHitLogModel>(jsonObject);

            string Trx_ID = DateTime.Now.ToString("yyyyMMddHHmmss");
            PAHLM.trx_Id = Trx_ID;

            int a = await _IPorichoyLogTaker.Porichoy_API_Hit(PAHLM);

            obj_BasicLiveData _objResponseModel = new obj_BasicLiveData();
          
            try
            {
                var result = await Porichoy_Basic_Live_Get_API(PILB, Trx_ID);

                if (result == null)
                {
                    _objResponseModel.BasicLiveData = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";
                    //   return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.BasicLiveData = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //     return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Get Porichoy Basic Live API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.BasicLiveData = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //  return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }


        [HttpPost]
        [Route("~/api/v{version:apiVersion}/PorichoyIntegration/Porichoy_Basic_FaceMatch_Get")]
        public async Task<IActionResult> Porichoy_Basic_FaceMatch_Get([FromBody] PorichoyIntegrationLive_FaceMatch PILFM)
        {
            string hash = Request.Headers["x-hash"].ToString(); // Check if the hash is valid

            PILFM.ActionMethod = "Porichoy_Basic_FaceMatch_Get";
            var jsonBody = JsonConvert.SerializeObject(PILFM);
            dynamic jsonObject = JObject.Parse(jsonBody);
            PorichoyAPIHitLogModel PAHLM = new PorichoyAPIHitLogModel();
            PAHLM = _mapper.Map<PorichoyAPIHitLogModel>(jsonObject);

            string Trx_ID = DateTime.Now.ToString("yyyyMMddHHmmss");
            PAHLM.trx_Id = Trx_ID;

            int a = await _IPorichoyLogTaker.Porichoy_API_Hit(PAHLM);

            obj_porichoy_FaceMatch _objResponseModel = new obj_porichoy_FaceMatch();


            try
            {
                var result = await Porichoy_FaceMatch_Live_Get_API(PILFM, Trx_ID);

                if (result == null)
                {
                    _objResponseModel.FaceMatch_Data = null;
                    _objResponseModel.StatusCode = 1;
                    _objResponseModel.ErrorDescription = "Successful Data Load- No Data found";
                    _objResponseModel.APIVersion = "0.1";

                    //    return NotFound( _objResponseModel);
                    return Created("Result", _objResponseModel);

                }
                else
                {
                    _objResponseModel.FaceMatch_Data = result;
                    _objResponseModel.StatusCode = 200;
                    _objResponseModel.ErrorDescription = null;
                    _objResponseModel.APIVersion = "0.1";



                    // API Related Information  start

                    // Method mongodb

                    // API Related Information  start

                    //   return Ok(_objResponseModel);
                    return Created("Result", _objResponseModel);

                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "We caught this exception in Get Porichoy Face Matching API.");
                string errorDescription_ = ex.Message.ToString();
                _objResponseModel.FaceMatch_Data = null;
                _objResponseModel.StatusCode = 1;
                // _objResponseModel.ErrorDescription = errorDescription_;
                _objResponseModel.ErrorDescription = "Something went wrong!Please try later.";
                _objResponseModel.APIVersion = "0.1";
                //    return BadRequest( _objResponseModel);
                return Created("Result", _objResponseModel);
            }


        }



        private async Task<obj_Porichoy_Basic> Porichoy_Basic_Autofill_Get_API(Porichoy_Basic_Autofill PBAF, string Trx_ID)
        {

            
            var jsonBody = JsonConvert.SerializeObject(PBAF);
            dynamic jsonObject_ = JObject.Parse(jsonBody);
            PorichoyAPIResponseLogModel PARM = new PorichoyAPIResponseLogModel();
            PARM = _mapper.Map<PorichoyAPIResponseLogModel>(jsonObject_);
            


            obj_Porichoy_Basic opb_obj = new obj_Porichoy_Basic();
            NID_data_Basic obj_data_basic = new NID_data_Basic();

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), _config["PorichoyEndPoints:Porichoy_Basic_Autofill"]))
                {
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _config["PorichoyEndPoints:API_Key"]);

                    var body = @"{
" + "\n" +
                    @"  ""nidNumber"": """+ PBAF.nidNumber+ @""",
" + "\n" +
                    @"  ""dateOfBirth"": """ + PBAF.dateOfBirth + @""",
" + "\n" +
                    @"  ""englishTranslation"": true
" + "\n" +
                    @"}";

                    request.Content = new StringContent(body);

                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode == true)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string Ser_success = await content.ReadAsStringAsync();

                            PARM.PorichoyResponse = Ser_success;
                            PARM.trx_Id = Trx_ID;
                            int a = await _IPorichoyLogTaker.Porichoy_API_Response(PARM);

                            dynamic jsonObject = JObject.Parse(Ser_success);
                            opb_obj = _mapper.Map<obj_Porichoy_Basic>(jsonObject);
                            obj_data_basic= _mapper.Map<NID_data_Basic>(jsonObject.data.nid);
                            opb_obj.NID_data_Basic = obj_data_basic;

                        }
                    }
                }
            }

            return opb_obj;

        }


        private async Task<BasicLiveData> Porichoy_Basic_Live_Get_API(PorichoyIntegrationLive_Basic PILB, string Trx_ID)
        {
            var jsonBody = JsonConvert.SerializeObject(PILB);
            dynamic jsonObject_ = JObject.Parse(jsonBody);
            PorichoyAPIResponseLogModel PARM = new PorichoyAPIResponseLogModel();
            PARM = _mapper.Map<PorichoyAPIResponseLogModel>(jsonObject_);

            BasicLiveData obj_data = new BasicLiveData();

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), _config["PorichoyEndPoints:Porichoy_Basic_Live"]))
                {
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _config["PorichoyEndPoints:API_Key"]);


                    var body = @"{
" + "\n" +
  @"    ""national_id"": """ + PILB.national_id + @""",
" + "\n" +
  @"    ""person_dob"": """ + PILB.person_dob + @""",
" + "\n" +
  @"    ""person_fullname"": """ + PILB.person_fullname + @""",
" + "\n" +
  @"    ""team_tx_id"": """",
" + "\n" +
  @"    ""match_name"": false
" + "\n" +
  @"}";

                    request.Content = new StringContent(body);

                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode == true)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string Ser_success = await content.ReadAsStringAsync();

                            PARM.PorichoyResponse = Ser_success;
                            PARM.trx_Id = Trx_ID;
                            int a = await _IPorichoyLogTaker.Porichoy_API_Response(PARM);

                            dynamic jsonObject = JObject.Parse(Ser_success);
                            obj_data = _mapper.Map<BasicLiveData>(jsonObject);


                        }
                    }
                }
            }

            return obj_data;

        }


        private async Task<FaceMatch_Data> Porichoy_FaceMatch_Live_Get_API(PorichoyIntegrationLive_FaceMatch PILFM, string Trx_ID)
        {

            var jsonBody = JsonConvert.SerializeObject(PILFM);
            dynamic jsonObject_ = JObject.Parse(jsonBody);
            PorichoyAPIResponseLogModel PARM = new PorichoyAPIResponseLogModel();
            PARM = _mapper.Map<PorichoyAPIResponseLogModel>(jsonObject_);

            FaceMatch_Data opb_obj = new FaceMatch_Data();

            faceMatchBasicData FMBD = new faceMatchBasicData();
            faceMatchComparison FMC = new faceMatchComparison();
            NID_Info_faceMatchResult NIFMR = new NID_Info_faceMatchResult();

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), _config["PorichoyEndPoints:Porichoy_FaceMatch_Live"]))
                {
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _config["PorichoyEndPoints:API_Key"]);


                    var body = @"{
" + "\n" +
                    @"  ""national_id"": """ + PILFM.national_id + @""",
" + "\n" +
                    @"  ""team_tx_id"": """",
" + "\n" +
                    @"  ""english_output"": false,
" + "\n" +
                    @"  ""person_dob"": """ + PILFM.person_dob + @""",
" + "\n" +
                    @"  ""person_photo"": """ + PILFM.person_photo + @"""
" + "\n" +
                    @"}";

                    request.Content = new StringContent(body);

                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode == true)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string Ser_success = await content.ReadAsStringAsync();

                            PARM.PorichoyResponse = Ser_success;
                            PARM.trx_Id = Trx_ID;
                            int a = await _IPorichoyLogTaker.Porichoy_API_Response(PARM);

                            dynamic jsonObject = JObject.Parse(Ser_success);

                            if(jsonObject.passKyc== "no")
                            {
                                FMBD = _mapper.Map<faceMatchBasicData>(jsonObject);
                            }
                            
                            if (jsonObject.passKyc == "yes")
                            {
                                FMBD = _mapper.Map<faceMatchBasicData>(jsonObject);
                                FMC = _mapper.Map<faceMatchComparison>(jsonObject.voter.faceMatchResult);
                                NIFMR = _mapper.Map<NID_Info_faceMatchResult>(jsonObject.voter);
                            }

                          

                            if (PILFM.UserId.Length>0)
                            {
                                var updatestat = _CustomerProfileRepository.Porichoy_Customer_Status_Update(PILFM.UserId,FMC.matched, FMC.percentage);
                            }

                        }
                    }
                }
            }

            opb_obj.faceMatchBasicData = FMBD;
            opb_obj.faceMatchComparison = FMC;
            opb_obj.NID_Info_faceMatchResult = NIFMR;

            return opb_obj;

        }


    }
}
