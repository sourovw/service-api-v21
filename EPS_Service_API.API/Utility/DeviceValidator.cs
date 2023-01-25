using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPS_Service_API.Utility
{
    public class DeviceValidator
    {
        public bool Validator(string token,string deviceId_in)
        {
            bool _return = true;

            #region Device ID validation

            string DeviceID = null;

            if (!string.IsNullOrEmpty(token))
            {
                var _token = token.Replace("Bearer ", "");
                var __token = new JwtSecurityTokenHandler().ReadJwtToken(_token);

                var jti = __token.Claims.First(claim => claim.Type == ClaimTypes.UserData);
                DeviceID = jti.Value;
            }

            if (DeviceID != null && DeviceID != "" && deviceId_in != null)
            {

                if (DeviceID.Trim() != deviceId_in.Trim())
                {
                    _return = false;
                }

            }

            else
            {

                _return = false;
            }

            #endregion

            return _return;
        }


        #region strong Methods

        //        #region Device ID validation

        //        string token = Request.Headers["Authorization"];
        //        string DeviceID = null;

        //            if (!string.IsNullOrEmpty(token))
        //            {
        //                var _token = token.Replace("Bearer ", "");
        //        var __token = new JwtSecurityTokenHandler().ReadJwtToken(_token);

        //        var jti = __token.Claims.First(claim => claim.Type == ClaimTypes.UserData);
        //        DeviceID = jti.Value;
        //            }

        //            if (DeviceID != null && DeviceID != "" && TBBRA.DeviceID != null)
        //            {

        //                if (DeviceID.Trim() != TBBRA.DeviceID.Trim())
        //                {

        //                    _objResponseModel.StatusCode = 401;
        //                    _objResponseModel.ErrorDescription = "Device Error!";
        //                    _objResponseModel.APIVersion = "0.1";

        //                    return Created("Result", _objResponseModel);
        //}

        //            }

        //            else
        //{

        //    _objResponseModel.StatusCode = 402;
        //    _objResponseModel.ErrorDescription = "Device Error!";
        //    _objResponseModel.APIVersion = "0.1";

        //    return Created("Result", _objResponseModel);
        //}

        //            #endregion

        #endregion

        public string FixPhoneNo(string phoneNo)
        {
            string output = Regex.Replace(phoneNo, @"[-+\(\)\.]|\s+", "");
            if (output.StartsWith("0"))
            {
                output = "+88" + output;
            }
            if (!output.StartsWith("+"))
            {
                output = '+' + output;
            }
            return output;
        }


    }
}
