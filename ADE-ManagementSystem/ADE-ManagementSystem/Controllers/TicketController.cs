using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ADE_ManagementSystem.Models;
using ADE_ManagementSystem.Models.Global;

namespace ADE_ManagementSystem.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> GetBaseData()
        {
            try
            {
                using (Entities db = new Entities())
                {
                    List<Ticket> list = db.Tickets.ToList();

                    return Json(new ResponseResultWithEntityViewModel<List<Ticket>>
                    {
                        Entity = list
                    }.Succeed(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new ResponseResultViewModel(e.Message).Failure(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}