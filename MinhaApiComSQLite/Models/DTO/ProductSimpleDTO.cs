﻿namespace MinhaApiComSQLite.Models.DTO
{
    public class ProductSimpleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
