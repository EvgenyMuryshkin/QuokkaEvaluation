using System;
using System.Collections.Generic;
using System.Text;

namespace Controllers
{
    public class Exception1 : Exception { }

    public class Exception2 : Exception { }

    public class Exception3 : Exception { }

    public class Exception4 : Exception { }

    public class BaseException : Exception { }

    public class DerivedException1 : BaseException { }

    public class DerivedException2 : BaseException { }
}
