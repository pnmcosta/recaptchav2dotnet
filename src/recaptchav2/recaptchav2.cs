using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace recaptchav2
{
    [ToolboxData("<{0}:recaptchav2 SiteKey=\"\" SecretKey=\"\" ErrorMessage=\"Human validation failed!\" runat=\"server\"></{0}:recaptchav2>")]
    public class recaptchav2 : WebControl, IValidator, IPostBackDataHandler
    {
        private bool _isValid = true;
     

        #region Properties
        [Bindable(true)]
        [Category("Recaptcha")]
        [Description("Your recaptcha site key")]
        [Localizable(true)]
        public string SiteKey
        {
            get { return GetValue("SiteKey", String.Empty); }
            set { SetValue("SiteKey", value); }
        }

        [Bindable(true)]
        [Category("Recaptcha")]
        [Description("Your recaptcha secret key")]
        [Localizable(true)]
        public string SecretKey
        {
            get { return GetValue("SecretKey", String.Empty); }
            set { SetValue("SecretKey", value); }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        [Browsable(false)]
        [DefaultValue("Human validation failed!")]
        [Description("Message to display in a Validation Summary when the RECAPTCHA fails to validate.")]
        public string ErrorMessage
        {
            get
            {
                if (!_isValid)
                {
                    return GetValue("ErrorMessage", String.Empty);
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                SetValue("ErrorMessage", value);
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                if (!value)
                    _isValid = false;
            }
        }
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
            }
        }
        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.RegisterRequiresControlState(this);
            Page.Validators.Add(this);
            Page.RegisterRequiresPostBack(this);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.Visible && this.Enabled && !this.DesignMode)
            {
                LiteralControl lc = new LiteralControl("<script src=\"https://www.google.com/recaptcha/api.js\" async defer></script>");
                Page.Header.Controls.Add(lc);
            }
        }
        protected override void OnUnload(EventArgs e)
        {
            if (Page != null)
            {
                Page.Validators.Remove(this);
            }
            base.OnUnload(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                //render control for designer...
                writer.Write(string.Format("<div>recaptcha \"{0}\"</div>", this.ID));
            }
            else
            {
                //render actual control in runtime...
                base.Render(writer);
            }
        } 
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.Visible && this.Enabled)
                output.Write("<div class=\"g-recaptcha\" data-sitekey=\"" + SiteKey + "\"></div>");
        }
       
      

        public void Validate()
        {
            // we validate in LoadPostData
        }

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            if (!Visible || !Enabled || HttpContext.Current == null)
            {
                _isValid = true;
                return _isValid;
            }

            string Response = HttpContext.Current.Request.Form["g-recaptcha-response"];//Getting Response String Append to Post Method
            //Request to Google Server
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + SecretKey + "&response=" + Response + "&remoteip=" + GetClientIp());
            try
            {
                //Google recaptcha Response
                using (WebResponse wResponse = req.GetResponse())
                {

                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        ReCaptchaResponse data = js.Deserialize<ReCaptchaResponse>(jsonResponse.Replace("error-codes", "errorCodes"));// Deserialize Json

                        _isValid = Convert.ToBoolean(data.success);
                    }
                }

                return _isValid;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        private string GetClientIp()
        {
            // Look for a proxy address first
            String _ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // If there is no proxy, get the standard remote address
            if (string.IsNullOrWhiteSpace(_ip) || _ip.ToLower() == "unknown")
                _ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return _ip;
        }
        public void RaisePostDataChangedEvent()
        {

        }
        public class ReCaptchaResponse
        {
            public string success { get; set; }
            public string[] errorCodes { get; set; }
        }

        #region ViewState Helpers
        private T GetValue<T>(string key, T nullValue)
        {
            if (ViewState[key] == null)
                return nullValue;
            else
                return (T)ViewState[key];
        }
        private void SetValue<T>(string key, T value)
        {
            ViewState[key] = value;
        }
        #endregion
    }
}
