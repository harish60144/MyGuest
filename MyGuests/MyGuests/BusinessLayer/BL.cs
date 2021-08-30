using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MyGuests.Models;
using MyGuests.DL;


namespace MyGuests.BusinessLayer
{
    public class BL
    {
        dlTransaction objDL = new dlTransaction();
        internal UserResponse AddGuests(AddGuestDTO req)
        {
            UserResponse response;
            DataSet dsResults = objDL.dlFillDataSet("AddGuests", "_Name", "varchar", req.Name,
                "_pax", "varchar", req.Pax,
                "_mobNumber", "varchar", req.MobNumber,
                "_emailId", "varchar", req.emailId,
                "_CreatedDt", "datetime", req.CreatedDt,
                "_Source", "varchar", req.Source,
                "_DOB", "varchar", req.DOB,
                "_DOA", "varchar", req.DOA,
                "_location", "varchar", req.Location,
                "_intInOffers", "bit",req.InteInOffers);
            if (dsResults.Tables[0].Rows.Count > 0)
            {
                response = new UserResponse
                {
                    code = 200,
                    data = dsResults.Tables[0],
                    msg = "User Login Successfully"

                };
            }
            else
            {
                response = new UserResponse
                {
                    code = 300,
                    data = "",
                    msg = "User Login Not Successfully"
                };
            }
            return response;
        }
    }
}