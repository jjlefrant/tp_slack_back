using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlackMVCBack.Models;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;

namespace SlackMVCBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DbSlackContext _context;
        public HomeController(ILogger<HomeController> logger, DbSlackContext context)
        {
            _context = context;
            _logger = logger;
        }

        // USER. effectue une requête HTTP POST vers 'http://localhost:3000/users' 
        //pour vérifier l'existence du user
        [HttpPost]
        [Route("/users")]
        public IActionResult loginUser([FromBody] UserInput user)
        {
            Console.WriteLine("gate 0 - loginUser");
            Console.WriteLine("Username");

            Console.WriteLine($">{user.Username}<");
            //User to_ret = _context.Users.Find(1);
            User to_ret = _context.Users.Where(u => u.Name == user.Username).FirstOrDefault();
            return Json(to_ret);

        }

        [HttpGet]
        [Route("/users")]
        public IActionResult getUsers()
        {
            //recuperer une liste de users

            var users = _context.Users;

            //renvoyer la liste en json

            return Json(users);
        }

        // 1. Cette méthode doit effectuer une requête HTTP GET :'http://localhost:3000/threads pour récupérer la liste des threads
        [HttpGet]
        [Route("/threads")]
        public IActionResult getThreads()
        {
            //recuperer une liste de threads

            var threads = _context.Threads;

            //renvoyer la liste en json

            return Json(threads);
        }


        // 2. Cette méthode doit effectuer une requête HTTP GET 'http://localhost:3000/threads/{id}'
        // pour récupérer un thread spécifique en fonction de l'ID fourni.
        [HttpGet]
        [Route("/threads/{id}")]
        public IActionResult getThread(int id)
        {
            //recuperer un  thread

            var thread = _context.Threads.Find(id);
            //renvoyer un thread en json

            return Json(thread);
        }

        // 3. Cette méthode doit effectuer une requête HTTP POST 'http://localhost:3000/threads'
        // pour créer un nouveau thread. Elle prend en paramètre un objet thread à ajouter.
        [HttpPost]
        [Route("/threads")]
        public async Task<IActionResult> createThread([FromBody] Thread newThread)
        {
            try
            {
                if (newThread == null)
                {
                    return BadRequest("Les données du fil de discussion sont nulles.");
                }

                _context.Threads.Add(newThread);
                await _context.SaveChangesAsync();

                // Retourne une réponse JSON avec le nouveau fil de discussion
                return Json(newThread);
            }
            catch (Exception ex)
            {
                // Gérez l'exception de manière appropriée (journalisation, renvoi d'un message d'erreur, etc.)
                return StatusCode(500, "Une erreur interne s'est produite lors de la création du fil de discussion.");
            }
        }

        // 4. effectue une requête HTTP PUT vers 'http://localhost:3000/threads/{id}' pour mettre à jour un thread existant.
        // Elle prend en paramètre un objet thread contenant les modifications à apporter.
        [HttpPut]
        [Route("/threads/{id}")]
        public IActionResult updateThread(int id, Thread updatedThread)
        {
            Console.WriteLine($"------updateThread {id}--------");
            var existingThread = _context.Threads.Find(id);

            existingThread.Messages=updatedThread.Messages;
            existingThread.Label = updatedThread.Label;

            _context.Update(existingThread);
            _context.SaveChanges();

            return Json(existingThread);
        }

        // 5. effectue une requête HTTP DELETE vers 'http://localhost:3000/threads/{id}'
        // pour supprimer un thread en fonction de l'ID fourni.
        [HttpDelete]
        [Route("/threads/{id}")]
        public IActionResult deleteThread(int id)
        {
            Console.WriteLine($"------deleteThread {id}--------");

            var thread1 = _context.Threads.Find(id);

            _context.Threads.Remove(thread1);

            _context.SaveChanges();

            return Ok();

        }

        // 1.  Cette méthode doit effectuer une requête HTTP GET vers 'http://localhost:3000/messages' pour récupérer la liste des messages.
        /*
        [HttpGet]
        [Route("/messages")]
        public IActionResult getMessages()
        {
            DbSet<Message> ma_liste_messages = _context.Messages;
            return Json(ma_liste_messages.OrderByDescending(m => m.Date).ToList());
        }*/

        // 2. Cette méthode doit effectuer une requête HTTP GET vers 'http://localhost:3000/messages/{id}'
        // pour récupérer un message spécifique en fonction de l'ID fourni.
        /*
        [HttpGet]
        [Route("/messages/{id}")]
        public IActionResult getMessage(int id)
        {
            Message mon_messages = _context.Messages.Find(id);
            return Json(mon_messages);
        }*/

        // 3. 
        [HttpGet]
        [Route("/messages")]
        public IActionResult GetMessages(int? threadId)
        {
            IQueryable<Message> messages = _context.Messages;
            if (threadId != null)
            {
                messages = messages.Where(m => m.ThreadId == threadId);
            }

            return Ok(messages);
        }

        // 4. Cette méthode doit effectuer une requête HTTP POST vers 'http://localhost:3000/messages' 
        //pour créer un nouveau message
        [HttpPost]
        [Route("/messages")]
        public async Task<IActionResult> createMessage([FromBody] Message newMessage)
        {
            try
            {
                if (newMessage == null)
                {
                    return BadRequest("Les données du message sont nulles.");
                }

                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();

                // Retourne une réponse JSON avec le nouveau message
                return Json(newMessage);
            }
            catch (Exception ex)
            {
                // Gérez l'exception de manière appropriée (journalisation, renvoi d'un message d'erreur, etc.)
                return StatusCode(500, "Une erreur interne s'est produite lors de la création du message.");
            }

        }

        // 6.
        [HttpDelete]
        [Route("/messages/{id}")]
        public IActionResult deleteMessage(int id)
        {
            Console.WriteLine($"------deleteMessage {id}--------");

            var messages = _context.Messages.Find(id);

            _context.Messages.Remove(messages);

            _context.SaveChanges();

            return Ok();

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
