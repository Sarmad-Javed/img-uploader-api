using ImageUploader.Data;
using ImageUploader.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Cors;

namespace ImageUploader.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploaderController : ControllerBase
    {
        private readonly UploaderDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UploaderController(UploaderDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IEnumerable<Image>> Get()
            => await _context.Images.ToListAsync();

        [HttpGet("id")]
        [ProducesResponseType(typeof(Image),StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageById(int id)
        {
            var image = await _context.Images.FindAsync(id);
            return image == null ? NotFound() : Ok(image);
        }

        [HttpPost]
         [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] ImageModel imageObj)
        {
            if (imageObj.file != null)
            {
                string folder = "imgs/";
                imageObj.imageUrl = await UploadImage(folder, imageObj.file);
            }
            var image = new Image()
            {
                imageName = imageObj.imageName,
                imageType = imageObj.imageType,
                imageUrl = imageObj.imageUrl,
                createdAt = DateTime.Now,
            };
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageById), new {id= image.id}, image);
        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "https://localhost:7245/" + folderPath;
        }
    }
    public class ImageModel
    {
        public string imageName { get; set; }
        public string imageUrl { get; set; }
        public string imageType { get; set; }
        public IFormFile file { get; set; }
    }
}
