using SOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOP.Controllers
{
    public class ApiRatingController : ApiController

    {
        /// <summary>
        /// Create card
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[Route("create")]
        //[HttpPost]
        //public Result_tblCustomer_API3rd Create([FromBody] RatingResult obj)
        //{
        //    var result = new Result_tblCustomer_API3rd()
        //    {
        //        isSuccess = false,
        //        Message = "",
        //        CustomerID = "",
        //        CustomerName = ""
        //    };
        //    string Err = String.Empty;
        //    if (String.IsNullOrEmpty(obj.CustomerName.ToString()))
        //    {
        //        Err = " Vui lòng nhập tên khách hàng";
        //    }

        //    //if ((String.IsNullOrEmpty(obj.IDNumber) || String.IsNullOrWhiteSpace(obj.Mobile)))
        //    //{

        //    //    Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập số số điện thoại hoặc căn cước / cmtnd" : ", Vui lòng nhập số số điện thoại hoặc căn cước / cmtnd";
        //    //}
        //    if (String.IsNullOrWhiteSpace(obj.Mobile))
        //    {

        //        Err += String.IsNullOrEmpty(Err) ? "Vui lòng nhập số số điện thoại" : ", Vui lòng nhập số số điện thoại";
        //    }


        //    var existedCard = _API_CustomerService.GetByMobileOrIdNumber(obj.Mobile, obj.IDNumber);
        //    if (existedCard != null)
        //    {
        //        Err += String.IsNullOrEmpty(Err) ? " Sđt đã được đăng ký" : ", Số sđt đã được đăng ký";
        //    }

        //    if (!String.IsNullOrEmpty(Err))
        //    {
        //        result.Message = Err;
        //        return result;
        //    }

        //    var model = new tblCustomer()
        //    {
        //        CustomerID = Guid.NewGuid(),
        //        CustomerName = obj.CustomerName,
        //        Address = obj.Address,
        //        IDNumber = obj.IDNumber,
        //        Mobile = obj.Mobile,
        //        CustomerGroupID = obj.CustomerGroupID,
        //        EnableAccount = true,
        //        Inactive = false,
        //        SortOrder = 1,
        //        AccessLevelID = "",
        //        Finger1 = "",
        //        Finger2 = "",
        //        DevPass = "",
        //        AccessExpireDate = Convert.ToDateTime("2099/12/31"),
        //        CompartmentId = !string.IsNullOrEmpty(obj.CompartmentId) ? obj.CompartmentId.Trim() : "",

        //    };

        //    var resultCreate = _API_CustomerService.Create(model);

        //    WriteLog.WriteAPI(resultCreate, null, model.CustomerID.ToString(), "", "tblCustomer", ConstField.ApiParking, ActionConfigO.Create);

        //    //Trả lại response
        //    if (resultCreate.isSuccess)
        //    {
        //        result.isSuccess = resultCreate.isSuccess;
        //        result.Message = resultCreate.Message;
        //        result.CustomerID = model.CustomerID.ToString();
        //        result.CustomerName = model.CustomerName;
        //    }
        //    else
        //    {
        //        result.isSuccess = resultCreate.isSuccess;
        //        result.Message = resultCreate.Message;
        //    }
        //    return result;
        //}
    }
}
