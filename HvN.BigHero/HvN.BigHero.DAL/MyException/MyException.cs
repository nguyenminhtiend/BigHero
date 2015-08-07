using System;

namespace HvN.BigHero.DAL.MyException
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("The page you requested was not found.")
        {
        }
    }
    public class InternalServerException : Exception
    {
        public InternalServerException()
            : base("The server occured error.")
        {
        }
    }
}
