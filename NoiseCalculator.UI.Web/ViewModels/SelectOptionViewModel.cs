namespace NoiseCalculator.UI.Web.ViewModels
{
    public class SelectOptionViewModel
    {
        public string Text { get; private set; }
        public string Value { get; private set; }
        public bool IsSelected { private get; set; }
        
        /// <summary>
        /// Return the SELECTED attribute if IsSelected is true, otherwise string.Empty is returned.
        /// </summary>
        public string SelectedAttribute
        { 
            get
            {
                if(IsSelected)
                {
                    return "SELECTED";
                }
                return string.Empty;
            } 
        }

        public SelectOptionViewModel(string text, string value)
        {
            Text = text;
            Value = value;
            IsSelected = false;
        }
    }
}