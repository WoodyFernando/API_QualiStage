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
    public partial class VendaItem
    {

        public DateTime? DataShow { get; set; }

        public string Setor { get; set; }

        public string NomePreco { get; set; }

        public int QtdIngressos { get; set; }

        public decimal ValorTotal { get; set; }

        public decimal ValorUnitario { get; set; }

        public string TipoPreco { get; set; }
    }
}