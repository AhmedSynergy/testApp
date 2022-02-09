using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Repositories;
using Catalog.Dtos;
using Catalog.Entities;
using System;
using System.Linq;
namespace Catalog.Controllers
{

    [ApiController] // brings in more additional behaviors
    [Route("items")] // defines which http route this controller will respond to
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository respository;

        public ItemsController(IItemsRepository repository)
        {
            this.respository = repository;        }

        // GET against /items, following method is called.
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = respository.GetItems().Select(item => item.AsDto());
            return items;   
        }

        // will return item .. GET against /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = respository. GetItem(id);

            if(item is null){
                return NotFound();
            }
            return item.AsDto();
        }


        // POST-- add new item
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto){

            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            respository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem),new { id=item.Id}, item.AsDto());
        }

        // PUT/items/{id} --  to update item
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto){
            
            var existingItem = respository.GetItem(id);

            if(existingItem is null){
               return NotFound(); 
            }

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            respository.UpdateItem(updatedItem);

            return NoContent();

        }

        // DELETE/items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id){
            var existingItem = respository.GetItem(id);

            if(existingItem is null){
               return NotFound(); 
            }

            respository.DeleteItem(id);

            return NoContent();
        }


    }
}