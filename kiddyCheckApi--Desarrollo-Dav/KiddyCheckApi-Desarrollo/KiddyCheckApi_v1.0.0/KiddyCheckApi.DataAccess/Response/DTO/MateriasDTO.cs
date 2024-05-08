﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyCheckApi.DataAccess.Response.DTO
{
    public class MateriasDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<int> Grados { get; set; }
        public List<string> GradosNombres { get; set; }
    }
}
