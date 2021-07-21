using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Fil.Rouge.DataAccessLayer.Context;
using Fil.Rouge.DataAccessLayer.Models;
using PagedList;

namespace Fil.Rouge2.Controllers
{
    public class QuizManagerController : Controller
    {
        private FilRougeContext db = new FilRougeContext();

        // GET: QuizManager
        public ActionResult Index(string sortOrderParticipant, string searchStrParticipant)
        {
            var agents = GetAgents();
            var participants = GetParticipants();
            var quizs = GetQuizs();
            

            // Gestion du tri de colonne Name
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrderParticipant) ? "name_desc" : "";

            QuizParticipantVM model = new QuizParticipantVM();
            model.Agents = (List<Agent>)agents;
            model.Participants = (List<Participant>)participants;
            model.Quizzes = (List<Quiz>)quizs;

            var part = from s in db.Participants
                       select s;
            if (!String.IsNullOrEmpty(searchStrParticipant))
            {
                part = part.Where(s => s.Lastname.Contains(searchStrParticipant));
                model.Participants = part.ToList();
            }

            // système de tri sur le Lastname
            switch (sortOrderParticipant)
            {
                case "name_desc":
                    model.Participants = model.Participants.OrderByDescending(s => s.Lastname).ToList();
                    break;
                default:
                    model.Participants = model.Participants.OrderBy(s => s.Lastname).ToList();
                    break;
            }
            return View(model);
        }

        // POST - Selected quiz
        [HttpPost]
        public ActionResult SaveSelectedQuiz(List<int> SelectedStatus) //,int participantId)
        {
            List<ParticipantData> pdataToAdd = new List<ParticipantData>();
            foreach (var item in SelectedStatus)
            {
                pdataToAdd.Add(new ParticipantData
                {
                    QuizId = 1,
                    QuizParticipId = 1,
                    QuizQuestionId = item,
                });
            }
            db.ParticipantDatas.AddRange(pdataToAdd);
            db.SaveChanges();

            ViewBag.Count = pdataToAdd.Count();

            return View(pdataToAdd);
        }

        private object GetQuizs()
        {
            return db.Quizs.ToList();
        }

        private object GetParticipants()
        {
            return db.Participants.ToList();
        }

        public object GetAgents()
        {
            return db.Agents.ToList();
        }
    }
}