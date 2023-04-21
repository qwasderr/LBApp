using LBApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DblibraryContext _context;
        public ChartController(DblibraryContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData() {
            var gs = _context.Genres.ToList();
            List<object> list= new List<object>();
            list.Add(new[] { "Жанр", "Кількість книжок" });
            foreach (var i in gs)
            {
                list.Add(new object[] {i.GenreName, i.Books.Count()});
            }
            return new JsonResult(list);
        }
        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            var gs = _context.Authors.ToList();
            List<object> list = new List<object>();
            list.Add(new[] { "Автор", "Кількість книжок" });
            foreach (var i in gs)
            {
                var temp = from d in _context.AuthorsBooks.Where(d => d.AuthorId == i.AuthorId) select d;

                list.Add(new object[] { i.AuthorName,  temp.Count()});
            }
            return new JsonResult(list);
        }
    }
}
