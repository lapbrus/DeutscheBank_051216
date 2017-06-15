using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeutscheBank.web.Models
{
    public class AntragBezahltModel 
    {

       
        public int ID_Kunde { get; set; }
        public bool Bezahlt { get; set; }
    }
}