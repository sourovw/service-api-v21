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
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace EPS_Service_API.API.Controllers.V1
{

[ApiVersion("1.0")]

    [ApiController]
    public class OffersController : ControllerBase
    {
        private SecurityHelper _securityHelper;
        private IRechargeOfferRepository _IRechargeOfferRepository;
        private IEmailSender emailSender;
        private IEmailTemplateService emailTemplateService;
        private readonly IConfiguration _config;
        private readonly ILogger<OffersController> _logger;
        private DeviceValidator _DeviceValidator;

        public OffersController(
            IRechargeOfferRepository IRechargeOfferRepository,
            SecurityHelper securityHelper,
            IEmailSender emailSender,
            IEmailTemplateService emailTemplateService,
            IConfiguration config,
            ILogger<OffersController> logger,
            DeviceValidator DeviceValidator
            )
        {
            _IRechargeOfferRepository = IRechargeOfferRepository;
            _securityHelper = securityHelper;
            this.emailSender = emailSender;
            this.emailTemplateService = emailTemplateService;
            _DeviceValidator = DeviceValidator;
            _config = config;
            _logger = logger;
        }


        [HttpPost]
        [Route("api/v{version:apiVersion}/RechargeOffers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<object> GetRechargeOffers([FromBody] offersGetModel model)
        {
            var result = await _IRechargeOfferRepository.GetRechargeOffers(model.operatorId, model.operatorTypeId);
            return result;
        }



    }
}
