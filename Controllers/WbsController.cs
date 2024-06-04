﻿using Microsoft.AspNetCore.Mvc;
using myte.Models;
using myte.Services;

namespace myte.Controllers
{
    public class WbsController : Controller
    {
        private WbsService _wbsService;

        public WbsController(WbsService wbsService)
        {
            _wbsService = wbsService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var wbs = await _wbsService.GetAllWbsAsync();
                return View(wbs);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível listar os registros.");
                return View(new List<Wbs>());
            }

            //return View(Repository.TodasAsWbs);

        }

        //recupera uma unica wbs

        /*public ViewResult GetWbsUnica() => View();

        //sobrecarga

        [HttpPost]
        public async Task<ActionResult> GetWbsUnica(string codigo) //testar essa action sem o Iactionresult (para nao retornar uma view) na busca de wbs
        {
            var wbs = await _wbsService.GetWbsByIdAsync(codigo);

            if(wbs == null)
            {
                return NotFound();
            }
            return View(wbs);
        }*/

        public ViewResult CreateWbs() => View();


        [HttpPost]
        public async Task<IActionResult> CreateWbs(Wbs wbs)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _wbsService.AddWbsAsync(wbs);
                    return RedirectToAction("Index");

                    TempData["SuccessMessage"] = "Wbs cadastrada com sucesso!";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erro ao criar registro");
                }

            }

            return View(wbs);
            TempData["ErrorMessage"] = "Não é possivel prosseguir com a ação";

        }


        public async Task<IActionResult> UpdateWbs(string codigo)
        {
            var wbs = await _wbsService.GetWbsByIdAsync(codigo);

            if (wbs == null)
            {
                return NotFound();
            }
            return View(wbs);

            /*Wbs consulta = Repository.TodasAsWbs.Where((r) => r.Codigo == id).First();
            return View(consulta);*/
        }

        //sobrecarga
        [HttpPost]
        public async Task<IActionResult> UpdateWbs(string codigo, Wbs wbs) //alterei o nome de Create para CreateWbs
        {

            if (codigo != wbs.Codigo)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _wbsService.UpdateWbsAsync(codigo, wbs);
                return Redirect("Index");
            }

            return View();

            /*if (ModelState.IsValid)
            {
                var consulta = Repository.TodasAsWbs.Where((r) => r.Codigo == id).First();

                consulta.Nome = wbsAlterada.Nome;
                consulta.Descricao = wbsAlterada.Descricao;
                consulta.Tipo = wbsAlterada.Tipo;
                consulta.Codigo = wbsAlterada.Codigo;

                
            }
           */

        }

        [HttpPost]
        public async Task<IActionResult> DeleteWbs(string codigo)
        {
            var wbs = await _wbsService.GetWbsByIdAsync(codigo);

            if (wbs == null)
            {
                return NotFound();
            }

            await _wbsService.DeleteWbsAsync(codigo);
            return RedirectToAction(nameof(Index));

            TempData["SuccessMessageDelete"] = "exclusao realizada com sucesso!";


            /*try
            {
                Wbs consulta = Repository.TodasAsWbs.Where((r) => r.Codigo == id).First();
                Repository.Excluir(consulta);
                
                
            }
            catch
            {
                TempData["ErrorMessageDelete"] = "Não é possivel prosseguir com a ação";
                return Redirect("Index");
            }*/
        }
    }
}
