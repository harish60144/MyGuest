using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGuests.Models
{
    public class MainDTO
    {

    }
    public class AddGuestDTO
    {
        public string Name { get; set; }
        public string Pax { get; set; }
        public string MobNumber { get; set; }
        public string emailId { get; set; }
        public DateTime CreatedDt { get; set; }
        public string Source { get; set; }
        public string DOB { get; set; }
        public string DOA { get; set; }
        public string Location { get; set; }
        public bool InteInOffers { get; set; }
    }
    public class UserResponse
    {
        public int code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
    public class reqLogin
    {
        public string username { get; set; }
        public string pwd { get; set; }
    }
    public class reqTenantId
    {
        public int tenantId { get; set; }
    }
    public class reqMenuId
    {
        public int MenuId { get; set; }
    }
    public class reqMenuItemId
    {
        public int MenuItemId { get; set; }
    }
    public class reqAddMenuItem
    {
        public int MenuId { get; set; }
        public int itemid { get; set; }
        public int catid { get; set; }
        public int amt { get; set; }
    }

    public class reqMenuDetails
    {
        public int TenantId { get; set; }
        public int MenuId { get; set; }
    }
    public class reqUpdateOrder
    {

        public int billid { get; set; }
        public string rp_payment_id { get; set; }
        public string rp_order_id { get; set; }
        public string rp_signature { get; set; }
    }
    public class reqRPOrder
    {
        public int amount { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
        public bool payment_capture { get; set; }
    }
    public class reqCreateOrder
    {
        public int amount { get; set; }
        public int userid { get; set; }
        public int tenantid { get; set; }
        public int menuid { get; set; }
        public Boolean isParcel { get; set; }
        public orderitems[] items { get; set; }

    }
    public class orderitems
    {
        public int itemid { get; set; }
        public int qty { get; set; }
        public int amt { get; set; }
        public int total { get; set; }
        public int parcel { get; set; }
    }
    public class reqRegUser
    {
        public string user { get; set; }
        public string mobNo { get; set; }
        public string eMail { get; set; }
    }
    public class resReceiptDetails
    {
        public int billid { get; set; }
        public double total { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double GrandTotal { get; set; }
        public Boolean isParcel { get; set; }
        public billItems[] items { get; set; }
    }
    public class billItems
    {
        public string itemname { get; set; }
        public int qty { get; set; }
        public double amt { get; set; }
        public double total { get; set; }
    }
}