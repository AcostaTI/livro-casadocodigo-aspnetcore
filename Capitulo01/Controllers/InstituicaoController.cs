using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capitulo01.Data;
using Capitulo01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capitulo01.Controllers
{
    public class InstituicaoController : Controller
    {
        private readonly IESContext _context;

        public InstituicaoController(IESContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instituicoes.OrderBy(i => i.Nome).ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Nome, Endereco")] Instituicao instituicao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(instituicao);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {

                ModelState.AddModelError("", "Não foi possivel inserir os dados.");
            }

            return View(instituicao);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoID == id);

            if (instituicao == null)
            {
                return NotFound();
            }

            return View(instituicao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long? id, [Bind("InstituicaoID, Nome, Endereco")] Instituicao instituicao)
        {
            if (id != instituicao.InstituicaoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instituicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!InstituicaoExists(instituicao.InstituicaoID))
                    {
                        return NotFound();
                    } else
                    {
                        throw;
                    }

                }
                return RedirectToAction(nameof(Index));
            }

            return View(instituicao);
        }

        private bool InstituicaoExists(long? id)
        {
            return _context.Departamentos.Any(i => i.DepartamentoID == id);
        }

        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoID == id);
            
            if (instituicao == null)
            {
                return NotFound();
            }

            return View(instituicao);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoID == id);

            if (instituicao == null)
            {
                return NotFound();
            }

            return View(instituicao);
          
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long? id)
        {
            var instituicao = await _context.Instituicoes.SingleOrDefaultAsync(i => i.InstituicaoID == id);
            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Instituição " + instituicao.Nome.ToUpper() + " foi removida";
            return RedirectToAction(nameof(Index));
        }
    }
}
