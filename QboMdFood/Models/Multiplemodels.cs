using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QboMdFood.Models.DTO;

namespace QboMdFood.Models
{
    public class Multiplemodels
    {
        public OAuthorizationdto OAuthorizationModel { get; set; }
        public bool IsConnected { get; set; }
    }
}