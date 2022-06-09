using ProyectoCarga.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoCarga.Controllers
{
    public class ArchivoController : Controller
    {
        static readonly string cadena = "Data Source=(local); Initial Catalog=DB_ARCHIVOS; Integrated Security=true;";
        static List<Archivos> oLista = new List<Archivos>();

        // GET: Archivo
        public ActionResult Index()
        {
            oLista = new List<Archivos>();
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ARCHIVOS", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Archivos archivos = new Archivos {
                            IdArchivo = Convert.ToInt32(dr["IdArchivo"]),
                            Nombre = dr["Nombre"].ToString(),
                            Archivo = dr["Archivo"] as byte[],
                            Extension = dr["Extension"].ToString()
                        };
                        oLista.Add(archivos);
                    }
                }
            }
            return View(oLista);
        }

        public ActionResult Subir()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubirArchivo(string Nombre, HttpPostedFileBase Archivo)
        {
            string extension = Path.GetExtension(Archivo.FileName);

            MemoryStream ms = new MemoryStream();
            Archivo.InputStream.CopyTo(ms);
            byte[] data = ms.ToArray();

            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ARCHIVOS(Nombre, Archivo, Extension) VALUES(@nombre, @archivo, @extension)", conn);
                cmd.Parameters.AddWithValue("@nombre", Nombre);
                cmd.Parameters.AddWithValue("@archivo", data);
                cmd.Parameters.AddWithValue("@extension", extension);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Subir", "Archivo");
        }

        [HttpPost]
        public FileResult DescargarArchivo(int IdArchivo)
        {
            Archivos archivo = oLista.Where(a => a.IdArchivo == IdArchivo).FirstOrDefault();
            string nombreCompleto = archivo.Nombre + archivo.Extension;
            

            return File(archivo.Archivo, "application/" + archivo.Extension.Replace(".", ""), nombreCompleto);
        }
    }
}