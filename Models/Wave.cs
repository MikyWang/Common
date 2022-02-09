namespace MilkSpun.Common.Models
{
    [System.Serializable]
    public struct Wave
    {
        public float seed;
        public float frequency;
        public float amplitude;

        public static readonly Wave Identity = new Wave { seed = 0f, frequency = 1f, amplitude = 1f };

    }
}
