using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer
{
    public class WebServer
    {        
        public static void Main ()
        {
            WebServer webServer = new WebServer();
            webServer.Start();
        }
        
        private readonly HttpListener _listener = new HttpListener();
 
        public void Start()
        {
            _listener.Prefixes.Add("http://*:5000/");            
            _listener.Start();

            Console.WriteLine("Launching server...");
            while (true)
            {
                HttpListenerContext ctx = _listener.GetContext();
                new Worker(ctx).ProcessRequest();
            }
        }
    }

    public class Worker
    {
        private readonly HttpListenerContext _context;
 
        public Worker(HttpListenerContext context)
        {
            _context = context;
        }
 
        public void ProcessRequest()
        {   
            LogContext();
            WriteResponse("Hello World from C#!"); 
        }

        private void LogContext()
        {
            Console.WriteLine(_context.Request.HttpMethod + ": " + _context.Request.RawUrl);
            foreach (var key in _context.Request.Headers.AllKeys)
            {
                Console.WriteLine(key + ": " + _context.Request.Headers.Get(key));
            }
        } 

        private void WriteResponse(string data)
        {
            byte[] b = Encoding.UTF8.GetBytes(data);
            _context.Response.ContentLength64 = b.Length;
            _context.Response.OutputStream.Write(b, 0, b.Length);
            _context.Response.OutputStream.Close();
            
        }
    }
}