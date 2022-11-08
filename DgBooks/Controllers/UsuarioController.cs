using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DgBooks.Comodin;
using DgBooks.Models;
using DgBooksDB;
using DgBooksDetails.DgBServices;

namespace DgBooks.Controllers
{
    public class UsuarioController : Controller
    {
        personaServices personaServices = new personaServices();
        usuarioServices services = new usuarioServices();
        validarServices validarServices = new validarServices();
        normalizarServices normalizar = new normalizarServices();

        // VARIABLES DE SESION PARA GUARDAR DATOS DEL USUARIO LOGUEADO

        // GET: Usuario
        public ActionResult Index()
        {
            //setSessionProperties();
            
            return RedirectToAction("LogIn");
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            ViewBag.sms = "";
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserLog log)
        {
            if (log.strUser_Email != null && log.strPassword != null)
            {
                var user = services.GetUsuarioByNameOrEmail(log.strUser_Email);
                if(user != null)
                {
                    if (user.Status == "Activo") 
                    { 
                        if (log.strPassword == user.strContraseña)
                        {
                        MySession.Current.IdUsuario = user.intIdUsuario;
                        MySession.Current.NombreUsuario = user.strNombreUsuario;
                        MySession.Current.EmailUsuario = log.strUser_Email;
                        MySession.Current.Usuario = user;

                        //MySession.Current.IdUsuario = user.intIdUsuario;
                        //Session["NombreUsuario"] = user.strNombreUsuario;
                        //Session["EmailUsuario"] = log.strUser_Email;
                        //MySession.Current.Usuario = user;
                        return RedirectToAction("PantallaP", "Libros");
                        }
                        ViewBag.sms = "Contraseña incorrecta";
                        return View();
                    }
                    ViewBag.sms = "Su suscripcion a vencido";
                    return View();
                }
                ViewBag.sms = "El usuario no existe";
                return View();
            }
            else
            {
                ViewBag.sms = "No dejes campos vacios";
                return View();
            }
        }

        private void setSessionProperties()
        {
            MySession.Current.IdUsuario = 0;
            Session["NombreUsuario"] = "";
            Session["EmailUsuario"] = "";
            MySession.Current.Usuario = null;
            MySession.Current.intIdLibor = 0;
            Session["IdGenero"] = 0;
            //Session["Generos"] = new List<Genero>();

            Session["MensageGeneral"] = "";
            Session["TipoMensage"] = "";
        }

        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(newUser user)
        {
            if (user!=null)
            {
                if(user.strContraseña == user.strConfirmContraseña)
                {
                    Usuario usuario = normalizar.normUsuario(user);
                    Persona persona = normalizar.normPersona(user);
                    if (validarServices.ValidPersona(persona) && validarServices.ValidUsuario(usuario)) 
                    {
                        if (personaServices.Insert(persona))
                        {
                            int id = personaServices.GetUltimaPersona();
                            usuario.intIdPersona = id;
                            usuario.Status = "Activo";
                            if (services.Insert(usuario))
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                personaServices.Delete(id);
                            }
                        }
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditarUsuario()
        {
            try
            {
                int id = Convert.ToInt32(MySession.Current.IdUsuario);
                var user = services.GetUsuarioById(id);
                var newU = normalizar.changePass(user);
                return View(newU);
            }
            catch
            {
                return RedirectToAction("PantallaP","Libros");
            }
        }

        [HttpPost]
        public ActionResult EditarUsuario(newPassword newPassword)
        {
            int id = newPassword.id;
            try
            {
                if(newPassword != null)
                {
                    if(newPassword.passI == newPassword.passII)
                    {
                        var user = services.GetUsuarioById(id);
                        var actUser=normalizar.norEditUser(newPassword, user);
                        if (services.Update(actUser))
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch
            {

            }
            return RedirectToAction("PantallaP", "Libros", new { id = id });
        }

    }
}