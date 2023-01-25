using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using EPS_Service_API.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EPS_Service_API.Repositories;
using System;
using EPS_Service_API.API.Data;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using EPS_Service_API.API.V1.Controllers;
using Microsoft.Extensions.Logging;
using System.IO;
//using System.Drawing;
using System.Net;
using SharpCompress.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Amazon.Runtime.Internal.Transform;
using System.Text.Json.Nodes;


namespace EPS_Service_API.API.Repositories
{
    public class RechargeOfferRepository : IRechargeOfferRepository
    {
        private DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;

        public RechargeOfferRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<object> GetRechargeOffers(int operatorId, int typeId)
        {
            var rom = new RechargeOfferModel();
            var rom2 = new RechargeOfferModel2();

            using (IDbConnection connection = new MySqlConnection(_config.GetConnectionString("MySQL")))
            {
                var p = new DynamicParameters();
                p.Add("OperatorId", operatorId);
                p.Add("TypeId", typeId);

                try
                {
                    var rows = await connection.QueryAsync("SP_Show_Recharge_Offers", p, commandType: CommandType.StoredProcedure);

                    foreach (var row in rows)
                    {
                        rom.AvailableOffers = row.Offers != null ? JsonConvert.DeserializeObject<List<ProcessRechargeOfferModel>>(row.Offers) : null;

                        if (rom.AvailableOffers != null)
                        {
                            foreach (var offer in rom.AvailableOffers)
                            {
                                switch (offer.OperatorId)
                                {
                                    case 1:
                                        offer.Operator = "Airtel";
                                        break;
                                    case 2:
                                        offer.Operator = "Banglalink";
                                        break;
                                    case 3:
                                        offer.Operator = "Robi";
                                        break;
                                    case 4:
                                        offer.Operator = "Airtel";
                                        break;
                                    case 5:
                                        offer.Operator = "Teletalk";
                                        break;
                                }

                                switch (offer.OperatorTypeId)
                                {
                                    case 1:
                                        offer.OperatorType = "Prepaid";
                                        break;
                                    case 2:
                                        offer.OperatorType = "Postpaid";
                                        break;
                                }

                                switch (offer.OfferTypeId)
                                {
                                    case 1:
                                        rom2.VoiceRechargeOffers = rom.AvailableOffers;
                                        break;
                                    case 2:
                                        rom2.InternetRechargeOffers = rom.AvailableOffers;
                                        break;
                                    case 3:
                                        rom2.BundleRechargeOffers = rom.AvailableOffers;
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return rom2;
            }
        }
    }
}
