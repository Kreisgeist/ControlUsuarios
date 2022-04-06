using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

//Modelo
using Examen.Models;

namespace Examen.Controllers
{
    public class AuthController : Controller
    {
        //Configuración de connection string en Web.config

        /// <summary>
        /// [HTTPEGET]
        ///     List<Users> usuarios - Colección de usuarios traidos desde la BD a través del procedimiento almacenado "sp_consultUserControlUsuarios" instanciado a través del objeto db.
        ///     model - Objeto devuelto a la vista conteniendo la colección completa de usuarios y su información
        /// </summary>
        /// <returns>
        ///     model
        /// </returns>
        [HttpGet]
        public ActionResult Control()
        {
            try
            {
                //No se le puede hacer dispose a la instancia de la BD porque se utilizan referencias asociadas por notación Razor que consultan el catálogo de generos
                ControlUsuariosEntities db = new ControlUsuariosEntities();

                List<Users> usuarios = db.sp_consultUserControlUsuarios().ToList();

                dynamic model = new ExpandoObject();
                model.Users = usuarios;
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// [HTTPEGET]
        ///     List<Genders> generos - Colección de generos (catalogo en BD) traidos desde la BD a través del procedimiento almacenado "sp_consultUserControlUsuarios" instanciado a través del objeto db.
        ///     model - Objeto devuelto a la vista conteniendo la colección completa de usuarios y su información
        /// </summary>
        /// <returns>
        ///     model
        /// </returns>
        [HttpGet]
        public ActionResult Creation()
        {
            try
            {
                List<Genders> generos = new List<Genders>();

                using (ControlUsuariosEntities db = new ControlUsuariosEntities())
                {
                    generos = (from g in db.Genders
                               select g).ToList();
                }

                dynamic model = new ExpandoObject();
                model.Generos = generos;
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// [HTTPEGET]
        ///     List<Users> usuarios - Colección de usuarios traidos desde la BD a través del procedimiento almacenado "sp_consultUserControlUsuarios" instanciado a través del objeto db.
        ///     model - Objeto devuelto a la vista conteniendo la colección completa de usuarios y su información
        /// </summary>
        /// <returns>
        ///     model
        /// </returns>
        [HttpGet]
        public ActionResult Deactivation()
        {
            try
            {
                //No se le puede hacer dispose a la instancia de la BD porque se utilizan referencias asociadas por notación Razor que consultan el catálogo de generos
                ControlUsuariosEntities db = new ControlUsuariosEntities();
                List<Users> usuarios = db.sp_consultUserControlUsuarios().ToList();

                dynamic model = new ExpandoObject();
                model.Users = usuarios;
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// [HTTPEGET]
        ///     List<Users> usuarios - Colección de usuarios traidos desde la BD a través del procedimiento almacenado "sp_consultUserControlUsuarios" instanciado a través del objeto db.
        ///     List<Genders> generos - Colección de generos (catalogo en BD) traidos desde la BD a través del procedimiento almacenado "sp_consultUserControlUsuarios" instanciado a través del objeto db.
        ///     model - Objeto devuelto a la vista conteniendo la colección completa de usuarios y su información
        /// </summary>
        /// <returns>
        ///     model
        /// </returns>
        [HttpGet]
        public ActionResult Updating()
        {
            try
            {
                using (ControlUsuariosEntities db = new ControlUsuariosEntities())
                {
                    List<Users> usuarios = db.sp_consultUserControlUsuarios().ToList();
                    List<Genders> generos = (from g in db.Genders
                                             select g).ToList();

                    dynamic model = new ExpandoObject();
                    model.Users = usuarios;
                    model.Generos = generos;
                    return View(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// [HTTPEPOST]
        ///     Users validationUser - objeto instanciado para buscar algun registro que coincida con el username que está intentandose dar de alta
        ///     Users validationEmail - objeto instanciado para buscar algun registro que coincida con el email que está intentandose dar de alta
        ///     sp_addUserControlUsuarios - Instancia del procedimiento almacenado para dar de alta en BD.
        /// </summary>
        /// <gets>
        ///     User user - Objeto enviado desde la vista con la abstracción de la clase Users
        /// </gets>
        /// <returns>
        ///     Json - Se retorna un valor numerico en Json que simboliza el estatus de la transacción
        /// </returns>
        [HttpPost]
        public JsonResult saveUser(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ControlUsuariosEntities db = new ControlUsuariosEntities())
                    {
                        Users validationUser = (from a in db.Users
                                                where a.user == user.user
                                                select a).FirstOrDefault();

                        Users validationEmail = (from a in db.Users
                                                 where a.email == user.email
                                                 select a).FirstOrDefault();

                        if (validationUser == null && validationEmail == null)
                        {
                            db.sp_addUserControlUsuarios(user.user, user.password, user.email, user.gender);

                            return Json(1, JsonRequestBehavior.AllowGet);
                        }
                        else if (validationUser != null)
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(3, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// [HTTPEPOST]
        ///     Users validation - objeto instanciado para buscar el registro cuyo id coincida con el que se solicita desactivar
        ///     sp_inactivateUserControlUsuarios - Instancia del procedimiento almacenado para la inactivación de usuarios en BD.
        /// </summary>
        /// <gets>
        ///     int id - id del usuario que se quiere desactivar enviado desde la vista
        /// </gets>
        /// <returns>
        ///     Json - Se retorna un valor numerico en Json que simboliza el estatus de la transacción
        /// </returns>
        [HttpPost]
        public JsonResult deactivateUser(int id)
        {
            try
            {
                using (ControlUsuariosEntities db = new ControlUsuariosEntities())
                {
                    Users validation = (from u in db.Users
                                        where u.id == id
                                        select u).FirstOrDefault();

                    if (validation != null)
                    {
                        db.sp_inactivateUserControlUsuarios(id);

                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// [HTTPEPOST]
        ///     Users user - objeto instanciado para buscar el registro que coincida con el id que se solicita reactivar
        ///     db.Entry(user).State = EntityState.Modified; - En este caso se utiliza la libreria Data.Entitiy para updatear el estatus del usuario
        /// </summary>
        /// <gets>
        ///     int id - id del usuario que se quiere reactivar enviado desde la vista
        /// </gets>
        /// <returns>
        ///     Json - Se retorna un valor numerico en Json que simboliza el estatus de la transacción
        /// </returns>
        [HttpPost]
        public JsonResult reactivateUser(int id)
        {
            try
            {
                using (ControlUsuariosEntities db = new ControlUsuariosEntities())
                {
                    Users user = (from u in db.Users
                                  where u.id == id
                                  select u).FirstOrDefault();

                    if (user != null)
                    {
                        user.status = true;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// [HTTPEPOST]
        ///     Users user - objeto instanciado para buscar el registro que coincida con el id que se está consultando
        ///    List<string> infoUsuario - se guarda la información del usuario en un List generico con toda su información formateada en strings
        /// </summary>
        /// <gets>
        ///     int id - id del usuario que se quiere actualizar enviado desde la vista
        /// </gets>
        /// <returns>
        ///     Json - Se retorna un valor numerico en Json que simboliza el estatus de la transacción
        /// </returns>
        [HttpPost]
        public JsonResult getUser(int id)
        {
            try
            {
                using (ControlUsuariosEntities db = new ControlUsuariosEntities())
                {
                    Users user = (from u in db.Users
                                  where u.id == id
                                  select u).FirstOrDefault();

                    if (user != null)
                    {
                        List<string> infoUsuario = new List<string> { user.user, user.password, user.email, user.gender.ToString() };

                        return Json(infoUsuario, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(2, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// [HTTPEPOST]
        ///     Users validationUser - objeto instanciado para comprobar que no exista un usuario dado de alta con el mismo username que se envia para la actualización
        ///     Users validationEmail - objeto instanciado para comprobar que no exista un usuario dado de alta con el mismo email que se envia para la actualización
        ///     sp_updateUserControlUsuarios - instancia del procedimiento almacenado para updatear los campos permitidos del usuario
        /// </summary>
        /// <gets>
        ///     User user - objeto formateado enviado desde la vist con la información que se va a actualizar
        ///     int id - id del usuario que se quiere actualizar enviado desde la vista
        /// </gets>
        /// <returns>
        ///     Json - Se retorna un valor numerico en Json que simboliza el estatus de la transacción
        /// </returns>
        [HttpPost]
        public JsonResult updateUser(Users user, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ControlUsuariosEntities db = new ControlUsuariosEntities())
                    {
                        Users validationUser = (from a in db.Users
                                                where a.user == user.user
                                                && a.id != id
                                                select a).FirstOrDefault();

                        Users validationEmail = (from a in db.Users
                                                 where a.email == user.email
                                                 && a.id != id
                                                 select a).FirstOrDefault();

                        if (validationUser == null && validationEmail == null)
                        {
                            db.sp_updateUserControlUsuarios(id, user.user, user.password, user.email, user.gender);

                            return Json(1, JsonRequestBehavior.AllowGet);
                        }
                        else if (validationUser != null)
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(3, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}