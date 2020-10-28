using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using AutoMapper;
using DevIO.Business.Interfaces;
using AppMvcBasica.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DevIO.App.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fonecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fonecedorRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _fonecedorRepository = fonecedorRepository;
            _mapper = mapper;
        }



        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutoFornecedores()));
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(Guid id)
        {

            var produtoViewModel = await ObterProduto(id) ;
                
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        // GET: Produtos/Create
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);
            if (!ModelState.IsValid) return View(produtoViewModel);


            var imgPrefixo = Guid.NewGuid() + "_";
            if(!await UploadArquivo(produtoViewModel.ImagemUpload,imgPrefixo))
            {
                return View(produtoViewModel);
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return View(produtoViewModel);
            
           
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            if (ModelState.IsValid) return View(produtoViewModel);

            await _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoViewModel));
            return RedirectToAction(nameof(Index));
            
            
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var produto = await ObterProduto(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto == null)
            {
                return NotFound();
            }

            await _produtoRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto= _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fonecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fonecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);
            
            if(System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }


            return true;
        }

    }
}
