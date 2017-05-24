﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Xone
{
    public class TblCitizenDetails
    {
        [Key]
        public Int16 CitizenID { get; set; }
        public string CitizenName { get; set; }
        public string CitizenCode { get; set; }
        public string CitizenDesc { get; set; }
        public bool IsDeletable { get; set; }
        public bool IsStatus { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

    public class TblRace
    {
        [Key]
        public Int16 RaceID { get; set; }
        public string RaceName { get; set; }
        public bool RaceStatus { get; set; }
        public Int64 ModifiedBy { get; set; } 
        public DateTime ModifiedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    

}