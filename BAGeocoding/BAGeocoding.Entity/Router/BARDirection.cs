namespace BAGeocoding.Entity.Router
{
    public class BARDirection
    {
        // Cho phép đi theo chiều vẽ segment
        public bool Forward { get; set; }
        // Cho phép đi ngược chiều vẽ segment
        public bool Reverse { get; set; }

        public BARDirection() { }
        
        public BARDirection(byte direction)
        {
            Forward = (direction == 0 || direction == 1);
            Reverse = (direction == 0 || direction == 2);
        }

        public BARDirection(BARDirection other)
        {
            Forward = other.Forward;
            Reverse = other.Reverse;
        }

        public byte GetByte()
        {
            if (Forward == true && Reverse == true)
                return 0;
            else if (Forward == true)
                return 1;
            else
                return 2;
        }
    }
}