using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JdSoft.Apple.Apns.Notifications;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace RoomService.NApns
{
	public class Provider
	{
		private NotificationService _service;
		public static TraceSource _source = new TraceSource("TraceSourceSTmate");

		public Provider(string p12File, string p12FilePassword, bool sandbox )
		{
            string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);

            _service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

            _service.SendRetries = 5;		//5 retries before generating notificationfailed event
            _service.ReconnectDelay = 5000; //5 seconds

            _service.Error += new NotificationService.OnError(service_Error);
            _service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

            _service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
            _service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
            _service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
            _service.Connecting += new NotificationService.OnConnecting(service_Connecting);
            _service.Connected += new NotificationService.OnConnected(service_Connected);
            _service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);
		}

		public NotificationService Service
		{
			get { return _service; }
		}

		public static  void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
		{
			Console.WriteLine("Bad Device Token: {0}", ex.Message);
		}

        public static void service_Disconnected(object sender)
		{
			_source.TraceEvent(TraceEventType.Critical, 3,  "Disconnected byTraceEvent");
            _source.Flush();
			//_source.TraceInformation("Disconnected");

		}

        public static void service_Connected(object sender)
		{
			//Console.WriteLine("Connected...");
			_source.TraceEvent(TraceEventType.Critical, 3,  "Connected by TraceEvent");
            _source.Flush();

		   // _source.TraceInformation("Connected");
		
		}

        public static void service_Connecting(object sender)
		{
			//Console.WriteLine("Connecting...");
			_source.TraceEvent(TraceEventType.Critical, 3,  "Connecting TraceEvent");
            _source.Flush();

	   //     _source.TraceInformation("Connecting");
		}

        public static void service_NotificationTooLong(object sender, NotificationLengthException ex)
		{
			//Console.WriteLine(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));

			//Console.WriteLine("Connecting...");
			_source.TraceEvent(TraceEventType.Critical, 3,
                  string.Format("Notification Too Long: {0} TraceEvent ", ex.Notification.ToString()));
            _source.Flush();

			//_source.TraceInformation(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));

		}

        public static void service_NotificationSuccess(object sender, Notification notification)
		{
			//Console.WriteLine(string.Format("Notification Success: {0}", notification.ToString()));

			_source.TraceEvent(TraceEventType.Critical, 3,
        string.Format("Notification Success: {0} TraceEvent ", notification.ToString()));
		//    _source.TraceInformation(string.Format("Notification Success: {0}", notification.ToString()));
            _source.Flush();


		}

		public static void service_NotificationFailed(object sender, Notification notification)
		{
			//Console.WriteLine(string.Format("Notification Failed: {0}", notification.ToString()));

            _source.TraceEvent(TraceEventType.Critical, 3, string.Format("Notification Failed: {0} TraceEvent ", notification.ToString()));
			//_source.TraceInformation(string.Format("Notification Failed: {0}", notification.ToString()));
            _source.Flush();


		}

		public static void service_Error(object sender, Exception ex)
		{
			//Console.WriteLine(string.Format("Error: {0}", ex.Message));


            _source.TraceEvent(TraceEventType.Critical, 3, string.Format("Error: {0} TraceEvent ", ex.Message));
			//_source.TraceInformation(string.Format("Error: {0}", ex.Message));
            _source.Flush();
		}
	}
}
