﻿using System.ComponentModel.DataAnnotations;

namespace FastFood.Services.Web.ViewModels.Items
{
    public class CreateItemInputModel
    {
        
        public string Name { get; set; } = null!;

        
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}
