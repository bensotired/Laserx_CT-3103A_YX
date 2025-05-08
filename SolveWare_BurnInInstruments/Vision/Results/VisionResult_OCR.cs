using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class VisionResult_OCR : VisionJsonCmdReceiver
    {
        public VisionResult_OCR() : base()
        {
            OCRBoxs = new List<LineCollection>();

        }
        public List<LineCollection> OCRBoxs { get; set; }

    }

    
}