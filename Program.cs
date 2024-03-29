﻿using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer
{
    public class Dispatcher
    {
        public static void Main ()
        {
            new Dispatcher().Start();
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
                var worker = new Worker(ctx);
                Task.Run(() => worker.ProcessRequest());
            }
        }
    }

    public class Worker
    {
        private readonly HttpListenerContext _context;
 
        public Worker(HttpListenerContext context)
        {
            _context = context;
            LogContext();
        }
 
        public void ProcessRequest()
        {   
            WriteResponse("Cheese Hamburger! from " + _context.Request.Url); 
        }

        private void LogContext()
        {
            Console.WriteLine(_context.Request.HttpMethod + ": " + _context.Request.RawUrl);
            foreach (var key in _context.Request.Headers.AllKeys)
            {
                Console.WriteLine(key + ": " + _context.Request.Headers.Get(key));
            }
            Console.WriteLine();
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