using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public class SnakeGameException : Exception { }

    public class CrashedException : SnakeGameException { }

    public class CrashedInWallException : CrashedException { }

    public class CrashedInSnakeException : CrashedException { }

    public class GameCompletedException : SnakeGameException { }
}
