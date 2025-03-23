namespace MobileRobotCommander.Data
{
    public class Grammar
    {
        public static readonly List<string> Forward =
        [
            "forward",
            "go",
            "move forward",
            "go straight",
            "keep going",
            "move ahead",
            "proceed",
            "advance"
        ];

        public static readonly List<string> Backward =
        [
            "backward",
            "go back",
            "move backward",
            "reverse",
            "back up",
            "step back"
        ];

        public static readonly List<string> Stop =
        [
            "stop",
            "don't move",
            "halt",
            "freeze",
            "hold position",
            "stay put"
        ];

        public static readonly List<string> RotateLeft =
        [
            "rotate left",
            "turn counterclockwise",
            "spin left",
            "pivot left",
            "rotate to the left"
        ];

        public static readonly List<string> RotateRight =
        [
            "rotate right",
            "turn clockwise",
            "spin right",
            "pivot right",
            "rotate to the right"
        ];

        public static readonly List<string> TurnLeft =
        [
            "turn left",
            "go left",
            "steer left",
            "veer left",
            "move left"
        ];

        public static readonly List<string> TurnRight =
        [
            "turn right",
            "go right",
            "steer right",
            "veer right",
            "move right"
        ];

        public static readonly List<string> BackwardLeft =
        [
            "go back left",
            "back left",
            "reverse left",
            "move backward left",
            "step back left"
        ];

        public static readonly List<string> BackwardRight =
        [
            "go back right",
            "back right",
            "reverse right",
            "move backward right",
            "step back right"
        ];
    }
}
