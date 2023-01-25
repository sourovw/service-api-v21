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

namespace EPS_Service_API.API.Repositories
{
    public class PorichoyLogTaker: IPorichoyLogTaker
    {
        private DataHelperSQL_Server _dataAccessHelper;
        private readonly IConfiguration _config;


        public PorichoyLogTaker(DataHelperSQL_Server dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<int> Porichoy_API_Hit(PorichoyAPIHitLogModel PARM) 
        {
            try { 
            DynamicParameters p = new DynamicParameters();
            
            p.Add("@DeviceTypeID", PARM.DeviceTypeID);
            p.Add("@DeviceID", PARM.DeviceID);
            p.Add("@DeviceModel", PARM.DeviceModel);
            p.Add("@DeviceOS", PARM.DeviceOS);
            p.Add("@DeviceDetails", PARM.DeviceDetails);
            p.Add("@LocationLattitude", PARM.LocationLattitude);
            p.Add("@LocationLongitude", PARM.LocationLongitude);
            p.Add("@IP_Address", PARM.IP_Address);
            p.Add("@BrowserDetails", PARM.BrowserDetails);
            p.Add("@ActionMethod", PARM.ActionMethod);
            p.Add("@ActionID", PARM.ActionID);
            p.Add("@UserId", PARM.UserId);
            p.Add("@nidNumber", PARM.nidNumber);
            p.Add("@dateOfBirth", PARM.dateOfBirth);
            p.Add("@englishTranslation", PARM.englishTranslation);
            p.Add("@national_id", PARM.national_id);
            p.Add("@person_dob", PARM.person_dob);
            p.Add("@person_fullname", PARM.person_fullname);
            p.Add("@team_tx_id", PARM.team_tx_id);
            p.Add("@match_name", PARM.match_name);
            p.Add("@english_output", PARM.english_output);
            p.Add("@person_photo", PARM.person_photo);
            p.Add("@trx_Id", PARM.trx_Id);

            await _dataAccessHelper.ExecuteData("PorichoyAPIHitLog_Set", p);
            }
            catch(Exception ex)
            {

            }

            return 1;
        }

        public async Task<int> Porichoy_API_Response(PorichoyAPIResponseLogModel PARM)
        {
            DynamicParameters p = new DynamicParameters();
           
            p.Add("@DeviceTypeID", PARM.DeviceTypeID);
            p.Add("@DeviceID", PARM.DeviceID);
            p.Add("@DeviceModel", PARM.DeviceModel);
            p.Add("@DeviceOS", PARM.DeviceOS);
            p.Add("@DeviceDetails", PARM.DeviceDetails);
            p.Add("@LocationLattitude", PARM.LocationLattitude);
            p.Add("@LocationLongitude", PARM.LocationLongitude);
            p.Add("@IP_Address", PARM.IP_Address);
            p.Add("@BrowserDetails", PARM.BrowserDetails);
            p.Add("@ActionMethod", PARM.ActionMethod);
            p.Add("@ActionID", PARM.ActionID);
            p.Add("@UserId", PARM.UserId);
            p.Add("@PorichoyResponse", PARM.PorichoyResponse);
            p.Add("@trx_Id", PARM.trx_Id);


            await _dataAccessHelper.ExecuteData("PorichoyAPIResponseLog_Set", p);
            return 1;

        }




    }
}
