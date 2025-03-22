namespace MobileRobotCommander.Data
{
    public class Grammar
    {
        public static readonly List<string> Forward = 
        [
            "forward",
            "go",
            "move forward",
            "ko straight",
            "keep going"
        ];

        public static readonly List<string> Backward =
        [
            "backward",
            "go back",
            "move backward",
            "go straight",
            "keep going"
        ];

        public static readonly List<string> Stop =
        [
            "stop",
            "don't move",
        ];

        public static readonly List<string> RotateLeft =
        [
            "rotate left",
        ];

        public static readonly List<string> RotateRight =
        [
            "rotate right",
        ];

        public static readonly List<string> TurnLeft =
        [
            "turn left",
        ];

        public static readonly List<string> TurnRight =
        [
            "turn left",
        ];

        public static readonly List<string> BackwardLeft =
        [
              "go back left",
              "back left",
        ];

        public static readonly List<string> BackwardRight =
        [
              "go back right",
              "back right",
        ];
    }
}
