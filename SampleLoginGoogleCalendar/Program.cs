using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalendarQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        static string Primary = "primary";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

            // View my Calendar List
            {
                var calenadrList = service.CalendarList.List();

                var response = calenadrList.Execute();
                foreach (var item in response.Items)
                {
                    Console.WriteLine("Owner:{0}, Summary:'{1}', Color:{2}", item.AccessRole, item.Summary, item.ColorId);
                }
            }

            // Test Delete Event
            {
                var request = service.Events.List(Primary);
                request.TimeMin = new DateTime(2017, 7, 31);
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var events = request.Execute();
                var deleteList = events.Items.Where(item => item.Summary == "여기가 이름인가");

                foreach(var item in deleteList)
                {
                    var deleteResult = service.Events.Delete(Primary, item.Id).Execute();
                    Console.WriteLine("Result deleting:{0}({1})", item.Summary, string.IsNullOrEmpty(deleteResult));
                }
            }

            // Test Insert event
            {
                Event insertEvent = new Event()
                {
                    Summary = "여기가 이름인가",
                    ColorId = "10",
                    Description = "이건 일정 설명",

                    Start = new EventDateTime()
                    {
                        DateTime = new DateTime(2017, 7, 31, 2, 55, 0)
                    },
                    End = new EventDateTime()
                    {
                        DateTime = new DateTime(2017, 7, 31, 5, 00, 0)
                    },
                    Location = "월스트리트 잉글리쉬 강남센터",
                };

                var result = service.Events.Insert(insertEvent, Primary).Execute();

                Console.WriteLine("Inserted New Event:");
                Console.WriteLine("{0} ({1})", result.Summary, result.Start.Date);
            }

            // Define parameters of request.
            {
                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                Events events = request.Execute();
                Console.WriteLine("Upcoming events:");
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        string when = eventItem.Start.DateTime.ToString();
                        if (String.IsNullOrEmpty(when))
                        {
                            when = eventItem.Start.Date;
                        }
                        Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                    }
                }
                else
                {
                    Console.WriteLine("No upcoming events found.");
                }
            }
            Console.Read();

        }
    }
}