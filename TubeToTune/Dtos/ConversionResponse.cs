using System.Collections.Generic;
using System.Linq;

namespace TubeToTune.Dtos
{
    public class ConversionResponse
    {
        public ConversionResponse(int TotalVideosToBeConverted)
        {
            this.TotalVideosToBeConverted = TotalVideosToBeConverted;
            ErrorMessages = new List<VideoConversionErrorDetailsDto>();
        }

        public bool AllFailed { get { return ErrorMessages.Count() == TotalVideosToBeConverted; } }
        public List<VideoConversionErrorDetailsDto> ErrorMessages { get; set; }
        public string ConvertedAudioFilename { get; set; }
        private int TotalVideosToBeConverted { get; set; }
    }
}