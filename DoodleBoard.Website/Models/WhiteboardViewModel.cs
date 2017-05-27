using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoodleBoard.Website.Models
{
    public class WhiteboardViewModel
    {
        public string WhiteboardId { get; private set; }
        public WhiteboardViewModel(string id)
        {
            WhiteboardId = id;
        }
    }
}