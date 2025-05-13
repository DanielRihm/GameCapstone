namespace LCPS.SlipForge.CustomRand
{
    public class CustomRandom
    {
        private System.Random _random;
        private int _numTaps;

        public CustomRandom(int _seed)
        {
            _numTaps = 0;
            _random = new System.Random(_seed);
        }

        public int Next()
        {
            _numTaps++;
            return _random.Next();
        }

        public int Next(int num)
        {
            _numTaps++;
            return _random.Next(num);
        }

        public int Next(int num1, int num2)
        {
            _numTaps++;
            return _random.Next(num1, num2);
        }

        public int GetTaps()
        {
            return _numTaps;
        }
    }
}
