﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SHikkhanobishAPI.Models.Shikkhanobish
{
    public class Notification
    {
        public string notificationID { get; set; }
        public int userId { get; set; }
        public int userType { get; set; }
        public int notificationType { get; set; }
        public string description { get; set; }
        public string refIDOne { get; set; }
        public string refIDTwo { get; set; }
        public string refIDThree { get; set; }
        public string refIDFour { get; set; }
        public string notificationDate { get; set; }
        public string Response { get; set; }

    }
}