﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models 
{
    public class OrtsModel : NachschlagefeldModel
    {

        public string PLZ { get; set; }
        public string FKLand { get; set; }


    }
}