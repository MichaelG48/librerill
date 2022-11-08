using DgBooksDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DgBooks.Models
{
    public class MySession
    {
        // private constructor
        private MySession()
        {
            IdUsuario = 0;
            NombreUsuario = "";
            EmailUsuario = "";
            Usuario = null;
            intIdLibor = 0;
            IdGenero = 0;
            Generos = new List<Genero>();

            MensageGeneral = "";
            TipoMensage = "";

            Property1 = "default value";
        }

        // Gets the current session.
        public static MySession Current
        {
            get
            {
                MySession session =
                  (MySession)HttpContext.Current.Session["__MySession__"];
                if (session == null)
                {
                    session = new MySession();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }

        // **** add your session properties here, e.g like this:
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int intIdLibor { get; set; }
        public int IdGenero { get; set; }
        public List<Genero> Generos { get; set; }
        public string MensageGeneral { get; set; }
        public string TipoMensage { get; set; }
        public string Property1 { get; set; }
        public DateTime MyDate { get; set; }
        public int LoginId { get; set; }
    }

}