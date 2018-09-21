namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTDisplayResponse : EFTResponse
    {
        /// <summary>Constructs a default display response object.</summary>
        public EFTDisplayResponse() : base(null, "display")
        {
        }

        public int NumberOfLines { get; set; }
        public int LineLength { get; set; }
        public string[] DisplayText { get; set; }
        public bool CancelKeyFlag { get; set; }
        public bool AcceptYesKeyFlag { get; set; }
        public bool DeclineNoKeyFlag { get; set; }
        public bool AuthoriseKeyFlag { get; set; }
        public bool OKKeyFlag { get; set; }
        public char InputType { get; set; }
        public char GraphicCode { get; set; }
        public dynamic PurchaseAnalysisData { get; set; }
    }
}
