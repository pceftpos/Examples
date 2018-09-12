using Test.Angular.SignalR.Model;

namespace Test.Angular.SignalR.Controllers
{  
    /// <summary>A PC-EFTPOS display response object.</summary>
	public class EFTDisplayResponse : EFTResponse
    {
        /// <summary>Constructs a default display response object.</summary>
        public EFTDisplayResponse() : base(null, "display")
        {
        }

        /// <summary>Number of lines to display.</summary>
        public int NumberOfLines { get; set; }

        /// <summary>Number of character per display line.</summary>
        public int LineLength { get; set; }

        /// <summary>Text to be displayed. Each display line is concatenated.</summary>
        public string[] DisplayText { get; set; }

        /// <summary>Indicates whether the Cancel button is to be displayed.</summary>
        public bool CancelKeyFlag { get; set; }

        /// <summary>Indicates whether the Accept/Yes button is to be displayed.</summary>
        public bool AcceptYesKeyFlag { get; set; }

        /// <summary>Indicates whether the Decline/No button is to be displayed.</summary>
        public bool DeclineNoKeyFlag { get; set; }

        /// <summary>Indicates whether the Authorise button is to be displayed.</summary>
        public bool AuthoriseKeyFlag { get; set; }

        /// <summary>Indicates whether the OK button is to be displayed.</summary>
        public bool OKKeyFlag { get; set; }

        /// <summary>Input type </summary>
        public char InputType { get; set; }

        /// <summary>Graphic code </summary>
        public char GraphicCode { get; set; }

        /// <summary>An object containing PAD tags and values </summary>
        public dynamic PurchaseAnalysisData { get; set; }
    }
}
