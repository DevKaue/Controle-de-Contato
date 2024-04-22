using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly ILogger<ContatoController> _logger;
        public ContatoController(IContatoRepositorio contatoRepositorio, ILogger<ContatoController> logger)
        {
            _contatoRepositorio = contatoRepositorio;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodos();
            return View(contatos);
        }

        public IActionResult CriarContato()
        {
            return View();
        }

        public IActionResult EditarContato(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);    
            return View(contato);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        public IActionResult Apagar(int id)
        {
                try
                {
                   bool apagado = _contatoRepositorio.Apagar(id);

                    if(apagado)
                    {
                     TempData["MensagemSucesso"] = "Contato apagado com sucesso!";
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Ops!, não conseguimos apagar seu contato";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Ops!, não foi possível apagar seu contato, tente novamente. Erro: {ex.Message}";
                    return RedirectToAction("Index");
                }
            
        }

        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Ops!, não foi possível cadastrar seu contato, tente novamente. Erro: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }
            // Se houver algum erro no modelo ou uma exceção ao adicionar o contato, 
            // retorne a mesma view com os dados do contato para que o usuário possa corrigir
            return View("CriarContato", contato);
        }


        [HttpPost]
        public IActionResult AtualizarContato(ContatoModel contato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contatoRepositorio.AtualizarContato(contato);
                    TempData["MensagemSucesso"] = "Contato atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Ops!, não foi possível atualizar seu contato, tente novamente. Erro: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }
            // Se houver algum erro no modelo ou uma exceção ao adicionar o contato, 
            // retorne a mesma view com os dados do contato para que o usuário possa corrigir
            return View("EditarContato", contato);
           
        }

    }
}
