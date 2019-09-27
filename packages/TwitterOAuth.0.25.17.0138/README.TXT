
Sample Usage available in source code https://twitteroauth.codeplex.com/


add to following configurations below appSettings node,

<add key="Tw-ApiKey" value="your consumer key here"/>
 <add key="Tw-AppSecret" value="your consumer secret here"/>
 <add key="Tw-RedirectUri" value="your redirect uri here"/>
 
 Basic Usage
 
 Build paramters:
  
 - markup:
  <a href='http://twitter.com/oauth/authorize?<%=GetUrlParamters()%>'>Login with Twitter</a>
 
 -code behind:
  protected string GetUrlParamters()
  {
       ITwitterOAuthManager oAuthManager = new TwitterOAuthManager();
        return oAuthManager.GetUrlParameters();
  }
  
  After authorization :
  
  page load: (redirected uri page)
  
   ITwitterOAuthManager oAuthManager = new TwitterOAuthManager();

   if (oAuthManager.CheckTwitterOAuthRequest(Request))
   {
        TwitterBasicProfile result = oAuthManager.Authenticate(Request);
        txtHtmlArea.InnerHtml = Server.HtmlEncode(result.ScreenName);
   }