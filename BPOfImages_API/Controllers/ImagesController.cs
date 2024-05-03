using Microsoft.AspNetCore.Mvc;

namespace BPOfImages_API.Controllers
{
    [Route ("api/images")]
    [ApiController]

    public class ImagesController : ControllerBase
    {
        /*GET*/
        [HttpGet("GetImage")]
        public IActionResult GetImage()
        {
            // folder where the images are stored
            string folderPath = "Images";

            string fileName = "fruits2.jpg";

            /* full path to the image */
            string pathToFile = Path.Combine(folderPath, fileName);

            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }
            // Read the file as bytes
            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            // Return the file as a FileResult with the appropriate content type
            return File(bytes, "image/jpeg", fileName);
        }

        /*----------------------------------------------------------------------------------------------------------------------------------------------------*/

        /*UPLOAD*/
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] int id ,
            [FromForm] string description, [FromForm] string newName, [FromForm] string location)
        {
            if (file == null || file.Length == 0 || file.ContentType == "images/jpeg")
            {
                return BadRequest("No file uploaded.");
            }

            if (string.IsNullOrEmpty(file.FileName))
            {
                return BadRequest("Provide file name");
            }

            string uploadsFolder = Path.Combine("Images");

            // Ensure the uploads folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            /*full path*/
            string filePath = Path.Combine(uploadsFolder, $"{newName}{Path.GetExtension(file.FileName)}");

            /* Copy the uploaded file to the uploads folder */
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(new { filePath });
        }

        /*----------------------------------------------------------------------------------------------------------------------------------------------------*/

        /*DELETE*/
        [HttpDelete("DeleteImage")]
        public IActionResult DeleteImage() 
        {
            string folderPath = "Images";

            string fileName = "$tomatoes1.jpg";

            string pathToFile = Path.Combine(folderPath, fileName);

            System.IO.File.Delete(pathToFile);

            return Ok("Image successfully deleted.");
        }            
    }
}
