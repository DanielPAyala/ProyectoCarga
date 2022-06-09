using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCarga.Models
{
    public class Archivos
    {
        public int IdArchivo { get; set; }
        public string Nombre { get; set; }
        public byte[] Archivo { get; set; }
        public string Extension { get; set; }
    }
}