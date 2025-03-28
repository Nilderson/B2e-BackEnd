using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Produtos.Core.Entities;
using Produtos.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtos.Core.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> ListAllProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetById(id) ?? throw new Exception("Produto não encontrado.");
        }

        public async Task<bool> AddProduct(Product produto)
        {
            if (string.IsNullOrEmpty(produto.Name) || produto.Amount <= 0)
                return false;

            await _productRepository.Add(produto);
            return true;
        }

        public async Task<bool> UpdateProduct(Product produto)
        {
            if (string.IsNullOrEmpty(produto.Name) || produto.Amount <= 0)
                return false;

            var exist = await _productRepository.GetById(produto.Id);
            if (exist == null) throw new Exception("Produto não encontrado.");

            exist.Name = produto.Name;
            exist.Amount = produto.Amount;

            await _productRepository.Update(exist);
            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var exist = await _productRepository.GetById(id);
            if (exist == null) throw new Exception("Produto não encontrado.");

            await _productRepository.Delete(id);
            return true;
        }
        public async Task<FileContentResult> ExportProductsToExcel()
        {
            var products = await _productRepository.GetAll();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Produtos");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nome";
                worksheet.Cell(1, 3).Value = "Valor (R$)";
                worksheet.Cell(1, 4).Value = "Data de Criação";

                int row = 2;
                foreach (var product in products)
                {
                    worksheet.Cell(row, 1).Value = product.Id;
                    worksheet.Cell(row, 2).Value = product.Name;
                    worksheet.Cell(row, 3).Value = product.Amount;
                    worksheet.Cell(row, 4).Value = product.DataCreate.ToString("dd/MM/yyyy HH:mm");
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Produtos.xlsx"
                    };
                }
            }
        }
    }
}
