﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevDefined.OAuth.Framework;
using Intuit.Ipp.LinqExtender;
using QboMdFood.Models.DTO;
using QboMdFood.Models.Repository;

namespace QboMdFood.Models.Service
{
    /// <summary>
    /// This class is responsible for authenticating users.
    /// </summary>
    public class OAuthService
    {
        private OAuthorizationdto oAuthorizationdto = null;
        private OAuthRepository oAuthRepository = null;
        /// <summary>
        /// Constructor instantiate the repository
        /// </summary>
        /// <param name="oAuthDto"></param>
        public OAuthService(OAuthorizationdto oAuthDto)
        {

            oAuthorizationdto = oAuthDto;
            oAuthRepository = new OAuthRepository();
        }
        /// <summary>
        /// This method generates grant url and save the request 
        /// in to the repository
        /// </summary>
        /// <param name="oauthController"></param>
        /// <returns></returns>
        public string GrantUrl(object oauthController)
        {
            try
            {
                oAuthorizationdto.Token = oAuthorizationdto.OAuthSession.GetRequestToken();
                oAuthorizationdto.ResponseLink = string.Format(oAuthorizationdto.ResponseFormat,
                    oAuthorizationdto.AuthorizeUrl,
                    oAuthorizationdto.Token.Token,
                    UriUtility.UrlEncode(oAuthorizationdto.CallBackUrl));
                oAuthRepository.Save(oauthController, oAuthorizationdto);
                return oAuthorizationdto.ResponseLink;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This function checks whether the token is present.
        /// </summary>
        /// <param name="oauthController"></param>
        /// <returns></returns>
        public OAuthorizationdto IsTokenAvailable(object oauthController)
        {
            var oAuthDetails = new OAuthorizationdto();
            using (var db = new OAuthdataContext("DBContext"))
            {
                foreach (var currentIndex in db.Tokens)
                {
                    oAuthDetails.Realmid = currentIndex.realmid.ToString();
                    string testAccesToken = Utility.Utility.Decrypt(currentIndex.access_token, oAuthDetails.SecurityKey);
                    string testAccesTokenSecret = Utility.Utility.Decrypt(currentIndex.access_secret, oAuthDetails.SecurityKey);
                    oAuthDetails.AccessToken = testAccesToken;
                    oAuthDetails.AccessTokenSecret = testAccesTokenSecret;
                    oAuthDetails.IsConnected = true;
                    oAuthDetails.DataSource = currentIndex.datasource;
                    oAuthRepository.Save(oauthController, oAuthDetails);
                }
            }
            return oAuthDetails;
        }
        /// <summary>
        /// This function retrieve the request token from the
        /// repository
        /// </summary>
        /// <param name="oauthController"></param>
        /// <returns></returns>
        internal OAuthorizationdto GetRequestToken(object oauthController)
        {
            return oAuthRepository.Get(oauthController);
        }
        /// <summary>
        /// Retrieve the access token from the repository
        /// </summary>
        /// <param name="oauthController"></param>
        /// <returns></returns>
        internal OAuthorizationdto GetAccessToken(object oauthController)
        {
            return oAuthRepository.Get(oauthController);
        }
        /// <summary>
        /// Retrieve the access token from the server.
        /// </summary>
        /// <param name="oauthController"></param>
        /// <param name="oAuthorizationdto"></param>
        /// <returns></returns>
        internal OAuthorizationdto GetAccessTokenFromServer(object oauthController, OAuthorizationdto oAuthorizationdto)
        {
            try
            {
                IToken accessToken = oAuthorizationdto.OAuthSession.ExchangeRequestTokenForAccessToken(oAuthorizationdto.Token, oAuthorizationdto.OauthVerifyer);
                oAuthorizationdto.AccessToken = accessToken.Token;
                oAuthorizationdto.AccessTokenSecret = accessToken.TokenSecret;
                oAuthorizationdto.IsConnected = true;
                oAuthRepository.Save(oauthController, oAuthorizationdto);
                return oAuthorizationdto;
            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.InvalidTokenException ex)
            {
                throw ex;
            }
            catch (Intuit.Ipp.Exception.SdkException ex)
            {
                throw ex;
            }
        }
    }
}