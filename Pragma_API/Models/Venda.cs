﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pragma_API.Models
{
    [Keyless]
    public partial class Venda
    {

        public int Pedido { get; set; }

        public int EventoID { get; set; }

        public string NomeEvento { get; set; }
        public string CategoriaEvento { get; set; }

        public DateTime? DataTransacao { get; set; }

        public int QtdTotal { get; set; }

        public decimal ValorTransacao { get; set; }

        public string NomeCliente { get; set; }

        public string Email { get; set; }

        public string TipoDocumento { get; set; }

        public string Documento { get; set; }

        public string Endereco { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }

        public string Uf { get; set; }

        public string Movimento { get; set; }

        public List<VendaItem> Itens { get; set; }
    }
}