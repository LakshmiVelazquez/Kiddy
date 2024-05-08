﻿using System.ComponentModel.DataAnnotations;

namespace KiddyCheckApi.Core.Requests
{
    public class AuthRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
