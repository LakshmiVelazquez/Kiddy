﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.DataAccess.Entities
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
