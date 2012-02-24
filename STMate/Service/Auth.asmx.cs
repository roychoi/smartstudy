using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Schema;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;
using System.Web.Caching;
using System.Web.Profile;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections;
using RoomService;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace STMate.Service
{
    /// <summary>
    /// Summary description for Auth
    /// </summary>
    [WebService(Namespace = "http://www.studyheyo.co.kr/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Auth : System.Web.Services.WebService
    {
        private SqlConnectionStringBuilder _connetionStringBuilder;
        
        private ChannelFactory<IRoom> factory = new ChannelFactory<IRoom>(new ServiceEndpoint(
            ContractDescription.GetContract(typeof(IRoom)),
          new BasicHttpBinding(),
            new EndpointAddress(new Uri("http://www.studyheyo.co.kr/Service/RoomService.RoomWCFService.svc"))));



        //private ChannelFactory<IRoom> factory = new ChannelFactory<IRoom>(new ServiceEndpoint(
        //        ContractDescription.GetContract(typeof(IRoom)),
        //        new NetTcpBinding(),
        //        new EndpointAddress(new Uri("net.tcp://localhost/wcf/roomwcfservice"))));
        //RoomClient test = new RoomClient("BasicHttpBinding_IRoom");


        [WebMethod(EnableSession = true)]
        public DataSet GetData()
        {
            _connetionStringBuilder = new SqlConnectionStringBuilder();

            _connetionStringBuilder["Data Source"] = "localhost\\SQLExpress";

            _connetionStringBuilder["integrated Security"] = true;
            _connetionStringBuilder["Initial Catalog"] = "STMEMBERSHIP";
            _connetionStringBuilder["Asynchronous Processing"] = true;

            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection( _connetionStringBuilder.ConnectionString ))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.aspnet_Users", connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataSet);
                }
            }
            
			SqlMembershipProvider test = new SqlMembershipProvider();

            //System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
            //nc.Add("connectionStringName", "StudyMateConnectionString");
            //nc.Add("applicationName", "/STMate");
            //nc.Add("requiresQuestionAndAnswer", "false");

            //test.Initialize("StudyMateMembershipProvider", nc);


            //System.Collections.Specialized.NameValueCollection nameColl = new System.Collections.Specialized.NameValueCollection();

            //nameColl.Add("connectionStringName", "StudyMateConnectionString");
            //nameColl.Add("applicationName", "/STMate");
            //nameColl.Add("requiresQuestionAndAnswer", "false");

            //defaultMembership.Initialize("StudyMateMembershipProvider", nameColl);


            //int Total= 0;
            //MembershipUserCollection col = defaultMembership.GetAllUsers(0, 10, out Total);

            //System.Collections.IEnumerator en =  col.GetEnumerator();

            //en.Reset();
            //while (en.MoveNext())
            //{
            //    MembershipUser firest = (MembershipUser)en.Current;
            //    Console.WriteLine(firest.Email);
            //}

            //int Total = 0;
            //MembershipUserCollection col = test.GetAllUsers(0, 10, out Total);

            //System.Collections.IEnumerator en = col.GetEnumerator();

            //en.Reset();
            //while (en.MoveNext())
            //{
            //    MembershipUser firest = (MembershipUser)en.Current;
            //    Console.WriteLine(firest.Email);
            //}
            //MembershipUser user = (MembershipUser)test.GetUser (userName, false);

            //public ActionResult ExpireSession()
            //{
            //    string sessionId = Session.SessionID;
            //    Session.Abandon();
            //    return new ContentResult()
            //    {
            //        Content = "Session '" + sessionId + "' abandoned at " + DateTime.Now
            //    };
            //}
            //this.Context.Application.Lock();
            //this.Context.Application.UnLock();

            //this.Context.Response.Redirect("http://www.google.co.kr");

            return dataSet;
        }

        [WebMethod(EnableSession = true)]
        public GAME_SERVER_INFO GetServerInfo( string game_no, string server_no )
        {
            GAME_SERVER_INFO game_server_info = new GAME_SERVER_INFO();

            IRoom proxy = factory.CreateChannel();
            (proxy as IDisposable).Dispose();

            game_server_info.game_no = Context.Session.SessionID;

            return game_server_info;
        }
	

        [WebMethod(EnableSession = true)]
		public CREATE_USER CreateUser( String loginEmail, String passWord, String userName, Byte gender, UInt16 birthYear 
											, String imageUrl )
        {
            CREATE_USER create_user = new CREATE_USER();

            try
            {

				if (RegexUtil.IsValidEmail(loginEmail) == false)
				{
					create_user.date_timeSpecified = false;
					create_user.login_id = loginEmail;
					create_user.result_code = (int)MembershipCreateStatus.InvalidEmail;

					return create_user;
				}

                MembershipProvider defaultMembership = Membership.Provider;
                MembershipCreateStatus status;

				MembershipUser newUser = defaultMembership.CreateUser(	loginEmail,
																		passWord,
																		loginEmail, 
																		null,
																		null,
																		true, 
																		System.Guid.NewGuid(), 
																		out status);
                if (status != MembershipCreateStatus.Success)
                {
                    create_user.date_timeSpecified = false;
					create_user.login_id = loginEmail;
					create_user.result_code = (int)status;

                    return create_user;
                }

                create_user.date_timeSpecified = true;
                create_user.date_time = newUser.CreationDate;
                create_user.login_id = newUser.UserName;
				create_user.result_code = (int)MembershipCreateStatus.Success;

				ProfileBase userProfile = ProfileBase.Create(loginEmail, true);

                userProfile["Gender"] = gender;
                userProfile["BirthYear"] = new DateTime(birthYear, 1, 1);
				userProfile["NickName"] = userName;
				userProfile["ImageUrl"] = imageUrl;

                userProfile.Save();

                return create_user;

            }
            catch (Exception e)
            {
                create_user.date_timeSpecified = false;
                create_user.login_id = userName;
				create_user.result_code = -1;

                return create_user;
            }
        }


        [WebMethod(EnableSession = true)]
		public AUTH_RESULT LoginUser(String loginEmail, String passWord, String deviceToken )
        {
            try
            {
                AUTH_RESULT auth_user_result = new AUTH_RESULT();
                MembershipProvider defaultMembership = Membership.Provider;

//                if (Context.Session.IsNewSession == true)
//                {
//                    Context.Session.Add(userName, passWord);
//                }

//                HttpCookie userCookie = Context.Request.Cookies["login_id"];
//                MembershipProvider defaultMembership = Membership.Provider;
                
//                if (userCookie != null)
//                {
//                    if (userCookie.Value.Equals(userName) == false)
//                    {
//                        auth_user_result.loginid = userName;
//                        auth_user_result.reason_sort = "Invalid";
//                        auth_user_result.date_timeSpecified = true;
//                        auth_user_result.date_time = userCookie.Expires;

//                        Context.Request.Cookies.Clear();
//                        this.Context.Response.Cookies.Clear();
//;
//                        return auth_user_result;
//                    }
//                }

				bool bresult = defaultMembership.ValidateUser(loginEmail, passWord);
                if (bresult == true)
                {
					MembershipUser user = (MembershipUser)defaultMembership.GetUser(loginEmail, true);

					HttpCookie httpCookie = new HttpCookie("login_id", loginEmail);
                    TimeSpan expiresTimeSpan = new TimeSpan(7, 0, 0, 0);
                    httpCookie.Expires = DateTime.Now + expiresTimeSpan;
                    
                    this.Context.Response.Cookies.Add(httpCookie);

                    System.Guid user_guid = (System.Guid) user.ProviderUserKey;

					auth_user_result.loginid = loginEmail;
                    auth_user_result.result = true;
                    auth_user_result.reason_sort = "Success";
                    auth_user_result.date_timeSpecified = false;
					//auth_user_result.user_no = user_guid.ToString("N");
					auth_user_result.user_no = user_guid.ToString("D");

					ProfileBase userProfile = ProfileBase.Create(loginEmail, true);

                    auth_user_result.gender = (byte)userProfile.GetPropertyValue("Gender");
					DateTime birth = (DateTime)userProfile.GetPropertyValue("BirthYear");
					String userName = (String)userProfile.GetPropertyValue("NickName");
					String imageUrl = (String)userProfile.GetPropertyValue("ImageUrl");

					userProfile["DeviceToken"] = deviceToken;	// last logined device
					userProfile.Save();

                    DateTime current = DateTime.Today;

                    if (current.Year < birth.Year)
                    {
                        auth_user_result.age = 0;
                    }
                    else
                    {
                        auth_user_result.age = (byte)(current.Year - birth.Year);
                    }

					//IRoom proxy = factory.CreateChannel();

					//bool bResult = proxy.LoginUser(user_guid.ToString("N"),
					//    loginEmail,		// from membership
					//    userName,		// from profile
					//    birth, // from profile
					//    auth_user_result.gender, // from profile 
					//    deviceToken
					//    );

					//(proxy as IDisposable).Dispose();

					//SqlProfileProvider sqlProfile = ProfileManager.Provider as SqlProfileProvider;
					//String Name = sqlProfile.ApplicationName;

                    return auth_user_result;
                }
                else
                {
					auth_user_result.loginid = loginEmail;
                    auth_user_result.reason_sort = "Login Invalid";

                    return auth_user_result;
                }
            }
            catch (Exception e)
            {
                AUTH_RESULT exception = new AUTH_RESULT();

                exception.result = false;
				exception.loginid = loginEmail;
                exception.reason_sort = e.Message;

                return exception;
            }
        }

        [WebMethod(EnableSession = true)]
        public UPDATE_DEVICE_INFO UpdateDeviceInfo(String userNo, String deviceToken)
		{
            IRoom proxy = factory.CreateChannel();

            UPDATE_DEVICE_INFO update_device_info = proxy.UpdateUserDeviceDb(userNo, deviceToken);

            if (update_device_info.result_code != 0)
            {
                return update_device_info;
            }

            try
            {
                ProfileBase userProfile = ProfileBase.Create(update_device_info.login_id, true);

				userProfile["DeviceToken"] = deviceToken;	// last logined device
                userProfile.Save();

                (proxy as IDisposable).Dispose();

                return update_device_info;
            }
            catch(Exception e )
            {
                update_device_info.result_code = -1;
                return update_device_info;
            }
		}

		[WebMethod(EnableSession = true)]
		public UPDATE_DEVICE_INFO UpdateImageUrl(String userNo, String imageUrl )
		{
			UPDATE_DEVICE_INFO update_device_info = new UPDATE_DEVICE_INFO();

			try
			{
				ProfileBase userProfile = ProfileBase.Create(userNo, true);

				userProfile["ImageUrl"] = imageUrl;	// last logined device
				userProfile.Save();

				return update_device_info;
			}
			catch ( Exception )
			{
				update_device_info.result_code = -1;
				return update_device_info;
			}
		}
    }
}
