using System.Collections.Generic;

namespace TubeToTune.Dtos
{
    public class ConversionRequest
    {
        public IEnumerable<VideoConversionDetailsDto> VideoConversionDetails { get; set; }
    }
}
