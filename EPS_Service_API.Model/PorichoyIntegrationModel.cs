using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPS_Service_API.Model
{
    public class PorichoyIntegrationModel
    {
    }

    public class Porichoy_Basic_Autofill: BasicApiCallingModel
    {
        public string nidNumber { set; get; }
        public string dateOfBirth { set; get; }
        public string englishTranslation { set; get; }

    }

    public class PorichoyIntegrationLive_Basic: BasicApiCallingModel
    {
        public string national_id { set; get; }
        public string person_dob { set; get; }
        public string person_fullname { set; get; }
        public string team_tx_id { set; get; }
        public string match_name { set; get; }
    }


    public class PorichoyIntegrationLive_FaceMatch: BasicApiCallingModel
    {
        public string national_id { set; get; }
        public string team_tx_id { set; get; }
        public string english_output { set; get; }
        public string person_dob { set; get; }
        public string person_photo { set; get; } //base 64 photo

    }

    public class objPorichoy_AF
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object obj_Porichoy_Basic { set; get; }

    }


    public class obj_Porichoy_Basic
    {
        public string transactionId { set; get; }
        public string creditCost { set; get; }
        public string creditCurrent { set; get; }
        public string status { set; get; }
        public string errors { set; get; }
        public object NID_data_Basic { set; get; }
    }



    public class NID_data_Basic
    {
        public string fullNameEN { set; get; }
        public string fathersNameEN { set; get; }
        public string mothersNameEN { set; get; }
        public string spouseNameEN { set; get; }
        public string presentAddressEN { set; get; }
        public string permenantAddressEN { set; get; }
        public string fullNameBN { set; get; }
        public string fathersNameBN { set; get; }
        public string mothersNameBN { set; get; }
        public string spouseNameBN { set; get; }
        public string presentAddressBN { set; get; }
        public string permanentAddressBN { set; get; }
        public string gender { set; get; }
        public string profession { set; get; }
        public string dateOfBirth { set; get; }
        public string nationalIdNumber { set; get; }
        public string oldNationalIdNumber { set; get; }
        public string photoUrl { set; get; }
    }


    public class PorichoyAPIHitLogModel
    {
        // API Related Information 
        public string DeviceTypeID { get; set; }
        public string DeviceID { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceOS { get; set; }
        public string DeviceDetails { get; set; }
        public string LocationLattitude { get; set; }
        public string LocationLongitude { get; set; }
        public string IP_Address { get; set; }

        public string BrowserDetails { get; set; }
        public string ActionMethod { get; set; }
        public string ActionID { get; set; }
        public string UserId { get; set; }
        // API Related Information 


        public string nidNumber { get; set; }
        public string dateOfBirth { get; set; }
        public string englishTranslation { get; set; }
        public string national_id { get; set; }
        public string person_dob { get; set; }
        public string person_fullname { get; set; }
        public string team_tx_id { get; set; }
        public string match_name { get; set; }
        public string english_output { get; set; }
        public string person_photo { get; set; }

        public string trx_Id { get; set; }

    }


    public class PorichoyAPIResponseLogModel
    {
        // API Related Information 
        public string DeviceTypeID { get; set; }
        public string DeviceID { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceOS { get; set; }
        public string DeviceDetails { get; set; }
        public string LocationLattitude { get; set; }
        public string LocationLongitude { get; set; }
        public string IP_Address { get; set; }

        public string BrowserDetails { get; set; }
        public string ActionMethod { get; set; }
        public string ActionID { get; set; }
        public string UserId { get; set; }
        // API Related Information 


        public string PorichoyResponse { get; set; }

        public string trx_Id { get; set; }


    }


    public class obj_BasicLiveData
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object BasicLiveData { set; get; }

    }

    public class BasicLiveData
    {
        public string passKyc { get; set; }
        public string errorCode { get; set; }
    }



    public class obj_porichoy_FaceMatch
    {
        public int StatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string APIVersion { get; set; }
        public object FaceMatch_Data { set; get; }

    }

    public class FaceMatch_Data
    {
      
        public object faceMatchBasicData { set; get; }
        public object faceMatchComparison { set; get; }
        public object NID_Info_faceMatchResult { set; get; }

    }

    public class faceMatchBasicData
    {
        public string passKyc { get; set; }
        public string errorCode { get; set; }
    }
    public class faceMatchComparison
    {
        public string matched { get; set; }
        public string percentage { get; set; }
        public string error { get; set; }


    }

    public class NID_Info_faceMatchResult
    {
        public string name { get; set; }
        public string nameEn { get; set; }
        public string father { get; set; }
        public string mother { get; set; }
        public string gender { get; set; }
        public string profession { get; set; }
        public string spouse { get; set; }
        public string dob { get; set; }
        public string permanentAddress { get; set; }
        public string presentAddress { get; set; }
        public string nationalId { get; set; }
        public string oldNationalId { get; set; }
        public string photo { get; set; }
        public string fatherEn { get; set; }
        public string motherEn { get; set; }
        public string spouseEn { get; set; }
        public string permanentAddressEn { get; set; }
        public string presentAddressEn { get; set; }
    }







}
