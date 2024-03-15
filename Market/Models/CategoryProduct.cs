using AutoMapper;
using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Market.Models
{
    public class CategoryProduct
    {
        // Уникальный идентификатор связи
        public Guid? Id { get; set; }

        // Внешний ключ для категории
        public Guid? CategoryId { get; set; }

        // Внешний ключ для продукта
        public Guid? ProductId { get; set; }

        // Навигационное свойство для категории
        public virtual Category? Category { get; set; }

        // Навигационное свойство для продукта
        public virtual Product? Product { get; set; }
    }
}