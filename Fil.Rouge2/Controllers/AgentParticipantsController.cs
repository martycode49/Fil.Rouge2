using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fil.Rouge.DataAccessLayer.Context;
using Fil.Rouge.DataAccessLayer.Models;

namespace Fil.Rouge2.Controllers
{
    public class AgentParticipantsController : Controller
    {
        private FilRougeContext db = new FilRougeContext();

        // GET: AgentParticipants
        public async Task<ActionResult> Index()
        {
            var agentParticipants = db.AgentParticipants.Include(a => a.Agent);
            return View(await agentParticipants.ToListAsync());
        }

        // GET: AgentParticipants/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentParticipant agentParticipant = await db.AgentParticipants.FindAsync(id);
            if (agentParticipant == null)
            {
                return HttpNotFound();
            }
            return View(agentParticipant);
        }

        // GET: AgentParticipants/Create
        public ActionResult Create()
        {
            ViewBag.IdAgent = new SelectList(db.Agents, "Id", "Civility");
            return View();
        }

        // POST: AgentParticipants/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,IdAgent,Datecreate,QuestionQty,Status")] AgentParticipant agentParticipant)
        {
            if (ModelState.IsValid)
            {
                db.AgentParticipants.Add(agentParticipant);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.IdAgent = new SelectList(db.Agents, "Id", "Civility", agentParticipant.IdAgent);
            return View(agentParticipant);
        }

        // GET: AgentParticipants/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentParticipant agentParticipant = await db.AgentParticipants.FindAsync(id);
            if (agentParticipant == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAgent = new SelectList(db.Agents, "Id", "Civility", agentParticipant.IdAgent);
            return View(agentParticipant);
        }

        // POST: AgentParticipants/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,IdAgent,Datecreate,QuestionQty,Status")] AgentParticipant agentParticipant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentParticipant).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.IdAgent = new SelectList(db.Agents, "Id", "Civility", agentParticipant.IdAgent);
            return View(agentParticipant);
        }

        // GET: AgentParticipants/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentParticipant agentParticipant = await db.AgentParticipants.FindAsync(id);
            if (agentParticipant == null)
            {
                return HttpNotFound();
            }
            return View(agentParticipant);
        }

        // POST: AgentParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AgentParticipant agentParticipant = await db.AgentParticipants.FindAsync(id);
            db.AgentParticipants.Remove(agentParticipant);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
