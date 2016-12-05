﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeutscheBank.web.Models
{
    public class ArbeitgeberModel
    {
        [Required(AllowEmptyStrings =false, ErrorMessage = "Pflichtfeld") ]
        [StringLength(30, ErrorMessage = "max 30 Zeichen erlaubt")]
        [Display(Name = "Firmen-Name")]
        public string FirmenName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Beschäftigungsart")]
        public int ID_BeschäftigungsArt { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Branche")]
        public int ID_Branche { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "MM.yyyy")]
        public string BeschäftigtSeit { get; set; }

    }
    

}