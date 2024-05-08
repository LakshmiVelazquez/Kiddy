﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.DataAccess.Entities
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime UltimoAcceso { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }

    }
}
