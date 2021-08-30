using MyGuests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MyGuests.BusinessLayer;

namespace MyGuests.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MainController : ApiController
    {
        BL objBL = new BL();
        [HttpPost]
        [Route("api/Login")]
        public UserResponse Login(AddGuestDTO req)
        {
            return objBL.AddGuests(req);
        }
    }
}
