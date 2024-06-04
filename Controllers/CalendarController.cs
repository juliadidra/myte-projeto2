using Microsoft.AspNetCore.Mvc;
using myte.Models;
using myte.Services;

namespace myte.Controllers
{
    public class CalendarController : Controller
    {
        private RegistroHorasService _registroHorasService;
        private WbsService _wbsService;

        public CalendarController(RegistroHorasService registroHorasService, WbsService wbsService)
        {
            _registroHorasService = registroHorasService;
            _wbsService = wbsService;
        }
        
        //[HttpGet]
        public async Task<IActionResult> Index(int quinzena = 1)
        {
            var wbsList = await _wbsService.GetAllWbsAsync();
            var registroHorasList = await _registroHorasService.GetRegistroHorasAsync();

            var model = new CalendarPageViewModel
            {
                WbsList = wbsList,
                RegistroHorasList = registroHorasList,
                Quinzena = quinzena,
                CalendarDays = GetCalendarDays(DateTime.Now, quinzena, registroHorasList)
            };
            return View(model);
        }


     

        public async Task<IActionResult> Create()
        {
            ViewBag.WbsList = await _wbsService.GetAllWbsAsync();
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistroHoras registroHoras)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    await _registroHorasService.AddRegistroHorasAsync(registroHoras);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    // Log the error message
                    Console.WriteLine($"Request error: {e.Message}");
                    ModelState.AddModelError(string.Empty, "Erro ao enviar os dados para a API.");
                }
            }
            ViewBag.WbsList = await _wbsService.GetAllWbsAsync();
            return View(registroHoras);
        }

        private List<CalendarViewModel> GetCalendarDays(DateTime date, int quinzena, List<RegistroHoras> registroHorasList)
        {
            var days = new List<CalendarViewModel>();
            var year = date.Year;
            var month = date.Month;
            var startDay = quinzena == 1 ? 1 : 16;
            var endDay = quinzena == 1 ? 15 : DateTime.DaysInMonth(year, month);

            for (int day = startDay; day <= endDay; day++)
            {
                var dateTime = new DateTime(year, month, day);
                var dateOnly = DateOnly.FromDateTime(dateTime); // Converte DateTime para DateOnly
                var registro = registroHorasList.FirstOrDefault(r => r.Dia == dateOnly); // Compara DateOnly diretamente
                days.Add(new CalendarViewModel
                {
                    Day = day,
                    DayOfWeek = dateTime.DayOfWeek.ToString(),
                    Hours = registro?.Horas ?? 0,
                    WbsCodigo = registro?.WBS_Codigo
                });
            }

            return days;
        }

        /*public IActionResult Index()
        {
            var model = new CalendarPageViewModel
            {
                //a lista de wbs esta vindo do repository por enquanto, porem, posteriormente precisara vir da API
                WbsList = Repository.TodasAsWbs.ToList(),
                CalendarDays = GetCalendarDays(DateTime.Now, 1) //os parametros da função indicam a data e horas atuais e a quinzena (1 ou 2)
            
                //essas informações serão carregadas na view index
            };
            return View(model);
        }*/

        //função que tera uma lista do tipo CalendarViewModel(dias, dias da semana e horas)
        /*private List<CalendarViewModel> GetCalendarDays(DateTime date, int quinzena)
        {
            //estabelece as variaveis que irao armazenar os dados do tempo
            var days = new List<CalendarViewModel>(); // a variavel days ira armazenar um objeto que é uma lista do tipo CalendarViewModel (dias, dias da semana e horas)
            var year = date.Year; //variavel que ira puxar o ano atual atraves do parametro "date" da função
            var month = date.Month; //variavel que ira puxar o mes atual atraves do parametro "date" da função
            var startDay = quinzena == 1 ? 1 : 16; //variavel que identifica: se a quinzena inserida no parametro for igual a 1, entao a variavel startDay ir assumir o valor 1.
            //se nao (ou seja, se a variavel quinzena for diferente de 1, a variavel startday ira assumir o valor 16
            
            var endDay = quinzena == 1 ? 15 : DateTime.DaysInMonth(year, month); //ja aqui, se a quinzena for igual a 1, o valor de endDay ser igual a 15
            //caso quinzena seja diferente de 1, endDay irá ter o valor do ultimo dia do mes atual.

            for (int day = startDay; day <= endDay; day++) // esse for ira construir os dias do mes a partir da quinzena (armazenada na variavel startDay)
                //e finalizara ate o ultimo dia do mes atual (endDay). - caso endDay seja igual 15, o calendario ira ter o ultimo dia como 15 (1 quinzena)
                //caso nao seja, ira terminar no ultimo dia do mes atual.
            {
                var dateTime = new DateTime(year, month, day);
                days.Add(new CalendarViewModel // ira adicionar na lista de CalendarViewModel, um objeto de mesmo tipo, 
                //com o dia (que esta sendo incrementado no for), o dia da semana (atraves da variavel datetime) e as horas estarao zeradas pois é o usuario quem ira inseri-las
                {
                    Day = day,
                    DayOfWeek = dateTime.DayOfWeek.ToString(),
                    
                    Hours = 0
                });
            }

            return days; //ira retornar os dias da quinzena (juntamente com o dia da semana)
        }*/

        /*[HttpGet]
        public JsonResult GetDays(DateTime date, int quinzena)
        {
            var registroHorasList = _registroHorasService.GetRegistroHorasAsync().Result;
            var days = GetCalendarDays(date, quinzena, registroHorasList);
            return Json(days);
        }*/

        [HttpGet]
        public async Task<JsonResult> GetAllWbs()
        {
            var wbsList = await _wbsService.GetAllWbsAsync();
            return Json(wbsList);
        }



        public async Task<IActionResult> UpdateRegistroHoras(int id)
        {
            var registroHoras = await _registroHorasService.GetRegistroHorasByIdAsync(id);
            if (registroHoras == null)
            {
                return NotFound();
            }

            ViewBag.WbsList = await _wbsService.GetAllWbsAsync();
            return View(registroHoras);
        }

        [HttpPost]
  
        public async Task<IActionResult> UpdateRegistroHoras(int id, RegistroHoras registroHoras)
        {
            if (id != registroHoras.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     await _registroHorasService.UpdateRegistroHorasAsync(id, registroHoras);
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException e)
                {
                    // Log the error message
                    Console.WriteLine($"Request error: {e.Message}");
                    ModelState.AddModelError(string.Empty, "Erro ao enviar os dados para a API.");
                }
            }

            ViewBag.WbsList = await _wbsService.GetAllWbsAsync();
            return View(registroHoras);
        }

        /*[HttpDelete]
        public async Task<IActionResult> DeleteRegistroHoras(int id)
        {
            var result = await _registroHorasService.DeleteRegistroHorasAsync(id);
            return Json(result);
        }*/
    }
}