using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TubeToTune.Dtos
{
    public class VideoConversionErrorDetailsDto
    {
        public int VideoId { get; set; }
        public string VideoConversionError { get; set; }
    }
}
