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
    public class ParticipantDatasController : Controller
    {
        private FilRougeContext db = new FilRougeContext();

        // GET: ParticipantDatas
        public async Task<ActionResult> Index()
        {
            var participantDatas = db.ParticipantDatas.Include(p => p.AgentParticipant).Include(p => p.Participant).Include(p => p.Quiz);
            return View(await participantDatas.ToListAsync());
        }

        // GET: ParticipantDatas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipantData participantData = await db.ParticipantDatas.FindAsync(id);
            if (participantData == null)
            {
                return HttpNotFound();
            }
            return View(participantData);
        }

        // GET: ParticipantDatas/Create
        public ActionResult Create()
        {
            ViewBag.QuizId = new SelectList(db.AgentParticipants, "Id", "Id");
            ViewBag.QuizParticipId = new SelectList(db.Participants, "Id", "Lastname");
            ViewBag.QuizQuestionId = new SelectList(db.Quizs, "Id", "QuizId");
            return View();
        }

        // POST: ParticipantDatas/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,QuizId,QuizQuestionId,QuizParticipId,QuizValidAnswer,QuizQstart,QuizQend,QuizCommentPart,QuizFreeAnswer,QuizValidFreeAnswer")] ParticipantData participantData)
        {
            if (ModelState.IsValid)
            {
                db.ParticipantDatas.Add(participantData);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.QuizId = new SelectList(db.AgentParticipants, "Id", "Id", participantData.QuizId);
            ViewBag.QuizParticipId = new SelectList(db.Participants, "Id", "Lastname", participantData.QuizParticipId);
            ViewBag.QuizQuestionId = new SelectList(db.Quizs, "Id", "QuizId", participantData.QuizQuestionId);
            return View(participantData);
        }

        // GET: ParticipantDatas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipantData participantData = await db.ParticipantDatas.FindAsync(id);
            if (participantData == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuizId = new SelectList(db.AgentParticipants, "Id", "Id", participantData.QuizId);
            ViewBag.QuizParticipId = new SelectList(db.Participants, "Id", "Lastname", participantData.QuizParticipId);
            ViewBag.QuizQuestionId = new SelectList(db.Quizs, "Id", "Question", participantData.QuizQuestionId);
            //ViewBag.QuizQuestionId = new SelectList(db.Quizs, "Id", "QuizId", participantData.QuizQuestionId);

            return View(participantData);
        }

        // POST: ParticipantDatas/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,QuizId,QuizQuestionId,QuizParticipId,QuizValidAnswer,QuizQstart,QuizQend,QuizCommentPart,QuizFreeAnswer,QuizValidFreeAnswer")] ParticipantData participantData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participantData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.QuizId = new SelectList(db.AgentParticipants, "Id", "Id", participantData.QuizId);
            ViewBag.QuizParticipId = new SelectList(db.Participants, "Id", "Lastname", participantData.QuizParticipId);
            ViewBag.QuizQuestionId = new SelectList(db.Quizs, "Id", "QuizId", participantData.QuizQuestionId);
            return View(participantData);
        }

        // GET: ParticipantDatas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParticipantData participantData = await db.ParticipantDatas.FindAsync(id);
            if (participantData == null)
            {
                return HttpNotFound();
            }
            return View(participantData);
        }

        // POST: ParticipantDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ParticipantData participantData = await db.ParticipantDatas.FindAsync(id);
            db.ParticipantDatas.Remove(participantData);
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
