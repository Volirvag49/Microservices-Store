using System;
using System.ServiceProcess;

namespace Service
{
    public class Program : ServiceBase
    {
        public void Main (string[] args)
        {
            Run(this);
        }

    }
}
