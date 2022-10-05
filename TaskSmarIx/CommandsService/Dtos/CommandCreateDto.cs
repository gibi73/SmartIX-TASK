using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos
{
    public class CommandCreateDto
    {
        [Required]
        public string HowLine { get; set; }

        [Required]
        public string CommandLine { get; set; }
    }
}